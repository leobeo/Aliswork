using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Aliswork.Tabbed
{
    class ClockViewModel : INotifyPropertyChanged
    {
        string dateTime;

        public event PropertyChangedEventHandler PropertyChanged;

        public ClockViewModel()
        {
            DateTime dt = DateTime.Now;
            //Debug.WriteLine("DateTime------------------------------------------" + dt);

            int day = dt.Day;
            // Debug.WriteLine("day--------------------------------------" + day);

            int month = dt.Month;
            //Debug.WriteLine("month--------------------------------------" + month);

            int year = dt.Year;
            //Debug.WriteLine("year--------------------------------------" + year);

            JToken dataEE_Timekepping;
            JToken dataTimeKeeping_month;
            JToken dataTimeKeeping;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>{
                dataEE_Timekepping = ContentGlobal.allldata["timekeeping"];

                if (dataEE_Timekepping != null)
                {
                    var dataTimeKeeping_year = dataEE_Timekepping[year.ToString()];
                    if (dataTimeKeeping_year != null)
                    {
                        dataTimeKeeping_month = dataTimeKeeping_year[month.ToString() + "_" + year.ToString()];
                        if (dataTimeKeeping_month != null)
                        {
                            // Debug.WriteLine("dataTimeKeeping_month--------------ClockViewModel------------------------" + dataTimeKeeping_month);

                            dataTimeKeeping = dataTimeKeeping_month[day.ToString() + "_" + month.ToString()];

                            //Debug.WriteLine("dataTimeKeeping--------------ClockViewModel------------------------" + dataTimeKeeping);
                            if (dataTimeKeeping != null)
                            {
                                if (dataTimeKeeping["time_out"] == null)
                                {
                                    if ((TimeSpan)dataTimeKeeping["base_time_in"] >= (TimeSpan)dataTimeKeeping["time_in"])
                                    {
                                        this.Time = (DateTime.Now.TimeOfDay - (TimeSpan)dataTimeKeeping["base_time_in"]).ToString(@"hh\:mm") + " phút";
                                    }
                                    else if ((TimeSpan)dataTimeKeeping["base_time_in"] < (TimeSpan)dataTimeKeeping["time_in"])
                                    {
                                        this.Time = (DateTime.Now.TimeOfDay - (TimeSpan)dataTimeKeeping["time_in"]).ToString(@"hh\:mm") + " phút";
                                    }
                                }
                                else
                                {

                                    if ((TimeSpan)dataTimeKeeping["time_out"] < (TimeSpan)dataTimeKeeping["base_time_out"])
                                    {
                                        if ((TimeSpan)dataTimeKeeping["base_time_in"] >= (TimeSpan)dataTimeKeeping["time_in"])
                                        {
                                            this.Time = ((TimeSpan)dataTimeKeeping["time_out"] - (TimeSpan)dataTimeKeeping["base_time_in"]).ToString(@"hh\:mm") + " phút";
                                        }
                                        else if ((TimeSpan)dataTimeKeeping["base_time_in"] < (TimeSpan)dataTimeKeeping["time_in"])
                                        {
                                            this.Time = ((TimeSpan)dataTimeKeeping["time_out"] - (TimeSpan)dataTimeKeeping["time_in"]).ToString(@"hh\:mm") + " phút";
                                        }
                                    }
                                    else
                                    {
                                        if ((TimeSpan)dataTimeKeeping["base_time_in"] >= (TimeSpan)dataTimeKeeping["time_in"])
                                        {
                                            this.Time = ((TimeSpan)dataTimeKeeping["base_time_out"] - (TimeSpan)dataTimeKeeping["base_time_in"]).ToString(@"hh\:mm") + " phút";
                                        }
                                        else if ((TimeSpan)dataTimeKeeping["base_time_in"] < (TimeSpan)dataTimeKeeping["time_in"])
                                        {
                                            this.Time = ((TimeSpan)dataTimeKeeping["base_time_out"] - (TimeSpan)dataTimeKeeping["time_in"]).ToString(@"hh\:mm") + " phút";
                                        }
                                    }
                                }
                                //lbTimeIn.Text = dataTimeKeeping["time_in"].ToString();
                            }
                            else
                            {
                                this.Time = "00:00 phút";
                            }
                        }
                        else
                        {
                            this.Time = "00:00 phút";
                            Debug.WriteLine("dataTimeKeeping_month--------------ClockViewModel------------------------NULL");
                        }
                    }
                    else
                    {
                        this.Time = "00:00 phút";
                        Debug.WriteLine("dataTimeKeeping_year--------------ClockViewModel------------------------NULL");
                    }
                }
                else
                {
                    this.Time = "00:00 phút";
                }
                return true;
            });

            #region commit
            /*
            DateTime dt = DateTime.Now;
            Debug.WriteLine("DateTime------------------------------------------" + dt);

            int day = dt.Day;
            Debug.WriteLine("day--------------------------------------" + day);

            int month = dt.Month;
            Debug.WriteLine("month--------------------------------------" + month);

            int year = dt.Year;
            Debug.WriteLine("year--------------------------------------" + year);

            var dataEE_Timekepping = ContentGlobal.allldata["timekeeping"];

            if (dataEE_Timekepping != null)
            {
                var dataTimeKeeping_month = dataEE_Timekepping[year.ToString()][month.ToString()];
                if (dataTimeKeeping_month != null)
                {
                    Debug.WriteLine("dataTimeKeeping_month--------------ClockViewModel------------------------" + dataTimeKeeping_month);

                    var dataTimeKeeping = dataTimeKeeping_month[day.ToString()];

                    Debug.WriteLine("dataTimeKeeping--------------ClockViewModel------------------------" + dataTimeKeeping);
                    if (dataTimeKeeping != null)
                    {
                        if (dataTimeKeeping["time_out"] == null)
                        {
                            if ((TimeSpan)dataTimeKeeping["base_time_in"] >= (TimeSpan)dataTimeKeeping["time_in"])
                            {
                                this.Time = (DateTime.Now.TimeOfDay - (TimeSpan)dataTimeKeeping["base_time_in"]).ToString(@"hh\:mm") + " phút";

                                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                                {
                                    this.Time = (DateTime.Now.TimeOfDay - (TimeSpan)dataTimeKeeping["base_time_in"]).ToString(@"hh\:mm") + " phút";
                                    return true;
                                });
                            }
                            else if ((TimeSpan)dataTimeKeeping["base_time_in"] < (TimeSpan)dataTimeKeeping["time_in"])
                            {
                                this.Time = (DateTime.Now.TimeOfDay - (TimeSpan)dataTimeKeeping["time_in"]).ToString(@"hh\:mm") + " phút";

                                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                                {
                                    this.Time = (DateTime.Now.TimeOfDay - (TimeSpan)dataTimeKeeping["time_in"]).ToString(@"hh\:mm") + " phút";
                                    return true;
                                });
                            }
                        }
                        else
                        {

                            if ((TimeSpan)dataTimeKeeping["time_out"] < (TimeSpan)dataTimeKeeping["base_time_out"])
                            {
                                if ((TimeSpan)dataTimeKeeping["base_time_in"] >= (TimeSpan)dataTimeKeeping["time_in"])
                                {
                                    this.Time = ((TimeSpan)dataTimeKeeping["time_out"] - (TimeSpan)dataTimeKeeping["base_time_in"]).ToString(@"hh\:mm") + " phút";
                                }
                                else if ((TimeSpan)dataTimeKeeping["base_time_in"] < (TimeSpan)dataTimeKeeping["time_in"])
                                {
                                    this.Time = ((TimeSpan)dataTimeKeeping["time_out"] - (TimeSpan)dataTimeKeeping["time_in"]).ToString(@"hh\:mm") + " phút";
                                }
                            }
                            else
                            {
                                if ((TimeSpan)dataTimeKeeping["base_time_in"] >= (TimeSpan)dataTimeKeeping["time_in"])
                                {
                                    this.Time = ((TimeSpan)dataTimeKeeping["base_time_out"] - (TimeSpan)dataTimeKeeping["base_time_in"]).ToString(@"hh\:mm") + " phút";
                                }
                                else if ((TimeSpan)dataTimeKeeping["base_time_in"] < (TimeSpan)dataTimeKeeping["time_in"])
                                {
                                    this.Time = ((TimeSpan)dataTimeKeeping["base_time_out"] - (TimeSpan)dataTimeKeeping["time_in"]).ToString(@"hh\:mm") + " phút";
                                }
                            }
                        }
                        //lbTimeIn.Text = dataTimeKeeping["time_in"].ToString();
                    }
                    else
                    {
                        this.Time = "00:00 phút";
                    }
                }
                else
                {
                    this.Time = "00:00 phút";
                    Debug.WriteLine("dataTimeKeeping_month--------------ClockViewModel------------------------NULL");
                }
            }
            else
            {
                this.Time = "00:00 phút";
            }*/
            #endregion
        }

        public string Time
        {
            set
            {
                dateTime = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                        new PropertyChangedEventArgs("Time"));
                }
            }
            get
            {
                return dateTime;
            }
        }
    }
}