/*** CloRoFeel - (c) QuidMind 2012
 * Ce code est la propriété de QuidMind et ne peut être utilisé sans accord explicite.
 * 
 * ** Program.cs : programme principal PandaII 
 * 
 * 
 */



using System;
using System.Threading;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.Hardware;
using QuidMind.MFCloRoFeel;
using System.IO.Ports;
using System.Text;

namespace QuidMind.MFCloRoFeel
{
    public class Program
    {
        static DistanceMonitor detecteurChute;
        static ChocMonitor detecteurChoc;
        static byte[] bufferCom;

        public static System.IO.Ports.SerialPort com1 = new SerialPort(Serial.COM1, 115200);
        public static OutputPort led = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.LED, true);

        public static void Main()
        {
            Debug.EnableGCMessages(false);
            Debug.Print("Starting up ...");            
            Debug.Print("OEM String: " + SystemInfo.OEMString);
            Debug.Print("Version: " + SystemInfo.Version.ToString());
            Debug.Print("Battery Voltage: " + (Battery.ReadVoltage() / 1000).ToString() + "." + (Battery.ReadVoltage() % 1000).ToString());            
            Debug.Print("Cpu SlowClock: " + Cpu.SlowClock.ToString());            
            Debug.Print("Cpu SystemClock: " + Cpu.SystemClock.ToString());

            com1.Open();
           
            

            detecteurChute = new DistanceMonitor();
            detecteurChute.Start();

            detecteurChoc = new ChocMonitor();
            detecteurChoc.Start();

            string msg = string.Empty;
            string lastmsg = string.Empty;  

            led.Write(false);
           
            while (true)
            {
                // Sleep for 500 milliseconds
                Thread.Sleep(20);
                msg = "{" +
                    ((detecteurChute.AvantGauche) ? "1" : "0") +
                    ((detecteurChute.AvantDroit) ? "1" : "0") +
                    ((detecteurChute.ArriereGauche) ? "1" : "0") +
                    ((detecteurChute.ArriereDroit) ? "1" : "0") +
                    ";"  +
                    ((detecteurChoc.AvantGauche) ? "1" : "0") +
                    ((detecteurChoc.AvantDroit) ? "1" : "0") +
                    ((detecteurChoc.ArriereGauche) ? "1" : "0") +
                    ((detecteurChoc.ArriereDroit) ? "1" : "0") +
                    "}";

                if (detecteurChute.AvantGauche ||
                    detecteurChute.AvantDroit ||
                    detecteurChute.ArriereGauche ||
                    detecteurChute.ArriereDroit
                    ||
                    detecteurChoc.AvantGauche ||
                    detecteurChoc.AvantDroit ||
                    detecteurChoc.ArriereGauche ||
                    detecteurChoc.ArriereDroit
                    )
                {
                    led.Write(true);
                }
                else
                {
                    led.Write(false);
                }

                if (msg != lastmsg)
                {
                  
                    WriteLine(msg);
                    
                    //bufferCom = Encoding.UTF8.GetBytes(msg + "\n");
                    //com1.Write(bufferCom, 0, bufferCom.Length);
                }
                lastmsg = msg;
            }
        }

        internal static void WriteLine(string message)
        {
            Debug.Print("UART : " + message);
            byte[] bufferCom = Encoding.UTF8.GetBytes(message+ "\n");
            com1.Write(bufferCom, 0, bufferCom.Length);
        }
    }
    
}
