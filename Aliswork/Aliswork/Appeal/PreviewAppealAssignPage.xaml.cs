using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aliswork.Appeal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreviewAppealAssignPage : ContentPage
    {
        public PreviewAppealAssignPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
        }

        protected override void OnAppearing()
        {
            DateTime date = DateTime.Now;

            int year = (int)ContentGlobal.allldata["timeoff"]["year"];
            int month = (int)ContentGlobal.allldata["timeoff"]["period"];

            var dataEE = ContentGlobal.allldata["appeals_receive"][year.ToString()][month.ToString()][Application.Current.Properties["NumberId"].ToString()];
            double timepreview = (double)dataEE["send_date"];
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timepreview / 1000).ToLocalTime();
            string formattedDate = dt.ToString("dd-MM-yyyy");

            lbDate_Send.Text = formattedDate;
            lbTypeAppeal.Text = (string)dataEE["typeAppeal"];
            lbNumberId.Text = Application.Current.Properties["NumberId"].ToString();
            lbBase_NumberId.Text = (string)dataEE["base_on"];
            edtCause.Text = (string)dataEE["content"];

            if ((int)dataEE["state"] != 0)
            {
                btnAccept.IsEnabled = false;
                btnDecline.IsEnabled = false;
            }

        }

        public async void btnDecline_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Hủy khiếu nại", "Bạn thực sự muốn hủy khiếu nại này", "Yes", "No");
            if (answer == true)
            {
                try
                {
                    //await Navigation.PopAsync();
                    Application.Current.Properties["TypeApprove"] = 2;
                    await Navigation.PushAsync(new Appeal.ApproveAppealPage());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async void btnAccpet_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Tiếp nhận khiếu nại", "Bạn chấp nhận khiếu nại này", "Yes", "No");
            if (answer == true)
            {
                try
                {
                    //await Navigation.PopAsync();

                    Application.Current.Properties["TypeApprove"] = 1;
                    await Navigation.PushAsync(new Appeal.ApproveAppealPage());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}