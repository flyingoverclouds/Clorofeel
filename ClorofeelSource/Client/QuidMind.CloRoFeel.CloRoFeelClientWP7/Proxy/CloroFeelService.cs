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
using Microsoft.Phone.Info;

namespace QuidMind.CloRoFeel.CloRoFeelClientWP7.Proxy
{
	public class CloroFeelService
	{
		static int unusedVal = 0;

		#region REST URL
		const string restBaseUrl = "http://clorofeel.servicebus.windows.net/robotremote/RobotBoard";

		const string restUrl_RegisterDevice = "/RegisterDevice?deviceId={0}";
		const string restUrl_GetRobotVersion = "/GetRobotVersion?deviceId={0}";
		const string restUrl_IsAuthorized = "/IsAuthorized?deviceId={0}";
		const string restUrl_SetSpeed = "/SetSpeed?deviceId={0}&leftSpeed={1}&rightSpeed={2}";
        const string restUrl_SetCameraPosition = "/SetCameraAbsolutePosition?deviceId={0}&camIsActive={1}&orientationHorizontale={2}&orientationVerticale={3}";
		#endregion


		#region private datas
		string userAnonymousID = "<<NOT SET>>";
		#endregion

		private static readonly int ANIDLength = 32;
		private static readonly int ANIDOffset = 2;   

		public CloroFeelService ()
		{
			//object value;
			//if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("DeviceName", out value))
			//    deviceName = value.ToString();
	
			object anid;   
			if (UserExtendedProperties.TryGetValue("ANID", out anid))   
			{   
				if (anid != null && anid.ToString().Length >= (ANIDLength + ANIDOffset))   
				{
					userAnonymousID = anid.ToString().Substring(ANIDOffset, ANIDLength);   
				}   
			}   
		}
		#region Proxy methode

		public void RegisterDeviceAsync( Action<string> registerDeviceCompletedAction )
		{
			var uri = new Uri(
				string.Format(
					string.Format("{0}{1}&unused={2}", restBaseUrl, restUrl_RegisterDevice, (unusedVal++).ToString()),
						userAnonymousID
					),
					UriKind.Absolute
				);
			WebClient wc = new WebClient();
			wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(
				(sender, e) => {
					registerDeviceCompletedAction.Invoke(e.Result);
				});
			wc.DownloadStringAsync(uri);
		}

		public void GetRobotVersionAsync( Action<string> getRobotVersionCompletedAction )
		{
			var uri = new Uri(
			   string.Format(
				   string.Format("{0}{1}&unused={2}", restBaseUrl, restUrl_GetRobotVersion, (unusedVal++).ToString()),
					   userAnonymousID
				   ),
				   UriKind.Absolute
			   );
			WebClient wc = new WebClient();
			wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(
				(sender, e) =>
				{
					getRobotVersionCompletedAction.Invoke(e.Result);
				});
			wc.DownloadStringAsync(uri);
		}

		public void IsAuthorizedAsync(Action<string> isAuthorizedCompletedAction)
		{
			var uri = new Uri(
				string.Format(
					string.Format("{0}{1}&unused={2}", restBaseUrl, restUrl_IsAuthorized, (unusedVal++).ToString()),
						userAnonymousID
					),
					UriKind.Absolute
				);
			WebClient wc = new WebClient();
			wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(
				(sender, e) =>
				{
					isAuthorizedCompletedAction.Invoke(e.Result);
				});
			wc.DownloadStringAsync(uri);
		}

		WebClient wcSetSpeed = null;
		public void SetSpeedAsync(int leftSpeed, int rightSpeed,Action<string> setSpeedCompletedAction)
		{
			var uri = new Uri(
				string.Format(
					string.Format("{0}{1}&unused={2}", restBaseUrl, restUrl_SetSpeed, (unusedVal++).ToString()),
						userAnonymousID,
						leftSpeed,
						rightSpeed
					),
					UriKind.Absolute
				);
			if (wcSetSpeed != null)
				wcSetSpeed.CancelAsync();
			wcSetSpeed = new WebClient();
			wcSetSpeed.DownloadStringCompleted += new DownloadStringCompletedEventHandler(
				(sender, e) =>
				{
					if (e.Cancelled || e.Error != null)
						return;
					setSpeedCompletedAction.Invoke(e.Result);
				});
			wcSetSpeed.DownloadStringAsync(uri);
		}

		WebClient wcSetCameraPosition = null;
		public void SetCameraPositionAsync(bool camActive, int positionHorizontale, int positionVerticale, Action<string> setSpeedCompletedAction)
		{
			var uri = new Uri(
				string.Format(
					string.Format("{0}{1}&unused={2}", restBaseUrl, restUrl_SetCameraPosition, (unusedVal++).ToString()),
						userAnonymousID,
						camActive,
						positionHorizontale,
						positionVerticale
					),
					UriKind.Absolute
				);
			if (wcSetCameraPosition != null)
				wcSetCameraPosition.CancelAsync();
			wcSetCameraPosition = new WebClient();
			wcSetCameraPosition.DownloadStringCompleted += new DownloadStringCompletedEventHandler(
				(sender, e) =>
				{
					if (e.Cancelled || e.Error != null)
						return;
					setSpeedCompletedAction.Invoke(e.Result);
				});
			wcSetCameraPosition.DownloadStringAsync(uri);
		}
		#endregion
	}
}
