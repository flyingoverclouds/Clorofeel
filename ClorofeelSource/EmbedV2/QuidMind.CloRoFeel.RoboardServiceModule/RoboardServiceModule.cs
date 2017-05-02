using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuidMind.CloRoFeel.CoreKernel;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;

namespace QuidMind.CloRoFeel.RoboardServices
{
    [RobotModule(Name = "RoboardServiceModule", Description = "Implemente the remote driving and management service", Version = "1.0.0.0")]
    public class RoboardServiceModule : IRobotModule
    {
        public void Initialize(InterModuleMessaging immContext, string initData)
        {
            try
            {
                Console.WriteLine("[RoboardServiceModule] RoboardServiceModule.Initialize()");
                OpenService();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[RoboardServiceModule] EXCEPTION in Initialize() : " + ex.ToString());
            }
        }

        Thread thRoboardHardwareMonitoring = null;
        public void Run()
        {
            Console.WriteLine("[RoboardServiceModule] RoboardServiceModule.Run()");
            thRoboardHardwareMonitoring = new Thread(DoRunServiceChannelMonitoring);
            thRoboardHardwareMonitoring.IsBackground = true;
            thRoboardHardwareMonitoring.Priority = ThreadPriority.BelowNormal;
            thRoboardHardwareMonitoring.Name = "RoboardServiceModule";
            thRoboardHardwareMonitoring.Start();
        }


        void DoRunServiceChannelMonitoring()
        {
            try
            {
                while (true)
                {
                    //Console.WriteLine("RoboardChannael state = " + _roboardHardwareServiceHost.State.ToString());
                    // TODO : add check de l'etat du service pour le reexposer automatiquement si besoin
                    Thread.Sleep(2000); // on check la vie du process webcam toute les 2 secondes
                }
            }
            catch (ThreadAbortException tae)
            {
                CloseService();
            }
        }

        ServiceHost _roboardHardwareServiceHost = null;

        void OpenService()
        {
            Console.WriteLine("[RoboardServiceModule] exposing Hardware service as local WCF service ...");
            _roboardHardwareServiceHost = new ServiceHost(typeof(QuidMind.CloRoFeel.RoboardServices.RoboardHardwareService));
            _roboardHardwareServiceHost.Open();
        }

        void CloseService()
        {
            Console.WriteLine("[RoboardServiceModule] Closing hardware services ...");
            if (_roboardHardwareServiceHost != null && _roboardHardwareServiceHost.State != CommunicationState.Closed)
                _roboardHardwareServiceHost.Close();
            _roboardHardwareServiceHost = null;
        }



        public void Terminate()
        {
            Console.WriteLine("[RoboardServiceModule]  RoboardServiceModule.Terminate()");
            thRoboardHardwareMonitoring.Abort();   
        }

        public void SendData(InterModuleMessage dataMessage)
        {
            if (dataMessage.TargetModule != "RoboardServiceModule")
                return;

            Console.WriteLine(string.Format("[RoboardServiceModule] RoboardServiceModule.SendData({0})", dataMessage));
        }
    }
}
