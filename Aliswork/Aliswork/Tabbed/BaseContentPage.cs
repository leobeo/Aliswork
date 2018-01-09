using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Aliswork.Tabbed
{
    public class BaseContentPage : ContentPage
    {
        public new void SendAppearing()
        {
            OnAppearing();
        }

        public new void SendDisappearing()
        {
            OnDisappearing();
        }
    }
}