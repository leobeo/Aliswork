using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aliswork.Personal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreviewAssignPage : ContentPage
    {
        public PreviewAssignPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");
        }
    }
}