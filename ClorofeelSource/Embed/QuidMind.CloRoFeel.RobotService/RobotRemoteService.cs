using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuidMind.CloRoFeel.Service;
using System.ServiceModel;
using QuidMind.CloRoFeel.RoboardService;
using QuidMind.CloRoFeel.EmbeddedHelper;

namespace QuidMind.CloRoFeel.RobotService
{
    [ServiceBehavior(Name="RobotRemoteService",
         Namespace = "http://clorofeel.quidmind.com/ServiceModel/RobotRemoteService",
         InstanceContextMode=InstanceContextMode.Single,
         ConcurrencyMode=ConcurrencyMode.Multiple
         )]
    public class RobotRemoteService : IRobotRemote
    {
        #region HardwareService connection
        IRoboardHardware _hardwareService = null;

        #endregion


        static ServoMotorDriver motorDriver = null;

        #region SecurityCheck
        /// <summary>
        /// Check if the phoneId is allowed to Admin the bot.
        /// Admin means : add/revoke phoneId, disconnect, reboot, update the bot
        /// </summary>
        /// <param name="phoneId"></param>
        /// <returns></returns>
        bool CheckPhoneIdForAdmin(string phoneId)
        {
            return phoneId == SecureData.AdminPhoneId || phoneId=="<<NOT SET>>";
        }


        /// <summary>
        ///  Check if the phone Id is allowed to drive the bot.
        /// </summary>
        /// <param name="phoneId"></param>
        /// <returns></returns>
        bool CheckPhoneIdForDriving(string phoneId)
        {
            return CheckPhoneIdForAdmin(phoneId);
        }

        /// <summary>
        /// Check if the phoneId is allowed to view data from the bot (picture, compass, sensor)
        /// </summary>
        /// <param name="phoneId"></param>
        /// <returns></returns>
        bool CheckPhoneIdForViewing(string phoneId)
        {
            return CheckPhoneIdForAdmin(phoneId);
        }

        
        
        #endregion

        int compassPosition=0;

        static object lockObject = new object();
        private int GetCompass()
        {
            
            lock (lockObject)
            {
                compassPosition += 10;
                if (compassPosition >= 380)
                    compassPosition = 0;
            }
            return compassPosition;
        }

        RobotResult GetResult()
        {
            var rr = new RobotResult();
            rr.Compass = GetCompass();
            rr.Message = Guid.NewGuid().ToString();
            rr.StatusOk = true;
            rr.TimeStamp = DateTime.Now.ToUniversalTime();
            return rr;
        }

        public RobotRemoteService()
        {
            try
            {
                Console.WriteLine(".ctor");
                _hardwareService = ChannelFactory<IRoboardHardware>.CreateChannel(new NetTcpBinding(), new EndpointAddress(Properties.Settings.Default.RoboardHardwareServiceUri));
                _hardwareService.Initialize();

                motorDriver = new ServoMotorDriver(_hardwareService);
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorLine("RobotRemoteService.ctor : EXCEPTION : " + ex.ToString());
            }
        }

        public bool RegisterDevice(string deviceId)
        {
            Console.WriteLine(string.Format("{1} [RegisterDevice] : DeviceId=HIDDENB",deviceId,DateTime.Now.ToLongTimeString()));
            return true;
        }

      
        public string GetRobotVersion(string deviceId)
        {
            Console.WriteLine(string.Format("[GetRobotVersion] : DeviceId=HIDDEN", deviceId));
            return "CloRoFeel;InternalBuild;v0.1103.22.0";
        }

        public bool IsAuthorized(string phoneId)
        {
            Console.WriteLine(string.Format("[IsAuthorized] : DeviceId=HIDDEN", phoneId));
            return CheckPhoneIdForAdmin(phoneId); ;
        }


        public RobotResult SetSpeed(string phoneId, int leftSpeed, int rightSpeed)
        {
            try { 
                Console.WriteLine(string.Format("[SetSpeed] : DeviceId=HIDDEN   left={1}    right={2}", phoneId, leftSpeed, rightSpeed));
                if (!CheckPhoneIdForAdmin(phoneId))
                {
                    Console.WriteLine("   -> NOT AUTHORIZED");
                    return null;
                }

                motorDriver.SetSpeed(leftSpeed,rightSpeed);
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorLine("RobotRemoteService.SetSpeed : EXCEPTION : " + ex.ToString());
            }

            return GetResult();
        }

        public RobotResult SetCameraAbsolutePosition(string phoneId, bool camIsActive, uint orientationHorizontale, uint orientationVerticale)
        {
            try
            {
                Console.WriteLine(string.Format("[SetCameraPosition] : DeviceId=HIDDEN   active={1}    horizontal={2} vertical={3}",
                    phoneId, camIsActive, orientationHorizontale, orientationVerticale));
                //if (!CheckPhoneIdForAdmin(phoneId))
                //{
                //    Console.WriteLine("   -> NOT AUTHORIZED");
                //    return null;
                //}
                //motorDriver.SetServoPosition(5, (uint)orientationHorizontale, 25);

                if (orientationHorizontale < -Properties.Settings.Default.seuilDeltaHorizontal || orientationHorizontale > Properties.Settings.Default.seuilDeltaHorizontal)
                    motorDriver.SetServoPosition(5, orientationHorizontale, 30);
                else
                    Console.WriteLine("   seuil horizontal non atteint");
                if (orientationVerticale < -Properties.Settings.Default.seuilDeltaHorizontal || orientationVerticale > Properties.Settings.Default.seuilDeltaVertical)
                    motorDriver.SetServoPosition(6, orientationVerticale, 30);
                else
                    Console.WriteLine("   seuil vertical non atteint");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorLine("RobotRemoteService.SetCameraAbsolutePosition : EXCEPTION : " + ex.ToString());
            }

            return GetResult();
        }



        public RobotResult SetCameraPosition(string phoneId, bool camIsActive, int orientationHorizontale, int orientationVerticale)
        {
            try
            {
                Console.WriteLine(string.Format("[SetCameraPosition] : DeviceId=HIDDEN   active={1}    horizontal={2} vertical={3}",
                    phoneId, camIsActive, orientationHorizontale, orientationVerticale));


                if (orientationHorizontale < -Properties.Settings.Default.seuilDeltaHorizontal || orientationHorizontale > Properties.Settings.Default.seuilDeltaHorizontal)
                    motorDriver.SetServoDelta(5, orientationHorizontale, 30);
                else
                    Console.WriteLine("   seuil horizontal non atteint");
                if (orientationVerticale < -Properties.Settings.Default.seuilDeltaHorizontal || orientationVerticale > Properties.Settings.Default.seuilDeltaVertical)
                    motorDriver.SetServoDelta(6, orientationVerticale, 30);
                else
                    Console.WriteLine("   seuil vertical non atteint");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorLine("RobotRemoteService.SetCameraAbsolutePosition : EXCEPTION : " + ex.ToString());
            }
            return GetResult();
        }

       
    }
}
