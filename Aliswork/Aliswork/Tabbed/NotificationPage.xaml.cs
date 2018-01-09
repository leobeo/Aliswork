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
    public class LVINotification
    {
        public string Id { set; get; }
        public string Day { set; get; }
        public string Title { set; get; }
        public FontAttributes Attribute { set; get; }
        public string Content { set; get; }
        public string Patch { set; get; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        bool boolPrivate = true;
        bool boolPublic = false;
        int selectedIndexMonth = -1;
        DateTime date = DateTime.Now;
        int year;
        int month;
        private ObservableCollection<LVINotification> Iitem;
        bool saveNoti = false;


        //Page pagePersion = new Personal.PersonalPage();

        public NotificationPage()
        {
            var stkPrivate = new StackLayout();
            var stkPublic = new StackLayout();

            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");


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
                Debug.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++changed month");
                selectedIndexMonth = pickerMonth.SelectedIndex;
                int tong= ContentGlobal.INTcountNotiUnread = ContentGlobal.CountNotiUnread("individualistic", selectedIndexMonth+1) + ContentGlobal.CountNotiUnread("public", selectedIndexMonth+1);
                if (tong > 0)
                {
                    DisplayAlert("Thông báo", string.Format("Tháng {0} bạn có {1} thông báo chưa đọc!",selectedIndexMonth+1,tong), "OK");
                }
                Iitem.Clear();
                Iitem = loadNotification();
                lvNotifications.ItemsSource = Iitem;
            };

            //stkContent.Children.Add(stkPrivate);


            var tapPrivate = new TapGestureRecognizer();
            tapPrivate.Tapped += (s, e) =>
            {
                if (boolPrivate == false)
                {
                    bvPrivate.BackgroundColor = Color.FromHex("#157EFB");
                    lbPrivate.FontAttributes = FontAttributes.Bold;
                    lbPrivate.TextColor = Color.White;

                    bvPublic.BackgroundColor = Color.White;
                    lbPublic.TextColor = Color.FromHex("#157EFB");
                    lbPublic.FontAttributes = FontAttributes.None;

                    boolPrivate = true;
                    boolPublic = false;

                    Iitem.Clear();
                    Iitem = loadNotification();
                    lvNotifications.ItemsSource = Iitem;

                }
            };
            lbPrivate.GestureRecognizers.Add(tapPrivate);

            var tapPublic = new TapGestureRecognizer();
            tapPublic.Tapped += (s, e) =>
            {
                if (boolPublic == false)
                {
                    bvPublic.BackgroundColor = Color.FromHex("#157EFB");
                    lbPublic.FontAttributes = FontAttributes.Bold;
                    lbPublic.TextColor = Color.White;

                    bvPrivate.BackgroundColor = Color.White;
                    lbPrivate.FontAttributes = FontAttributes.None;
                    lbPrivate.TextColor = Color.FromHex("#157EFB");

                    boolPublic = true;
                    boolPrivate = false;

                    Iitem.Clear();
                    Iitem = loadNotification();
                    lvNotifications.ItemsSource = Iitem;

                }
            };
            lbPublic.GestureRecognizers.Add(tapPublic);
        }

        private ObservableCollection<LVINotification> loadNotification()
        {
            ObservableCollection<LVINotification> items = new ObservableCollection<LVINotification>();

            var dataEE_notificaton = ContentGlobal.allldata["notificaton"];

            if(dataEE_notificaton!=null)
            {
                if (boolPrivate == true)
                {
                    var dataEE_notificaton_year = dataEE_notificaton["individualistic"][year.ToString()];
                    if (dataEE_notificaton_year != null)
                    {
                        var dataEE = dataEE_notificaton_year[(selectedIndexMonth + 1).ToString()+"_"+ year.ToString()];
                        if (dataEE != null)
                        {
                            Debug.WriteLine("dataEE-----------------------" + year + "__________________" + dataEE);

                            var dict = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(dataEE.ToString());
                            foreach (var kv in dict)
                            {
                                Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);

                                string formattedDate;
                                DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double)kv.Value["date"] / 1000).ToLocalTime();
                                formattedDate = String.Format("{0:MM/dd/yy H:mm}", dt);


                                FontAttributes att;
                                if ((int)kv.Value["state"] == 0)
                                {
                                    att = FontAttributes.Bold;
                                }
                                else
                                {
                                    att = FontAttributes.None;
                                }

                                string Shiftname = (string)kv.Value["shiftname"];

                                Debug.WriteLine("++++++++++++++++++++++addd+++++++++++++++++++++++" + kv.Key);
                                items.Add(new LVINotification()
                                {
                                    Id = (string)kv.Key,
                                    Day = formattedDate,
                                    Title = (string)kv.Value["title"],
                                    Attribute = att,
                                    Content = (string)kv.Value["content"],
                                    Patch = (string)kv.Value["path"]
                                });
                            }
                        }
                    }           
                }
                else
                {
                    var dataEE_public = dataEE_notificaton["public"];
                    if (dataEE_public != null)
                    {
                        var dataEE_public_year = dataEE_public[year.ToString()];

                        if (dataEE_public_year != null)
                        {
                            var dataEE =dataEE_public_year[(selectedIndexMonth + 1).ToString()+"_" + year.ToString()];
                            if (dataEE != null)
                            {
                                Debug.WriteLine("dataEE-----------------------" + year + "__________________" + dataEE);

                                var dict = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(dataEE.ToString());
                                foreach (var kv in dict)
                                {
                                    Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);

                                    string formattedDate;
                                    DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double)kv.Value["date"] / 1000).ToLocalTime();
                                    formattedDate = String.Format("{0:MM/dd/yy H:mm}", dt);


                                    FontAttributes att;
                                    if ((int)kv.Value["state"] == 0)
                                    {
                                        att = FontAttributes.Bold;
                                    }
                                    else
                                    {
                                        att = FontAttributes.None;
                                    }

                                    string Shiftname = (string)kv.Value["shiftname"];

                                    items.Add(new LVINotification()
                                    {
                                        Id = (string)kv.Key,
                                        Day = formattedDate,
                                        Title = (string)kv.Value["title"],
                                        Attribute = att,
                                        Content = (string)kv.Value["content"],
                                        Patch = (string)kv.Value["path"]
                                    });
                                }
                            }
                        }
                       
                    }
                }
            }


            items = new ObservableCollection<LVINotification>(items.OrderByDescending(x => x.Day).ToList());
            return items;
        }


        protected override void OnAppearing()
        {
            year = (int)ContentGlobal.allldata["timeoff"]["year"];
            month = (int)ContentGlobal.allldata["timeoff"]["period"];

            Iitem = loadNotification();
            lvNotifications.ItemsSource = Iitem;
        }

        async void OnRefresh(object sender, EventArgs e)
        {
            if (ContentGlobal.statusNetwork().Equals("Connected") != false)
            {
                try
                {
                    ContentGlobal.allldata = await ContentGlobal.GetAllOfPersonal();
                    //ContentGlobal.loadListRequest();
                    var list = (ListView)sender;
                    //put your refreshing logic here
                    Iitem.Clear();
                    Iitem = loadNotification();
                    lvNotifications.ItemsSource = Iitem;
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

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }

            saveNoti = true;
            LVINotification lt = e.SelectedItem as LVINotification;


            string id = lt.Id;

            var item = Iitem.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                item.Attribute =FontAttributes.None;
            };

            /*int type;
            if (boolPrivate == true)
            {
                type = 1;
                Application.Current.Properties["type_notification"] = 2;
            }
            else
            {
                type = 0;
                Application.Current.Properties["type_notification"] = 1;
            }*/

            /*string strSub = "UpdateNotification";
            string strParam = @"{""uid"":""" + Application.Current.Properties["uId"].ToString() + @""",""type"":" + type + @",""year"":" + year + @",""period"":" + (selectedIndexMonth + 1) + @",""id"":""" + id + @""",""state"":1}";

            var dataEE = ContentGlobal.FirebasePUTFunctions(strSub, strParam);

            Debug.WriteLine("dataEE------------------------------------------------------------" + dataEE);*/


            /*Application.Current.Properties["id_notification"] = id;
            Application.Current.Properties["month_notification"] = selectedIndexMonth + 1;*/

            DisplayAlert(lt.Title,string.Format(lt.Content), "OK");

            

            if (boolPrivate == true)
            {
                ContentGlobal.allldata["notificaton"]["individualistic"][year.ToString()][(selectedIndexMonth + 1).ToString()+"_"+year.ToString()][id]["state"] = 1;
            }
            else
            {
                ContentGlobal.allldata["notificaton"]["public"][year.ToString()][(selectedIndexMonth + 1).ToString() + "_" + year.ToString()][id]["state"] = 1;
            }

            Iitem.Clear();
            Iitem = loadNotification();
            lvNotifications.ItemsSource = Iitem;




            //Navigation.PushAsync(new Account.ContentNotificationPage());

            /*Application.Current.Properties["Patch"] = lt.Patch;
            Debug.WriteLine("Selected Patch" + Application.Current.Properties["Patch"]);
            if ((string)Application.Current.Properties["Patch"] == "R")
            {
                Navigation.PushAsync(new Requests.RequestSendPage());
            }
            else if ((string)Application.Current.Properties["Patch"] == "A")
            {
                Navigation.PushAsync(new Appeal.AppealAssignPage());
            }
            else
            {
                DisplayAlert("False", "you clicked to empty page", "OK");
            }*/

            ((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
        }

        protected override void OnDisappearing()
        {
            Debug.WriteLine("close Notification-----------------------------------------------------------");

            var month = (int)ContentGlobal.allldata["timeoff"]["period"];
            ContentGlobal.INTcountNotiUnread = ContentGlobal.CountNotiUnread("individualistic", month) + ContentGlobal.CountNotiUnread("public", month);

            if (saveNoti == true)
            {
                if (ContentGlobal.statusNetwork().Equals("Connected") != false)
                {
                    try
                    {
                        var data = ContentGlobal.allldata["notificaton"];
                        Debug.WriteLine("date save---------------" + data);

                        string strSub = "UpdateNotification";
                        string strParam = @"{""uid"":""" + Application.Current.Properties["uId"].ToString() + @""",""content"":" + data + @"}";

                        Debug.WriteLine("strParam-------------------------------------------------" + strParam);

                        var dataEE = ContentGlobal.FirebasePUTFunctions(strSub, strParam);

                        Debug.WriteLine("dataEE----------------------------------" + dataEE);

                        ContentGlobal.saveSetting = false;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Err Notifiaction----------------------------------------" + ex);
                    }
                }     
            }
        }
    }
}