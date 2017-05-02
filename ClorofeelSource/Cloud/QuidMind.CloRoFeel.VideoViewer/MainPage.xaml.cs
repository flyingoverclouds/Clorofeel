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
using System.Threading;
using System.IO;
using QuidMind.CloRoFeel.VideoViewer.ClorofeelCloudServices;

namespace QuidMind.CloRoFeel.VideoViewer
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        Timer tmrRefresh = null;
        ClorofeelCloudServices.RobotVideoClient robotService = null;
        bool isBusy = false;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            robotService = new ClorofeelCloudServices.RobotVideoClient();
            //robotService.GetLastPictureCompleted += new EventHandler<ClorofeelCloudServices.GetLastPictureCompletedEventArgs>(robotService_GetLastPictureCompleted);
            robotService.OpenCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(robotService_OpenCompleted);
            robotService.GetLastFaceRecognitionResultCompleted += new EventHandler<ClorofeelCloudServices.GetLastFaceRecognitionResultCompletedEventArgs>(robotService_GetLastFaceRecognitionResultCompleted);
            robotService.OpenAsync();
        }

        void robotService_OpenCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            txbStatus.Text = "Service channel opened.";
            tmrRefresh = new Timer(new TimerCallback(TimerTick),null,1000,510);
        }

        void TimerTick(object o)
        {
            if (isBusy)
                return;
            if (robotService != null)
            {
                isBusy = true;
                //robotService.GetLastPictureAsync();
                robotService.GetLastFaceRecognitionResultAsync();
            }
        }

        void robotService_GetLastFaceRecognitionResultCompleted(object sender, ClorofeelCloudServices.GetLastFaceRecognitionResultCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == false && e.Error == null)
                {
                    byte[] buffer = Convert.FromBase64String(e.Result.ImageBase64);
                    MemoryStream ms = new MemoryStream(buffer);

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
                            bi.SetSource(ms);
                            imgRobot.Source = bi;
                            displayFaceRecognitionResult.DataContext = e.Result;
                            DisplayFaceData(e.Result);
                        }
                        catch
                        { }
                    }));
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        txbStatus.Text = "Invalid GetLastFaceRecognitionResultCompleted" + e.Error.ToString();
                    }));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    txbStatus.Text = DateTime.Now.ToString() + " Erreur : " + ex.GetType().Name;
                }));
            }
            finally
            {
                isBusy = false;
            }
        }

        private void DisplayFaceData(ClorofeelCloudServices.FaceRecognitionResult faceRecognitionResult)
        {
            canDetectArea.Children.Clear();
            FacePosition targetedFace = null;
            foreach(var f in faceRecognitionResult.FacePositions)
            //for (int n = 0; n < faceRecognitionResult.FacePositions.Count; n++)
            {
                Rectangle r = new Rectangle();
                r.StrokeThickness = 3;
                if (f.TargetedFace)
                {
                    targetedFace = f;
                    r.Stroke = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    r.Stroke = new SolidColorBrush(Colors.White);
                }
                r.Fill = new SolidColorBrush(Color.FromArgb(80, 128, 128, 128));
                r.Width = f.Width;
                r.Height = f.Width;
                Canvas.SetLeft(r, f.X - f.Width / 2);
                Canvas.SetTop(r, f.Y - f.Width / 2);
                canDetectArea.Children.Add(r);
            }

            if (targetedFace != null)
            {
                double eyeCenterX = (targetedFace.Eye1.X + targetedFace.Eye2.X) / 2;
                double eyeCenterY = (targetedFace.Eye1.Y + targetedFace.Eye2.Y) / 2;
                
                #region Draw Delta Line
                Line li = new Line();
                li.Stroke = new SolidColorBrush(Colors.Blue);
                li.StrokeThickness = 3;
                li.X1 = eyeCenterX;
                li.Y1 = eyeCenterY;
                li.X2 = 640/2;
                li.Y2 = 480/2;
                canDetectArea.Children.Add(li);
                #endregion
            }
        }


        void robotService_GetLastPictureCompleted(object sender, ClorofeelCloudServices.GetLastPictureCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == false && e.Error == null)
                {
                    byte[] buffer = Convert.FromBase64String(e.Result.Message);
                    MemoryStream ms = new MemoryStream(buffer);

                    Dispatcher.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
                                bi.SetSource(ms);
                                imgRobot.Source = bi;
                            }
                            catch
                            { }
                        }));
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                        {
                            txbStatus.Text = "Invalid GetLastPictureCompleted";
                        }));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    txbStatus.Text = DateTime.Now.ToString() + " Erreur : " + ex.GetType().Name;
                }));
            }
            finally
            {
                isBusy = false;
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            tmrRefresh.Dispose();
            tmrRefresh = null;
            robotService.Abort();
            robotService.CloseAsync();
            robotService = null;
        }

      
    }
}
