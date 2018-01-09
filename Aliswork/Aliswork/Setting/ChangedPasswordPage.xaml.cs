using Newtonsoft.Json.Linq;
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
    public partial class ChangedPasswordPage : ContentPage
    {
        bool viewNewPass = false;
        bool viewReNewPass = false;
        public ChangedPasswordPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
            entOldPassword.Focus();

            var tapViewNewPass = new TapGestureRecognizer();
            tapViewNewPass.Tapped += (s, e) =>
            {
                if (viewNewPass == false)
                {
                    entNewPassword.IsPassword = false;
                    imgViewNewPassword.Source = "eye_disabled.png";
                    viewNewPass = true;
                }
                else
                {
                    entNewPassword.IsPassword = true;
                    imgViewNewPassword.Source = "eye.png";
                    viewNewPass = false;
                }
            };

            imgViewNewPassword.GestureRecognizers.Add(tapViewNewPass);


            var tapViewReNewPass = new TapGestureRecognizer();
            tapViewReNewPass.Tapped += (s, e) =>
            {
                if (viewReNewPass == false)
                {
                    entReNewPassword.IsPassword = false;
                    imgViewReNewPassword.Source = "eye_disabled.png";
                    viewReNewPass = true;
                }
                else
                {
                    entReNewPassword.IsPassword = true;
                    imgViewReNewPassword.Source = "eye.png";
                    viewReNewPass = false;
                }
            };

            imgViewReNewPassword.GestureRecognizers.Add(tapViewReNewPass);
        }

        public async Task btnChangedPassword_Click(object sender, EventArgs e)
        {
            if(ContentGlobal.statusNetwork().Equals("Connected") == true)
            {
                try
                {
                    if (entOldPassword.Text.ToString().Length > 6 && entOldPassword.IsEnabled == true && entNewPassword.Text.ToString().Length > 6 && entNewPassword.IsEnabled == true && entReNewPassword.Text.ToString().Length > 6 && entReNewPassword.IsEnabled == true && entNewPassword.Text.ToString().Equals(entReNewPassword.Text.ToString()) && btnChange.IsEnabled == true)
                    {
                        //entOldPassword.IsEnabled = false;
                        //entNewPassword.IsEnabled = false;
                        //entReNewPassword.IsEnabled = false;
                        stkLoad.IsVisible = true;

                        string strSub = "resetPassword";
                        string strParam = @"{""newPassword"": """ + entNewPassword.Text.ToString() + @""", ""oldPassword"": """ + entOldPassword.Text.ToString() + @""", ""email"": """ + ContentGlobal.allldata["info"]["email"] + @""" }";

                        Debug.WriteLine("param------------------" + strParam);
                        JContainer data = await ContentGlobal.PostAllFirebase_Auth(strSub, strParam);

                        Debug.WriteLine("----datattttttttttttttttttttt--------------" + data["email"]);

                        if ((string)data["email"] == (string)ContentGlobal.allldata["info"]["email"])
                        {
                            string strSubFB = "PasswordSafety";
                            string strParamFB = @"{""uid"":""" + Application.Current.Properties["uId"].ToString() + @""",""password_safety"":1}";

                            Debug.WriteLine("--------------param--------------" + strParamFB);
                            Debug.WriteLine("---------------111111111111111111111----------------------");
                            var dataFB = await ContentGlobal.FirebasePUTFunctions(strSubFB, strParamFB);
                            Debug.WriteLine("---------------22222222222222222222222222----------------------" + dataFB["return"]);
                            if ((string)dataFB["return"] == "OK")
                            {
                                Application.Current.Properties["TokenFB"] = "";
                                Page page = new SignIn_Page();
                                NavigationPage.SetHasNavigationBar(page, false);
                                App.Current.MainPage = new NavigationPage(page);
                            }
                        }
                        else
                        {
                            entOldPassword.Text = entNewPassword.Text = entReNewPassword.Text = "";
                            stkLoad.IsVisible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Lỗi", "Có vấn dề đã sảy ra, thử lại sau !", "OK");

                    Debug.WriteLine("Error--ChangedPassword--------------------------------", ex);
                }
            }
            else
            {
                await DisplayAlert("Cảnh báo mạng", "Bạn hãy kết nối mạng để thực hiện tác vụ này !", "OK");
            }
          
        }

        public void OnentOldPasswordChanged(object sender, EventArgs e)
        {
            if (entOldPassword.Text.ToString().Length > 6)
            {
                entNewPassword.IsEnabled = true;
            }
            else
            {
                entNewPassword.IsEnabled = false;
            }
        }

        public void OnentNewPasswordChanged(object sender, EventArgs e)
        {
            if (entNewPassword.Text.ToString().Length > 6)
            {
                entReNewPassword.IsEnabled = true;
            }
            else
            {
                entReNewPassword.IsEnabled = false;
            }
        }

        public void OnentReNewPasswordChanged(object sender, EventArgs e)
        {
            if (entReNewPassword.Text.ToString().Length > 6)
            {
                if (entNewPassword.Text.ToString().Equals(entReNewPassword.Text.ToString()) == true)
                {
                    lbUnMatch.IsVisible = false;
                    btnChange.IsEnabled = true;
                }
                else
                {
                    lbUnMatch.IsVisible = true;
                    btnChange.IsEnabled = false;
                }
            }
            else
            {
                lbUnMatch.IsVisible = false;
                btnChange.IsEnabled = false;
            }
        }
    }
}