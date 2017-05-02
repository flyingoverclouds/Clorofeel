using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuidMind.CloRoFeel.CoreKernel;
using System.Threading;
using System.ServiceModel;
using QuidMind.CloRoFeel.RoboardService;
using System.ServiceModel.Web;
using Microsoft.ServiceBus;
using System.ServiceModel.Description;
using QuidMind.CloRoFeel.Service;
using QuidMind.CloRoFeel.RobotServiceModule;

namespace QuidMind.CloRoFeel.RobotService
{
    [RobotModule(Name = "RobotService", Description = "Service de pilotage du robot exposé via AppFabric", Version = "1.0.0.0")]
    public class RobotServiceModule : IRobotModule
    {
        public void Initialize(InterModuleMessaging immContext, string initData)
        {
            try
            {
                Console.WriteLine("[RobotServiceModule] RobotServiceModule.Initialize()");

                ConfigureService();

                ThreadPool.QueueUserWorkItem((state) => { OpenService(); }); // pour eviter un blocage si AppFabric est bloqué par une FW
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("[RobotServiceModule] EXCEPTION in Initialize() : " + ex.ToString());
            }

        }

        Thread thRobotServiceMonitoring = null;
        public void Run()
        {
            Console.WriteLine("[RobotServiceModule] RobotServiceModule.Run()");

            thRobotServiceMonitoring = new Thread(DoRunServiceChannelMonitoring);
            thRobotServiceMonitoring.IsBackground = true;
            thRobotServiceMonitoring.Priority = ThreadPriority.BelowNormal;
            thRobotServiceMonitoring.Name = "RobotServiceModule";
            thRobotServiceMonitoring.Start();
        }

        void DoRunServiceChannelMonitoring()
        {
            try
            {
                Thread.Sleep(2000); // pause de 2 seconde pour attendre la disponibilité du service hardware
                InitializeHardware();
                //ConfigureService();
                //OpenService();
                while (true)
                {
                    //Console.WriteLine("RoboardChannael state = " + _roboardHardwareServiceHost.State.ToString());
                    // TODO : add check de l'etat du service pour le reexposer automatiquement si besoin
                    Thread.Sleep(15000); 
                }
            }
            catch (ThreadAbortException tae)
            {
                CloseService();
            }
        }

        WebServiceHost hostRobotService = null;

        void ConfigureService()
        {
            Console.WriteLine("[RobotServiceModule] Configuring WCF bindings ...");

            Uri uriSbRobotRemote = Microsoft.ServiceBus.ServiceBusEnvironment.CreateServiceUri(
             "http",  //"sb", //"https",
             "clorofeelv2",
             "RobotRemote/RobotBoard"
             );
            Console.WriteLine("[RobotServiceModule]    CloRoFeel RobotService AppFabric URL = " + uriSbRobotRemote.ToString());

            hostRobotService = new WebServiceHost(typeof(RobotRemoteService), uriSbRobotRemote);
            //ServiceHost hostRobotService = new ServiceHost(typeof(RobotRemoteService), uriSbRobotRemote);

            TransportClientEndpointBehavior sharedSecretServiceBusCredential = new TransportClientEndpointBehavior();
            sharedSecretServiceBusCredential.CredentialType = TransportClientCredentialType.SharedSecret;
            sharedSecretServiceBusCredential.Credentials.SharedSecret.IssuerName = SecureData.IssuerName;
            sharedSecretServiceBusCredential.Credentials.SharedSecret.IssuerSecret = SecureData.IssuerSecret;

            ContractDescription contractDescription = ContractDescription.GetContract(typeof(IRobotRemote), typeof(RobotRemoteService));

            ServiceEndpoint serviceEndPoint = new ServiceEndpoint(contractDescription);
            serviceEndPoint.Address = new EndpointAddress(uriSbRobotRemote);

            //serviceEndPoint.Binding = new NetTcpRelayBinding(EndToEndSecurityMode.None, RelayClientAuthenticationType.None) 
            //                                { ConnectionMode = TcpRelayConnectionMode.Relayed };
            //serviceEndPoint.Binding = new BasicHttpRelayBinding(EndToEndBasicHttpSecurityMode.None, RelayClientAuthenticationType.None);
            serviceEndPoint.Binding = new WebHttpRelayBinding(EndToEndWebHttpSecurityMode.None, RelayClientAuthenticationType.None);
            serviceEndPoint.Behaviors.Add(sharedSecretServiceBusCredential);
            hostRobotService.Description.Endpoints.Add(serviceEndPoint);
        }

