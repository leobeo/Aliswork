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
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {

            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");

            var tapimgSetCheckin = new TapGestureRecognizer();
            tapimgSetCheckin.Tapped += (s, e) =>
            {
                Application.Current.Properties["SettingFor"] = "checkin";
                Navigation.PushAsync(new RemindPage());
            };
            imgSetCheckin.GestureRecognizers.Add(tapimgSetCheckin);

            var tapimgSetCheckout = new TapGestureRecognizer();
            tapimgSetCheckout.Tapped += (s, e) =>
            {
                Application.Current.Properties["SettingFor"] = "checkout";
                Navigation.PushAsync(new RemindPage());
            };
            imgSetCheckout.GestureRecognizers.Add(tapimgSetCheckout);

            var tapimgSetChangeplanAlert = new TapGestureRecognizer();
            tapimgSetChangeplanAlert.Tapped += (s, e) =>
            {
                Application.Current.Properties["SettingFor"] = "changeplan";
                Navigation.PushAsync(new RemindPage());
            };
            imgSetChangeplanAlert.GestureRecognizers.Add(tapimgSetChangeplanAlert);

            var tapimgSetBefor1Hour = new TapGestureRecognizer();
            tapimgSetBefor1Hour.Tapped += (s, e) =>
            {
                Application.Current.Properties["SettingFor"] = "onehourbefor";
                Navigation.PushAsync(new RemindPage());
            };
            imgSetBefor1Hour.GestureRecognizers.Add(tapimgSetBefor1Hour);

            var tapimgSetForgetCheckin = new TapGestureRecognizer();
            tapimgSetForgetCheckin.Tapped += (s, e) =>
            {
                Application.Current.Properties["SettingFor"] = "forgotcheckin";
                Navigation.PushAsync(new RemindPage());
            };
            imgSetForgetCheckin.GestureRecognizers.Add(tapimgSetForgetCheckin);

            var tapimgSetForgetCheckout = new TapGestureRecognizer();
            tapimgSetForgetCheckout.Tapped += (s, e) =>
            {
                Application.Current.Properties["SettingFor"] = "forgotcheckout";
                Navigation.PushAsync(new RemindPage());
            };
            imgSetForgetCheckout.GestureRecognizers.Add(tapimgSetForgetCheckout);

            var tapimgDayAlert = new TapGestureRecognizer();
            tapimgDayAlert.Tapped += (s, e) =>
            {
                Application.Current.Properties["SettingFor"] = "date";
                Navigation.PushAsync(new ChoiseDayAlertPage());
            };
            imgDayAlert.GestureRecognizers.Add(tapimgDayAlert);
        }

        protected override void OnAppearing()
        {
            CheckinAlertBy.Text = AlertBy("checkin");
            CheckinAlertAt.Text = (string)ContentGlobal.allldata["setting"]["checkin"]["time"];

            CheckoutAlertBy.Text = AlertBy("checkout");
            CheckoutAlertAt.Text = (string)ContentGlobal.allldata["setting"]["checkout"]["time"];

            DayAlert.Text = AlertAt();

            ChangeplanAlertBy.Text = AlertBy("changeplan");

            Befor1HourAlertBy.Text = AlertBy("onehourbefor");

            ForgetCheckin.Text = AlertBy("forgotcheckin");

            ForgetCheckout.Text = AlertBy("forgotcheckout");
        }

        private string AlertBy(string strSub)
        {
            string str = "";

            if ((int)ContentGlobal.allldata["setting"][strSub]["email"] == 1)
            {
                str += "Qua email";
            }
            if ((int)ContentGlobal.allldata["setting"][strSub]["phone"] == 1)
            {
                if (str.Length > 0)
                {
                    str += ", Qua điện thoại";
                }
                else
                {
                    str += "Qua điện thoại";
                }

            }

            return str;
        }

        private string AlertAt()
        {
            string str = "";

            if ((int)ContentGlobal.allldata["setting"]["date"]["mon"] == 1)
            {
                str += "Hai";
            }

            if ((int)ContentGlobal.allldata["setting"]["date"]["tue"] == 1)
            {
                if (str.Length > 0)
                {
                    str += ", Ba";
                }
                else
                {
                    str += "Ba";
                }

            }

            if ((int)ContentGlobal.allldata["setting"]["date"]["wed"] == 1)
            {
                if (str.Length > 0)
                {
                    str += ", Tư";
                }
                else
                {
                    str += "Tư";
                }

            }

            if ((int)ContentGlobal.allldata["setting"]["date"]["thu"] == 1)
            {
                if (str.Length > 0)
                {
                    str += ", Năm";
                }
                else
                {
                    str += "Năm";
                }

            }

            if ((int)ContentGlobal.allldata["setting"]["date"]["fri"] == 1)
            {
                if (str.Length > 0)
                {
                    str += ", Sáu";
                }
                else
                {
                    str += "Sáu";
                }

            }

            if ((int)ContentGlobal.allldata["setting"]["date"]["sat"] == 1)
            {
                if (str.Length > 0)
                {
                    str += ", Bảy";
                }
                else
                {
                    str += "Bảy";
                }

            }

            if ((int)ContentGlobal.allldata["setting"]["date"]["sun"] == 1)
            {
                if (str.Length > 0)
                {
                    str += ", CN";
                }
                else
                {
                    str += "CN";
                }

            }



            return str;
        }


        protected override void OnDisappearing()
        {
            if (ContentGlobal.saveSetting == true)
            {
                if (ContentGlobal.statusNetwork().Equals("Connected") != false)
                {
                    try
                    {
                        var data = ContentGlobal.allldata["setting"];
                        Debug.WriteLine("date save---------------" + data);

                        string strSub = "UpdateSetting";
                        string strParam = @"{""uid"":""" + Application.Current.Properties["uId"].ToString() + @""",""content"":" + data + @"}";

                        Debug.WriteLine("strParam-------------------------------------------------" + strParam);

                        var dataEE = ContentGlobal.FirebasePUTFunctions(strSub, strParam);

                        Debug.WriteLine("dataEE----------------------------------" + dataEE);

                        ContentGlobal.saveSetting = false;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Er-------Setting Page" + ex);
                    }
                    
                }
            }
        }
    }
}