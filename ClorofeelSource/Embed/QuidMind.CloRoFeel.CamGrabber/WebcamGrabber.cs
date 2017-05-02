using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;

namespace QuidMind.CloRoFeel.CamGrabber
{
    /*
     * use some programming concept from 
     * http://www.Planet-Source-Code.com/vb/scripts/ShowCode.asp?txtCodeId=1339&lngWId=10
     */


    class WebcamGrabber
    {

        IntPtr grabWindowHandle = IntPtr.Zero;

        public WebcamGrabber(IntPtr winHandle)
        {
            this.grabWindowHandle = winHandle;

            //videoService = new ClorofeelCloudServices.RobotVideoClient("CustomBinding_RobotVideo", QuidMind.CloRoFeel.CamGrabber.Properties.Settings.Default.robotVideoServiceUrl);
            //videoService.Open();
        }

        // property variables
       
        private int m_Width = 640;
        private int m_Height = 480;
        private int mCapHwnd;
        
        
        private IDataObject tempObj;
        private System.Drawing.Image tempImg;
        
        MemoryStream msGrabbedPicture = new MemoryStream();
        long msGrabbedPicturePosition = 0;

        ClorofeelCloudServices.RobotVideoClient videoService = null;

        #region API Declarations

        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("avicap32.dll", EntryPoint = "capCreateCaptureWindowA")]
        public static extern int capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int X, int Y, int nWidth, int nHeight, int hwndParent, int nID);

        [DllImport("user32", EntryPoint = "OpenClipboard")]
        public static extern int OpenClipboard(int hWnd);

        [DllImport("user32", EntryPoint = "EmptyClipboard")]
        public static extern int EmptyClipboard();

        [DllImport("user32", EntryPoint = "CloseClipboard")]
        public static extern int CloseClipboard();

        #endregion

        #region API Constants

        public const int WM_USER = 1024;

        public const int WM_CAP_CONNECT = 1034;
        public const int WM_CAP_DISCONNECT = 1035;
        public const int WM_CAP_GET_FRAME = 1084;
        public const int WM_CAP_COPY = 1054;

        public const int WM_CAP_START = WM_USER;

        public const int WM_CAP_DLG_VIDEOFORMAT = WM_CAP_START + 41;
        public const int WM_CAP_DLG_VIDEOSOURCE = WM_CAP_START + 42;
        public const int WM_CAP_DLG_VIDEODISPLAY = WM_CAP_START + 43;
        public const int WM_CAP_GET_VIDEOFORMAT = WM_CAP_START + 44;
        public const int WM_CAP_SET_VIDEOFORMAT = WM_CAP_START + 45;
        public const int WM_CAP_DLG_VIDEOCOMPRESSION = WM_CAP_START + 46;
        public const int WM_CAP_SET_PREVIEW = WM_CAP_START + 50;

        #endregion


        #region Control Properties

        public bool Busy { get; private set; }

        #endregion



        /// <summary>
        /// Starts the video capture
        /// </summary>
        public void Start()
        {
            try
            {
                // for safety, call stop, just in case we are already running
                this.Stop();

                //videoService = new ClorofeelCloudServices.RobotVideoClient("CustomBinding_RobotVideo", QuidMind.CloRoFeel.CamGrabber.Properties.Settings.Default.robotVideoServiceUrl);
                //videoService.Open();

                // setup a capture window
                mCapHwnd = capCreateCaptureWindowA("WebCap", 0, 0, 0, m_Width, m_Height, this.grabWindowHandle.ToInt32(), 0);

                // connect to the capture device
                Application.DoEvents();
                SendMessage(mCapHwnd, WM_CAP_CONNECT, 0, 0);
                SendMessage(mCapHwnd, WM_CAP_SET_PREVIEW, 0, 0);
               
            }

            catch (Exception excep)
            {
                MessageBox.Show("An error ocurred while starting the video capture. Check that your webcamera is connected properly and turned on.\r\n\n" + excep.ToString(),"Start()");
                //this.Stop();
                this.Start();
            }
        }

        /// <summary>
        /// Stops the video capture
        /// </summary>
        public void Stop()
        {
            try
            {
                //// stop the timer
                //bStopped = true;
                //this.timer1.Stop();

                // disconnect from the video source
                Application.DoEvents();
                SendMessage(mCapHwnd, WM_CAP_DISCONNECT, 0, 0);
            }
            catch (Exception excep)
            { // don't raise an error here.
                MessageBox.Show("Erreur : " + excep.ToString(), "Stop()");
            }
        }


        public string LastError
        {
            get;
            set;
        }
        
        public void GrabPicture()
        {
            Busy = true;
            try
            {
                if (videoService == null)
                {
                    videoService = new ClorofeelCloudServices.RobotVideoClient("CustomBinding_RobotVideo", QuidMind.CloRoFeel.CamGrabber.Properties.Settings.Default.robotVideoServiceUrl);
                    videoService.Endpoint.Address = new System.ServiceModel.EndpointAddress(Properties.Settings.Default.robotVideoServiceUrl);
                    videoService.Open();
                }
                // get the next frame;
                SendMessage(mCapHwnd, WM_CAP_GET_FRAME, 0, 0);

                // copy the frame to the clipboard
                SendMessage(mCapHwnd, WM_CAP_COPY, 0, 0);


                // get from the clipboard
                tempObj = Clipboard.GetDataObject();
                tempImg = (System.Drawing.Bitmap)tempObj.GetData(System.Windows.Forms.DataFormats.Bitmap);
                if (tempImg != null)
                {
                    msGrabbedPicture.Seek(0, SeekOrigin.Begin);
                    tempImg.Save(msGrabbedPicture, ImageFormat.Jpeg);

                    msGrabbedPicturePosition = msGrabbedPicture.Position;

                    string base64 = Convert.ToBase64String(msGrabbedPicture.GetBuffer(), 0, Convert.ToInt32(msGrabbedPicturePosition));
                    videoService.SendLastPicture("XX123456789XX", 0, base64);
                }
                LastError = "-";
                Application.DoEvents();
            }
            catch (Exception excep)
            {
                //MessageBox.Show("An error ocurred while capturing the video image. The video capture will now be terminated.\r\n\n" + excep.ToString(),"GrabPicture()");
                LastError = DateTime.Now.ToLongTimeString() + " " + excep.GetType().Name;
                //this.Stop(); // stop the process
                videoService.Close();
                videoService = null;
            }
            finally
            {
                Busy = false;
            }
        }

    }
}
