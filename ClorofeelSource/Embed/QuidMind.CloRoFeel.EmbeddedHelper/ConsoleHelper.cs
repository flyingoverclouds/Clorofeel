using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuidMind.CloRoFeel.EmbeddedHelper
{
    public static class ConsoleHelper
    {
        public static void WriteHeaderLine(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
        }

        public static void WriteLine(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(msg);
        }

        public static void WriteErrorLine(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(msg);
        }

        public static void WriteWarningLine(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(msg);
        }

    }
}
