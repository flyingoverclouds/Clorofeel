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
using System.ServiceModel;
using QuidMind.CloRoFeel.RoboardService;

namespace QuidMind.CloRoFeel.RoboardServiceImplementation.Test
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

        IRoboardHardware _hardwareService = null;

        private void butInitialize_Click(object sender, RoutedEventArgs e)
        {
            _hardwareService = ChannelFactory<IRoboardHardware>.CreateChannel(new NetTcpBinding(), new EndpointAddress(Properties.Settings.Default.RoboardHardwareServiceUri));
            _hardwareService.Initialize();
        }




        private void butTerminate_Click(object sender, RoutedEventArgs e)
        {
            if (_hardwareService == null)
                return;
            _hardwareService.Terminate();
        }

        private void butSetServo_Click(object sender, RoutedEventArgs e)
        {
            _hardwareService.SetServo(
                Convert.ToInt32(cbxServoNum.SelectedItem),
                Convert.ToUInt32(txtServoFreq.Text),
                Convert.ToUInt32(txtServoRepeat.Text)
                );
        }

        private void butCamRepositionne_Click(object sender, RoutedEventArgs e)
        {
            string value = (sender as Button).Tag as string;
            if (value == null)
                return;

            var tabVal = value.Split('-');
            if (tabVal.Length != 2)
                return;
            _hardwareService.SetServo(5, Convert.ToUInt32(tabVal[0]), 30);
            _hardwareService.SetServo(6, Convert.ToUInt32(tabVal[1]), 30);
        }

        private void butDeltaDec_Click(object sender, RoutedEventArgs e)
        {
            _hardwareService.SetServoDelta(
               Convert.ToInt32(cbxServoNum.SelectedItem),
               -10,
               Convert.ToUInt32(txtServoRepeat.Text)
               );
        }

        private void butDeltaInc_Click(object sender, RoutedEventArgs e)
        {
            _hardwareService.SetServoDelta(
               Convert.ToInt32(cbxServoNum.SelectedItem),
               10,
               Convert.ToUInt32(txtServoRepeat.Text)
               );
        }

        private void butGaucheAV_Click(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
