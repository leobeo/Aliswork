using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aliswork.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoAcctivePage : ContentPage
    {
        public NoAcctivePage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#61bafe");
        }

        public void btnExit(object sender, EventArgs e)
        {
            Application.Current.Properties["uId"] = "";
            var page = new SignIn_Page();
            NavigationPage.SetHasNavigationBar(page, false);
            App.Current.MainPage = new NavigationPage(page);
        }
    }
}