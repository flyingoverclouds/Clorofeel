using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Luxand;
using System.IO;
using System.Net;

namespace FaceDetectionTestLuxand
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void butActivateFaceCropLib_Click(object sender, RoutedEventArgs e)
        {
            if (FSDK.FSDKE_OK != FSDK.ActivateLibrary("6ECDDB0D383705026222D0F2C152F73183D0075260FCC30E561C62C11491C6B952FE8256C6CF615760A7D36113E27B2241E95FD832487A1AB04B7B1BACC9BEC5"))
            {
                MessageBox.Show("Run the License Key Wizard", "Error activating FaceSDK",MessageBoxButton.OK);
                return;
            }

            if (FSDK.InitializeLibrary() != FSDK.FSDKE_OK)
                txbApiActivation.Text = "Error initializing FaceSDK!";

        }

        private System.Windows.Media.Imaging.BitmapSource GetBitmapSource(System.Drawing.Bitmap _image) 
        { 
            System.Drawing.Bitmap bitmap = _image; 
            System.Windows.Media.Imaging.BitmapSource bitmapSource = 
                System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(), 
                    IntPtr.Zero, 
                    Int32Rect.Empty, 
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions()); 
            return bitmapSource; 
        }

        string tempFileName = null;

        private void butGetPictures_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClorofeelCloudServices.RobotVideoClient rvc = new ClorofeelCloudServices.RobotVideoClient("CustomBinding_RobotVideo", Properties.Settings.Default.ClorofeelCloudServicesUri.ToString());

                var r = rvc.GetLastPicture();
                if (string.IsNullOrEmpty(r.Message))
                {
                    txbResultGetPicture.Text = "No picture available.";
                    return;
                }

                byte[] buffer = Convert.FromBase64String(r.Message);
                MemoryStream ms = new MemoryStream(buffer);

                imgToAnalyse.Source = null;
                //System.Drawing.Bitmap.FromStream(ms);
                //if (tempFileName != null)
                //    File.Delete(tempFileName);
                tempFileName = System.IO.Path.GetTempFileName() + ".jpg";
                FileStream picFile = File.Create(tempFileName);
                ms.WriteTo(picFile);
                picFile.Close();
                ms.Close();
                picFile.Dispose();
                ms.Dispose();

                canDetectArea.Children.Clear();
                imgToAnalyse.Source = new BitmapImage(new Uri(tempFileName));
            }
            catch (Exception ex)
            {
                txbResultGetPicture.Text = "Erreur : " + ex.GetType().Name;
                return;
            }

        }

        public static float FaceDetectionThreshold = 3;
        public static float FARValue = 100;

        string GetFsdkErrorMessage(int fsdkError)
        {
            if (fsdkError == FSDK.FSDK_FACIAL_FEATURE_COUNT)
            { return "Facial Feature Cound";}
            else if (fsdkError == FSDK.FSDKE_BAD_FILE_FORMAT)
            { return "Bad file format";}
            else if (fsdkError == FSDK.FSDKE_CANNOT_CREATE_FILE)
            { return "Cannot create file";}
            else if (fsdkError == FSDK.FSDKE_CANNOT_OPEN_FILE)
            {return "Cannot open file"; }
            else if (fsdkError == FSDK.FSDKE_FACE_NOT_FOUND)
            { return "Face not found";}
            else if (fsdkError == FSDK.FSDKE_FAILED)
            { return "Failed";}
            else if (fsdkError == FSDK.FSDKE_FILE_NOT_FOUND)
            { return "File not found";}
            else if (fsdkError == FSDK.FSDKE_IMAGE_TOO_SMALL)
            { return "Image too small";}
            else if (fsdkError == FSDK.FSDKE_INSUFFICIENT_BUFFER_SIZE)
            { return "Insufficient buffer size";}
            else if (fsdkError == FSDK.FSDKE_INVALID_ARGUMENT)
            { return "Invalid argument";}
            else if (fsdkError == FSDK.FSDKE_IO_ERROR)
            { return "IO Error";}
            else if (fsdkError == FSDK.FSDKE_NOT_ACTIVATED)
            { return "Not activated";}
            else if (fsdkError == FSDK.FSDKE_OK)
            {return "Ok (no error)"; }
            else if (fsdkError == FSDK.FSDKE_OUT_OF_MEMORY)
            {return "Out of memory"; }
            else if (fsdkError == FSDK.FSDKE_UNSUPPORTED_IMAGE_EXTENSION)
            { return "Unsupported Image extension";}
            else
            { 
                return "Unknow error code : " + fsdkError.ToString();
            }
        }

        private void butFaceDetection_Click(object sender, RoutedEventArgs e)
        {
            FSDK.SetFaceDetectionParameters(false, true, 384);
            FSDK.SetFaceDetectionThreshold((int)FaceDetectionThreshold);

            FaceRecord fr = new FaceRecord();
           
            // chargement de l'image
            Int32 hImg=0;
            if (FSDK.LoadImageFromFile(ref hImg, tempFileName) != FSDK.FSDKE_OK)
            {
                txbFaceDetection.Text = "Erreur LoadImagefromFile";
                return;
            }

            // recherche des visages
            FSDK.TFacePosition[] facePositions;
            int nbFaces = 0;
            int fsdkRes = FSDK.DetectMultipleFaces(hImg,ref nbFaces, out facePositions, 20000);
            if (fsdkRes != FSDK.FSDKE_OK)
            {
                txbNbFaces.Text = "0";
                txbFaceDetection.Text = GetFsdkErrorMessage(fsdkRes);
                return;
            }

            #region Draw detected Faces
            canDetectArea.Children.Clear();
            for (int n = 0; n < nbFaces; n++)
            {
                Rectangle r = new Rectangle();
                r.StrokeThickness = 3;
                r.Stroke = Brushes.White;
                r.Fill = new SolidColorBrush(Color.FromArgb(80, 128, 128, 128));
                r.Width = facePositions[n].w;
                r.Height= facePositions[n].w;
                Canvas.SetLeft(r, facePositions[n].xc - facePositions[n].w/2);
                Canvas.SetTop(r, facePositions[n].yc - facePositions[n].w/2);
                canDetectArea.Children.Add(r);
            }
            #endregion


            // recherche des yeux et du visage le plus proche
            txbNbFaces.Text = nbFaces.ToString();    
            int targetedFace = -1;
            FSDK.TPoint[] targetedEyes=null;
            double targetedEyeDistance = -1;
            for (int n = 0; n < nbFaces;n++ )
            {
                FSDK.TPoint[] eyePositions = new FSDK.TPoint[2];
                fsdkRes = FSDK.DetectEyesInRegion(hImg, ref facePositions[n], out eyePositions);
                if (fsdkRes != FSDK.FSDKE_OK)
                {
                    txbFaceDetection.Text = GetFsdkErrorMessage(fsdkRes);
                    continue;
                }

                #region Draw Eye
                Ellipse eye = new Ellipse();
                eye.Stroke = Brushes.OrangeRed; eye.StrokeThickness = 2;
                eye.Width = 10;
                eye.Height = 10;
                Canvas.SetLeft(eye, eyePositions[0].x-5);
                Canvas.SetTop(eye, eyePositions[0].y-5);
                canDetectArea.Children.Add(eye);

                eye = new Ellipse();
                eye.Stroke = Brushes.OrangeRed; eye.StrokeThickness = 2; 
                eye.Width = 10;
                eye.Height = 10;
                Canvas.SetLeft(eye, eyePositions[1].x-5);
                Canvas.SetTop(eye, eyePositions[1].y-5);
                canDetectArea.Children.Add(eye);


                #endregion

                if (targetedFace == -1) // on preselectionne la 1ere paire d'yeux
                {
                    targetedFace = n;
                    targetedEyes = eyePositions;
                    targetedEyeDistance =
                        Math.Sqrt((targetedEyes[1].x - targetedEyes[0].x) ^ 2 + (targetedEyes[1].y - targetedEyes[0].y) ^ 2);
                }
                else // si d'autres visage, on calcule la distance entre les 2 yeux. 
                {
                    double newDistance = Math.Sqrt((eyePositions[1].x - eyePositions[0].x) ^ 2 + (eyePositions[1].y - eyePositions[0].y) ^ 2);
                    if (newDistance > targetedEyeDistance)
                    {
                        targetedFace = n;
                        targetedEyes = eyePositions;
                        targetedEyeDistance = newDistance;
                    }
                }
                txbTargetedFace.Text = targetedFace.ToString();
            }
            #region Draw targeted face
            Rectangle sr = new Rectangle();
            sr.StrokeThickness = 4;
            sr.Stroke = Brushes.Green;
            sr.Fill = new SolidColorBrush(Color.FromArgb(80, 0, 128, 0));
            sr.Width = facePositions[targetedFace].w;
            sr.Height = facePositions[targetedFace].w;
            Canvas.SetLeft(sr, facePositions[targetedFace].xc - facePositions[targetedFace].w / 2);
            Canvas.SetTop(sr, facePositions[targetedFace].yc - facePositions[targetedFace].w / 2);
            canDetectArea.Children.Add(sr);
            #endregion

            // Calcul du delta par rapport au centre de l'image 
            var bi = imgToAnalyse.Source as BitmapImage;
            double imgCenterX = bi.Width / 2;
            double imgCenterY = bi.Height / 2;

            double eyeCenterX = (targetedEyes[0].x + targetedEyes[1].x) / 2;
            double eyeCenterY = (targetedEyes[0].y + targetedEyes[1].y) / 2;

            #region Draw Delta Line
            Line li = new Line();
            li.Stroke = Brushes.DodgerBlue;
            li.StrokeThickness = 3;
            li.X1 = eyeCenterX;
            li.Y1 = eyeCenterY;
            li.X2 = imgCenterX;
            li.Y2 = imgCenterY;
            canDetectArea.Children.Add(li);
            #endregion

            // calcul du delta de déplacement de la tete pour recentrer le visage sélectionné
            const int leftExtreme = 1800;
            const int rightExtreme = 1200;
            const int bottomExtreme = 1725;
            const int upExtreme = 1275;

            double deltaPixelX = imgCenterX - eyeCenterX;
            double deltaPixelY = imgCenterY - eyeCenterY;

            int deltaServoX = (int)((leftExtreme - rightExtreme) * deltaPixelX / bi.Width);
            int deltaServoY = -(int)((bottomExtreme - upExtreme) * deltaPixelY / bi.Height);

            txbDeltaServo.Text = string.Format(" {0} , {1}", deltaServoX, deltaServoY);
            string url =
                string.Format("http://clorofeel.servicebus.windows.net/robotremote/RobotBoard/SetCameraPosition?deviceId=12345&camIsActive=true&orientationHorizontale={0}&orientationVerticale={1}",
                deltaServoX,deltaServoY
                );
            var wreq = HttpWebRequest.Create(url);
            wreq.BeginGetResponse((ar) =>{}, null);

        }

        private void butGetAndDetectFace_Click(object sender, RoutedEventArgs e)
        {
            butGetPictures_Click(sender, e);
            butFaceDetection_Click(sender, e);
        }

        #region test speed moteur
        
        void SetSpeed(int gauche,int droite)
        {
           string url =
               string.Format("http://clorofeel.servicebus.windows.net/robotremote/RobotBoard//SetSpeed?deviceId=5BE9FF477FA04B06A846AFA3FFFFFFFF&leftSpeed={0}&rightSpeed={1}&tick={2}",
               gauche,droite,DateTime.Now.Ticks);
            var wreq = HttpWebRequest.Create(url);
            wreq.BeginGetResponse((ar)=>{},null);
        }

        private void butGaucheAV_Click(object sender, RoutedEventArgs e)
        {
            SetSpeed(5, 0);
        }

        private void butGaucheSTOP_Click(object sender, RoutedEventArgs e)
        {
            SetSpeed(0, 0);
        }

        private void butGaucheREC_Click(object sender, RoutedEventArgs e)
        {
            SetSpeed(-5, 0);
        }

        private void butAvance_Click(object sender, RoutedEventArgs e)
        {
            SetSpeed(5, 5);
        }

        private void butStop_Click(object sender, RoutedEventArgs e)
        {
            SetSpeed(0, 0);
        }

        private void butRecule_Click(object sender, RoutedEventArgs e)
        {
            SetSpeed(-5, -5);
        }

        private void butDroiteAV_Click(object sender, RoutedEventArgs e)
        {
            SetSpeed(0, 5);
        }

        private void butDroiteSTOP_Click(object sender, RoutedEventArgs e)
        {
            SetSpeed(0,0);
        }

        private void butDroiteREC_Click(object sender, RoutedEventArgs e)
        {
            SetSpeed(0, -5);
        }
        #endregion

        List<byte[]> lstKnownTemplate = new List<byte[]>();

        private void butAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            Int32 hImg = 0;
            if (FSDK.LoadImageFromFile(ref hImg, tempFileName) != FSDK.FSDKE_OK)
            {
                txbFaceDetection.Text = "Erreur LoadImagefromFile";
                return;
            }
            FSDK.TFacePosition facePosition = new FSDK.TFacePosition();
            if (FSDK.FSDKE_OK == FSDK.DetectFace(hImg, ref facePosition))
            {
                byte[] templateData = new byte[FSDK.TemplateSize];

                if (FSDK.FSDKE_OK == FSDK.GetFaceTemplateInRegion(hImg, ref facePosition, out templateData))
                {
                    lstKnownTemplate.Add(templateData);
                    txbSearchForFace.Text = "Face template stored";
                }
            }
            else
            {
                txbSearchForFace.Text = "No face detected";
            }
        }

        private void butSearchTemplate_Click(object sender, RoutedEventArgs e)
        {
            Int32 hImg = 0;
            if (FSDK.LoadImageFromFile(ref hImg, tempFileName) != FSDK.FSDKE_OK)
            {
                txbFaceDetection.Text = "Erreur LoadImagefromFile";
                return;
            }
            FSDK.TFacePosition facePosition = new FSDK.TFacePosition();
            if (FSDK.FSDKE_OK == FSDK.DetectFace(hImg, ref facePosition))
            {
                byte[] searchTemplateData = new byte[FSDK.TemplateSize];
                bool match = false;
                if (FSDK.FSDKE_OK == FSDK.GetFaceTemplateInRegion(hImg, ref facePosition, out searchTemplateData))
                {
                    foreach (byte[] storedFaceTemplate in lstKnownTemplate)
                    {
                        float similarity = 0.0f;
                        byte[] sft = storedFaceTemplate;
                        FSDK.MatchFaces(ref searchTemplateData, ref sft, ref similarity);
                        float threshold = 0.0f;
                        FSDK.GetMatchingThresholdAtFAR(0.01f, ref threshold); // set FAR to 1%
                        if (similarity > threshold)
                        {
                            match = true;
                            break;
                        }
                    }
                    if (match)
                    {
                        txbSearchForFace.Text = "KNOWN FACE DETECTED !!!";
                    }
                    else
                        txbSearchForFace.Text = "No face match";
                }
            }
            else
            {
                txbSearchForFace.Text = "No face detected";
            }
        }

    }
}
