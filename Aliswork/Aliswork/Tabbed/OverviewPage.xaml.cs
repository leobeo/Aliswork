using Plugin.DeviceInfo;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using Plugin.Permissions.Abstractions;
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
    public partial class OverviewPage : ContentPage
    {
        bool boolAnimation = false;
        /* Page pagePersion = new Personal.PersonalPage();
         Page pageTotalFinger = new Timekeeping.AllTimekeepingPage();
         Page pageWhoWorking = new Timekeeping.WhoWorkingPage();
         Page pageOneFinger = new Timekeeping.OneTimekeepingPage();*/
        public OverviewPage()
        {
            InitializeComponent();

            if ((int)Application.Current.Properties["from_noti"] == 1)
            {
                Debug.WriteLine("from_noti---------------------------------------");
                DisplayAlert(Application.Current.Properties["title_noti"].ToString(), Application.Current.Properties["content_noti"].ToString(), "OK");
            }

            this.BackgroundColor = Color.FromHex("#F0EFF5");

            if (Device.RuntimePlatform == Device.iOS)
            {
                stkHeader.Margin = new Thickness(0, 20, 0, 0);
            }

            if (ContentGlobal.Userroot == false)
            {
                stkWorking.IsVisible = false;
                stkAccept.IsVisible = false;
            }
            else
            {
                stkWorking.IsVisible = true;
                stkAccept.IsVisible = true;
            }

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Timekeeping.AllTimekeepingPage());
            };
            toTotalFinger.GestureRecognizers.Add(tapGestureRecognizer);

            var tapimgWhoWorking = new TapGestureRecognizer();
            tapimgWhoWorking.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Timekeeping.WhoWorkingPage());
            };
            stkWorking.GestureRecognizers.Add(tapimgWhoWorking);

            var tapAccept = new TapGestureRecognizer();
            tapAccept.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Timekeeping.PreviewTimeKeepingPage());
            };
            stkAccept.GestureRecognizers.Add(tapAccept);

            var tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Timekeeping.OneTimekeepingPage());
            };
            toOneFinger.GestureRecognizers.Add(tapGestureRecognizer1);

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


            var tapImgCircle = new TapGestureRecognizer();
            tapImgCircle.Tapped += async (s, e) =>
            {
                if (boolAnimation == false)
                {
                    // await DisplayAlert("Info", await takeImageAsync(), "OK");

                    // if (await takeImageAsync() == "1")
                    // {

                    var answer = await DisplayAlert("Checkin", "Bạn có muốn bắt đầu ca làm việc ngay lúc này!", "Yes", "No");
                    if (answer == true)
                    {
                        try
                        {
                            var hasPermission = await Utils.CheckPermissions(Permission.Location);
                            if (!hasPermission)
                            {
                                Debug.WriteLine("hasPermission---------------------------------------------");
                                return;
                            }

                            var position = await ContentGlobal.getLocation();

                            if (position == null)
                            {
                                await DisplayAlert("Thông báo", "Không thể xác định vị trí lúc này, thử lại sau!", "OK");
                                return;
                            }

                            Debug.WriteLine("Position Status: Latitude ------------------------------------------" + position.Latitude);
                            Debug.WriteLine("Position Status: Longitude ------------------------------------------" + position.Longitude);

                            //await DisplayAlert("Location", string.Format("Latitude :{0} \nLongitude:{1}", position.Latitude, position.Longitude), "OK");

                            var strLatitude = position.Latitude.ToString().Replace(",", ".");
                            var strLongitude = position.Longitude.ToString().Replace(",", ".");
                            var device_id= CrossDeviceInfo.Current.Id;
                            string strSubFirebase = "CheckOdooByApp";
                            string strParam = @"{""uid"":""" + Application.Current.Properties["uId"].ToString() + @""",""location"":""" + strLatitude + @"," + strLongitude + @""",""device_id"":"""+ device_id+ @"""}";

                            Debug.WriteLine("strParam CheckOdooByApp-------------------------------------------------- " + strParam);

                            var data = await ContentGlobal.FirebasePOSTFunctions(strSubFirebase, strParam);

                            if ((string)data["return"] == "OK")
                            {
                                ContentGlobal.allldata = await ContentGlobal.GetAllOfPersonal();

                                OnAppearing();

                                await DisplayAlert("Thông báo", "Bạn đã bắt đầu ca làm việc", "OK");

                                /*boolAnimation = true;
                                int totalminute = 12 * 60;
                                await imgRound.RotateTo(360 * totalminute, (uint)totalminute * 36000);*/
                            }
                            else
                            {
                                await DisplayAlert("Thông báo", "Không thể thực hiện tác vụ này, xin thử lại sau!", "OK");
                            }

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Error start Circle" + ex.ToString());
                            await DisplayAlert("Thông báo", "Có lỗi sảy ra, kiểm tra lại kết nối mạng hoặc cài đặt vị trí của bạn và thử lại sau !", "OK");
                        }
                    }
                    //}
                }
                else
                {
                    //await DisplayAlert("Info", await takeImageAsync(), "OK");

                    var answer = await DisplayAlert("Checkout", "Bạn có muốn kết thúc làm việc ngay lúc này!", "Yes", "No");
                    if (answer == true)
                    {
                        try
                        {
                            var hasPermission = await Utils.CheckPermissions(Permission.Location);
                            if (!hasPermission)
                            {
                                Debug.WriteLine("hasPermission---------------------------------------------");
                                return;
                            }

                            var position = await ContentGlobal.getLocation();

                            if (position == null)
                            {
                                await DisplayAlert("Thông báo", "Không thể xác định vị trí lúc này, thử lại sau!", "OK");
                                return;
                            }

                            Debug.WriteLine("Position Status: Latitude ------------------------------------------" + position.Latitude);
                            Debug.WriteLine("Position Status: Longitude ------------------------------------------" + position.Longitude);

                            //await DisplayAlert("Location", string.Format("Latitude :{0} \n Longitude:{1}", position.Latitude, position.Longitude), "OK");
                            var strLatitude = position.Latitude.ToString().Replace(",", ".");
                            var strLongitude = position.Longitude.ToString().Replace(",", ".");
                            var device_id = CrossDeviceInfo.Current.Id;

                            string strSubFirebase = "CheckOdooByApp";
                            string strParam = @"{""uid"":""" + Application.Current.Properties["uId"].ToString() + @""",""location"":""" + strLatitude + @"," + strLongitude + @""",""device_id"":""" + device_id + @"""}";

                            Debug.WriteLine("strParam CheckOdooByApp-------------------------------------------------- " + strParam);

                            var data = await ContentGlobal.FirebasePOSTFunctions(strSubFirebase, strParam);

                            if ((string)data["return"] == "OK")
                            {
                                ContentGlobal.allldata = await ContentGlobal.GetAllOfPersonal();

                                OnAppearing();

                                await DisplayAlert("Thông báo", "Bạn đã kết thúc ca làm việc", "OK");
                                /*ViewExtensions.CancelAnimations(imgRound);
                                boolAnimation = false;*/
                            }
                            else
                            {
                                await DisplayAlert("Thông báo", "Không thể thực hiện tác vụ này, xin thử lại sau!", "OK");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Error stop Circle" + ex.ToString());
                            await DisplayAlert("Thông báo", "Có lỗi sảy ra, kiểm tra lại kết nối mạng hoặc cài đặt vị trí của bạn và thử lại sau !", "OK");
                        }
                    }


                }
            };
            imgCircle.GestureRecognizers.Add(tapImgCircle);
            //lbDateOfToday.GestureRecognizers.Add(tapImgCircle);
            //lbTimeIn.GestureRecognizers.Add(tapImgCircle);
            imgRound.GestureRecognizers.Add(tapImgCircle);
            lbTimeIn.GestureRecognizers.Add(tapImgCircle);
            lbTimework.GestureRecognizers.Add(tapImgCircle);
            stkTimeIn.GestureRecognizers.Add(tapImgCircle);
            stkTimework.GestureRecognizers.Add(tapImgCircle);
        }

        protected async override void OnAppearing()
        {
            stkHeader.IsVisible = true;

            Debug.WriteLine("Platform --------------------------------------------------------" + CrossDeviceInfo.Current.Platform);
            Debug.WriteLine("Idiom --------------------------------------------------------" + CrossDeviceInfo.Current.Idiom);
            Debug.WriteLine("Version --------------------------------------------------------" + CrossDeviceInfo.Current.Version);
            Debug.WriteLine("Device ID --------------------------------------------------------" + CrossDeviceInfo.Current.Id);
            Debug.WriteLine("Device Model --------------------------------------------------------" + CrossDeviceInfo.Current.Model);


            if (ContentGlobal.INTcountNotiUnread > 0)
            {
                lbcountNotiUnread.Text = ContentGlobal.INTcountNotiUnread.ToString();
            }
            else
            {
                lbcountNotiUnread.IsVisible = false;
            }

            var dataTimeOff = ContentGlobal.allldata["timeoff"];
            Debug.WriteLine("dataTimeOff---------------------------------------" + dataTimeOff);

            DateTimekeeping.Text = "Tháng " + dataTimeOff["period"].ToString() + "/" + dataTimeOff["year"].ToString();

            manday_total.Text = dataTimeOff["timeoff"]["man_day"].ToString();
            timeoff_total.Text = dataTimeOff["timeoff"]["dayoff"]["timecout"].ToString();

            late_count.Text = dataTimeOff["timeoff"]["checkinlate"]["timecount"].ToString() + " lần";
            late_time.Text = dataTimeOff["timeoff"]["checkinlate"]["actualtime"].ToString() + " phút";

            early_count.Text = dataTimeOff["timeoff"]["checkoutearly"]["timecount"].ToString() + " lần";
            early_time.Text = dataTimeOff["timeoff"]["checkoutearly"]["actualtime"].ToString() + " phút";


            DateTime dt = DateTime.Now;
            Debug.WriteLine("DateTime------------------------------------------" + dt);

            string dayofweek = dt.DayOfWeek.ToString();
            Debug.WriteLine("dayofweek---------------------------------" + dayofweek);

            string date = dt.ToString("dd/MM/yyyy");
            Debug.WriteLine("date--------------------------------------" + date);

            lbDateOfToday.Text = dayofweek + " (" + date + ")";

            int day = dt.Day;
            Debug.WriteLine("day--------------------------------------" + day);

            int month = dt.Month;
            Debug.WriteLine("month--------------------------------------" + month);

            int year = dt.Year;
            Debug.WriteLine("year--------------------------------------" + year);

            var dataEE_Timekepping = ContentGlobal.allldata["timekeeping"];

            if (dataEE_Timekepping != null)
            {
                var dataTimeKeeping_year = dataEE_Timekepping[year.ToString()];
                if (dataTimeKeeping_year != null)
                {
                    var dataTimeKeeping_month = dataTimeKeeping_year[month.ToString() + "_" + year.ToString()];

                    if (dataTimeKeeping_month != null)
                    {
                        var dataTimeKeeping = dataTimeKeeping_month[day.ToString() + "_" + month.ToString()];

                        if (dataTimeKeeping != null)
                        {
                            Debug.WriteLine("dataTimeKeeping--------------------------------------" + dataTimeKeeping);

                            lbTimeStart.Text = ((TimeSpan)dataTimeKeeping["base_time_in"]).ToString(@"hh\:mm");
                            lbTimeEnd.Text = ((TimeSpan)dataTimeKeeping["base_time_out"]).ToString(@"hh\:mm");
                            lbTimeIn.Text = ((TimeSpan)dataTimeKeeping["time_in"]).ToString(@"hh\:mm");

                            if (dataTimeKeeping["time_out"] != null)
                            {
                                if (dataTimeKeeping["time_out"].ToString().Length > 0)
                                {
                                    Debug.WriteLine("STOP by Time out---------------------------------------------------------");
                                    ViewExtensions.CancelAnimations(imgRound);
                                    boolAnimation = false;
                                    imgCircle.Source = "circle_stop.png";
                                    lbTextTimeIn.TextColor = Color.White;
                                    lbTimeIn.TextColor = Color.White;
                                    imgRound.Source = "round_stop.png";
                                }
                            }
                            else if (DateTime.Now.TimeOfDay > (TimeSpan)dataTimeKeeping["base_time_out"])
                            {
                                Debug.WriteLine("STOP by Time base out---------------------------------------------------------");
                                ViewExtensions.CancelAnimations(imgRound);
                                boolAnimation = false;
                                imgCircle.Source = "circle_stop.png";
                                lbTextTimeIn.TextColor = Color.White;
                                lbTimeIn.TextColor = Color.White;
                                imgRound.Source = "round_stop.png";
                            }
                            else
                            {
                                Debug.WriteLine("Wait run");

                                if (dt.TimeOfDay > (TimeSpan)dataTimeKeeping["time_in"] && dt.TimeOfDay < (TimeSpan)dataTimeKeeping["base_time_out"])
                                {
                                    Debug.WriteLine("RUN by RUN---------------------------------------------");
                                    imgCircle.Source = "circle.png";
                                    lbTextTimeIn.TextColor = Color.White;
                                    lbTimeIn.TextColor = Color.White;
                                    //lbTextTimeIn.TextColor = Color.FromHex("#676f7a");
                                    //lbTimeIn.TextColor = Color.FromHex("#676f7a");
                                    imgRound.Source = "round.png";
                                    boolAnimation = true;
                                    int totalminute = 12 * 60;
                                    await imgRound.RotateTo(360 * totalminute, (uint)totalminute * 36000);
                                    
                                }
                            }
                        }

                    }
                }

               
            }
        }

        private async Task<string> takeImageAsync()
        {
            string filePath = "";

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                //await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                filePath = " No camera avaialble.";
            }
            else
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if (file == null)
                {
                    filePath = "Can't save image";
                }
                else
                {
                    filePath = "1";
                }
            }
            return filePath;
        }


    }
}