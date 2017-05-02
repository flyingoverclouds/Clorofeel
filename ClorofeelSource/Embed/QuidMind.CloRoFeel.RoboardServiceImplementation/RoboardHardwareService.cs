using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuidMind.CloRoFeel.RoboardService;
using QuidMind.CloRoFeel.EmbeddedHelper;
using System.ServiceModel;
using RoBoIO_DotNet;

namespace QuidMind.CloRoFeel.RoboardService
{

    [ServiceBehavior(
        ConcurrencyMode=ConcurrencyMode.Single,
        InstanceContextMode=InstanceContextMode.Single
        )]
    class RoboardHardwareService : IRoboardHardware
    {
        public bool SimulationMode { get; set; }
        public bool HardwareInitialized { get; set; }

        uint[] lastServoPulse = new uint[20];

        #region Roboard Hardware method
        void InitializeClorofeelPWM()
        {
            uint motorChannels = (uint)(
               RoBoIO.RCSERVO_USECHANNEL0 +
               RoBoIO.RCSERVO_USECHANNEL1 +
               RoBoIO.RCSERVO_USECHANNEL2 +
               RoBoIO.RCSERVO_USECHANNEL3 +
               RoBoIO.RCSERVO_USECHANNEL5 +
               RoBoIO.RCSERVO_USECHANNEL6);

            RoBoIO.rcservo_SetServo(0, RoBoIO.RCSERVO_SERVO_DEFAULT_NOFB);
            RoBoIO.rcservo_SetServo(1, RoBoIO.RCSERVO_SERVO_DEFAULT_NOFB);
            RoBoIO.rcservo_SetServo(2, RoBoIO.RCSERVO_SERVO_DEFAULT_NOFB);
            RoBoIO.rcservo_SetServo(3, RoBoIO.RCSERVO_SERVO_DEFAULT_NOFB);
            RoBoIO.rcservo_SetServo(5, RoBoIO.RCSERVO_SERVO_DEFAULT_NOFB);

            if (!RoBoIO.rcservo_Initialize(motorChannels))
            {
                ConsoleHelper.WriteErrorLine("rcservo_initialize : " + RoBoIO.roboio_GetErrMsg());
                SimulationMode = true;
                return;
            }
            RoBoIO.rcservo_EnterPWMMode();

            for (int n = 0; n < lastServoPulse.Length; n++)
                lastServoPulse[n] = 1500;
            Console.WriteLine("ServoMotorDriver initialized.");
        }
#endregion


        public RoboardHardwareService()
        {
            SimulationMode = false;
            HardwareInitialized = false;
            ConsoleHelper.WriteLine("** Service instanciated");
        }

        public void Initialize()
        {
            if (!HardwareInitialized)
            {
                InitializeClorofeelPWM();
                HardwareInitialized = true;
            }
            ConsoleHelper.WriteLine("Initialize()");
            if (SimulationMode)
                ConsoleHelper.WriteWarningLine("ROBOARD HARDWARE SIMULATION");
            else
                ConsoleHelper.WriteLine("Real Roboard Hardware");
        }

        public void SetServo(int servoPortNum, uint pulseWidth, uint nbPulse)
        {
            ConsoleHelper.WriteLine(string.Format("SetServo({0},{1},{2})",servoPortNum,pulseWidth,nbPulse));
            lastServoPulse[servoPortNum] = pulseWidth;
            if (SimulationMode) { ConsoleHelper.WriteWarningLine("   simulation mode."); return;  };

            RoBoIO.rcservo_SendPWMPulses(servoPortNum, 20000 + pulseWidth, pulseWidth, nbPulse);
        }

        public void Terminate()
        {
            ConsoleHelper.WriteLine("Terminate()");
        }


        public void SetServoDelta(int servoPortNum, int deltaPulseWidth, uint nbPulse)
        {
            ConsoleHelper.WriteLine(string.Format("SetServoDelta({0},{1},{2})", servoPortNum, deltaPulseWidth, nbPulse));
            lastServoPulse[servoPortNum] = (uint)(lastServoPulse[servoPortNum] + deltaPulseWidth);
            ConsoleHelper.WriteLine(string.Format("    final pulse = {0}", lastServoPulse[servoPortNum])); 
            if (SimulationMode) { ConsoleHelper.WriteWarningLine("   simulation mode."); return; };

            uint v;
            switch(servoPortNum)
            {
                case 5:
                    v = QuidMind.CloRoFeel.RoboardServiceImplementation.Properties
                        .Settings.Default.Servo5_MinPulse;
                    if (lastServoPulse[5] < v)
                        lastServoPulse[5] = v;
                    v = QuidMind.CloRoFeel.RoboardServiceImplementation.Properties
                        .Settings.Default.Servo5_MaxPulse;
                    if (lastServoPulse[5] > v)
                        lastServoPulse[5] = v;
                    break;
                case 6:
                    v = QuidMind.CloRoFeel.RoboardServiceImplementation.Properties
                        .Settings.Default.Servo6_MinPulse;
                    if (lastServoPulse[6] < v)
                        lastServoPulse[6] = v;
                    v = QuidMind.CloRoFeel.RoboardServiceImplementation.Properties
                        .Settings.Default.Servo6_MaxPulse;
                    if (lastServoPulse[6] > v)
                        lastServoPulse[6] = v;                    
                    break;
            }
            RoBoIO.rcservo_SendPWMPulses(servoPortNum, 20000 + lastServoPulse[servoPortNum], lastServoPulse[servoPortNum], nbPulse);
        }

        public uint GetServoPulse(int servoPortNum)
        {
            ConsoleHelper.WriteLine(string.Format("GetServoPulse({0})", servoPortNum));
            if (SimulationMode) { ConsoleHelper.WriteWarningLine("   simulation mode."); };
            return lastServoPulse[servoPortNum];
        }
    }
}
