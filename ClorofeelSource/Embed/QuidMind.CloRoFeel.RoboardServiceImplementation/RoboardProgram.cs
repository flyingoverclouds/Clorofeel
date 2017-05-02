using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuidMind.CloRoFeel.EmbeddedHelper;
using System.ServiceModel;

namespace QuidMind.CloRoFeel.RoboardServiceImplementation
{
    class RoboardProgram
    {
        static ServiceHost _roboardHardwareServiceHost = null;

        static void Main(string[] args)
        {
            ConsoleHelper.WriteHeaderLine("Roboard Hardware service                         ");
            ConsoleHelper.WriteLine("-- exposing Roboard Hardware thru WCF --");
            ConsoleHelper.WriteLine("Exposing Services ...");

            _roboardHardwareServiceHost = new ServiceHost(typeof(QuidMind.CloRoFeel.RoboardService.RoboardHardwareService));
            _roboardHardwareServiceHost.Open();
            ConsoleHelper.WriteLine("Roboard hardware service exposed. Ready to work.");
            ConsoleHelper.WriteLine("");
            ConsoleHelper.WriteLine("Press [ENTER] to stop");
            Console.ReadLine();
            if (_roboardHardwareServiceHost.State != CommunicationState.Closed)
                _roboardHardwareServiceHost.Close();
        }
    }
}
