using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aliswork.Requests
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApproveRequestPage : ContentPage
    {
        int year, month;
        public ApproveRequestPage()
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
            if (ContentGlobal.statusNetwork().Equals("Connected") != false)
            {
                if (edtCause.Text.ToString().Length > 0)
                {
                    string strSub = "UpdateOdooRequest";
                    string appealId = Application.Current.Properties["NumberId"].ToString();
                    int state = (int)Application.Current.Properties["TypeApprove"];
                    string conform_content = string.Format( edtCause.Text.ToString());

                    /*DateTime date = DateTime.Now;
                    var month = (int)date.Month;
                    var year = date.Year;*/

                    /* year = (int)ContentGlobal.allldata["timeoff"]["year"];
                     month = (int)ContentGlobal.allldata["timeoff"]["period"];

                     var dataEE = ContentGlobal.allldata["requests_receive"][year.ToString()][month.ToString()][Application.Current.Properties["NumberId"].ToString()];
                     double timepreview = (double)dataEE["send_date"];*/

                    string strParam = @"{""uid"":""" + Application.Current.Properties["uId"].ToString() + @""",""request_id"":""" + appealId + @""",""action"":" + state + @",""comment"":""" + conform_content + @"""}";

                    Debug.WriteLine("strParam---------------------------------" + strParam);
                    var data = await ContentGlobal.FirebasePOSTFunctions(strSub, strParam);

                    if ((string)data["return"] == "OK")
                    {
                        var page = new MainPage();
                        NavigationPage.SetHasNavigationBar(page, false);
                        App.Current.MainPage = new NavigationPage(page);
                    }
                    else
                    {
                        await DisplayAlert("Thông báo", "Có lỗi đã sảy ra, thử lại sau", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Warring", "Hay điền đủ lý do", "OK");
                    edtCause.Text = "";
                    edtCause.Focus();
                }
            }
            else
            {
                await DisplayAlert("Cảnh báo", "Kiểm tra lại kết nối mạng của bạn và thử lại sau", "OK");
            }
        }

    }
}