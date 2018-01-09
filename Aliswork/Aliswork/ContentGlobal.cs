using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Aliswork;

namespace Aliswork
{
    
    public class LVItemAppeal
    {
        public string NumberId { set; get; }
        public string IconSource { set; get; }
        public string Name { set; get; }
        public string TimePreview { set; get; }
    }

    public class LVItemRequest
    {
        public string NumberId { set; get; }
        public string IconSource { set; get; }
        public string Name { set; get; }

        public string TimeOff { set; get; }
        public string TimePreview { set; get; }
    }

    public static class ContentGlobal
    {
        public static string registrationId = "";
        public static string path = "";
        public static Dictionary<string, string> dictNotiRecieve = new Dictionary<string, string>();

        private static int year_y = DateTime.Now.Year;

        public static List<String> ListYear = new List<String> { (year_y-2).ToString(), (year_y - 1).ToString(), (year_y).ToString() };
        public static List<String> ListMonth = new List<String> { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };
        public static List<String> ListAppeal = new List<String> { "Khiếu nại chấm công", "Khiếu nại bảng lương" };
        //public static List<String> ListRequest= new List<String> ();//{ "Xin nghỉ phép", "Xin nghỉ không lương", "Xin nghỉ khác", " Đăng ký làm thêm giờ" };
        public static Dictionary<string, string> ListRequest = new Dictionary<string, string>();

        public static string email;
        public static int INTcountNotiUnread = 0;
        public static int statusCode;

        public static JContainer allldata;
        public static JContainer PeopleWorking;

        public static bool Userroot = true;

        public static bool saveSetting = false;


        public static string statusNetwork()
        {
            string status = "";
            status = CrossConnectivity.Current.IsConnected ? "Connected" : "No Connection";

            return status;
        }

        public static async Task<JContainer> PostAllbyForm(string strSub, FormUrlEncodedContent pair)
        {
            string uri = @"http://45.76.9.123" + strSub;
            var formContent = pair;

            var myHttpClient = new HttpClient();
            var response = await myHttpClient.PostAsync(uri, formContent);
            ContentGlobal.statusCode = (int)response.StatusCode;
            string json = response.Content.ReadAsStringAsync().Result;
            JContainer data = (JContainer)JsonConvert.DeserializeObject(json);
            return data;
        }

