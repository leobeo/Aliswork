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

namespace Aliswork.Timekeeping
{
    public class IitemTimesheets
    {
        public string TimesheetsID { set; get; }
        public string Name { set; get; }
        public string Group { set; get; }
        public int State { set; get; }
        public string Date { set; get; }
        public string Dayoff { set; get; }
        public string Later { set; get; }
        public string Early { set; get; }
        public string ForgetKepping { set; get; }
        public string ManDay { set; get; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreviewTimeKeepingPage : ContentPage
    {
        int selectedPeople = -1;
        int selectedIndexMonth = -1;

        int year, month;

        List<IitemTimesheets> listTimeSheets;

        //DateTime date = DateTime.Now;

        public PreviewTimeKeepingPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");

            List<String> listmonth = ContentGlobal.ListMonth;
            foreach (String nn in listmonth)
            {
                pickerMonth.Items.Add(nn);
            }

            month = (int)ContentGlobal.allldata["timeoff"]["period"];

            selectedIndexMonth = month - 1;
            pickerMonth.SelectedIndex = selectedIndexMonth;

            pickerMonth.SelectedIndexChanged += (sender, e) =>
            {
                selectedIndexMonth = pickerMonth.SelectedIndex;
                listTimeSheets.Clear();
                listTimeSheets = loadTimeSheet();
                loadPeople(0);
            };

            var tapPrev = new TapGestureRecognizer();
            tapPrev.Tapped += (s, e) =>
            {
                Debug.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++  Prev People");
                if (selectedPeople > 0)
                {
                    selectedPeople--;
                }
                else
                {
                    selectedPeople = listTimeSheets.Count - 1;
                }
                loadPeople(selectedPeople);
            };
            stkPrev.GestureRecognizers.Add(tapPrev);

            //bvPrev.GestureRecognizers.Add(tapPrev);

            var tapNext = new TapGestureRecognizer();
            tapNext.Tapped += (s, e) =>
            {
                Debug.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ Next People");
                if (selectedPeople < listTimeSheets.Count() - 1)
                {
                    selectedPeople++;
                }
                else
                {
                    selectedPeople = 0;
                }
                loadPeople(selectedPeople);
            };

            stkNext.GestureRecognizers.Add(tapNext);
            //vNext.GestureRecognizers.Add(tapNext);
        }

        protected override void OnAppearing()
        {
            listTimeSheets = loadTimeSheet();
            year = (int)ContentGlobal.allldata["timeoff"]["year"];
            month = (int)ContentGlobal.allldata["timeoff"]["period"];

            loadPeople(0);

        }

        private List<IitemTimesheets> loadTimeSheet()
        {
            List<IitemTimesheets> listtimesheets = new List<IitemTimesheets>();

            var dataEE_timesheets_assign = ContentGlobal.allldata["timesheets_assign"];

            if(dataEE_timesheets_assign!=null)
            {
                var dataEE_year = dataEE_timesheets_assign[year.ToString()];
                if (dataEE_year != null)
                {
                    var dataEE = dataEE_year[(selectedIndexMonth + 1).ToString()];
                    if (dataEE != null)
                    {
                        Debug.WriteLine("dataEE-----------------------" + year + "---------------" + month + "-------------" + dataEE);

                        var dict = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(dataEE.ToString());
                        foreach (var kv in dict)
                        {
                            Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);

                            listtimesheets.Add(new IitemTimesheets
                            {
                                TimesheetsID = (string)kv.Key,
                                Name = (string)kv.Value["people_assign"],
                                Group = (string)kv.Value["group"],
                                State = (int)kv.Value["state"],
                                Date = (string)kv.Value["date"],
                                Dayoff = (string)kv.Value["time_dayoff"],
                                Later = (string)kv.Value["time_later"],
                                Early = (string)kv.Value["time_early"],
                                ForgetKepping = (string)kv.Value["time_forget_keeping"],
                                ManDay = (string)kv.Value["man_day"]
                            });
                        }
                    }
                }
            }
            return listtimesheets;
        }

