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
    public partial class PreviewAppealSendPage : ContentPage
    {
        int year, month;
        public PreviewAppealSendPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
        }

        protected override void OnAppearing()
        {
            DateTime date = DateTime.Now;
            year = (int)ContentGlobal.allldata["timeoff"]["year"];
            month = (int)ContentGlobal.allldata["timeoff"]["period"];

            var dataEE = ContentGlobal.allldata["appeals"][year.ToString()][month.ToString()][Application.Current.Properties["NumberId"].ToString()];
            double timepreview = (double)dataEE["send_date"];
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timepreview / 1000).ToLocalTime();
            string formattedDate = dt.ToString("dd-MM-yyyy");

            lbDateSend.Text = formattedDate;
            lbTypeAppeal.Text = (string)dataEE["typeAppeal"];
            lbNumberId.Text = Application.Current.Properties["NumberId"].ToString();
            lbBase_NumberId.Text = (string)dataEE["base_on"];
            lbPeopleAssign.Text = (string)ContentGlobal.allldata["info"]["managed"][(string)dataEE["people_assign"]]["name"];
            edtCause.Text = (string)dataEE["content"];

            if ((int)dataEE["state"] == 0)
            {
                btnAppeal.IsEnabled = false;
                btnCancel.IsEnabled = true;
            }
            else
            {
                btnAppeal.IsEnabled = true;
                btnCancel.IsEnabled = false;
            }

        }

        public async void btnCancel_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Hủy yêu cầu", "Bạn có thực sự muốn hủy đơn khiếu nại này", "Yes", "No");
            if (answer == true)
            {
                try
                {
                    string strSub = "UpdateAppeal";
                    string companyId = (string)ContentGlobal.allldata["info"]["company"]["id"];
                    string appealId = Application.Current.Properties["NumberId"].ToString();
                    int state = 3;
                    string conform_content = "Người dùng hủy";

                    var dataEE = ContentGlobal.allldata["appeals"][year.ToString()][month.ToString()][Application.Current.Properties["NumberId"].ToString()];
                    double timepreview = (double)dataEE["send_date"];

                    string strParam = @"{""companyId"":""" + companyId + @""",""appealId"":""" + appealId + @""",""state"":" + state + @",""conform_content"":""" + conform_content + @""",""send_date"":" + timepreview + @"}";

                    Debug.WriteLine("strParam-----------------------------" + strParam);
                    var data = await ContentGlobal.FirebasePUTFunctions(strSub, strParam);
                    if ((string)data["return"] == "OK")
                    {
                        //await Navigation.PopAsync();
                        Page page = new MainPage();
                        NavigationPage.SetHasNavigationBar(page, false);
                        App.Current.MainPage = new NavigationPage(page);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async void btnAppeal_Clicked(object sender, EventArgs e)
        {
            try
            {
                string strSub = "UpdateAppeal";
                string companyId = (string)ContentGlobal.allldata["info"]["company"]["id"];
                string appealId = Application.Current.Properties["NumberId"].ToString();
                int state = 4;
                string conform_content = "Người dùng Khiếu nại";

                var dataEE = ContentGlobal.allldata["appeals"][year.ToString()][month.ToString()][Application.Current.Properties["NumberId"].ToString()];
                double timepreview = (double)dataEE["send_date"];

                string strParam = @"{""companyId"":""" + companyId + @""",""appealId"":""" + appealId + @""",""state"":" + state + @",""conform_content"":""" + conform_content + @""",""send_date"":" + timepreview + @"}";

                Debug.WriteLine("strParam-----------------------------" + strParam);
                var data = await ContentGlobal.FirebasePUTFunctions(strSub, strParam);
                if ((string)data["return"] == "OK")
                {
                    Application.Current.Properties["baseAppeal"] = lbNumberId.Text.ToString();
                    await Navigation.PushAsync(new Appeal.NewAppealPage());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }

    }
}