using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class NewRequestPage : ContentPage
    {
        int selectedIndexTypeRequest = -1;
        int selectedIndexSubTypeRequest = -1;
        string selectedIndexValue = "";
        string sub_type_rq = "";
        Dictionary<string, string> dictionarySub = new Dictionary<string, string>();


        public NewRequestPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");

            foreach (string requestName in ContentGlobal.ListRequest.Keys)
            {
                pkTypeRequest.Items.Add(requestName);
            }

            pkTypeRequest.SelectedIndexChanged += (sender, e) =>
            {
                selectedIndexTypeRequest = pkTypeRequest.SelectedIndex;
                if (selectedIndexTypeRequest >= 0)
                {
                    //string subTitle = pkTypeRequest.Items[pkTypeRequest.SelectedIndex];
                    selectedIndexValue = ContentGlobal.ListRequest[pkTypeRequest.Items[pkTypeRequest.SelectedIndex]];
                    loadTimePicker();
                    loadSubRequets();
                    pkSubTypeRequest.IsEnabled = true;
                    //edtCause.IsEnabled = true;
                }
            };

            pkSubTypeRequest.SelectedIndexChanged += (sender, e) =>
             {
                 selectedIndexSubTypeRequest = pkSubTypeRequest.SelectedIndex;
                 if (selectedIndexSubTypeRequest >= 0)
                 {
                     sub_type_rq = dictionarySub[pkSubTypeRequest.Items[pkSubTypeRequest.SelectedIndex]];
                     Debug.WriteLine("sub_type_rq-----------------------------------------------" + sub_type_rq);
                     edtCause.IsEnabled = true;
                     edtCause.Text = " ";
                 }

             };


            var tapImgreset = new TapGestureRecognizer();
            tapImgreset.Tapped += (s, e) =>
            {
                pkTypeRequest.SelectedIndex = -1;
                pkSubTypeRequest.SelectedIndex = -1;
                edtCause.Text = " ";
            };
            imgReset.GestureRecognizers.Add(tapImgreset);
        }

        public void edtEdit(object sender, EventArgs e)
        {
            if (edtCause.Text.ToString().Length > 0)
            {
                imgSend.IsEnabled = true;
                imgReset.IsEnabled = true;
            }
            else
            {
                imgSend.IsEnabled = false;
                imgReset.IsEnabled = false;
            }
        }

        protected async override void OnAppearing()
        {
            ContentGlobal.loadListRequest();
            var dataEEProgess = ContentGlobal.allldata["timeoff"]["timeoff"];

            if (dataEEProgess != null)
            {
                Debug.WriteLine("dataEE-------------------------------------------------------------" + dataEEProgess);

                if ((float)dataEEProgess["dayoff"]["total"] > 0.0)
                {
                    float dateoff = (float)dataEEProgess["dayoff"]["timecout"] / (float)dataEEProgess["dayoff"]["total"];
                    await progessPer.ProgressTo(dateoff, 250, Easing.Linear);
                }

                lbDateoff.Text = (string)dataEEProgess["dayoff"]["timecout"];
                lbTotalDateoff.Text = "(Tiêu chuẩn: " + (string)dataEEProgess["dayoff"]["total"] + " ngày)";
                lboffPertotal.Text = (string)dataEEProgess["dayoff"]["timecout"] + " /" + (string)dataEEProgess["dayoff"]["total"] + "n";
            }



            /*string companyId = (string)ContentGlobal.allldata["info"]["company"]["id"];
            string strParam = @"{""companyId"":""" + companyId + @""",""type"":1}";
            Debug.WriteLine("strParam---------------------" + strParam);

            var getid = await ContentGlobal.FirebasePOSTFunctions("GetIdLetter", strParam);
            Debug.WriteLine("getid---------------------" + getid);

            string id = (string)getid["ID"];
            //lbNumberId.Text = "LR" + id;


            string People_AssignName = "";
            var dataEE = ContentGlobal.allldata["info"]["managed"];

            Debug.WriteLine("dataEE-----------------------------" + dataEE);

            var dict = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(dataEE.ToString());
            foreach (var kv in dict)
            {
                Debug.WriteLine("key :--------" + kv.Key + "value-----------------" + kv.Value);
                if ((int)kv.Value["state"] == 1)
                {
                    //People_AssignId = kv.Key;
                    People_AssignName = (string)kv.Value["name"];
                }
            }

            //lbPeopleAssign.Text = People_AssignName;*/
        }
        public async void SendRequest(object sender, EventArgs e)
        {
            if(selectedIndexTypeRequest>=0 && selectedIndexSubTypeRequest>=0 && edtCause!=null && edtCause.Text.ToString().Trim().Length > 0)
            {
                if (ContentGlobal.statusNetwork().Equals("Connected") != false)
                {
                    var answer = await DisplayAlert("Phiếu Xin nghỉ", "Bạn có thực sự muốn gửi phiếu nghỉ này", "Yes", "No");
                    if (answer == true)
                    {
                        try
                        {
                            string strSub = "CreateOdooRequest";

                            string subTypeRequest = sub_type_rq;
                            Debug.WriteLine("subTypeRequest---------------------" + sub_type_rq);

                            string people_send = Application.Current.Properties["uId"].ToString();
                            Debug.WriteLine("people_send-------------------------" + people_send);

                            // string people_assign = People_AssignId;
                            // Debug.WriteLine("people_assign-------------------------" + people_assign);

                            // string requestId = lbNumberId.Text.ToString();
                            //Debug.WriteLine("requestId-------------------------" + requestId);

                            string content = edtCause.Text.ToString().Trim();
                            Debug.WriteLine("content-------------------------" + content);

                            long DatetimeMinTimeTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

                            long startdate = (long)((pkFromDate.Date.Add(pkFromTime.Time).ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000);
                            Debug.WriteLine("startdate-------------------------" + startdate);

                            long enddate = (long)((pkToDate.Date.Add(pkToTime.Time).ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000);
                            Debug.WriteLine("enddate-------------------------" + enddate);

                            string strParam = @"{ ""uid"":""" + people_send + @""",""sub_type_rq"":""" + subTypeRequest + @""",""rq_comment"":""" + content + @""",""from_date"":""" + startdate + @""",""to_date"":""" + enddate + @"""}";

                            Debug.WriteLine("strParam-------------------------" + strParam);

                            var data = ContentGlobal.FirebasePOSTFunctions(strSub, strParam);
                            Debug.WriteLine("data-------------------------------------" + data);
                            await Navigation.PopAsync();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Cảnh báo", "Kiểm tra lại kết nối mạng của bạn và thử lại sau", "OK");
                }
            }
            else
            {
                await DisplayAlert("Thông báo", "Bạn phải nhập đủ các trường thông tin !", "OK");
            }

           
        }

        public void loadSubRequets()
        {
            pkSubTypeRequest.Items.Clear();
            dictionarySub.Clear();
            if (selectedIndexValue.Length > 0)
            {
                Debug.WriteLine("selectedIndexValue---------------------------------------------" + selectedIndexValue);
                var data = ContentGlobal.allldata["info"]["company_rules"]["list_request"][selectedIndexValue]["sub_name"];

                Debug.WriteLine("------------------------------- string selectedIndexValue------------------------------------" + data);

                if (data != null)
                {
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.ToString());
                    foreach (var kv in dict)
                    {
                        //pkSubTypeRequest.Items.Add(kv.Key.ToString() + " - " + kv.Value.ToString());
                        pkSubTypeRequest.Items.Add(kv.Value.ToString());
                        dictionarySub.Add(kv.Value.ToString(), kv.Key.ToString());
                    }
                }

            }
        }

        public void loadTimePicker()
        {
            if (selectedIndexValue.Length > 0)
            {
                var data = ContentGlobal.allldata["info"]["company_rules"]["list_request"][selectedIndexValue]["time_type"];

                if (data != null)
                {
                    if ((int)data == 0)
                    {
                        stkFromDate.IsVisible = false;
                        pkFromDate.IsEnabled = false;
                        pkFromTime.IsEnabled = false;

                        stkToDate.IsVisible = false;
                        pkToDate.IsEnabled = false;
                        pkToTime.IsEnabled = false;
                    }
                    else if ((int)data == 1)
                    {
                        stkFromDate.IsVisible = true;

                        var s = new FormattedString();
                        s.Spans.Add(new Span { Text = "Ngày "});
                        s.Spans.Add(new Span { Text = "*",ForegroundColor=Color.Red });

                        lbTextFromDate.FormattedText = s;

                        pkFromDate.IsEnabled = true;
                        pkFromTime.IsEnabled = false;

                        stkToDate.IsVisible = false;
                        pkToDate.IsEnabled = false;
                        pkToTime.IsEnabled = false;
                    }
                    else if ((int)data == 2)
                    {
                        stkFromDate.IsVisible = true;

                        var s = new FormattedString();
                        s.Spans.Add(new Span { Text = "Thời gian từ " });
                        s.Spans.Add(new Span { Text = "*", ForegroundColor = Color.Red });
                        lbTextFromDate.FormattedText = s;

                        pkFromDate.IsEnabled = true;
                        pkFromTime.IsEnabled = true;

                        stkToDate.IsVisible = true;
                        pkToDate.IsEnabled = true;
                        pkToTime.IsEnabled = true;
                    }
                }
            }
        }
    }
}