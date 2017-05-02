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
    public partial class CGU : PhoneApplicationPage
    {
        public CGU()
        {
            InitializeComponent();
        }

        private void imgQuidMind_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Phone.Tasks.WebBrowserTask webbrowser = new Microsoft.Phone.Tasks.WebBrowserTask();
            webbrowser.URL = "http://www.quidmind.com";
            webbrowser.Show();
        }

        private void butAcceptCGU_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.CguAccepted = true;
            App.ViewModel.SavePersistantContext();
            NavigationService.GoBack();
        }

        private void butRefusCGU_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Vous avez refuser la licence d'utilisation. Le logiciel ne sera pas fonctionnel.");
            NavigationService.GoBack();
        }
    }
}