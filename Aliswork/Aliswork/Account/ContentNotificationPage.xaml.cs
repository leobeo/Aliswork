using Plugin.Geolocator;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aliswork.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContentNotificationPage : ContentPage
    {
        public ContentNotificationPage()
        {
            InitializeComponent();
            PopUpDialog.ShowDialog();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
        }

        
        private async void ButtonGetGPS_Clicked(object sender, EventArgs e)
        {
            PopUpDialog.IsVisible = false;
            PopUpDialog.ShowDialog();
            try
            {
                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission)
                    return;

                ButtonGetGPS.IsEnabled = false;

               /* var locator =  CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                lbData.Text = "Getting gps...";
*/
                var position = await ContentGlobal.getLocation();

                if (position == null)
                {
                    lbData.Text = "null gps :(";
                    return;
                }


                Debug.WriteLine("Position Status: Latitude ------------------------------------------"+ position.Latitude);
                Debug.WriteLine("Position Status: Longitude ------------------------------------------"+position.Longitude);

                lbData.Text = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
                Debug.WriteLine("Er------------------------" + ex);
            }
            finally
            {
                ButtonGetGPS.IsEnabled = true;
            }
        }
    }
}