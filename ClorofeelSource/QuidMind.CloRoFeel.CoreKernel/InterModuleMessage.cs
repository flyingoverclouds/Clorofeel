using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuidMind.CloRoFeel.CoreKernel
{
    [Serializable]
    public class InterModuleMessage
    {
        public InterModuleMessage()
        {
            this.SourceInstance = -1;
            this.SourceModule = string.Empty;
            this.TargetInstance = -1;
            this.TargetModule = string.Empty;
            this.Body = string.Empty;
        }
        public string SourceModule { get; set; }
        public int SourceInstance { get; set; }

        public string TargetModule { get; set; }
        public int TargetInstance { get; set; }

        public string Body { get; set; }

        public override string ToString()
        {
            return string.Format("IMM ({0}[{1}]->{2}[{3}])=[{4}]",
                SourceModule, SourceInstance, TargetModule, TargetInstance, Body);
        }
    }
}
