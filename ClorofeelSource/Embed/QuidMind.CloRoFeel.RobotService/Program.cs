using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using Microsoft.ServiceBus;
using System.ServiceModel.Description;
using QuidMind.CloRoFeel;
using QuidMind.CloRoFeel.Service;
using System.ServiceModel.Web;
using QuidMind.CloRoFeel.RoboardService;

namespace QuidMind.CloRoFeel.RobotService
{
    class Program
    {
        static void InitializeHardware()
        {
            var _hardwareService = ChannelFactory<IRoboardHardware>.CreateChannel(new NetTcpBinding(), new EndpointAddress(Properties.Settings.Default.RoboardHardwareServiceUri));
            _hardwareService.Initialize(); System.Threading.Thread.Sleep(100);
            _hardwareService.SetServo(0, 1500, 30); System.Threading.Thread.Sleep(100);
            _hardwareService.SetServo(1, 1500, 30); System.Threading.Thread.Sleep(100);
            _hardwareService.SetServo(2, 1500, 30); System.Threading.Thread.Sleep(100);
            _hardwareService.SetServo(3, 1500, 30); System.Threading.Thread.Sleep(100);
            _hardwareService.SetServo(5, 1500, 30); System.Threading.Thread.Sleep(100);
            _hardwareService.SetServo(6, 1500, 30); System.Threading.Thread.Sleep(100);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing hardware position to default value");
            InitializeHardware();

            Console.WriteLine("Configuring Services .... for AppFabric");
            // ATTENTION : Necessite une compilation pour le Framework.Net 4 complet (pas le client !)
            Uri uriSbRobotRemote = Microsoft.ServiceBus.ServiceBusEnvironment.CreateServiceUri(
                "http",  //"sb", //"https",
                "clorofeel",
                "RobotRemote/RobotBoard"
                );
            Console.WriteLine("CloRoFeel AppFabric URL = " + uriSbRobotRemote.ToString());
            
            WebServiceHost hostRobotService = new WebServiceHost(typeof(RobotRemoteService), uriSbRobotRemote);
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
            Console.WriteLine("Exposing .... ");
            hostRobotService.Open();
            Console.WriteLine("");
            Console.WriteLine("Robot service is exposed. Press Enter to Terminate ....");
            Console.ReadLine();
            Console.WriteLine("==> CONFIRM BY PRESSING ENTER AGAIN");
            Console.ReadLine();
            hostRobotService.Close();

            
        }
    }
}
