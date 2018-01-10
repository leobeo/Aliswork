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

namespace Aliswork
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignIn_Page : ContentPage
    {
        public SignIn_Page()
        {
            InitializeComponent();
            //this.BackgroundImage = "background.jpg";
            this.BackgroundColor = Color.FromHex("#61bafe");
            EntryUserName.Focus();
        }


        protected async override void OnAppearing()
        {
            var properties = Xamarin.Forms.Application.Current.Properties;
            if (properties.ContainsKey("uId") && properties["uId"].ToString().Length > 0)
            {
                Debug.WriteLine("uId------------------------------------"+ properties["uId"].ToString());
                stkMain.IsVisible = false;
                stkLoad.IsVisible = true;

                if (ContentGlobal.statusNetwork().Equals("Connected") != false)
                {
                    try
                    {
                        ContentGlobal.allldata = await ContentGlobal.GetAllOfPersonal();

                        if ((int)ContentGlobal.allldata["info"]["active"] == 0)
                        {
                            App.Current.MainPage = new Account.NoAcctivePage();
                        }
                        else
                        {
                            ContentGlobal.loadListRequest();

                            if (ContentGlobal.registrationId.Length > 0)
                            {
                                var registrationId = ContentGlobal.allldata["info"]["registrationId"];

                                if (registrationId != null)
                                {
                                    if (registrationId.ToString().Equals(ContentGlobal.registrationId) == false)
                                    {
                                        string strSubFirebase = "pushData";
                                        string strParam = @"{""uid"" : """ + properties["uId"].ToString() + @""", ""registrationId"" :""" + ContentGlobal.registrationId + @""" }";

                                        Debug.WriteLine("strParam-----------------------------------------------------" + strParam);

                                        var rt = ContentGlobal.FirebasePOSTFunctions(strSubFirebase, strParam);
                                    }
                                }
                                else
                                {
                                    string strSubFirebase = "pushData";
                                    string strParam = @"{""uid"" : """ + properties["uId"].ToString() + @""", ""registrationId"" :""" + ContentGlobal.registrationId + @""" }";

                                    Debug.WriteLine("strParam-----------------------------------------------------" + strParam);

                                    var rt = ContentGlobal.FirebasePOSTFunctions(strSubFirebase, strParam);
                                }
                            }

                            if ((int)ContentGlobal.allldata["info"]["acted"] == 1)
                            {
                                ContentGlobal.PeopleWorking = await ContentGlobal.peopleWorkingAsync();
                                ContentGlobal.Userroot = true;
                            }
                            else
                            {
                                ContentGlobal.Userroot = false;
                            }

                           var month = (int)ContentGlobal.allldata["timeoff"]["period"];

                            ContentGlobal.INTcountNotiUnread = ContentGlobal.CountNotiUnread("individualistic", month) + ContentGlobal.CountNotiUnread("public", month);
                            Debug.WriteLine("INTcountNotiUnread--------------------------------------------------" + ContentGlobal.INTcountNotiUnread);

                            var list_request = ContentGlobal.allldata["info"]["company_rules"]["list_request"];

                            ContentGlobal.loadListRequest();


                            Device.StartTimer(TimeSpan.FromMinutes(5), () => {

                                Task.Factory.StartNew(async () =>
                                {
                                    try
                                    {
                                        ContentGlobal.allldata = await ContentGlobal.GetAllOfPersonal();
                                        ContentGlobal.loadListRequest();

                                    }
                                    catch (Exception el)
                                    {
                                        Debug.WriteLine("erorr refesh data" + el.ToString());
                                    }
                                });

                                return true;
                            });

                            Page page = new MainPage();
                            NavigationPage.SetHasNavigationBar(page, false);
                            App.Current.MainPage = new NavigationPage(page);
                        }
                    }
                    catch (Exception ex)
                    {
                        stkMain.IsVisible = true;
                        stkLoad.IsVisible = false;

                        await DisplayAlert("Lỗi","Có vấn dề đã sảy ra, thử lại sau !", "OK");

                        Debug.WriteLine("Error OnAppearing--------------------------------", ex);
                    }
                }
                else
                {
                    stkMain.IsVisible = true;
                    stkLoad.IsVisible = false;

                    //await DisplayAlert("thong bao mang", ContentGlobal.statusNetwork(), "OK");
                }
            }
            else
            {
                stkLoad.IsVisible = false;
                stkMain.IsVisible = true;
                /*Page page = new SignIn_Page();
                NavigationPage.SetHasNavigationBar(page, false);
                App.Current.MainPage = new NavigationPage(page);*/
            }
        }
        public void OnentUsernameChanged(object sender, EventArgs e)
        {
            if (EntryUserName.Text.ToString().Length > 0)
            {
                EntryPassword.IsEnabled = true;
            }
            else
            {
                EntryPassword.IsEnabled = false;
            }
        }


        public void OnentPasswordChanged(object sender, EventArgs e)
        {
            if (EntryPassword.Text.ToString().Length > 7)
            {
                /*if (ContentGlobal.PasswordChecked(EntryPassword.Text.ToString()) == false)
                    EntryPassword.TextColor = Color.Red;
                else
                    EntryPassword.TextColor = Color.White;*/
                btnSignin.IsEnabled = true;
            }
            else
            {
                btnSignin.IsEnabled = false;
            }
        }

        public async void SignInButtonClicked(object sender, EventArgs e)
        {
            stkMain.IsVisible = false;
            stkLoad.IsVisible = true;

            if (ContentGlobal.statusNetwork().Equals("Connected") == false)
            {
                stkMain.IsVisible = true;
                stkLoad.IsVisible = false;

                //await DisplayAlert("thong bao mang", ContentGlobal.statusNetwork(), "OK");
            }
            else
            {
                try
                {
                    if (EntryUserName.Text.ToString().Length > 0 &&
                        EntryPassword.Text.ToString().Length > 0)
                    {
                        stkMain.IsVisible = false;
                        stkLoad.IsVisible = true;

                        string strSubFirebase = "verifyPassword";
                        string strParam = @"{ ""email"" : """ + EntryUserName.Text.ToString().ToLower() +
                            @""",""password"" : """ + EntryPassword.Text.ToString() +
                            @""",""returnSecureToken"" : ""true"" } ";
                        Debug.WriteLine("PARAM---------------------------------------------------------------" + strParam);
                        var rtFB = await ContentGlobal.PostAllFirebase_Auth(strSubFirebase, strParam);

                        Debug.WriteLine("rtFB-------------------" + rtFB);

                        string dataFB = (string)rtFB["idToken"];

                        Debug.WriteLine("idToken---------------------------" + dataFB);

                        if (dataFB != null)
                        {
                            Application.Current.Properties["uId"] = (string)rtFB["localId"];

                            ContentGlobal.allldata = await ContentGlobal.GetAllOfPersonal();

                            ContentGlobal.loadListRequest();

                            Debug.WriteLine("allldata---------------------------" + ContentGlobal.allldata);

                            if ((int)ContentGlobal.allldata["info"]["active"] == 0)
                            {
                                App.Current.MainPage = new Account.NoAcctivePage();
                            }
                            else
                            {
                                if (ContentGlobal.registrationId.Length > 0)
                                {
                                    var registrationId = ContentGlobal.allldata["info"]["registrationId"];

                                    if (registrationId != null)
                                    {
                                        if (registrationId.ToString().Equals(ContentGlobal.registrationId) == false)
                                        {
                                            strSubFirebase = "pushData";
                                            strParam = @"{""uid"" : """ + Application.Current.Properties["uId"].ToString() + @""", ""registrationId"" :""" + ContentGlobal.registrationId + @""" }";

                                            Debug.WriteLine("strParam-----------------------------------------------------" + strParam);

                                            var rt = ContentGlobal.FirebasePOSTFunctions(strSubFirebase, strParam);
                                        }
                                    }
                                    else
                                    {
                                        strSubFirebase = "pushData";
                                        strParam = @"{""uid"" : """ + Application.Current.Properties["uId"].ToString() + @""", ""registrationId"" :""" + ContentGlobal.registrationId + @""" }";

                                        Debug.WriteLine("strParam-----------------------------------------------------" + strParam);

                                        var rt = ContentGlobal.FirebasePOSTFunctions(strSubFirebase, strParam);
                                    }
                                }


                                if ((int)ContentGlobal.allldata["info"]["acted"] == 1)
                                {
                                    ContentGlobal.PeopleWorking = await ContentGlobal.peopleWorkingAsync();
                                    ContentGlobal.Userroot = true;
                                }
                                else
                                {
                                    ContentGlobal.Userroot = false;
                                }

                                ContentGlobal.email = EntryUserName.Text.ToString().ToLower();

                                if ((int)ContentGlobal.allldata["info"]["password_safety"] == 0)
                                {
                                    //await DisplayAlert("Warring", "Bạn cần thay đổi mật khẩu hiện tại", "OK");
                                    App.Current.MainPage = new Account.FistLoginPage();
                                }
                                else
                                {
                                    var month = (int)ContentGlobal.allldata["timeoff"]["period"];

                                    ContentGlobal.INTcountNotiUnread = ContentGlobal.CountNotiUnread("individualistic", month) + ContentGlobal.CountNotiUnread("public", month);
                                    Debug.WriteLine("INTcountNotiUnread--------------------------------------------------" + ContentGlobal.INTcountNotiUnread);

                                    Device.StartTimer(TimeSpan.FromMinutes(5), () => {

                                        Task.Factory.StartNew(async () =>
                                        {
                                            try
                                            {
                                                ContentGlobal.allldata = await ContentGlobal.GetAllOfPersonal();
                                                ContentGlobal.loadListRequest();

                                            }
                                            catch (Exception el)
                                            {
                                                Debug.WriteLine("erorr refesh data" + el.ToString());
                                            }
                                        });

                                        return true;
                                    });

                                    var page = new MainPage();
                                    NavigationPage.SetHasNavigationBar(page, false);
                                    App.Current.MainPage = new NavigationPage(page);
                                }
                            }
                        }
                        else
                        {
                            stkMain.IsVisible = true;
                            stkLoad.IsVisible = false;
                            if((string)rtFB["error"]["message"] == "INVALID_PASSWORD")
                            {
                                EntryPassword.Text = "";
                                await DisplayAlert("Lỗi","Mật khẩu bạn vừa nhập không đúng, nhập và thử lại!" , "OK");
                            }
                            else if((string)rtFB["error"]["message"] == "EMAIL_NOT_FOUND")
                            {
                                EntryUserName.Text = "";
                                EntryPassword.Text = "";
                                await DisplayAlert("Lỗi","Tài khoản đăng nhập không đúng, nhập lại email và password!" , "OK");
                            }
                            else
                            {
                                await DisplayAlert("Lỗi", (string)rtFB["error"]["message"], "OK");
                            }
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    stkMain.IsVisible = true;
                    stkLoad.IsVisible = false;

                    await DisplayAlert("Lỗi", "Có vấn dề đã sảy ra, thử lại sau !", "OK");

                    Debug.WriteLine("Error SignInButtonClicked--------------------------------", ex);
                   // throw ex;
                }
            }
        }


    }
}