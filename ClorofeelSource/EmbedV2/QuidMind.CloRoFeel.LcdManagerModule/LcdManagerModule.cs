using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuidMind.CloRoFeel.CoreKernel;
using System.Threading;
using System.IO.Ports;
using QuidMind.CloRoFeel.LcdManagerModule.LcdPaginator;
using System.Net;

namespace QuidMind.CloRoFeel.LcdManager
{
    [RobotModule(Name="LcdManager",Description="LCD Screen manager with auto pagination",Version="1.0.0.0")]
    public class LcdManagerModule : IRobotModule
    {
        public void Initialize(InterModuleMessaging immContext, string initData)
        {
            try
            {
                //immContext.PushMessage("", "", "LCD MANAGER");
                Console.Write("[LcdManagerModule] LcdManagerModule.Initialize()");
                OpenLcd(initData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[LcdManagerModule] EXCEPTION : " + ex.ToString());
            }
        }

        LcdPager pager;
        Thread thModule = null;
        public void Run()
        {
            Console.Write("[LcdManagerModule] LcdManagerModule.Run()");

            thModule = new Thread(() =>
            {
                try
                {
                    
                    pager = new LcdPager();
                    pager.LcdRefreshed += new LcdRefreshDelegate(pager_LcdRefreshed);
                    pager.Start();
                    pager.RegisterPage("00.00", "CloRoFeel V2K12", "(c)QuidMind 2012");
                    CreateIpconfigPage();
                    while (true)
                    {
                        Thread.Sleep(2000);
                    }
                }
                catch (ThreadAbortException tae)
                {
                }
            });
            thModule.Priority = ThreadPriority.BelowNormal;
            thModule.IsBackground = true;
            thModule.Name = "LcdManagerModuleThread";
            thModule.Start();
        }

        public void Terminate()
        {
            Console.Write("[LcdManagerModule] LcdManagerModule.Terminate()");
            if (thModule != null)
                thModule.Abort();
        }

        public void SendData(InterModuleMessage dataMessage)
        {
            //Console.Write(string.Format("[LcdManagerModule] LcdManagerModule.SendData({0})", dataMessage));

            //_immContext.PushMessage("MFSensor", "LcdManager", "[04.00]SENSOR " + receivedMessage);
            Console.WriteLine("LCD: " + dataMessage.Body);
            string pageNum = dataMessage.Body.Substring(1, 5);
            pager.RegisterPage(pageNum,
                (dataMessage.Body + "                                    ").Substring(7, 16),
                (dataMessage.Body + "                                    ").Substring(7 + 16, 16));
            pager.MoveToPage(pageNum,3);
        }

        private void CreateIpconfigPage()
        {
            try
            {
                pager.RegisterPage((num, l1, l2) =>
                {
                    pager.DeletePages("01.");
                    CreateIpconfigPage();
                }, "01.00", "IP CONFIG", "----------------");

                int ipNum = 1;
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        pager.RegisterPage("01.0" + ipNum.ToString(), "IP " + ipNum.ToString(), ip.ToString());
                        ipNum++;
                    }
                }

                pager.RegisterPage("01.00", "IP CONFIG", "----------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[LcdManagerModule] EXCEPTION in CreateIpconfigPage : " + ex.ToString());
            }
        }

        #region LCD Com6
        SerialPort lcd = null;
        
        void pager_LcdRefreshed(string line1, string line2)
        {
            if (lcd != null)
            {
                lcd.ClearScreen().GoToLine0().WriteLineEx(line1).GoToLine1().WriteLineEx(line2);
            }
        }


        void OpenLcd(string comPort)
        {
            Console.WriteLine("[LcdManagerModule]  Opening LCD COM port : " + comPort);
            try
            {
                lcd = new SerialPort(comPort);
                lcd.BaudRate = 19200;
                lcd.DataBits = 8;
                lcd.Parity = Parity.None;
                lcd.StopBits = StopBits.One;
                lcd.Open();
                System.Threading.Thread.Sleep(300); // temps d'initialiser le port COM

                lcd.SwitchOn_NoCursor_NoBlink().BacklightOn();
                lcd.ClearScreen();
                lcd.GoToLine0();
                lcd.WriteLine("  CloRoFeel V2");
                lcd.GoToLine1();
                lcd.WriteLine("(c)Quidmind 2012");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Console.WriteLine("[LcdManagerModule] EXCEPTION in OpenLcd() : \r\n" + ex.ToString());
                lcd = null;
            }
        }
        #endregion
    }
}
