using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoBoIO_DotNet;
using System.Threading;
using QuidMind.CloRoFeel.RoboardService;

namespace QuidMind.CloRoFeel.RobotService
{
    class ServoMotorDriver
    {
        const int PwmPulseRightForwardMax = 1400;
        const int PwmPulseRightStop = 1510;
        const int PwmPulseRightBackward = 1720;

        const int PwmPulseLeftForwardMax = 1720;
        const int PwmPulseLeftStop = 1510;
        const int PwmPulseLeftBackward = 1400;

        Thread thServoSpeed = null;

        int currentLeftSpeed = 0;
        int currentRightSpeed = 0;

        IRoboardHardware _hardwareService;

        public ServoMotorDriver(IRoboardHardware hardwareService)
        {
            _hardwareService = hardwareService;
            thServoSpeed = new Thread(new ThreadStart(DoServoSpeedManagement));
            //InitRoboard();
            thServoSpeed.Start();
        }

        //void InitRoboard()
        //{
        //    Console.WriteLine("ServoMotorDriver: Initializing PWM port ...");
        //    uint motorChannels = (uint)(
        //        RoBoIO.RCSERVO_USECHANNEL0 +
        //        RoBoIO.RCSERVO_USECHANNEL1 +
        //        RoBoIO.RCSERVO_USECHANNEL2 +
        //        RoBoIO.RCSERVO_USECHANNEL3 +
        //        RoBoIO.RCSERVO_USECHANNEL5);

        //    RoBoIO.rcservo_SetServo(0, RoBoIO.RCSERVO_SERVO_DEFAULT_NOFB);
        //    RoBoIO.rcservo_SetServo(1, RoBoIO.RCSERVO_SERVO_DEFAULT_NOFB);
        //    RoBoIO.rcservo_SetServo(2, RoBoIO.RCSERVO_SERVO_DEFAULT_NOFB);
        //    RoBoIO.rcservo_SetServo(3, RoBoIO.RCSERVO_SERVO_DEFAULT_NOFB);
        //    RoBoIO.rcservo_SetServo(5, RoBoIO.RCSERVO_SERVO_DEFAULT_NOFB);

        //    if (!RoBoIO.rcservo_Initialize(motorChannels))
        //    {
        //        Console.WriteLine("ERREUR : rcservo_initialize : " + RoBoIO.roboio_GetErrMsg());
        //        return;
        //    }
        //    RoBoIO.rcservo_EnterPWMMode();
        //    Console.WriteLine("ServoMotorDriver: Servo Ready.");
        //}


        public void Terminate()
        {
            //RoBoIO.rcservo_Close();
        }


        void DoServoSpeedManagement()
        {
            uint pwmDuty, pwmPeriod, pwmCount;
            int lastLeftSpeed = currentLeftSpeed;
            int lastRightSpeed = currentRightSpeed;
            Console.WriteLine("ServoMotorDriver: Thread speed management started");
            while (true)
            {
                Thread.Sleep(200);
                try
                {


                    //Console.WriteLine("Thred Speed / Iteration.");
                    if (lastLeftSpeed == 0 && currentLeftSpeed == lastLeftSpeed &&
                        lastRightSpeed == 0 && currentRightSpeed == lastRightSpeed)
                        continue; // on etait a zero et on ne change pas de vitesse -> on ne touche pas au servo.
                    //Console.WriteLine("    Setting speed");
                    lastLeftSpeed = currentLeftSpeed;
                    lastRightSpeed = currentRightSpeed;

                    #region Right Speed Management
                    if (currentRightSpeed == 0) // on stop
                    {
                        pwmDuty = PwmPulseRightStop;
                        pwmPeriod = 20000 + pwmDuty;
                        pwmCount = 1;
                    }
                    else if (currentRightSpeed > 0) // on avance
                    {
                        pwmDuty = Convert.ToUInt32(
                            PwmPulseRightStop - (PwmPulseRightStop - PwmPulseRightForwardMax) * (currentRightSpeed / 10.0)
                            );
                        pwmPeriod = 20000 + pwmDuty;
                        pwmCount = 20;
                    }
                    else // on recule
                    {
                        pwmDuty = Convert.ToUInt32(
                            PwmPulseRightStop + (PwmPulseRightBackward - PwmPulseRightStop) * (-currentRightSpeed / 10.0)
                            );
                        pwmPeriod = 20000 + pwmDuty;
                        pwmCount = 20;
                    }

                    //RoBoIO.rcservo_SendPWMPulses(0, pwmPeriod, pwmDuty, pwmCount);
                    _hardwareService.SetServo(0, pwmDuty, pwmCount);
                    //RoBoIO.rcservo_SendPWMPulses(1, pwmPeriod, pwmDuty, pwmCount);
                    _hardwareService.SetServo(1, pwmDuty, pwmCount);
                    //Console.WriteLine("  right = " + pwmDuty);
                    #endregion

                    #region Left Speed Management
                    if (currentLeftSpeed == 0) // on stop
                    {
                        pwmDuty = PwmPulseLeftStop;
                        pwmPeriod = 20000 + pwmDuty;
                        pwmCount = 1;
                    }
                    else if (currentLeftSpeed > 0) // on avance
                    {
                        pwmDuty = Convert.ToUInt32(
                            PwmPulseLeftStop + (PwmPulseLeftForwardMax - PwmPulseLeftStop) * (currentLeftSpeed / 10.0)
                            );
                        pwmPeriod = 20000 + pwmDuty;
                        pwmCount = 10;
                    }
                    else // on recule
                    {
                        pwmDuty = Convert.ToUInt32(
                            PwmPulseLeftStop - (PwmPulseLeftStop - PwmPulseLeftBackward) * (-currentLeftSpeed / 10.0)
                            );
                        pwmPeriod = 20000 + pwmDuty;
                        pwmCount = 10;
                    }
                    //RoBoIO.rcservo_SendPWMPulses(2, pwmPeriod, pwmDuty, pwmCount);
                    _hardwareService.SetServo(2, pwmDuty, pwmCount);
                    //RoBoIO.rcservo_SendPWMPulses(3, pwmPeriod, pwmDuty, pwmCount);
                    _hardwareService.SetServo(3, pwmDuty, pwmCount);
                    //Console.WriteLine("  left = " + pwmDuty);
                    #endregion
                }
                
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR IN THREAD : " + ex.ToString());
                }
            }
        }


        public void SetSpeed(int left,int right)
        {
            currentRightSpeed = right;
            currentLeftSpeed = left;
        }

        public void SetServoPosition(int servoPort, uint pulseWidth,uint nbPulse)
        {
            //RoBoIO.rcservo_SendPWMPulses(servoPort, 20000+pulseWidth,pulseWidth, nbPulse);
            _hardwareService.SetServo(servoPort, pulseWidth, nbPulse);
        }


        public void SetServoDelta(int servoPort, int pulseWidth, uint nbPulse)
        {
            _hardwareService.SetServoDelta(servoPort, pulseWidth, nbPulse);
        }
    }
}
