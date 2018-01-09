using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aliswork.Appeal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApproveAppealPage : ContentPage
    {
        int year, month;
        public ApproveAppealPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
            lbAppealId.Text = Application.Current.Properties["NumberId"].ToString();
            edtCause.Focus();
        }

        public void edtChange(object sender, EventArgs e)
        {
            if (edtCause.Text.ToString().Length > 0)
            {
                btnSend.IsEnabled = true;
            }
            else
            {
                btnSend.IsEnabled = false;
            }
        }

        public async void SendRequest(object sender, EventArgs e)
        {
            if (edtCause.Text.ToString().Length > 0)
            {
                string strSub = "UpdateAppeal";
                string companyId = (string)ContentGlobal.allldata["info"]["company"]["id"];
                string appealId = Application.Current.Properties["NumberId"].ToString();
                int state = (int)Application.Current.Properties["TypeApprove"];
                string conform_content = edtCause.Text.ToString();

                year = (int)ContentGlobal.allldata["timeoff"]["year"];
                month = (int)ContentGlobal.allldata["timeoff"]["period"];

                var dataEE = ContentGlobal.allldata["appeals_receive"][year.ToString()][month.ToString()][Application.Current.Properties["NumberId"].ToString()];
                double timepreview = (double)dataEE["send_date"];

                string strParam = @"{""companyId"":""" + companyId + @""",""appealId"":""" + appealId + @""",""state"":" + state + @",""conform_content"":""" + conform_content + @""",""send_date"":" + timepreview + @"}";

                Debug.WriteLine("strParam--------------------------------" + strParam);
                var data = await ContentGlobal.FirebasePUTFunctions(strSub, strParam);
                if ((string)data["return"] == "OK")
                {
                    var page = new MainPage();
                    NavigationPage.SetHasNavigationBar(page, false);
                    App.Current.MainPage = new NavigationPage(page);
                }
            }
            else
            {
                await DisplayAlert("Warring", "Hay điền đủ lý do", "OK");
                edtCause.Text = "";
                edtCause.Focus();
            }
        }

    }
}