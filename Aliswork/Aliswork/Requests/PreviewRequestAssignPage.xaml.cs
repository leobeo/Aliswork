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
    public partial class PreviewRequestAssignPage : ContentPage
    {
        public PreviewRequestAssignPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
        }

        protected override void OnAppearing()
        {
            /*DateTime date = DateTime.Now;
            var month = date.Month;
            int year = date.Year;*/

            int year = (int)ContentGlobal.allldata["timeoff"]["year"];
            int month = (int)ContentGlobal.allldata["timeoff"]["period"];

            var dataEE = ContentGlobal.allldata["requests_receive"][year.ToString()][month.ToString()+"_"+year.ToString()][Application.Current.Properties["NumberId"].ToString()];

            Debug.WriteLine("dataEE----------------------------------------------------" + dataEE);

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
            lbTypeRequest.Text = ContentGlobal.allldata["info"]["company_rules"]["list_request"][(string)dataEE["typeRequest"]]["name"].ToString(); ;
            lbSubTypeRequest.Text = ContentGlobal.allldata["info"]["company_rules"]["list_request"][(string)dataEE["typeRequest"]]["sub_name"][(string)dataEE["subTypeRequest"]].ToString();
            lbNumberId.Text = Application.Current.Properties["NumberId"].ToString();

            var time_type = ContentGlobal.allldata["info"]["company_rules"]["list_request"][(string)dataEE["typeRequest"]]["time_type"];
            if (time_type != null)
            {
                if ((int)time_type == 0)
                {
                    stkFromDate.IsVisible = false; ;

                    stkToDate.IsVisible = false;
                }
                else if ((int)time_type == 1)
                {
                    stkFromDate.IsVisible = true;
                    lbTextFromDate.Text = "Ngày";

                    stkToDate.IsVisible = false;
                }
                else if ((int)time_type == 2)
                {
                    stkFromDate.IsVisible = true;
                    lbTextFromDate.Text = "Thời gian từ";

                    stkToDate.IsVisible = true;
                }
            }

            lbStartDate.Text = dt_start_date.ToString("dd-MM-yyyy");
            lbStartTime.Text = dt_start_date.ToString("HH:mm");

            lbEndDate.Text = dt_end_date.ToString("dd-MM-yyyy");
            lbEndTime.Text = dt_end_date.ToString("HH:mm");

            edtCause.Text = (string)dataEE["content"];

            if ((int)dataEE["state"] != 0)
            {
                btnAccept.IsEnabled = false;
                btnDecline.IsEnabled = false;
            }
        }

        public async void btnDecline_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Hủy đơn", "Bạn thực sự muốn hủy đơn yêu cầu này", "Yes", "No");
            if (answer == true)
            {
                try
                {
                    //await Navigation.PopAsync();
                    Application.Current.Properties["TypeApprove"] = 2;
                    await Navigation.PushAsync(new Requests.ApproveRequestPage());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async void btnAccpet_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Tiếp nhận", "Bạn muốn chấp nhận đơn này", "Yes", "No");
            if (answer == true)
            {
                try
                {
                    //await Navigation.PopAsync();
                    Application.Current.Properties["TypeApprove"] = 1;
                    await Navigation.PushAsync(new Requests.ApproveRequestPage());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}