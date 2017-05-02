using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuidMind.CloRoFeel.CoreKernel
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RobotModuleAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
    }
}
