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
    public partial class TestServices : PhoneApplicationPage
    {
        public TestServices()
        {
            InitializeComponent();
        }

        private void butRegisterDevice_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = string.Empty;
            Proxy.CloroFeelService svc = new Proxy.CloroFeelService();
            svc.RegisterDeviceAsync((registered) => { txtResult.Text=registered.ToString(); });
        }

        private void butGetRobotVersion_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = string.Empty;
            Proxy.CloroFeelService svc = new Proxy.CloroFeelService();
            svc.GetRobotVersionAsync((version) => { txtResult.Text=version; });
        }

        private void butIsAuthorized_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = string.Empty;
            Proxy.CloroFeelService svc = new Proxy.CloroFeelService();
            svc.IsAuthorizedAsync((authorized) => { txtResult.Text=authorized.ToString(); });
        }

        private void butSetSpeed_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = string.Empty;
            Proxy.CloroFeelService svc = new Proxy.CloroFeelService();
            svc.SetSpeedAsync(
                int.Parse(txtSpeedLeft.Text),
                int.Parse(txtSpeedRight.Text),
                (result) => { txtResult.Text=result.ToString(); });
        }

        private void butSetCameraPosition_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = string.Empty;
            Proxy.CloroFeelService svc = new Proxy.CloroFeelService();
            svc.SetCameraPositionAsync(
                chkCamActive.IsChecked==true,
                int.Parse(txtHorizontale.Text),
                int.Parse(txtVerticale.Text),
                (result) => { txtResult.Text=result.ToString(); });
        }
    }
}