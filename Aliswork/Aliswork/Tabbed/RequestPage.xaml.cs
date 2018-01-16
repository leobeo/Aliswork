using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aliswork.Tabbed
{
    public class LVIListRequest
    {
        public string NameRequest { set; get; }
        public string Accept { set; get; }
        public string Decline { set; get; }
        public string Wait { set; get; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestPage : ContentPage
    {
        //Page pagePersion = ;
        //Page pageNewRequest = ;
        DateTime date = DateTime.Now;
        int year;
        int month;
        private ObservableCollection<LVIListRequest> Iitem_Send;
        private ObservableCollection<LVIListRequest> Iitem_Receive;

        string selectedItemYear = "";
        int selectedIndexMonth = -1;
        public RequestPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");

            if (Device.RuntimePlatform == Device.iOS)
            {
                stkHeader.Margin = new Thickness(0, 20, 0, 0);
            }

            List<String> years = ContentGlobal.ListYear;
            foreach (String nn in years)
            {
                pickerYear.Items.Add(nn);
            }

            pickerYear.SelectedIndex = (int)ContentGlobal.allldata["timeoff"]["year"];
            pickerYear.SelectedIndexChanged += (sender, e) =>
            {
                selectedItemYear = pickerYear.Items[pickerYear.SelectedIndex];

                Iitem_Send.Clear();
                Iitem_Send = loadRequestSend();
                lvRequest_Send.ItemsSource = Iitem_Send;

                lvRequest_Send.HeightRequest = Iitem_Send.Count * 40 + (Iitem_Send.Count - 2) * 2;
                //loadRequestSend();
            };

            List<String> months = ContentGlobal.ListMonth;
            foreach (String nn in months)
            {
                pickerMonth.Items.Add(nn);
            }

            month = (int)ContentGlobal.allldata["timeoff"]["period"];
            selectedIndexMonth = month - 1;
            pickerMonth.SelectedIndex = selectedIndexMonth;
            pickerMonth.SelectedIndexChanged += (sender, e) =>
            {
                selectedIndexMonth = pickerMonth.SelectedIndex;

                Iitem_Receive.Clear();
                Iitem_Receive = loadRequestReceive();
                lvRequest_Receive.ItemsSource = Iitem_Receive;

                lvRequest_Receive.HeightRequest = Iitem_Receive.Count * 40 + (Iitem_Receive.Count - 2) * 2;

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
            //imgtoNotification.GestureRecognizers.Add(tapimgtoNotification);
            grdNoti.GestureRecognizers.Add(tapimgtoNotification);

            var tapNew = new TapGestureRecognizer();
            tapNew.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Requests.NewRequestPage());
            };
            //imgToNewRequest.GestureRecognizers.Add(tapNew);
            grdNewRequest.GestureRecognizers.Add(tapNew);


            var tapHistory = new TapGestureRecognizer();
            tapHistory.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Requests.RequestSendPage());
            };
            //imgToRequestPage.GestureRecognizers.Add(tapHistory);
            grdRequestPage.GestureRecognizers.Add(tapHistory);

            var tapAssign = new TapGestureRecognizer();
            tapAssign.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Requests.RequestAssignPage());
            };
            //imgToPreviewRequestAssign.GestureRecognizers.Add(tapAssign);
            grdPreviewRequestAssign.GestureRecognizers.Add(tapAssign);


        }
        protected override void OnAppearing()
        {
            stkHeader.IsVisible = true;

            if (ContentGlobal.INTcountNotiUnread > 0)
            {
                frcountNotiUnread.IsVisible = true;
                lbcountNotiUnread.Text = ContentGlobal.INTcountNotiUnread.ToString();
            }
            else
            {
                frcountNotiUnread.IsVisible = false;
            }

            year = (int)ContentGlobal.allldata["timeoff"]["year"];
            month = (int)ContentGlobal.allldata["timeoff"]["period"];

            selectedItemYear = year.ToString();

            Iitem_Send = loadRequestSend();
            lvRequest_Send.ItemsSource = Iitem_Send;
            lvRequest_Send.HeightRequest = Iitem_Send.Count * 40 + (Iitem_Send.Count-2) * 2;

            if (ContentGlobal.Userroot == false)
            {
                stkRequestAssign.IsVisible = false;
                grdPreviewRequestAssign.IsVisible = false;
            }
            else
            {
                stkRequestAssign.IsVisible = true;
                grdPreviewRequestAssign.IsVisible = true;
                Iitem_Receive = loadRequestReceive();
                lvRequest_Receive.ItemsSource = Iitem_Receive;
                lvRequest_Receive.HeightRequest = Iitem_Receive.Count * 40 + (Iitem_Receive.Count-2) * 2;
            }
        }

        private ObservableCollection<LVIListRequest> loadRequestSend()
        {
            ObservableCollection<LVIListRequest> items = new ObservableCollection<LVIListRequest>();

            var dataEE_requests = ContentGlobal.allldata["requests"];
            if (dataEE_requests != null)
            {
                Debug.WriteLine("dataEE_requests-------------------------------------------------------" + dataEE_requests);

                Debug.WriteLine("selectedItemYear------------------------------------------------------" + selectedItemYear);
                var dataRequest_send_year = dataEE_requests[selectedItemYear];

                if (dataRequest_send_year != null)
                {
                    Debug.WriteLine("dataRequest_send_year----------------------------------------------" + dataRequest_send_year);

                    var dataRequest_send = dataRequest_send_year["total"];
                    if (dataRequest_send != null)
                    {
                        Debug.WriteLine("dataRequest_send-------------------------------------------" + dataRequest_send);

                        var dictTimeOff_send = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(dataRequest_send.ToString());
                        foreach (var kv in dictTimeOff_send)
                        {
                            Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);
                            items.Add(new LVIListRequest()
                            {
                                NameRequest=kv.Key,
                                Accept=(string)kv.Value["accept"],
                                Decline=(string)kv.Value["decline"],
                                Wait=(string)kv.Value["wait"]
                            });

                        }
                    }

                }

            }

            return items;
        }

        private ObservableCollection<LVIListRequest> loadRequestReceive()
        {
            ObservableCollection<LVIListRequest> items = new ObservableCollection<LVIListRequest>();

            var dataEE_requests_receive = ContentGlobal.allldata["requests_receive"];

            if (dataEE_requests_receive != null)
            {
                var dataRequest_receive_year = dataEE_requests_receive[year.ToString()];   
                if (dataRequest_receive_year != null)
                {
                    Debug.WriteLine("dataRequest_receive_year----------------------------------------" + dataRequest_receive_year);
                    var dataRequest_receive = dataRequest_receive_year[(selectedIndexMonth + 1).ToString() + "_total"];
                   
                    if (dataRequest_receive != null)
                    {
                        Debug.WriteLine("dataRequest_receive-----------------------" + year + "_______" + month + "___________" + dataRequest_receive);
                        var dictTimeOff_receive = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(dataRequest_receive.ToString());
                        foreach (var kv in dictTimeOff_receive)
                        {
                            items.Add(new LVIListRequest()
                            {
                                NameRequest = kv.Key,
                                Accept = (string)kv.Value["accept"],
                                Decline = (string)kv.Value["decline"],
                                Wait = (string)kv.Value["wait"]
                            });
                        }
                    }
                   
                }
            }

            return items;
        }
    }
}