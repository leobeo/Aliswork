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

namespace Aliswork.Appeal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppealAssignPage : ContentPage
    {
        DateTime date = DateTime.Now;
        int month;
        int year;

        private ObservableCollection<LVItemAppeal> Iitem;
        Page pageAppealAssign = new Appeal.PreviewAppealAssignPage();
        public AppealAssignPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            year = (int)ContentGlobal.allldata["timeoff"]["year"];
            month = (int)ContentGlobal.allldata["timeoff"]["period"];

            Iitem = loadAppeal();
            lvSearch.ItemsSource = Iitem;
        }
        private ObservableCollection<LVItemAppeal> loadAppeal()
        {
            ObservableCollection<LVItemAppeal> items = new ObservableCollection<LVItemAppeal>();

            var dataEE_appeals_receive = ContentGlobal.allldata["appeals_receive"];
            if (dataEE_appeals_receive != null)
            {
                var dataEE = dataEE_appeals_receive[year.ToString()][month.ToString()];

                if (dataEE != null)
                {

                    Debug.WriteLine("dataEE-----------------------" + year + "_______" + month + "___________" + dataEE);

                    var dict = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(dataEE.ToString());
                    foreach (var kv in dict)
                    {
                        Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);


                        double timepreview = (double)kv.Value["date_preview"];
                        string formattedDate;
                        if (timepreview > 0)
                        {
                            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timepreview / 1000).ToLocalTime();
                            formattedDate = "Ngày duyệt :" + String.Format("{0:dd/MM/yyyy}", dt);
                        }
                        else
                        {
                            formattedDate = "";
                        }
                        items.Add(new LVItemAppeal()
                        {
                            NumberId = kv.Key,
                            IconSource = "payroll.png",
                            Name = (string)ContentGlobal.allldata["info"]["managed"][(string)kv.Value["people_send"]]["name"],
                            TimePreview = formattedDate
                        });
                    }
                }
            }
            return items;
        }

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            LVItemAppeal lt = e.SelectedItem as LVItemAppeal;

            Application.Current.Properties["NumberId"] = lt.NumberId;
            Debug.WriteLine("Selected NumberId" + Application.Current.Properties["NumberId"]);
            Navigation.PushAsync(pageAppealAssign);
            ((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
        }

        async void OnRefresh(object sender, EventArgs e)
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
            Iitem = loadAppeal();
            lvSearch.ItemsSource = Iitem;
            //make sure to end the refresh state
            list.IsRefreshing = false;
        }
    }
}