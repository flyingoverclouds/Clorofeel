using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace QuidMind.CloRoFeel.CloRoFeelClientWP7.ViewModels
{
    public enum ClorofeelDeviceStatus
    {
        NotRegistered,  // Your device has not been registered to CloRoFeel robot
        Registered,     // Your device is registered to CloRoFeel robot
        Waiter,         // Your device is in the access waiting list of CloRoFeel
        Viewer,         // Your device received data from CloRoFeel but can't control it
        Controller      // Your device has received the full control of CloRoFeel. You can drive it.
    }
}