        public static async Task<JContainer> PostAllFirebase_Auth(string strSub, string strParam)
        {
            var client = new HttpClient();
            string url = @"https://www.googleapis.com/identitytoolkit/v3/relyingparty/" + strSub + @"?key=AIzaSyBUUdE2FGpT9Qtg9GXfKNvUiySiaR45ln0";
            var content = new StringContent(strParam, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            // this result string should be something like: "{"token":"rgh2ghgdsfds"}"
            string json = response.Content.ReadAsStringAsync().Result;
            JContainer data = (JContainer)JsonConvert.DeserializeObject(json);
            return data;
        }

        public static async Task<JContainer> FirebasePOSTFunctions(string strSubPath, string strParam)
        {
            var client = new HttpClient();
            string url = @"https://us-central1-xamarinfcm-ce873.cloudfunctions.net/" + strSubPath;
            var content = new StringContent(strParam, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            if ((int)response.StatusCode == 200)
            {
                ContentGlobal.allldata = await ContentGlobal.GetAllOfPersonal();
                loadListRequest();
            }
            // this result string should be something like: "{"token":"rgh2ghgdsfds"}"
            string json = response.Content.ReadAsStringAsync().Result;
            JContainer data = (JContainer)JsonConvert.DeserializeObject(json);
            return data;
        }

        public static async Task<JContainer> FirebasePUTFunctions(string strSubPath, string strParam)
        {
            var client = new HttpClient();
            string url = @"https://us-central1-xamarinfcm-ce873.cloudfunctions.net/" + strSubPath;
            var content = new StringContent(strParam, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url, content);
            if ((int)response.StatusCode == 200)
            {
                ContentGlobal.allldata = await ContentGlobal.GetAllOfPersonal();
                loadListRequest();
            }
            string json = response.Content.ReadAsStringAsync().Result;
            JContainer data = (JContainer)JsonConvert.DeserializeObject(json);
            return data;
            // this result string should be something like: "{"token":"rgh2ghgdsfds"}"

        }

        #region Count Unread
        public static int CountNotiUnread(string type_noti,int month)
        {
            Debug.WriteLine("type_noti--------------------------------------" + type_noti);
            Debug.WriteLine("month-------------------------------------------" + month);

            var year = (int)ContentGlobal.allldata["timeoff"]["year"];
            int count=0;
            try
            {
                var data = allldata["notificaton"][type_noti][year.ToString()][month.ToString()+"_"+year.ToString()];

                Debug.WriteLine("data CountNotiUnread---------------------------------------------------" + data);

                var dict = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(data.ToString());
                foreach (var kv in dict)
                {
                    if ((int)kv.Value["state"] == 0)
                    {
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Err CountNotiUnread--------------------------------------"+ex);
            }

            return count;
        }
        #endregion

        #region Get Info Person
        public static async Task<JContainer> GetAllOfPersonal()
        {
            var properties = Application.Current.Properties;
            var client = new HttpClient();
            string strSubPath = "AllOfPersonal";
            string url = @"https://us-central1-xamarinfcm-ce873.cloudfunctions.net/" + strSubPath;
            string strParam = @"{""uid"" : """ + properties["uId"].ToString() + @"""}";
            var content = new StringContent(strParam, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            if ((int)response.StatusCode == 200)
            {
                Application.Current.Properties["date_clone"]= DateTime.Now.ToString();
            }
            // this result string should be something like: "{"token":"rgh2ghgdsfds"}"
            string json = response.Content.ReadAsStringAsync().Result;
            JContainer data = (JContainer)JsonConvert.DeserializeObject(json);
            return data;
        }

        #endregion

        #region Load List Request
        public static void loadListRequest()
        {
            ListRequest.Clear();
            var list_request = ContentGlobal.allldata["info"]["company_rules"]["list_request"];


            if (list_request != null)
            {
                var dictList_request = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(list_request.ToString());
                foreach (var kv in dictList_request)
                {
                    Debug.WriteLine("key_list_request :--------" + kv.Key + "valuelist_request : -----------------" + kv.Value);
                    ListRequest.Add(kv.Value["name"].ToString(),kv.Key.ToString());
                }
            }
        }

        #endregion

        #region People Working
        public static async Task<JContainer> peopleWorkingAsync()
        {
            string strSubFirebase = "AllTimekeeping";
            string strParam = @"{ ""companyId"" : """ + ContentGlobal.allldata["info"]["company"]["id"].ToString() + @"""} ";
            var data = await ContentGlobal.FirebasePOSTFunctions(strSubFirebase, strParam);
            return data;
        }
        #endregion

        #region Validate values string
        const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        const string passwordRegex = @"^(?=.*[A-Za-z])([0-9])[A-Za-z]{8,}$";
        #endregion

        #region Validate_Input
        public static bool Datepicker_DateSelected(DateTime dt)
        {
            DateTime value = dt;
            int year = DateTime.Now.Year;
            int selyear = value.Year;
            int result = selyear - year;
            bool isValid = false;
            if (result <= 100 && result > 0)
            {
                isValid = true;
            }

            return isValid;
        }

        public static bool EmailChecked(string strInput)
        {
            bool IsValid = false;
            IsValid = (Regex.IsMatch(strInput, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            return IsValid;
        }

        public static bool NumberChecked(string strInput)
        {
            int result;
            bool isValid = int.TryParse(strInput, out result);
            return isValid;
        }

        public static bool PasswordChecked(string strInput)
        {
            var input = strInput;

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            var isValidated = hasNumber.IsMatch(input) && hasUpperChar.IsMatch(input) && hasLowerChar.IsMatch(input) && hasMinimum8Chars.IsMatch(input) && !hasSymbols.IsMatch(input);
            return isValidated;
        }

        public static bool OnlyTextChecked(string strInput)
        {
            var input = strInput;

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]+");
            var hasSpace = new Regex(@"\s*");

            var isValidated = !hasNumber.IsMatch(input) && hasUpperChar.IsMatch(input) && hasLowerChar.IsMatch(input) && hasMinimum8Chars.IsMatch(input) && !hasSymbols.IsMatch(input);
            return isValidated;
        }
        #endregion

        #region Image_load
        public static byte[] GetImageStreamAsBytes(Stream input)
        {
            var buffer = new byte[64 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static string StreamToBase64String(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                //ms.ToArray();
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        public static Stream StringFromBase64Stream(string input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] b = Convert.FromBase64String(input);
                //ms.ToArray();

                return new MemoryStream(b);
            }
        }
        #endregion

        #region getLocation
        public static async Task<Position> getLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5), null, false);

            return position;
        }
        #endregion
    }
}