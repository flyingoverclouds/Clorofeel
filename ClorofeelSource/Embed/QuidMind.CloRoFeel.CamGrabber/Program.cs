using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QuidMind.CloRoFeel.CamGrabber
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFormGrabber());

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show("EXCEPTION NON GERE \r\n" + e.Exception.ToString());
        }
    }
}
