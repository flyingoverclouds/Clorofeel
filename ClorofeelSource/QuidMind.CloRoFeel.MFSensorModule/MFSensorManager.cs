using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuidMind.CloRoFeel.CoreKernel;
using System.IO.Ports;
using System.Threading;

namespace QuidMind.CloRoFeel.MFSensor
{
    [RobotModule(Name = "MFSensorModule", Description = "Manage sensor information from MF PandaII", Version = "1.0.0.0")]
    public class MFSensorModule : IRobotModule
    {
        InterModuleMessaging _immContext = null;
        SerialPort comSensor = null;
        public void Initialize(InterModuleMessaging immContext, string initData)
        {
            try
            {
                _immContext = immContext;
                Console.WriteLine("[MFSensorModule] MFSensorModule.Initialize()");
                comSensor = new SerialPort(initData);
                comSensor.BaudRate = 115200;
                comSensor.StopBits = StopBits.One;
                comSensor.Parity = Parity.None;
                comSensor.Handshake = Handshake.None;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[MFSensorModule] EXCEPTION in Initialize : " + ex.ToString());
            }

        }
        Thread thSensor = null;
        public void Run()
        {
            Console.WriteLine("[MFSensorModule] MFSensorModule.Run()");
            thSensor = new Thread(() =>
            {
                try
                {
                    if (comSensor != null)
                    {
                        try
                        {
                            comSensor.Open();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("[MFSensorModule] : unable to open sensor COM port -> use simulation");
                            comSensor = null;
                        }
                    }
                    byte[] buf = new byte[256];
                    var utf8encoder = UTF8Encoding.GetEncoding(0);
                    string msgReceived;
                    while (true)
                    {
                        if (comSensor == null) // pas de carte senseur en debug
                        {
                            System.Threading.Thread.Sleep(2000);
                            msgReceived = DateTime.Now.ToLongTimeString();
                        }
                        else
                        {
                            msgReceived = comSensor.ReadLine();
                        }
                        //int nbChar = comSensor.Read(buf, 0, buf.Length);
                        //Console.WriteLine("NbChar = " + nbChar);
                        //msgReceived = utf8encoder.GetString(buf, 0, nbChar);
                        ParseMessage(msgReceived);
                    }
                }
                catch (ThreadAbortException tae)
                {
                    if (comSensor != null)
                        comSensor.Close();
                }
            });
            thSensor.IsBackground = true;
            thSensor.Name = "MFSensorModuleThread";
            thSensor.Start();
        }

        public void Terminate()
        {
            Console.WriteLine("[MFSensorModule] MFSensorModule.Terminate()");
            if (thSensor != null)
                thSensor.Abort();
        }

        public void SendData(InterModuleMessage dataMessage)
        {
            Console.WriteLine(string.Format("[MFSensorModule] MFSensorModule.SendData({0})", dataMessage));
        }

        #region Traitement des message recu de la carte Senseur
        void ParseMessage(string receivedMessage)
        {
            receivedMessage = receivedMessage.Replace("\r", "").Replace("\n", "");
            if (string.IsNullOrEmpty(receivedMessage))
                return;
            Console.WriteLine("[MFSensorModule]  ParseMessage : " + receivedMessage);

            try
            {
                // on les pousse dans le system de messagerie intermodule
                if (receivedMessage.StartsWith("{"))
                {
                    _immContext.PushMessage("MFSensor", "RobotService", receivedMessage);
                    //_immContext.PushMessage("MFSensor", "LcdManager", "[04.00]SENSOR          " + receivedMessage);
                }
                else
                    _immContext.PushMessage("MFSensor", "LcdManager", "[04.00]SENSOR " + receivedMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[MFSensorModule] : ParseMessage Error : " + ex.ToString());
            }
        }

        #endregion
    }
}
