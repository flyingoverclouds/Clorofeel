using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuidMind.CloRoFeel.EmbeddedHelper;
using System.Diagnostics;
using System.Threading;

namespace QuidMind.CloRoFeel.BootLoader
{
    class Program
    {

        static void StartProcess(string processExePath,int timeToWaitAfterLaunch)
        {
            if (string.IsNullOrEmpty(processExePath))
                return;
            try
            {
                ConsoleHelper.WriteLine("Starting " + processExePath + " ...");
                new Process();
                var psi = new ProcessStartInfo(processExePath);
                Process proc = Process.Start(psi);
                ConsoleHelper.WriteLine("    wait " + timeToWaitAfterLaunch.ToString() + "ms");
                Thread.Sleep(timeToWaitAfterLaunch);
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorLine("     "+ ex.Message);
            }
        }
        
        static void Main(string[] args)
        {
            ConsoleHelper.WriteHeaderLine("Clorofeel BOOTLOADER v1.1201.29.2332                ");

            StartProcess(Properties.Settings.Default.Exe1_Path, Properties.Settings.Default.Exe1_StartWait);
            StartProcess(Properties.Settings.Default.Exe2_Path, Properties.Settings.Default.Exe2_StartWait);
            StartProcess(Properties.Settings.Default.Exe3_Path, Properties.Settings.Default.Exe3_StartWait);
            StartProcess(Properties.Settings.Default.Exe4_Path, Properties.Settings.Default.Exe4_StartWait);
            StartProcess(Properties.Settings.Default.Exe5_Path, Properties.Settings.Default.Exe5_StartWait);
            
            ConsoleHelper.WriteLine("Press [Enter] to terminate clorofeel robot operation.");
            Console.ReadLine();


        }
    }
}
