using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aliswork.Timekeeping
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OneTimekeepingPage : ContentPage
    {
        public OneTimekeepingPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
        }

        protected async override void OnAppearing()
        {

            var dataEE = ContentGlobal.allldata["timeoff"]["timeoff"];

            if (dataEE != null)
            {
                Debug.WriteLine("dataEE-------------------------------------------------------------" + dataEE);
                if ((float)dataEE["dayoff"]["total"] > 0.0)
                {
                    float dateoff = (float)dataEE["dayoff"]["timecout"] / (float)dataEE["dayoff"]["total"];
                    await progessOff.ProgressTo(dateoff, 250, Easing.Linear);
                }

                if ((float)dataEE["checkinlate"]["total"] > 0.0)
                {
                    float late = (float)dataEE["checkinlate"]["actualtime"] / (float)dataEE["checkinlate"]["total"];
                    await progessLater.ProgressTo(late, 250, Easing.Linear);
                }

                if ((float)dataEE["checkoutearly"]["total"] > 0.0)
                {
                    float early = (float)dataEE["checkoutearly"]["actualtime"] / (float)dataEE["checkoutearly"]["total"];
                    await progessEarly.ProgressTo(early, 250, Easing.Linear);
                }
                

                //LoadProgess(dateoff, late, early);


                lbDateOffCount.Text = (string)dataEE["dayoff"]["timecout"];
                lbTotalDateOffCount.Text = "(Tổng: " + (string)dataEE["dayoff"]["total"] + " ngày)";
                lbDateOffPer.Text = (string)dataEE["dayoff"]["timecout"] + "/ " + (string)dataEE["dayoff"]["total"];

                lbLateCount.Text = (string)dataEE["checkinlate"]["timecount"];
                lbTotalLateTime.Text = "(Tổng: " + (string)dataEE["checkinlate"]["total"] + " phút)";
                lbLateTimePer.Text = (string)dataEE["checkinlate"]["actualtime"] + "/ " + (string)dataEE["checkinlate"]["total"];

                lbEarlyCount.Text = (string)dataEE["checkoutearly"]["timecount"];
                lbTotalEarlyTime.Text = "(Tổng: " + (string)dataEE["checkoutearly"]["total"] + " phút)";
                lbTotalEarlyPer.Text = (string)dataEE["checkoutearly"]["actualtime"] + "/ " + (string)dataEE["checkoutearly"]["total"];

                lbNoPaid.Text = (string)dataEE["dayoffoutpermit"];
                lbPaidLeave.Text = (string)dataEE["paid_leave"];
            }
        }

        private async void LoadProgess(float off, float late, float early)
        {
            await progessOff.ProgressTo(off, 250, Easing.Linear);

            await progessLater.ProgressTo(late, 250, Easing.Linear);

            await progessEarly.ProgressTo(early, 250, Easing.Linear);
        }
    }
}