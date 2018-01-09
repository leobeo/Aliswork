using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Aliswork
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            Children.Add(new Tabbed.OverviewPage()
            {
                Icon = "chamcong.png",
                Title = "Chấm công",
            });
            Children.Add(new Tabbed.RequestPage()
            {
                Icon = "dontu.png",
                Title = "Đơn từ"
            });
            /*Children.Add(new Tabbed.AppealPage()
            {
                Icon = "khieunai.png",
                Title = "Khiếu nại"
            });*/
            Children.Add(new Tabbed.PayrollPage()
            {
                Icon = "bangluong.png",
                Title = "Bảng lương"
            });
            /*Children.Add(new Tabbed.NotificationPage()
            {
                Icon = "billboard_icon.png",
                Title = "Thông báo"
            });*/
        }
        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            Title = CurrentPage?.Title;
        }
    }
}

