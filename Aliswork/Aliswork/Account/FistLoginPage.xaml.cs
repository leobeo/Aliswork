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
    public partial class FistLoginPage : ContentPage
    {
        public FistLoginPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#61bafe");
        }

        public void ChangedPassword(object sender, EventArgs e)
        {
            App.Current.MainPage = new Setting.ChangedPasswordPage();
        }
    }
}