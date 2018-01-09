using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aliswork.Personal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();
            //this.BackgroundColor = Color.FromHex("#F0EFF5");
            this.BackgroundImage = "background.jpg";

            var tapLogout = new TapGestureRecognizer();
            tapLogout.Tapped += async (s, e) =>
            {
                if (ContentGlobal.statusNetwork().Equals("Connected") != false)
                {
                    try
                    {
                        string strSubFirebase = "ClearRegis";
                        string strParam = @"{ ""uid"" : """ + Application.Current.Properties["uId"].ToString() + @"""} ";
                        var data = ContentGlobal.FirebasePOSTFunctions(strSubFirebase, strParam);

                        Application.Current.Properties["uId"] = "";
                        var page = new SignIn_Page();
                        NavigationPage.SetHasNavigationBar(page, false);
                        App.Current.MainPage = new NavigationPage(page);

                        /*if ((string)data["return"] == "OK")
                        {
                            Application.Current.Properties["uId"] = "";
                            var page = new SignIn_Page();
                            NavigationPage.SetHasNavigationBar(page, false);
                            App.Current.MainPage = new NavigationPage(page);
                        }
                        else
                        {
                            await DisplayAlert("Thông báo", "Có lỗi đã sảy ra, thử lại sau!", "OK");
                        }*/

                    }
                    catch(Exception ex)
                    {
                        await DisplayAlert("Thông báo", "Có lỗi đã sảy ra, thử lại sau!", "OK");
                        Debug.WriteLine("Er Account Page--------------------------------------" + ex);
                    }
                }
                else
                {
                    await DisplayAlert("Cảnh báo", "Kiểm tra lại kết nối mạng của bạn và thử lại sau!", "OK");
                }
            };

            stkLogout.GestureRecognizers.Add(tapLogout);

            var tapChangedPassword = new TapGestureRecognizer();
            tapChangedPassword.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Setting.ChangedPasswordPage());
            };
            stkChangedPassword.GestureRecognizers.Add(tapChangedPassword);


            var tapAvatar = new TapGestureRecognizer();
            tapAvatar.Tapped += async (s, e) =>
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    return;
                }
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });


                if (file == null)
                    return;


                var stream = file.GetStream();

                string Subs = ContentGlobal.StreamToBase64String(stream);

                Debug.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaa===================================== :    " + Subs);

                string NameAPI = "pushImage";
                string pram = @"{""uid"" : """ + Application.Current.Properties["uId"].ToString() + @""", ""image_data"" : """ + Subs + @"""}";

                Debug.WriteLine("Param------------------------------" + pram);

                var dataEE = await ContentGlobal.FirebasePOSTFunctions(NameAPI, pram);

                //var byte002 = ContentGlobal.StringFromBase64Stream(Subs);

                //imgAvatar.Source = ImageSource.FromStream(() => new MemoryStream(ContentGlobal.GetImageStreamAsBytes(byte002)));

                 imgAvatar.Source = ImageSource.FromStream(() =>
                 {
                     //var stream = file.GetStream();
                     file.Dispose();
                     return stream;
                 });
            };

            imgAvatar.GestureRecognizers.Add(tapAvatar);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var data = ContentGlobal.allldata["info"];
            lbPersionalName.Text = (string)data["name"];
            lbOdooPersionalId.Text = (string)data["id"];
            lbEmail.Text = lbPersionalEmail.Text = (string)data["email"];
            string image_data = (string)data["image_data"];
            imgAvatar.Source = ImageSource.FromStream(() => new MemoryStream(ContentGlobal.GetImageStreamAsBytes(ContentGlobal.StringFromBase64Stream(image_data))));
            if (ContentGlobal.Userroot == true)
            {
                lbActed.Text = "Quản lý";
            }
            else
            {
                lbActed.Text = "Không";
            }
        }
    }
}