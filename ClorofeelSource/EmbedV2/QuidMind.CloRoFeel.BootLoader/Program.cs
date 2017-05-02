using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuidMind.CloRoFeel.CoreKernel;

namespace QuidMind.CloRoFeel.BootLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("************* CloRoFeel Bootloader V2 ***************");

            ModuleLoader loader = new ModuleLoader();
            loader.LoadModules();

            
            Console.WriteLine("**");
            Console.WriteLine("** PRESS ENTER TO TERMINATE **");
            Console.WriteLine("**");
            Console.ReadLine();
            loader.TerminateAllModules();
            
        }
    }
}
