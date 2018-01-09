using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aliswork.Setting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChoiseDayAlertPage : ContentPage
    {
        bool boolMon = false;
        bool boolTue = false;
        bool boolWed = false;
        bool boolThu = false;
        bool boolFri = false;
        bool boolSat = false;
        bool boolSun = false;

        public ChoiseDayAlertPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");

            var tapMon = new TapGestureRecognizer();
            tapMon.Tapped += (s, e) => {
                if (boolMon == true)
                {
                    imgMon.IsVisible = false;
                    boolMon = false;
                }
                else
                {
                    imgMon.IsVisible = true;
                    boolMon = true;
                }
            };

            stkMon.GestureRecognizers.Add(tapMon);

            var tapTue = new TapGestureRecognizer();
            tapTue.Tapped += (s, e) => {
                if (boolTue == true)
                {
                    imgTue.IsVisible = false;
                    boolTue = false;
                }
                else
                {
                    imgTue.IsVisible = true;
                    boolTue = true;
                }
            };
            stkTue.GestureRecognizers.Add(tapTue);

            var tapWed = new TapGestureRecognizer();
            tapWed.Tapped += (s, e) => {
                if (boolWed == true)
                {
                    imgWed.IsVisible = false;
                    boolWed = false;
                }
                else
                {
                    imgWed.IsVisible = true;
                    boolWed = true;
                }
            };
            stkWed.GestureRecognizers.Add(tapWed);

            var tapThu = new TapGestureRecognizer();
            tapThu.Tapped += (s, e) => {
                if (boolThu == true)
                {
                    imgThu.IsVisible = false;
                    boolThu = false;
                }
                else
                {
                    imgThu.IsVisible = true;
                    boolThu = true;
                }
            };
            stkThu.GestureRecognizers.Add(tapThu);

            var tapFri = new TapGestureRecognizer();
            tapFri.Tapped += (s, e) => {
                if (boolFri == true)
                {
                    imgFri.IsVisible = false;
                    boolFri = false;
                }
                else
                {
                    imgFri.IsVisible = true;
                    boolFri = true;
                }
            };
            stkFri.GestureRecognizers.Add(tapFri);

            var tapSat = new TapGestureRecognizer();
            tapSat.Tapped += (s, e) => {
                if (boolSat == true)
                {
                    imgSat.IsVisible = false;
                    boolSat = false;
                }
                else
                {
                    imgSat.IsVisible = true;
                    boolSat = true;
                }
            };
            stkSat.GestureRecognizers.Add(tapSat);

            var tapSun = new TapGestureRecognizer();
            tapSun.Tapped += (s, e) => {
                if (boolSun == true)
                {
                    imgSun.IsVisible = false;
                    boolSun = false;
                }
                else
                {
                    imgSun.IsVisible = true;
                    boolSun = true;
                }
            };
            stkSun.GestureRecognizers.Add(tapSun);
        }

        protected override void OnAppearing()
        {
            ContentGlobal.saveSetting = true;
            var data = ContentGlobal.allldata["setting"]["date"];

            if ((int)data["mon"] == 1)
            {
                boolMon = true;
                imgMon.IsVisible = true;
            }

            if ((int)data["tue"] == 1)
            {
                boolTue = true;
                imgTue.IsVisible = true;
            }

            if ((int)data["wed"] == 1)
            {
                boolWed = true;
                imgWed.IsVisible = true;
            }

            if ((int)data["thu"] == 1)
            {
                boolThu = true;
                imgThu.IsVisible = true;
            }
            if ((int)data["fri"] == 1)
            {
                boolFri = true;
                imgFri.IsVisible = true;
            }

            if ((int)data["sat"] == 1)
            {
                boolSat = true;
                imgSat.IsVisible = true;
            }

            if ((int)data["sun"] == 1)
            {
                boolSun = true;
                imgSun.IsVisible = true;
            }


        }

        protected override void OnDisappearing()
        {
            int mon, tue, wed, thu, fri, sat, sun;
            mon = tue = wed = thu = fri = sat = sun = 0;

            if (boolMon == true)
            {
                mon = 1;
            }

            if (boolTue == true)
            {
                tue = 1;
            }


            if (boolWed == true)
            {
                wed = 1;
            }

            if (boolThu == true)
            {
                thu = 1;
            }


            if (boolFri == true)
            {
                fri = 1;
            }

            if (boolSat == true)
            {
                sat = 1;
            }

            if (boolSun == true)
            {
                sun = 1;
            }

            var data = ContentGlobal.allldata["setting"]["date"];

            data["mon"] = mon;
            data["tue"] = tue;
            data["wed"] = wed;
            data["thu"] = thu;
            data["fri"] = fri;
            data["sat"] = sat;
            data["sun"] = sun;

            Debug.WriteLine("data-----------------------------" + data);
            /*string strSub = "UpdateSetting";
            string name_setting = (string)Application.Current.Properties["SettingFor"];

            string strParam = @"{""uid"":""" + ContentGlobal.uId + @""",""name_setting"":""" + name_setting + @""",""content"":{""fri"" : " + fri + @",""mon"" :" + mon + @",""sat"" : " + sat + @",""sun"" : " + sun + @",""thu"" : " + thu + @",""tue"" : " + tue + @",""wed"" : " + wed + @"}}";

            Debug.WriteLine("Param-==-=-=-=-=-=-=-=-=-=-=-=" + strParam);

            var data = await ContentGlobal.FirebasePUTFunctions(strSub, strParam);

            if ((string)data["return"] == "OK")
            {
                await DisplayAlert("Thông báo", "Thay đổi của bạn đã được lưu", "OK");
                //await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Thay đổi của bạn chưa được lưu", "OK");
                //await Navigation.PopAsync();
            }*/

        }
    }
}