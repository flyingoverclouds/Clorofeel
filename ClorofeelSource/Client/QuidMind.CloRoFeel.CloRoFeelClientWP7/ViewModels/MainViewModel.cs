using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.IO.IsolatedStorage;
using QuidMind.CloRoFeel.CloRoFeelClientWP7.ViewModels;
using System.Threading;
using System.Windows.Threading;
using QuidMind.CloRoFeel.CloRoFeelClientWP7.ClorofeelCloudServices;
using System.IO;


namespace QuidMind.CloRoFeel.CloRoFeelClientWP7
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        { }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            LoadPersistantContext();
            this.IsDataLoaded = true;

        }

        #region Persistance/Chargement des données local (IsolatedStorage) du ViewModel
        public void LoadPersistantContext()
        {
            //MessageBox.Show("Load context");
            var isf = IsolatedStorageFile.GetUserStoreForApplication();
            if (isf.FileExists("context.xml"))
            {
                //MessageBox.Show("Context file found !");
                IsolatedStorageFileStream isfs = new IsolatedStorageFileStream("context.xml", System.IO.FileMode.Open, IsolatedStorageFile.GetUserStoreForApplication());
                XElement context = XElement.Load(isfs);
                isfs.Close();

                XElement elem = null;

                elem = context.Element("CguAccepted");
                if (elem != null)
                    this.CguAccepted = bool.Parse(elem.Value);
            }
        }

        public void SavePersistantContext()
        {
            //MessageBox.Show("Save context");
            // creation du document XML de context
            XElement xecontext = new XElement("PersistantContext");
            xecontext.Add(new XElement("CguAccepted", CguAccepted));

            // sauvegarde du document XML de context dans l'isolated storage
            IsolatedStorageFileStream isfs = new IsolatedStorageFileStream("context.xml", System.IO.FileMode.Create, IsolatedStorageFile.GetUserStoreForApplication());
            xecontext.Save(isfs);
            isfs.Close();
        }



        #endregion

        #region public Local Properties (From/To IsolatedStorage)
#if DEBUG
        bool _cguAccepted = true;
#else
        bool _cguAccepted = false;
