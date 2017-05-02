using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuidMind.CloRoFeel.CoreKernel
{
    public interface IRobotModule
    {
        void Initialize(InterModuleMessaging immContext, string initData);
        void Run();
        void Terminate();
        void SendData(InterModuleMessage imm);
        
    }
}
