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

namespace Aliswork.Requests
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestAssignPage : ContentPage
    {
        DateTime date = DateTime.Now;
        int month;
        int year;

        private ObservableCollection<LVItemRequest> Iitem;

        //Page pageRequestAssign = ;
        public RequestAssignPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            year = (int)ContentGlobal.allldata["timeoff"]["year"];
            month = (int)ContentGlobal.allldata["timeoff"]["period"];

            Iitem = loadRequests();
            lvSearch.ItemsSource = Iitem;
        }

        private ObservableCollection<LVItemRequest> loadRequests()
        {
            ObservableCollection<LVItemRequest> items = new ObservableCollection<LVItemRequest>();

            var dataEE_requests_receive = ContentGlobal.allldata["requests_receive"];

            if (dataEE_requests_receive != null)
            {
                var dataEE_year = dataEE_requests_receive[year.ToString()];

                if (dataEE_year != null)
                {
                    var dataEE = dataEE_year[month.ToString() + "_" + year.ToString()];
                    if(dataEE!=null)
                    {
                        Debug.WriteLine("---------------wgdhg c gidufge--------" + year + "_______" + month + "___________" + dataEE);

                        var dict = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(dataEE.ToString());
                        foreach (var kv in dict)
                        {
                            Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);

                            double timepreview = (double)kv.Value["date_preview"];
                            string formattedDate;
                            if (timepreview > 0)
                            {
                                DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timepreview / 1000).ToLocalTime();
                                formattedDate = "Ngày duyệt :" + String.Format("{0:dd/M/yyyy}", dt);
                            }
                            else
                            {
                                formattedDate = "";
                            }
                            double start_date = (double)kv.Value["start_date"];
                            string strstart_date = String.Format("{0:HH:mm dd/M/yyyy}", new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(start_date).ToLocalTime());

                            double end_date = (double)kv.Value["end_date"];
                            string strend_date = String.Format("{0:HH:mm dd/M/yyyy}", new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(end_date).ToLocalTime());

                            items.Add(new LVItemRequest()
                            {
                                NumberId = kv.Key,
                                IconSource = "congtac.png",
                                Name = (string)ContentGlobal.allldata["info"]["company_people"][(string)kv.Value["people_send"]]["name"],
                                TimeOff = strstart_date + " - " + strend_date,
                                TimePreview = formattedDate
                            });
                        }
                    }
                }
            }
            items = new ObservableCollection<LVItemRequest>(items.OrderByDescending(x => x.NumberId).ToList());
            return items;
        }
        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            LVItemRequest lt = e.SelectedItem as LVItemRequest;

            Application.Current.Properties["NumberId"] = lt.NumberId;
            Debug.WriteLine("Selected NumberId" + Application.Current.Properties["NumberId"]);
            Navigation.PushAsync(new Requests.PreviewRequestAssignPage());
            ((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
        }

        async void OnRefresh(object sender, EventArgs e)
        {
            if (ContentGlobal.statusNetwork().Equals("Connected") != false)
            {
                try
                {
                    ContentGlobal.allldata = await ContentGlobal.GetAllOfPersonal();
                    ContentGlobal.loadListRequest();

                    var list = (ListView)sender;
                    //put your refreshing logic here
                    /*var itemList = Iitem.Reverse().ToList();
                    Iitem.Clear();
                    foreach (var s in itemList)
                    {
                        Iitem.Add(s);
                    }*/
                    Iitem.Clear();
                    Iitem = loadRequests();
                    lvSearch.ItemsSource = Iitem;
                    //make sure to end the refresh state
                    list.IsRefreshing = false;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Thông báo", "Có lỗi đã sảy ra, thử lại sau!", "OK");
                    Debug.WriteLine("Er PersionPage Sync---------------------------------------" + ex);
                }
            }
            else
            {
                await DisplayAlert("Cảnh báo", "Kiểm tra lại kết nối mạng của bạn và thử lại sau", "OK");
            }
            
        }
    }
}