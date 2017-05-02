using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace QuidMind.CloRoFeel.Service
{
    [DataContract(Namespace = "http://clorofeel.quidmind.com/DataModel/RobotResult")]
    public class RobotResult
    {
        [DataMember]
        public int Compass { get; set; }
        
        [DataMember]
        public string Message { get; set; }
        
        [DataMember]
        public bool StatusOk { get; set; }
        
        [DataMember]
        public DateTime TimeStamp { get; set; }
    }
}
