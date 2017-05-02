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
using Microsoft.Phone.Controls;
using System.Diagnostics;

namespace QuidMind.CloRoFeel.CloRoFeelClientWP7
{
    public partial class DriverModePage : PhoneApplicationPage
    {
        public DriverModePage()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(DriverModePage_Loaded);
            Unloaded += new RoutedEventHandler(DriverModePage_Unloaded);
        }


        void DriverModePage_Loaded(object sender, RoutedEventArgs e)
        {
            Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);
        }

        void DriverModePage_Unloaded(object sender, RoutedEventArgs e)
        {
            Touch.FrameReported-=new TouchFrameEventHandler(Touch_FrameReported);
        }


        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            var filteredTouchPoints = e.GetTouchPoints(this).Where((tp,b) => tp.TouchDevice.DirectlyOver == bdrSpeedGauche || tp.TouchDevice.DirectlyOver == bdrSpeedDroite).ToList();
            if (filteredTouchPoints.Count == 0)
                return;

            foreach (var touchPoint in filteredTouchPoints)
            {
                Border bdr = touchPoint.TouchDevice.DirectlyOver as Border;
                Slider sld = bdr.Child as Slider;
                if (sld == null)
                    continue;
                double fourchette = sld.Maximum - sld.Minimum;
                double yInSld = touchPoint.Position.Y - bdr.Margin.Top;
                if (yInSld > bdr.ActualHeight)
                    yInSld = bdr.ActualHeight;
                double val = fourchette/2 - ( yInSld * fourchette / bdr.ActualHeight);
                //Debug.WriteLine("pur = " + val);
                if (val < sld.Minimum)
                    val = sld.Minimum;
                if (val > sld.Maximum)
                    val = sld.Maximum;
                if (val > -2 && val < 2)
                    val = 0;
                //Debug.WriteLine("min/max "  + val);
                sld.Value = val;
            }
            
        }



        private void bdrSpeedGauche_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            double fourchette = sldSpeedGauche.Maximum - sldSpeedGauche.Minimum;
            double ratio = e.ManipulationOrigin.Y * fourchette / (480 - sldSpeedGauche.Margin.Top - sldSpeedGauche.Margin.Bottom);
            double posSld = (fourchette/2) - ratio;

            if (posSld > sldSpeedGauche.Maximum)
                posSld = sldSpeedGauche.Maximum;
            if (posSld < sldSpeedGauche.Minimum)
                posSld = sldSpeedGauche.Minimum;
            sldSpeedGauche.Value = posSld;
            //Debug.WriteLine(e.ManipulationOrigin.Y.ToString() + "      " + posSld.ToString());
        }


        Proxy.CloroFeelService clorofeelService = new Proxy.CloroFeelService();
        int lastLeftSpeed=0;
        int lastRightSpeed = 0;
        private void sldSpeedGauche_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int speed = Convert.ToInt32(e.NewValue);
            if (speed == lastLeftSpeed)
                return;
            lastLeftSpeed=speed;
            clorofeelService.SetSpeedAsync(lastLeftSpeed, lastRightSpeed, (r) => { });
        }

        private void sldSpeedDroite_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int speed = Convert.ToInt32(e.NewValue);
            if (speed == lastRightSpeed)
                return;
            lastRightSpeed = speed;
            clorofeelService.SetSpeedAsync(lastLeftSpeed, lastRightSpeed, (r) => { });
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = App.ViewModel;
            App.ViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(ViewModel_PropertyChanged);
            App.ViewModel.StartRefreshPicture();
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Compass")
                rotationBousolle.Rotation = App.ViewModel.Compass;
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            App.ViewModel.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(ViewModel_PropertyChanged);
            App.ViewModel.StopRefreshPicture();
        }

        private void butOrientationCam_MouseUp(object sender, MouseButtonEventArgs e)
        {
            clorofeelService.SetCameraPositionAsync(true, 
                int.Parse((sender as Rectangle).Tag.ToString()), 
                1500,
                (r) => { });
        }

    
    
    }
}