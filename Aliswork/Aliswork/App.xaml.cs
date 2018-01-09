using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Aliswork
{
    public partial class App : Application
    {
        Page page = new SignIn_Page();

        public App()
        {
            InitializeComponent();

            //MainPage = new Account.ContentNotificationPage();
           
            Debug.WriteLine("Patch-------------------" + ContentGlobal.path);
            if (ContentGlobal.path.ToString().Length > 0)
            {
                Application.Current.Properties["from_noti"] = 1;
                Debug.WriteLine("dictNotiRecieve--------------------------------------------------" + ContentGlobal.dictNotiRecieve["title"].ToString());
                Application.Current.Properties["title_noti"] = ContentGlobal.dictNotiRecieve["title"].ToString();

                Debug.WriteLine("dictNotiRecieve--------------------------------------------------" + ContentGlobal.dictNotiRecieve["message"].ToString());
                Application.Current.Properties["content_noti"] = ContentGlobal.dictNotiRecieve["message"].ToString();

                //MainPage = new NavigationPage(new Account.ContentNotificationPage());

                /*Debug.WriteLine("dictNotiRecieve--------------------------------------------------" + ContentGlobal.dictNotiRecieve["message"].ToString());

                 var content = new ContentPage
                 {
                     Title = "FCM",
                     Content = new StackLayout
                     {
                         VerticalOptions = LayoutOptions.Center,
                         Children = {
                          new Label {
                              HorizontalTextAlignment = TextAlignment.Center,
                              Text = ContentGlobal.path,
                          }
                      }
                     }
                 };

                 MainPage = new NavigationPage(content);*/
            }
            else
            {
                Application.Current.Properties["from_noti"] = 0;
            }
                NavigationPage.SetHasNavigationBar(page, false);
                MainPage = new NavigationPage(page);
          // }

            //MainPage = new SignIn_Page();*/
        }

        protected override void OnStart()
        {
            // Handle when your app starts

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