#endif
        
        /// <summary>
        /// True if user accept the CGU/EULA agreement.
        /// <remarks>PERSISTANT</remarks>
        /// </summary>
        public bool CguAccepted
        {
            get { return _cguAccepted; }
            set { _cguAccepted = value; NotifyPropertyChanged("CguAccepted"); }
        }


        #endregion

        #region public non local Properties

        public Dispatcher AppDispatcher { get; set; }

        ClorofeelDeviceStatus _currentStatus = ClorofeelDeviceStatus.NotRegistered;
        public ClorofeelDeviceStatus DeviceStatus
        {
            get { return _currentStatus; }
            set { _currentStatus = value; NotifyPropertyChanged("DeviceStatus"); }
        }
        #region secure
        string _clorofeelDeviceId = new Guid(1,2,3,4,5,6,7,8,9,10,11).ToString();
        public string ClorofeelDeviceId
        {
            get { return _clorofeelDeviceId; }
            set { _clorofeelDeviceId = value; NotifyPropertyChanged("ClorofeelDeviceId"); }
        }
        #endregion 

        BitmapImage _robotCameraPicture = null;
        public BitmapImage RobotCameraPicture
        {
            get { return _robotCameraPicture; }
            set { _robotCameraPicture = value; NotifyPropertyChanged("RobotCameraPicture"); }
        }

      

        double _compass;
        public double Compass
        {
            get { return _compass; }
            set { _compass = value; NotifyPropertyChanged("Compass"); }
        }

        #endregion

        #region Public method
        public void RegisterDevice()
        {
            //fake_RegisterDevice(ClorofeelDeviceId);
        }
        #endregion


        #region Windows Phone Device data
        //public static byte[] GetDeviceUniqueID()   
        //{   
        //    byte[] result = null;   
        //    object uniqueId;   
        //    if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))   
        //        result = (byte[])uniqueId;   
        //    return result;   
        //}   

        public string DeviceName
        {
            get
            {
                string result = string.Empty;
                object value;
                if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("DeviceName", out value))
                    result = value.ToString();
                return result;
            }
        }   


        #endregion

     

        #region Robot Video implémentation
        RobotVideoClient videoServices = null;
        DispatcherTimer dt = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 2000) };
        public void StartRefreshPicture()
        {
            videoServices = new RobotVideoClient("CustomBinding_RobotVideo", "http://clorofeel.cloudapp.net/robotvideo.svc");
            //videoServices = new RobotVideoClient("CustomBinding_RobotVideo", "http://127.0.0.1:81/robotvideo.svc");
            videoServices.GetLastPictureCompleted += new EventHandler<GetLastPictureCompletedEventArgs>(videoServices_GetLastPictureCompleted);
            videoServices.GetLastFaceRecognitionResultCompleted += new EventHandler<GetLastFaceRecognitionResultCompletedEventArgs>(videoServices_GetLastFaceRecognitionResultCompleted);
            videoServices.OpenCompleted += new EventHandler<AsyncCompletedEventArgs>(videoServices_OpenCompleted);
            videoServices.OpenAsync();
        }

        void videoServices_GetLastFaceRecognitionResultCompleted(object sender, GetLastFaceRecognitionResultCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == false && e.Error == null)
                {
                    byte[] buffer = Convert.FromBase64String(e.Result.ImageBase64);
                    if (buffer.Length > 10)   // le buffer contient des données
                    {
                        MemoryStream ms = new MemoryStream(buffer);

                        AppDispatcher.BeginInvoke(new Action(() =>
                        {
                            //this.Compass = e.Result.Compass * 1.0;
                            System.Windows.Media.Imaging.BitmapImage bi =
                                new System.Windows.Media.Imaging.BitmapImage();

                            // ICI : ajouter le code pour dessiner les rectangles sur les visages
                            
                            bi.SetSource(ms);
                            this.RobotCameraPicture = bi;
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                AppDispatcher.BeginInvoke(new Action(() =>
                {
                    MessageBox.Show("Error video complete " + ex.ToString());
                }));
            }
            finally
            {
                isBusy = false;
            }
        }

        void videoServices_OpenCompleted(object sender, AsyncCompletedEventArgs e)
        {
            dt.Tick += new EventHandler(dt_Tick);
            dt.Start();
        }

        bool isBusy = false;
        void dt_Tick(object sender, EventArgs e)
        {
            if (isBusy)
                return;
            isBusy = true;
            videoServices.GetLastPictureAsync();
        }

        void videoServices_GetLastPictureCompleted(object sender, GetLastPictureCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == false && e.Error == null)
                {
                    byte[] buffer = Convert.FromBase64String(e.Result.Message);
                    if (buffer.Length > 10)   // le buffer contient des données
                    {
                        MemoryStream ms = new MemoryStream(buffer);

                        AppDispatcher.BeginInvoke(new Action(() =>
                        {
                            this.Compass = e.Result.Compass * 1.0;
                            System.Windows.Media.Imaging.BitmapImage bi =
                                new System.Windows.Media.Imaging.BitmapImage();
                            bi.SetSource(ms);
                            this.RobotCameraPicture = bi;
                        }));
                    }
                }
                //else
                //{
                //    AppDispatcher.BeginInvoke(new Action(() =>
                //    {
                //        //txbStatus.Text = "Invalid GetLastPictureCompleted";
                //    }));
                //}
            }
            catch (Exception ex)
            {
                AppDispatcher.BeginInvoke(new Action(() =>
                {
                    MessageBox.Show("Error video complete " + ex.ToString());
                    //txbStatus.Text = DateTime.Now.ToString() + " Erreur : " + ex.GetType().Name;
                }));
            }
            finally
            {
                isBusy = false;
            }
        }


        public void StopRefreshPicture()
        {
            dt.Stop();
            dt.Tick -= new EventHandler(dt_Tick);
            videoServices.Abort();
            videoServices.CloseAsync();
        }

        #endregion

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

}
