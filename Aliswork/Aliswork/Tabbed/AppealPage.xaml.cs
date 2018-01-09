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

namespace Aliswork.Tabbed
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppealPage : ContentPage
    {
        //Page pagePersion = ;
        //Page pageNewAppeal = ;

        DateTime date = DateTime.Now;
        int year;
        int month;

        string selectedItemYear = "";
        int selectedIndexMonth = -1;

        public AppealPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");

            if (Device.RuntimePlatform == Device.iOS)
            {
                stkHeader.Margin = new Thickness(0, 20, 0, 0);
            }

            if (ContentGlobal.Userroot == false)
            {
                stkAppealAssign.IsVisible = false;
                grdAppealAssign.IsVisible = false;
            }

            List<String> years = ContentGlobal.ListYear;
            foreach (String nn in years)
            {
                pickerYear.Items.Add(nn);
            }

            pickerYear.SelectedIndex = 1;
            pickerYear.SelectedIndexChanged += (sender, e) =>
            {
                selectedItemYear = pickerYear.Items[pickerYear.SelectedIndex];
                loadAppealSend();
            };

            List<String> months = ContentGlobal.ListMonth;
            foreach (String nn in months)
            {
                pickerMonth.Items.Add(nn);
            }
            //month = date.Month;
            month = (int)ContentGlobal.allldata["timeoff"]["period"];
            selectedIndexMonth = month - 1;

            pickerMonth.SelectedIndex = selectedIndexMonth;

            Debug.WriteLine("===============================selectedIndexMonth=========================" + selectedIndexMonth);
            Debug.WriteLine("===============================pickerMonth.SelectedIndex=========================" + pickerMonth.SelectedIndex);


            pickerMonth.SelectedIndexChanged += (sender, e) =>
            {
                selectedIndexMonth = pickerMonth.SelectedIndex;
                loadAppealReceive();
            };

            var tapPersion = new TapGestureRecognizer();
            tapPersion.Tapped += (s, e) =>
            {
                stkHeader.IsVisible = false;
                Navigation.PushAsync(new Personal.PersonalPage());
            };
            imgtoPersion.GestureRecognizers.Add(tapPersion);

            var tapimgtoNotification = new TapGestureRecognizer();
            tapimgtoNotification.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new NotificationPage());
            };
            imgtoNotification.GestureRecognizers.Add(tapimgtoNotification);

            var tapNew = new TapGestureRecognizer();
            tapNew.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Appeal.NewAppealPage());
            };
            //imgToNewAppeal.GestureRecognizers.Add(tapNew);
            grdNewdAppeal.GestureRecognizers.Add(tapNew);


            var tapHistory = new TapGestureRecognizer();
            tapHistory.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Appeal.AppealSendPage());
            };
            //imgToRequestPage.GestureRecognizers.Add(tapHistory);
            grdAppealSend.GestureRecognizers.Add(tapHistory);

            var tapAssign = new TapGestureRecognizer();
            tapAssign.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Appeal.AppealAssignPage());
            };
            //imgToAssignAppeal.GestureRecognizers.Add(tapAssign);
            grdAppealAssign.GestureRecognizers.Add(tapAssign);
        }

        protected override void OnAppearing()
        {
            stkHeader.IsVisible = true;

            year = (int)ContentGlobal.allldata["timeoff"]["year"];
            month = (int)ContentGlobal.allldata["timeoff"]["period"];

            Debug.WriteLine("==============================================month==============================" + month);
            selectedItemYear = year.ToString();

            loadAppealSend();

            if (ContentGlobal.Userroot == false)
            {
                stkAppealAssign.IsVisible = false;
                grdAppealAssign.IsVisible = false;
            }
            else
            {
                stkAppealAssign.IsVisible = true;
                grdAppealAssign.IsVisible = true;

                loadAppealReceive();
            }
        }


        private void loadAppealSend()
        {
            Debug.WriteLine("=========================================selectedItemYear========================" + selectedItemYear);

            var dataEE_appeals = ContentGlobal.allldata["appeals"];

            if(dataEE_appeals != null)
            {
                var dataAppeal_send_year = dataEE_appeals[selectedItemYear.ToString()];

                if (dataAppeal_send_year != null)
                {
                    var dataAppeal_send = dataAppeal_send_year["total"];

                    if (dataAppeal_send != null)
                    {
                        Debug.WriteLine("dataAppeal_send-----------------------" + year + "-------------" + month + "------------" + dataAppeal_send);

                        var dictTimekeeping_send = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataAppeal_send["timekeeping"].ToString());
                        foreach (var kv in dictTimekeeping_send)
                        {
                            Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);
                            if (kv.Key == "wait")
                            {
                                TimekeepingWait_send.Text = kv.Value.ToString();
                            }
                            if (kv.Key == "accept")
                            {
                                TimekeepingAccept_send.Text = kv.Value.ToString();
                            }
                        }


                        var dictPayroll_send = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataAppeal_send["payroll"].ToString());
                        foreach (var kv in dictPayroll_send)
                        {
                            Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);
                            if (kv.Key == "wait")
                            {
                                PayrollWait_send.Text = kv.Value.ToString();
                            }
                            if (kv.Key == "accept")
                            {
                                PayrollAccept_send.Text = kv.Value.ToString();
                            }
                        }
                    }
                    else
                    {
                        TimekeepingWait_send.Text = "0";
                        TimekeepingAccept_send.Text = "0";

                        PayrollWait_send.Text = "0";
                        PayrollAccept_send.Text = "0";
                    }
                }
                else
                {
                    TimekeepingWait_send.Text = "0";
                    TimekeepingAccept_send.Text = "0";

                    PayrollWait_send.Text = "0";
                    PayrollAccept_send.Text = "0";
                }
            }
            else
            {
                TimekeepingWait_send.Text = "0";
                TimekeepingAccept_send.Text = "0";

                PayrollWait_send.Text = "0";
                PayrollAccept_send.Text = "0";
            }

        }

        private void loadAppealReceive()
        {

            var dataEE_appeals_receive = ContentGlobal.allldata["appeals_receive"];

            if (dataEE_appeals_receive != null)
            {
                var dataAppeal_receive = dataEE_appeals_receive[year.ToString()][(selectedIndexMonth + 1).ToString() + "_total"];

                if (dataAppeal_receive != null)
                {
                    Debug.WriteLine("dataAppeal_receive-----------------------" + year + "_______" + month + "___________" + dataAppeal_receive);

                    var dictTimekeeping_receive = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataAppeal_receive["timekeeping"].ToString());
                    foreach (var kv in dictTimekeeping_receive)
                    {
                        Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);
                        if (kv.Key == "wait")
                        {
                            TimekeepingWait_receive.Text = kv.Value.ToString();
                        }
                        if (kv.Key == "accept")
                        {
                            TimekeepingAccept_receive.Text = kv.Value.ToString();
                        }
                        if (kv.Key == "decline")
                        {
                            TimekeepingDecline_receive.Text = kv.Value.ToString();
                        }
                    }


                    var dictPayroll_receive = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataAppeal_receive["payroll"].ToString());
                    foreach (var kv in dictPayroll_receive)
                    {
                        Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);
                        if (kv.Key == "wait")
                        {
                            PayrollWait_receive.Text = kv.Value.ToString();
                        }
                        if (kv.Key == "accept")
                        {
                            PayrollAccept_receive.Text = kv.Value.ToString();
                        }
                        if (kv.Key == "decline")
                        {
                            PayrollDecline_receive.Text = kv.Value.ToString();
                        }
                    }
                }
                else
                {
                    TimekeepingWait_receive.Text = "0";
                    TimekeepingAccept_receive.Text = "0";
                    TimekeepingDecline_receive.Text = "0";

                    PayrollWait_receive.Text = "0";
                    PayrollAccept_receive.Text = "0";
                    PayrollDecline_receive.Text = "0";
                }
            }
            else
            {
                TimekeepingWait_receive.Text = "0";
                TimekeepingAccept_receive.Text = "0";
                TimekeepingDecline_receive.Text = "0";

                PayrollWait_receive.Text = "0";
                PayrollAccept_receive.Text = "0";
                PayrollDecline_receive.Text = "0";
            }
           
        }
    }
}