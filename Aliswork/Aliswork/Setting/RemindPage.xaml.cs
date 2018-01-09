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
    public partial class RemindPage : ContentPage
    {
        public RemindPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
        }

        protected override void OnAppearing()
        {
            ContentGlobal.saveSetting = true;

            var data = ContentGlobal.allldata["setting"][(string)Application.Current.Properties["SettingFor"]];
            if (data["time"] != null)
            {
                stkTime.IsVisible = true;
                pkTime.Time = (TimeSpan)data["time"];
            }
            if ((int)data["email"] == 1)
            {
                SwitchEmail.IsToggled = true;
            }
            else
            {
                SwitchEmail.IsToggled = false;
            }

            if ((int)data["phone"] == 1)
            {
                SwitchPhone.IsToggled = true;
            }
            else
            {
                SwitchPhone.IsToggled = false;
            }
        }

        protected override void OnDisappearing()
        {
            var data = ContentGlobal.allldata["setting"][(string)Application.Current.Properties["SettingFor"]];
            int isEmail = 0;
            int isPhone = 0;
            if (SwitchEmail.IsToggled == true)
                isEmail = 1;
            if (SwitchPhone.IsToggled == true)
                isPhone = 1;

            string name_setting = (string)Application.Current.Properties["SettingFor"];
            string strParam = "";
            if (stkTime.IsVisible == true)
            {
                string time = pkTime.Time.Hours.ToString() + ":" + pkTime.Time.Minutes.ToString();
                strParam = @"{""uid"":""" + Application.Current.Properties["uId"].ToString() + @""",""name_setting"":""" + name_setting + @""",""content"":{ ""email"":" + isEmail + @",""time"":""" + time + @""", ""phone"":" + isPhone + @"}}";

                data["time"] = time;
            }
            else
            {
                strParam = @"{""uid"":""" + Application.Current.Properties["uId"].ToString() + @""",""name_setting"":""" + name_setting + @""",""content"":{ ""email"":" + isEmail + @", ""phone"":" + isPhone + @"}}";
            }

            data["email"] = isEmail;
            data["phone"] = isPhone;

            Debug.WriteLine("Data------------------------" + data);



            //var data = await ContentGlobal.FirebasePUTFunctions(strSub, strParam);
            /*if ((string)data["return"] == "OK")
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