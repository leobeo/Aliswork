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
    public class LVTimekepping
    {
        public int Day { set; get; }
        public string TimeIn { set; get; }
        public string TimeOut { set; get; }
        public string ShiftName { set; get; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllTimekeepingPage : ContentPage
    {
        int year, month;
        int selectedIndexMonth = -1;

        private ObservableCollection<LVTimekepping> Iitem;
        public AllTimekeepingPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
            ObservableCollection<LVTimekepping> items = new ObservableCollection<LVTimekepping>();

            List<String> months = ContentGlobal.ListMonth;
            foreach (String nn in months)
            {
                pickerMonth.Items.Add(nn);
            }

            year = (int)ContentGlobal.allldata["timeoff"]["year"];
            month = (int)ContentGlobal.allldata["timeoff"]["period"];

            selectedIndexMonth = month - 1;

            pickerMonth.SelectedIndex = month - 1;

            pickerMonth.SelectedIndexChanged += (sender, e) =>
            {
                selectedIndexMonth = pickerMonth.SelectedIndex;
                Iitem.Clear();
                Iitem = loadTimekeeping();
                lvTimekeeping.ItemsSource = Iitem;
            };



        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            Iitem = loadTimekeeping();
            lvTimekeeping.ItemsSource = Iitem;
        }

        private ObservableCollection<LVTimekepping> loadTimekeeping()
        {
            ObservableCollection<LVTimekepping> items = new ObservableCollection<LVTimekepping>();

            var dataEE_timekeeping = ContentGlobal.allldata["timekeeping"];

            if (dataEE_timekeeping != null)
            {
                var dataEE_timekeeping_year = dataEE_timekeeping[year.ToString()];

                if (dataEE_timekeeping_year != null)
                {
                    var dataEE_timekeeping_month = dataEE_timekeeping_year[(selectedIndexMonth + 1).ToString() + "_" + year.ToString()];

                    if (dataEE_timekeeping_month != null)
                    {
                        Debug.WriteLine("dataEE-----------------------" + year + "_______" + month + "___________" + dataEE_timekeeping_month);

                        var dict = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(dataEE_timekeeping_month.ToString());
                        foreach (var kv in dict)
                        {
                            Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);

                            string Time_in = (string)kv.Value["time_in"];
                            string Time_out = "";
                            if (kv.Value["time_out"] != null)
                            {
                                Time_out = (string)kv.Value["time_out"];
                            }

                            string Shiftname = (string)kv.Value["shiftname"];
                            var subday = kv.Key.Split('_')[0];
                            items.Add(new LVTimekepping()
                            {
                                Day = int.Parse(subday),
                                TimeIn = Time_in,
                                TimeOut = Time_out,
                                ShiftName = Shiftname,
                            });
                        }

                    }
                }
            }
            items = new ObservableCollection<LVTimekepping>(items.OrderBy(x => x.Day).ToList());
            return items;
        }
    }
}