        private void loadPeople(int stt)
        {
            if (listTimeSheets.Count > 0)
            {
                IitemTimesheets it = listTimeSheets[stt];
                NamePeople.Text = it.Name;
                Group.Text = it.Group;
                DateAccept.Text = it.Date;
                DateOff.Text = it.Dayoff;
                Late.Text = it.Later;
                Early.Text = it.Early;
                Forget_keeping.Text = it.ForgetKepping;
                ManDay.Text = it.ManDay;

                if (it.State == 0)
                {
                    btnAccept.IsEnabled = true;
                    btnAccept_All.IsEnabled = true;
                }
                else
                {
                    btnAccept.IsEnabled = false;
                    btnAccept_All.IsEnabled = false;
                }
            }
            else
            {
                NamePeople.Text = "";
                Group.Text = "";
                DateAccept.Text = "";
                DateOff.Text = "";
                Late.Text = "";
                Early.Text = "";
                Forget_keeping.Text = "";
                ManDay.Text = "";

                btnAccept.IsEnabled = false;
                btnAccept_All.IsEnabled = false;
            }

        }

        private async Task btnAcceptClicked(object sender, EventArgs e)
        {
            if (ContentGlobal.statusNetwork().Equals("Connected") != false)
            {
                var answer = await DisplayAlert("Phiếu chấm công", "Bạn xác nhận duyệt phiếu chấm công này", "Yes", "No");
                if (answer == true)
                {
                    try
                    {
                        string strSub = "UpdateTimeSheets";

                        string uid = Application.Current.Properties["uId"].ToString();

                        string timesheets_id = listTimeSheets[selectedPeople].TimesheetsID;
                        Debug.WriteLine("timesheets_id--------------------------------------------" + timesheets_id);

                        Debug.WriteLine("year--------------------------------------------" + year);

                        int period = selectedIndexMonth + 1;
                        Debug.WriteLine("period--------------------------------------------" + period);


                        string strParam = @"{""uid"":""" + uid + @""",""timesheets_id"":""" + timesheets_id + @""",""year"":" + year + @",""period"":" + period + @",""state"": 2}";

                        Debug.WriteLine("strParam-------------------------" + strParam);

                        var data = await ContentGlobal.FirebasePOSTFunctions(strSub, strParam);

                        listTimeSheets = loadTimeSheet();
                        loadPeople(selectedPeople + 1);
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Thông báo", "Có lỗi đã sảy ra, thử lại sau!", "OK");
                        Debug.WriteLine("Er PreviewTimekeeping---------------------------------------" + ex);
                    }
                }
            }
            else
            {
                await DisplayAlert("Cảnh báo", "Kiểm tra lại kết nối mạng của bạn và thử lại sau", "OK");
            }
        }

        private async Task btnAcceptAllClicked(object sender, EventArgs e)
        {
            if (ContentGlobal.statusNetwork().Equals("Connected") != false)
            {
                var answer = await DisplayAlert("Phiếu chấm công", "Bạn xác nhận duyệt tất cả các phiếu chấm công", "Yes", "No");
                if (answer == true)
                {
                    try
                    {
                        int sl = 0;
                        for (int i = 0; i < listTimeSheets.Count; i++)
                        {
                            IitemTimesheets it = listTimeSheets[i];

                            if (it.State == 0)
                            {
                                string strSub = "UpdateTimeSheets";

                                string uid = Application.Current.Properties["uId"].ToString();

                                string timesheets_id = it.TimesheetsID;
                                Debug.WriteLine("timesheets_id--------------------------------------------" + timesheets_id);

                                Debug.WriteLine("year--------------------------------------------" + year);

                                int period = selectedIndexMonth + 1;
                                Debug.WriteLine("period--------------------------------------------" + period);


                                string strParam = @"{""uid"":""" + uid + @""",""timesheets_id"":""" + timesheets_id + @""",""year"":" + year + @",""period"":" + period + @",""state"": 2}";

                                Debug.WriteLine("strParam-------------------------" + strParam);

                                var data = await ContentGlobal.FirebasePOSTFunctions(strSub, strParam);

                                sl++;
                            }
                        }

                        await DisplayAlert("Hoàn thành", sl + "/" + listTimeSheets.Count + " phiếu chấm công đã được duyệt", "OK");

                        listTimeSheets = loadTimeSheet();
                        loadPeople(selectedPeople + 1);

                        await Navigation.PopAsync();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Thông báo", "Có lỗi đã sảy ra, thử lại sau!", "OK");
                        Debug.WriteLine("Err PreviewTimeKeeping All---------------------------------------" + ex);
                    }
                }
            }
            else
            {
                await DisplayAlert("Cảnh báo", "Kiểm tra lại kết nối mạng của bạn và thử lại sau", "OK");
            }
        }
    }
}