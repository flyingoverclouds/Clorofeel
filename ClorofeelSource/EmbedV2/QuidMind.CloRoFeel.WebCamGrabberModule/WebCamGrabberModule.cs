using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuidMind.CloRoFeel.CoreKernel;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace QuidMind.CloRoFeel.WebCamGrabber
{
    [RobotModule(Name = "WebCamGrabber", Description = "Manage the webcam grabber process", Version = "1.0.0.0")]
    public class WebCamGrabberModule : IRobotModule
    {
        const string WebCamGrabberExeFileName = "QuidMind.CloRoFeel.CamGrabber.exe";


        string fullExeName = null;

        bool usable = false;
        public void Initialize(InterModuleMessaging immContext, string initData)
        {
            Console.WriteLine("[WebCamGrabberModule] WebCamGrabberModule.Initialize()");

            fullExeName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),WebCamGrabberExeFileName);

            if (!File.Exists(fullExeName))
            {
                Console.WriteLine("** WebCamGrabberModule ERROR : " + WebCamGrabberExeFileName + " not found.");
            }
            else
                usable = true;
            
        }

        Thread thWebCamGrabberMonitoring = null;
        public void Run()
        {
            Console.WriteLine("WebCamGrabberModule.Run()");
            thWebCamGrabberMonitoring = new Thread(DoRunWebCamMonitoring);
            thWebCamGrabberMonitoring.Priority = ThreadPriority.BelowNormal;
            thWebCamGrabberMonitoring.Start();
        }


        void DoRunWebCamMonitoring()
        {
            Process webCamGrabberProcess = null;
            try
            {
                webCamGrabberProcess = Process.Start(fullExeName);
                
                while (true)
                {
                    if (webCamGrabberProcess.HasExited)
                    {
                        Console.WriteLine("WebCamGrabber process exited. Restartint  it ...");
                        Thread.Sleep(1000); // pause d'une seconde par sécurité
                        webCamGrabberProcess = Process.Start(fullExeName);
                    }
                    Thread.Sleep(2000); // on check la vie du process webcam toute les 2 secondes
                }
            }
            catch (ThreadAbortException tae)
            {
                if (webCamGrabberProcess != null)
                {
                    Console.WriteLine("WebCamGrabber process terminating ....");
                    webCamGrabberProcess.CloseMainWindow();
                }
            }
        }


        public void Terminate()
        {
            Console.WriteLine("WebCamGrabberModule.Terminate()");
            thWebCamGrabberMonitoring.Abort();
            
        }

        public void SendData(InterModuleMessage dataMessage)
        {
            Console.WriteLine(string.Format("WebCamGrabberModule.SendData({0})", dataMessage));
        }
    }
}
