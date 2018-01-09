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

namespace Aliswork.Timekeeping
{
    public class LVsearItem
    {
        public string IconSource { set; get; }
        public string Name { get; set; }

        public string TextColor { set; get; }

        public string TimeIn { set; get; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WhoWorkingPage : ContentPage
    {
        private ObservableCollection<LVsearItem> Iitem;

        public WhoWorkingPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
            // #676f7a
            // #57cf85
           
/*
            items.Add(new LVsearItem()
            {
                IconSource = "checked.png",
                TextColor = "#57cf85",
                Name = "Nguyễn Văn C",
                TimeIn = "07:58",
            });
            items.Add(new LVsearItem()
            {
                IconSource = "checked.png",
                TextColor = "#57cf85",
                Name = "Trần Văn A",
                TimeIn = "07:58",
            });
            items.Add(new LVsearItem()
            {
                IconSource = "",
                TextColor = "#676f7a",
                Name = "Nguyễn Thị A",
                TimeIn = "07:58",
            });
            items.Add(new LVsearItem()
            {
                IconSource = "checked.png",
                TextColor = "#57cf85",
                Name = "Nguyễn Văn Cầm A",
                TimeIn = "07:58",
            });
            items.Add(new LVsearItem()
            {
                IconSource = "",
                TextColor = "#676f7a",
                Name = "Nguyễn Văn A",
                TimeIn = "07:58",
            });
            Iitem = items;
            lvSearch.ItemsSource = Iitem;*/
        }

        protected async override void OnAppearing()
        {
            ObservableCollection<LVsearItem> items = new ObservableCollection<LVsearItem>();

                try
                {
                    var data =ContentGlobal.PeopleWorking;

                    if (data != null)
                    {
                        var dict = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(data.ToString());
                        foreach (var kv in dict)
                        {
                            string Name = ContentGlobal.allldata["info"]["company_people"][kv.Key.ToString()]["name"].ToString();
                            string time_in = kv.Value["time_in"].ToString();
                            items.Add(new LVsearItem()
                            {
                                IconSource = "checked.png",
                                TextColor = "#57cf85",
                                Name = Name,
                                TimeIn = time_in,
                            });
                        }
                    }

                    Iitem = items;
                    lbCountPeople.Text = Iitem.Count.ToString() + " người đang làm việc";
                    lvSearch.ItemsSource = Iitem;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Thông báo", "Có lỗi đã sảy ra, thử lại sau!", "OK");
                    Debug.WriteLine("Er PersionPage Sync---------------------------------------" + ex);
                }
        }

        async void OnRefresh(object sender, EventArgs e)
        {
            ObservableCollection<LVsearItem> items = new ObservableCollection<LVsearItem>();

            if (ContentGlobal.statusNetwork().Equals("Connected") != false)
            {
                try
                {
                    var data = await ContentGlobal.peopleWorkingAsync();

                    if (data != null)
                    {
                        var dict = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(data.ToString());
                        foreach (var kv in dict)
                        {
                            string Name = ContentGlobal.allldata["info"]["managed"][kv.Key.ToString()]["name"].ToString();
                            string time_in = kv.Value["time_in"].ToString();
                            items.Add(new LVsearItem()
                            {
                                IconSource = "checked.png",
                                TextColor = "#57cf85",
                                Name = Name,
                                TimeIn = time_in,
                            });
                        }
                    }
                    Iitem.Clear();
                    Iitem = items;
                    lbCountPeople.Text = Iitem.Count.ToString() + " người đang làm việc";
                    lvSearch.ItemsSource = Iitem;

                    lvSearch.IsRefreshing = false;
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

        private void FilterNames()
        {
            string filter = searchPeople.Text;
            lvSearch.BeginRefresh();
            if (string.IsNullOrWhiteSpace(filter))
            {
                lvSearch.ItemsSource = Iitem;
            }
            else
            {
                lvSearch.ItemsSource = Iitem.Where(x => x.Name.ToLower().Contains(filter.ToLower()));
            }
            lvSearch.EndRefresh();
        }
        void OnSearchBarTextChanged(object sender, TextChangedEventArgs args)
        {
            FilterNames();
        }
        void OnSearchBarButtonPressed(object sender, EventArgs args)
        {
            FilterNames();
        }

        private ObservableCollection<LVsearItem> loadPeopleWorking()
        {
            ObservableCollection<LVsearItem> items = new ObservableCollection<LVsearItem>();



            return items;
        }
    }
}