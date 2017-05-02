using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace QuidMind.CloRoFeel.CamGrabber
{
    public partial class MainFormGrabber : Form
    {
        public MainFormGrabber()
        {
            InitializeComponent();
        }

        
        WebcamGrabber grabber = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            grabber = new WebcamGrabber(this.Handle);
            grabber.Start();
            tmrGrabPic.Interval = CamGrabber.Properties.Settings.Default.grabInterval;
            tmrGrabPic.Start();

            // settings the priority a bit under the normal priority
            Process camGrabberProcess = Process.GetCurrentProcess();
            camGrabberProcess.PriorityClass = ProcessPriorityClass.BelowNormal;

        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmrGrabPic.Stop();
            grabber.Stop();
        }

        private void tmrGrabPic_Tick(object sender, EventArgs e)
        {
            if (!grabber.Busy)
            {
                grabber.GrabPicture();
                lblLastError.Text = grabber.LastError;
            }
            lblLastGrab.Text = DateTime.Now.ToString();
        }

        
    }
}
