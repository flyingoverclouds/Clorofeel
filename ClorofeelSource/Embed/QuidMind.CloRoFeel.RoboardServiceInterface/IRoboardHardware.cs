using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace QuidMind.CloRoFeel.RoboardService
{

    [ServiceContract(
        Name = "IRoboardHardware",
        Namespace = "http://clorofeel.quidmind.com/ServiceModel/RoboardHardware/v1.0",
        SessionMode=SessionMode.Required)]
    public interface IRoboardHardware
    {
        [OperationContract(IsInitiating=true)]
        void Initialize();

        [OperationContract(IsOneWay=true)]
        void SetServo(int servoPortNum, uint pulseWidth, uint nbPulse);

        [OperationContract(IsOneWay = true)]
        void SetServoDelta(int servoPortNum, int deltaPulseWidth, uint nbPulse);

        [OperationContract]
        uint GetServoPulse(int servoPortNum);

        [OperationContract(IsTerminating=true)]
        void Terminate();
    }
}
