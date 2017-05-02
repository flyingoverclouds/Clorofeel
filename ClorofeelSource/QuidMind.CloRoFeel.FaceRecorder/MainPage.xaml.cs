using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using FluxJpeg;
using FluxJpeg.Core.Encoder;
using System.IO;

namespace QuidMind.CloRoFeel.FaceRecorder
{
    public partial class MainPage : UserControl
    {
        CaptureSource cs = new CaptureSource();

        RobotVideoService.RobotVideoClient rvc = new RobotVideoService.RobotVideoClient();

        public MainPage()
        {
            InitializeComponent();
            cs.CaptureImageCompleted += new EventHandler<CaptureImageCompletedEventArgs>(cs_CaptureImageCompleted);
            rvc.AddFaceToDatabaseCompleted += new EventHandler<RobotVideoService.AddFaceToDatabaseCompletedEventArgs>(rvc_AddFaceToDatabaseCompleted);
        }

        private void butStartCam_Click(object sender, RoutedEventArgs e)
        {
            if (CaptureDeviceConfiguration.RequestDeviceAccess())
            {
                VideoBrush vb = new VideoBrush();
                
                cs.VideoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();
                vb.SetSource(cs);
                cs.Start();
                butStartCam.IsEnabled = false;
                borderVideo.Visibility = Visibility.Visible;
                recVideo.Fill = vb;
                
            }
        }

        private void butAddToClorofeelDatabase_Click(object sender, RoutedEventArgs e)
        {
            cs.CaptureImageAsync();
        }

        void cs_CaptureImageCompleted(object sender, CaptureImageCompletedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("Veuillez indiquez votre identité");
                return;
            }

            MemoryStream ms = new MemoryStream();
            e.Result.EncodeJpeg(ms);

            string base64 = Convert.ToBase64String(ms.GetBuffer());

            rvc.AddFaceToDatabaseAsync(txtName.Text.Trim(), base64);
        }

        void rvc_AddFaceToDatabaseCompleted(object sender, RobotVideoService.AddFaceToDatabaseCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
            {
                MessageBox.Show("Erreur lors de l'appel au service");
                return;
            }
            MessageBox.Show(e.Result);
        }
    }
}
