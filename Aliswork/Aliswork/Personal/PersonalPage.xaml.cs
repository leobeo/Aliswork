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
    public partial class PersonalPage : ContentPage
    {
        public PersonalPage()
        {
            InitializeComponent();
            //this.BackgroundColor = Color.FromHex("#F0EFF5");
            this.BackgroundImage = "background.jpg";

            var tapAccount = new TapGestureRecognizer();
            tapAccount.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new AccountPage());
            };

            imgToAccount.GestureRecognizers.Add(tapAccount);

            var tapSetting = new TapGestureRecognizer();
            tapSetting.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Setting.SettingPage());
            };

            imgToSetting.GestureRecognizers.Add(tapSetting);


            var tapTurn = new TapGestureRecognizer();
            tapTurn.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new Personal.AssignPage());
            };
            imgToTurn.GestureRecognizers.Add(tapTurn);

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
                    catch (Exception ex)
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

            var tapSync = new TapGestureRecognizer();
            tapSync.Tapped += async (s, e) =>
            {
                if (ContentGlobal.statusNetwork().Equals("Connected") != false)
                {
                    try
                    {
                        NavigationPage.SetHasNavigationBar(this, false);
                        await imgSync.RotateTo(-360, 2000);
                        imgSync.Rotation = 0;

                        ContentGlobal.allldata = await ContentGlobal.GetAllOfPersonal();
                        //ContentGlobal.loadListRequest();
                        lbLastSync.Text = "Lần gần nhất:  " + Application.Current.Properties["date_clone"].ToString();
                        NavigationPage.SetHasNavigationBar(this, true);
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Thông báo", "Có lỗi đã sảy ra, thử lại sau!", "OK");
                        Debug.WriteLine("Er PersionPage Sync---------------------------------------" + ex);
                    }
                }
                else
                {
                    await DisplayAlert("Cảnh báo", "Kiểm tra lại kết nối mạng của bạn và thử lại sau", "OK");
                }
            };
            grdSync.GestureRecognizers.Add(tapSync);

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

                string Subs =ContentGlobal.StreamToBase64String(stream);

                Debug.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaa===================================== :    " + Subs);

                string NameAPI = "pushImage";
                string pram = @"{""uid"" : """ + Application.Current.Properties["uId"].ToString() + @""", ""image_data"" : """ + Subs + @"""}";

                Debug.WriteLine("Param------------------------------" + pram);

                var dataEE = await ContentGlobal.FirebasePOSTFunctions(NameAPI, pram);

                var byte002 = ContentGlobal.StringFromBase64Stream(Subs);

                imgAvatar.Source = ImageSource.FromStream(() => new MemoryStream(ContentGlobal.GetImageStreamAsBytes(byte002)));
                /*{
                    var stream = file.GetStream();

                    StreamReader reader = new StreamReader(stream);
                    string text = stream.ToString();

                    Debug.WriteLine("aaaaaaaaaaaaaaaaaaaa-----------------------------------" + text);

                    file.Dispose();
                    return stream;
                });*/
            };

            imgAvatar.GestureRecognizers.Add(tapAvatar);

        }

        protected override void OnAppearing()
        {
            lbLastSync.Text = "Lần gần nhất:  " + Application.Current.Properties["date_clone"].ToString();
            var data = ContentGlobal.allldata["info"];
            lbPersionalName.Text = lbEdit_PersionalName.Text = (string)data["name"];
            lbPersionalEmail.Text = (string)data["email"];
            string image_data = (string)data["image_data"];
            imgAvatar.Source = ImageSource.FromStream(()=>new MemoryStream(ContentGlobal.GetImageStreamAsBytes(ContentGlobal.StringFromBase64Stream(image_data))));

           /* if (ContentGlobal.Userroot == false)
            {
                stkAssign.IsVisible = false;
            }
            else
            {
                stkAssign.IsVisible = true;
            }*/
        }
    }

}