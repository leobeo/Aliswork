using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aliswork.Tabbed
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayrollPage : ContentPage
    {
        bool Countbool = false;
        bool Protectbool = false;
        bool Remissionbool = false;
        //Page pagePersion = ;

        int year;
        int month;

        int selectedIndexMonth = -1;

        public PayrollPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");

            if (Device.RuntimePlatform == Device.iOS)
            {
                stkHeader.Margin = new Thickness(0, 20, 0, 0);
            }


            var tapPersion = new TapGestureRecognizer();
            tapPersion.Tapped += (s, e) =>
            {
                stkHeader.IsVisible = false;
                Navigation.PushAsync(new Personal.PersonalPage());
            };
            imgtoPersion.GestureRecognizers.Add(tapPersion);

            var tapimgtoNotification = new TapGestureRecognizer();
            tapimgtoNotification.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new NotificationPage());
            };
            //imgtoNotification.GestureRecognizers.Add(tapimgtoNotification);
            grdNoti.GestureRecognizers.Add(tapimgtoNotification);



            List<String> listmonth = ContentGlobal.ListMonth;
            foreach (String nn in listmonth)
            {
                pickerMonth.Items.Add(nn);
            }

            month = (int)ContentGlobal.allldata["timeoff"]["period"];

            selectedIndexMonth = month - 1;

            pickerMonth.SelectedIndex = selectedIndexMonth;

            pickerMonth.SelectedIndexChanged += (sender, e) =>
            {
                selectedIndexMonth = pickerMonth.SelectedIndex;
                loadPayroll();
            };



            var tapimgCount = new TapGestureRecognizer();
            tapimgCount.Tapped += (s, e) =>
            {
                if (Countbool == false)
                {
                    imgCount.Source = "arrow.png";
                    imgCount.Rotation = -90;
                    stkIncome.IsVisible = true;
                    Countbool = true;
                }
                else
                {
                    imgCount.Source = "arrow.png";
                    imgCount.Rotation = 90;
                    stkIncome.IsVisible = false;
                    Countbool = false;
                }
            };

            imgCount.GestureRecognizers.Add(tapimgCount);


            var tapimgProtect = new TapGestureRecognizer();
            tapimgProtect.Tapped += (s, e) =>
            {
                if (Protectbool == false)
                {
                    imgProtect.Source = "arrow.png";
                    imgProtect.Rotation = -90;
                    stkInsurance.IsVisible = true;
                    Protectbool = true;
                }
                else
                {
                    imgProtect.Source = "arrow.png";
                    imgProtect.Rotation = 90;
                    stkInsurance.IsVisible = false;
                    Protectbool = false;
                }
            };
            imgProtect.GestureRecognizers.Add(tapimgProtect);


            var tapimgRemission = new TapGestureRecognizer();
            tapimgRemission.Tapped += (s, e) =>
            {
                if (Remissionbool == false)
                {
                    imgRemission.Source = "arrow.png";
                    imgRemission.Rotation = -90;
                    stkDeductions.IsVisible = true;
                    Remissionbool = true;
                }
                else
                {
                    imgRemission.Source = "arrow.png";
                    imgRemission.Rotation = 90;
                    stkDeductions.IsVisible = false;
                    Remissionbool = false;
                }
            };

            imgRemission.GestureRecognizers.Add(tapimgRemission);
        }

        protected override void OnAppearing()
        {
            stkHeader.IsVisible = true;

            if (ContentGlobal.INTcountNotiUnread > 0)
            {
                lbcountNotiUnread.Text = ContentGlobal.INTcountNotiUnread.ToString();
            }
            else
            {
                lbcountNotiUnread.IsVisible = false;
            }

            stkDeductions.Children.Clear();
            stkIncome.Children.Clear();
            stkInsurance.Children.Clear();
            year = (int)ContentGlobal.allldata["timeoff"]["year"];
            month = (int)ContentGlobal.allldata["timeoff"]["period"];

            loadPayroll();
        }


        public void loadPayroll()
        {
            var dataEE_payroll = ContentGlobal.allldata["payroll"];

            if (dataEE_payroll != null)
            {
                var dataEE_year = dataEE_payroll[year.ToString()];
                if (dataEE_year != null)
                {
                    var dataEE = dataEE_year[(selectedIndexMonth + 1).ToString()];

                    if (dataEE != null)
                    {
                        Debug.WriteLine("dataEE-----------------------" + year + "---------------" + month + "-------------" + dataEE);

                        TotalMoney.Text = dataEE["total_money"].ToString() + " VND";
                        lbDateReceive.Text = dataEE["date_receive"].ToString();

                        var dataIncome = dataEE["income_details"];
                        Debug.WriteLine("dataIncome-----------------------------------------" + dataIncome);

                        var dictIncome = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataIncome.ToString());
                        foreach (var kv in dictIncome)
                        {
                            Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);

                            var stkSub = new StackLayout()
                            {
                                Orientation = StackOrientation.Horizontal,
                                Margin = new Thickness(20, 0, 20, 0),
                            };

                            var lbContent = new Label()
                            {
                                Text = kv.Key.ToString(),
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                FontSize = 14,
                            };

                            var lbValues = new Label()
                            {
                                Text = kv.Value.ToString(),
                                HorizontalOptions = LayoutOptions.End,
                                FontSize = 14,
                            };

                            stkSub.Children.Add(lbContent);
                            stkSub.Children.Add(lbValues);

                            stkIncome.Children.Add(stkSub);
                        }


                        var dataInsurance = dataEE["insurance_details"];
                        Debug.WriteLine("dataInsurance-----------------------------------------" + dataInsurance);

                        var dictInsurance = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataInsurance.ToString());
                        foreach (var kv in dictInsurance)
                        {
                            Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);

                            var stkSub = new StackLayout()
                            {
                                Orientation = StackOrientation.Horizontal,
                                Margin = new Thickness(20, 0, 20, 0),
                            };

                            var lbContent = new Label()
                            {
                                Text = kv.Key,
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                FontSize = 14,
                            };

                            var lbValues = new Label()
                            {
                                Text = kv.Value.ToString(),
                                HorizontalOptions = LayoutOptions.End,
                                FontSize = 14,
                            };

                            stkSub.Children.Add(lbContent);
                            stkSub.Children.Add(lbValues);

                            stkInsurance.Children.Add(stkSub);
                        }


                        var dataDeductions = dataEE["deductions_detail"];
                        Debug.WriteLine("dataDeductions-----------------------------------------" + dataDeductions);

                        var dictDeductions = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataDeductions.ToString());
                        foreach (var kv in dictDeductions)
                        {
                            Debug.WriteLine("key :--------" + kv.Key + "valueeeeeeeeeeee-----------------" + kv.Value);

                            var stkSub = new StackLayout()
                            {
                                Orientation = StackOrientation.Horizontal,
                                Margin = new Thickness(20, 0, 20, 0),
                            };

                            var lbContent = new Label()
                            {
                                Text = kv.Key,
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                FontSize = 14,
                            };

                            var lbValues = new Label()
                            {
                                Text = kv.Value.ToString(),
                                HorizontalOptions = LayoutOptions.End,
                                FontSize = 14,
                            };

                            stkSub.Children.Add(lbContent);
                            stkSub.Children.Add(lbValues);

                            stkDeductions.Children.Add(stkSub);
                        }
                    }
                    else
                    {
                        lbDateReceive.Text = "";
                        TotalMoney.Text = "X.XXX.XXX.XXX VND";
                        stkIncome.Children.Clear();
                        stkInsurance.Children.Clear();
                        stkDeductions.Children.Clear();
                    }
                   

                }
                else
                {
                    lbDateReceive.Text = "";
                    TotalMoney.Text = "X.XXX.XXX.XXX VND";
                    stkIncome.Children.Clear();
                    stkInsurance.Children.Clear();
                    stkDeductions.Children.Clear();
                }
            }
            else
            {
                lbDateReceive.Text = "";
                TotalMoney.Text = "X.XXX.XXX.XXX VND";
                stkIncome.Children.Clear();
                stkInsurance.Children.Clear();
                stkDeductions.Children.Clear();
            }

           
        }
    }


}