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

namespace Aliswork.Personal
{
    public class LVIShift
    {
        public string Day { set; get; }
        public string ID { set; get; }
        public string NameShift { set; get; }
        public string TimeStart { set; get; }
        public string TimeEnd { set; get; }
        public string TimeCount { set; get; }
    }



    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssignPage : ContentPage
    {
        bool myshift = true;
        bool allshift = false;

        int year;
        int month;

        private ObservableCollection<LVIShift> Iitem;

        public AssignPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Personal.PreviewAssignPage());
            };

            var stkMyturn = new StackLayout
            {
                Margin = new Thickness(15, 0, 15, 0),
            };

            var stkAllturn = new StackLayout
            {
                Margin = new Thickness(15, 0, 15, 0),
            };


            var tapMyturn = new TapGestureRecognizer();
            tapMyturn.Tapped += (s, e) =>
            {

                Myturn.TextColor = Color.White;
                bvMyturn.BackgroundColor = Color.FromHex("#157EFB");

                Allturn.TextColor = Color.FromHex("#157EFB");
                bvAllturn.BackgroundColor = Color.White;

                myshift = true;
                allshift = false;

                Iitem.Clear();
                Iitem = loadShift();
                lvShift.ItemsSource = Iitem;

            };

            Myturn.GestureRecognizers.Add(tapMyturn);


            var tapAllturn = new TapGestureRecognizer();
            tapAllturn.Tapped += (s, e) =>
            {

                Allturn.TextColor = Color.White;
                bvAllturn.BackgroundColor = Color.FromHex("#157EFB");

                Myturn.TextColor = Color.FromHex("#157EFB");
                bvMyturn.BackgroundColor = Color.White;

                myshift = false;
                allshift = true;

                Iitem.Clear();
                Iitem = loadShift();
                lvShift.ItemsSource = Iitem;
            };

            Allturn.GestureRecognizers.Add(tapAllturn);





        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            year = (int)ContentGlobal.allldata["timeoff"]["year"];
            month = (int)ContentGlobal.allldata["timeoff"]["period"];

            Iitem = loadShift();
            lvShift.ItemsSource = Iitem;
        }

        private ObservableCollection<LVIShift> loadShift()
        {
            ObservableCollection<LVIShift> items = new ObservableCollection<LVIShift>();

            var dataEE_shifts = ContentGlobal.allldata["shifts"];

            if(dataEE_shifts!=null)
            {
                if (myshift == true)
                {
                    var dataEE_year = dataEE_shifts[year.ToString()];
                    if (dataEE_year != null)
                    {
                        var dataEE = dataEE_year[month.ToString()];

                        if (dataEE != null)
                        {
                            Debug.WriteLine("dataEE---------------------------------------------------" + dataEE);

                            var dict = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(dataEE.ToString());
                            foreach (var kv in dict)
                            {
                                Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);

                                TimeSpan timecount = (TimeSpan)kv.Value["time_end"] - (TimeSpan)kv.Value["time_start"];

                                items.Add(new LVIShift()
                                {
                                    Day = kv.Key,
                                    ID = (string)kv.Value["Id"],
                                    NameShift = (string)kv.Value["shiftname"],
                                    TimeStart = (string)kv.Value["time_start"],
                                    TimeEnd = " - " + (string)kv.Value["time_end"],
                                    TimeCount = "( " + timecount.Hours.ToString() + " giờ " + timecount.Minutes.ToString() + " phút )"
                                });
                            }

                        }
                    }
                }
                else
                {
                    string companyId = (string)ContentGlobal.allldata["info"]["company"]["id"];

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
            LVIShift lt = e.SelectedItem as LVIShift;

            Application.Current.Properties["ShiftId"] = lt.ID;
            Debug.WriteLine("Selected NumberId-----------------------------" + Application.Current.Properties["ShiftId"]);
            Navigation.PushAsync(new PreviewAssignPage());
            ((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
        }

    }
}