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

namespace QuidMind.CloRoFeel.CloRoFeelClientWP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            App.ViewModel.AppDispatcher = this.Dispatcher;
        }

        private void butOpenLicence_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/CGU.xaml", UriKind.Relative));
        }

        private void butRegisterWindowsPhone7_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Not Implemented");
            App.ViewModel.RegisterDevice();
        }

        private void butViewerMode_Click(object sender, RoutedEventArgs e)
        {
            //if (App.ViewModel.DeviceStatus == ViewModels.ClorofeelDeviceStatus.Viewer ||
            //    App.ViewModel.DeviceStatus == ViewModels.ClorofeelDeviceStatus.Controller)
            //{
            //    MessageBox.Show("VIEWER MODE");
            //}
            //else
            //    MessageBox.Show("Votre status ne permet pas d'accéder à ce mode.");
            NavigationService.Navigate(new Uri("/TestServices.xaml", UriKind.Relative));
        }

        private void butControllerMode_Click(object sender, RoutedEventArgs e)
        {
            //if (App.ViewModel.DeviceStatus == ViewModels.ClorofeelDeviceStatus.Controller)
            //{
            //    MessageBox.Show("CONTROLLER MODE");
            //}
            //else
            //    MessageBox.Show("Votre status ne permet pas d'accéder au mode de pilotage du Robot.");

            NavigationService.Navigate(new Uri("/DriverModePage.xaml", UriKind.Relative));
        }

     
    }
}