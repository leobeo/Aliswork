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
    public partial class PreviewRequestSendPage : ContentPage
    {
        //Page pageNewAppeal = new Appeal.NewAppealPage();

        int year, month;
        public PreviewRequestSendPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
        }

        protected override void OnAppearing()
        {
            /*DateTime date = DateTime.Now;
            var month = date.Month;
            int year = date.Year;*/

            year = (int)ContentGlobal.allldata["timeoff"]["year"];
            month = (int)ContentGlobal.allldata["timeoff"]["period"];

            var dataEE = ContentGlobal.allldata["requests"][year.ToString()][month.ToString()+"_"+year.ToString()][Application.Current.Properties["NumberId"].ToString()];

            Debug.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-" + dataEE);

            int state = (int)dataEE["state"];
            switch (state)
            {
                case 0:
                    lbState.Text = "Đang chờ xử lý";
                    break;
                case 1:
                    lbState.Text = "Đã được duyệt";
                    break;
                case 2:
                    lbState.Text = "Đã bị từ chối";
                    break;
                case 3:
                    lbState.Text = "Người gửi hủy";
                    break;
                default:
                    lbState.Text = "Không xác định";
                    break;
            }

            double timepreview = (double)dataEE["send_date"];
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timepreview/1000).ToLocalTime();
            string formattedDate = dt.ToString("dd-MM-yyyy");

            double start_date = (double)dataEE["start_date"];
            DateTime dt_start_date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(start_date).ToLocalTime();

            double end_date = (double)dataEE["end_date"];
            DateTime dt_end_date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(end_date).ToLocalTime();

            lbDateSend.Text = formattedDate;
            lbTypeRequest.Text = ContentGlobal.allldata["info"]["company_rules"]["list_request"][(string)dataEE["typeRequest"]]["name"].ToString();
            lbSubTypeRequest.Text = ContentGlobal.allldata["info"]["company_rules"]["list_request"][(string)dataEE["typeRequest"]]["sub_name"][(string)dataEE["subTypeRequest"]].ToString();
            /*int time_type=(int)ContentGlobal.allldata["info"]["company_rules"]["list_request"][(string)dataEE["typeRequest"]]["time_type"];

            if (time_type == 0)
            {
                stkFromDate.IsVisible = false;;

                stkToDate.IsVisible = false;
            }
            else if (time_type == 1)
            {
                stkFromDate.IsVisible = true;
                lbTextFromDate.Text = "Ngày";

                stkToDate.IsVisible = false;
            }
            else if (time_type == 2)
            {
                stkFromDate.IsVisible = true;
                lbTextFromDate.Text = "Thời gian từ";

                stkToDate.IsVisible = true;
            }*/
            lbNumberId.Text = Application.Current.Properties["NumberId"].ToString();
            lbPeopleAssign.Text = (string)ContentGlobal.allldata["info"]["company_people"][(string)dataEE["people_assign"]]["name"];

            lbStartDate.Text = dt_start_date.ToString("dd-MM-yyyy");
            lbStartTime.Text = dt_start_date.ToString("HH:mm");

            lbEndDate.Text = dt_end_date.ToString("dd-MM-yyyy");
            lbEndTime.Text = dt_end_date.ToString("HH:mm");

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
            if (ContentGlobal.statusNetwork().Equals("Connected") == false)
            {
                await DisplayAlert("Cảnh báo mạng", "Bạn hãy kết nối mạng để thực hiện tác vụ này !", "OK");
            }
            else
            {
                var answer = await DisplayAlert("Hủy yêu cầu", "Bạn có thực sự muốn hủy đơn nghỉ này", "Yes", "No");
                if (answer == true)
                {
                    try
                    {
                        //await Navigation.PopAsync();
                        string strSub = "UpdateOdooRequest";
                        string appealId = Application.Current.Properties["NumberId"].ToString();
                        int state = 3;
                        string conform_content = "Người dùng hủy";

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
                    catch (Exception ex)
                    {
                        await DisplayAlert("Lỗi", "Có vấn dề đã sảy ra, thử lại sau !", "OK");
                        Debug.WriteLine("Error--ChangedPassword--------------------------------", ex);
                    }
                }
            }
        }

        public async void btnAppeal_Clicked(object sender, EventArgs e)
        {
            if(ContentGlobal.statusNetwork().Equals("Connected") == false)
            {
                await DisplayAlert("Cảnh báo mạng", "Bạn hãy kết nối mạng để thực hiện tác vụ này !", "OK");
            }
            else
            {
                try
                {
                    string strSub = "UpdateOdooRequest";
                    string appealId = Application.Current.Properties["NumberId"].ToString();
                    int state = 4;
                    string conform_content = "Người dùng Khiếu nại";

                    var dataEE = ContentGlobal.allldata["requests"][year.ToString()][month.ToString()][Application.Current.Properties["NumberId"].ToString()];
                    double timepreview = (double)dataEE["send_date"];

                    string strParam = @"{""uid"":""" + Application.Current.Properties["uId"].ToString() + @""",""request_id"":""" + appealId + @""",""action"":" + state + @",""comment"":""" + conform_content + @"""}";

                    Debug.WriteLine("strParam---------------------------------" + strParam);
                    var data = await ContentGlobal.FirebasePOSTFunctions(strSub, strParam);

                    if ((string)data["return"] == "OK")
                    {
                        //await Navigation.PopAsync();
                        Application.Current.Properties["baseAppeal"] = lbNumberId.Text.ToString();
                        //await Navigation.PushAsync(new Requests.NewRequestPage());
                        App.Current.MainPage = new Requests.NewRequestPage();
                    }
                    else
                    {
                        await DisplayAlert("Thông báo", "Có lỗi đã sảy ra, thử lại sau", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Lỗi", "Có vấn dề đã sảy ra, thử lại sau !", "OK");
                    Debug.WriteLine("Error--ChangedPassword--------------------------------", ex);
                }
            }
           

           
        }
    }
}