        void OpenService()
        {
            Console.WriteLine("[RobotServiceModule] exposing WCF bindings on Azure ...");
            //hostRobotService = new ServiceHost(typeof(QuidMind.CloRoFeel.RobotServiceModule.RobotRemoteService));
            try
            {
                hostRobotService.Open();
                Console.WriteLine("[RobotServiceModule] services exposed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[RobotServiceModule] ERROR Exposing clorofeel service.");
            }
        }

        void CloseService()
        {
            if (hostRobotService != null && hostRobotService.State != CommunicationState.Closed)
                hostRobotService.Close();
            hostRobotService = null;
        }
        public void Terminate()
        {
            Console.WriteLine("[RobotServiceModule] RobotServiceModule.Terminate()");
        }

        public void SendData(InterModuleMessage dataMessage)
        {
            Console.WriteLine(string.Format("[RobotServiceModule] Msg from ({0}) : ", dataMessage.SourceModule,dataMessage.Body));
            // {0000;0000}
            if (dataMessage.Body.Length == 11 && dataMessage.Body[0] == '{' && dataMessage.Body[5] == ';' && dataMessage.Body[10] == '}')
                ParseSensorData(dataMessage.Body);
        }

        private void ParseSensorData(string sensorData)
        {
            Console.WriteLine("Parsing REAL Sensor data");
            bool emergencyStop = false;
            string msg = string.Empty;
            if (sensorData[1] != '0')
            {
                emergencyStop = true;
                msg += "AvG ";
            }
            if (sensorData[2] != '0')
            {
                emergencyStop = true;
                msg += "AvD ";
            }
            if (sensorData[3] != '0')
            {
                emergencyStop = true;
                msg += "ArG ";
            }
            if (sensorData[4] != '0')
            {
                emergencyStop = true;
                msg += "ArD ";
            }
            if (emergencyStop)
            {
                Console.WriteLine("EMERGENCY STOP ");
                InterModuleMessaging.Current.PushMessage("RobotService", "LcdManager", "[05.00] ! FALL RISK !  " + msg);
                _hardwareService.SetEmergencyStatus(true);
            }
            else
            {
                Console.WriteLine("NORMAL RUN ");
                _hardwareService.SetEmergencyStatus(false);
                InterModuleMessaging.Current.PushMessage("RobotService","LcdManager","[05.00]                                ");
            }
        }

        IRoboardHardware _hardwareService = null;


        void InitializeHardware()
        {
            Console.WriteLine("[RobotServiceModule] Initializing hardware using roboardServices ...");
            _hardwareService = ChannelFactory<IRoboardHardware>.CreateChannel(new NetTcpBinding(), new EndpointAddress(QuidMind.CloRoFeel.RobotServiceModule.Properties.Settings.Default.RoboardHardwareServiceUri));
            _hardwareService.Initialize(); System.Threading.Thread.Sleep(100);
            _hardwareService.SetServo(0, 1500, 30); System.Threading.Thread.Sleep(100);
            _hardwareService.SetServo(1, 1500, 30); System.Threading.Thread.Sleep(100);
            _hardwareService.SetServo(2, 1500, 30); System.Threading.Thread.Sleep(100);
            _hardwareService.SetServo(3, 1500, 30); System.Threading.Thread.Sleep(100);
            _hardwareService.SetServo(5, 1500, 30); System.Threading.Thread.Sleep(100);
            _hardwareService.SetServo(6, 1500, 30); System.Threading.Thread.Sleep(100);
            Console.WriteLine("[RobotServiceModule] CloRoFeel Hardware initialized.");
        }
    }
}
