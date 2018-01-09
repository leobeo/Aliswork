using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aliswork.Appeal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAppealPage : ContentPage
    {
        int selectedIndexTypeAppeal = -1;
        string People_AssignId = "";
        public NewAppealPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.FromHex("#F0EFF5");

            List<String> typeAppeal = ContentGlobal.ListAppeal;
            foreach (String nn in typeAppeal)
            {
                pkTypeAppeal.Items.Add(nn);
            }

            pkTypeAppeal.SelectedIndexChanged += (sender, e) =>
            {
                selectedIndexTypeAppeal = pkTypeAppeal.SelectedIndex;
                if (selectedIndexTypeAppeal > -1)
                {
                    edtCause.IsEnabled = true;
                }
            };

            var tapImgreset = new TapGestureRecognizer();
            tapImgreset.Tapped += (s, e) =>
            {
                pkTypeAppeal.SelectedIndex = -1;
                edtCause.Text = "";
            };
            imgReset.GestureRecognizers.Add(tapImgreset);
        }

        public void edtEdit(object sender, EventArgs e)
        {
            if (edtCause.Text.ToString().Length > 0)
            {
                imgReset.IsEnabled = true;
                imgSend.IsEnabled = true;
            }
            else
            {
                imgReset.IsEnabled = false;
                imgSend.IsEnabled = false;
            }
        }

        protected async override void OnAppearing()
        {
            string companyId = (string)ContentGlobal.allldata["info"]["company"]["id"];
            string strParam = @"{""companyId"":""" + companyId + @""",""type"":2}";
            Debug.WriteLine("strParam---------------------" + strParam);

            var getid = await ContentGlobal.FirebasePOSTFunctions("GetIdLetter", strParam);
            Debug.WriteLine("getid---------------------" + getid);

            string id = (string)getid["ID"];
            lbNumberId.Text = "BS" + id;

            lbBase_NumberId.Text = Application.Current.Properties["baseAppeal"].ToString();

            string People_AssignName = "";
            var dataEE = ContentGlobal.allldata["info"]["managed"];
            Debug.WriteLine("dataEE-----------------------------" + dataEE);

            var dict = JsonConvert.DeserializeObject<Dictionary<string, JContainer>>(dataEE.ToString());
            foreach (var kv in dict)
            {
                Debug.WriteLine("key :--------" + kv.Key + "value-----------------" + kv.Value);
                if ((int)kv.Value["state"] == 1)
                {
                    People_AssignId = kv.Key;
                    People_AssignName = (string)kv.Value["name"];
                }
            }

            lbPeopleAssign.Text = People_AssignName;
        }

        public async void SendRequest(object sender, EventArgs e)
        {

            var answer = await DisplayAlert("Phiếu Xin nghỉ", "Bạn có thực sự muốn gửi phiếu nghỉ này", "Yes", "No");
            if (answer == true)
            {
                try
                {
                    string strSub = "CreateAppeal";
                    string companyId = (string)ContentGlobal.allldata["info"]["company"]["id"];

                    int typeAppeal = selectedIndexTypeAppeal;
                    Debug.WriteLine("typeAppeal-------------------------" + typeAppeal);

                    string appealId = lbNumberId.Text.ToString();
                    Debug.WriteLine("appealId-------------------------" + appealId);

                    string people_send = Application.Current.Properties["uId"].ToString();
                    Debug.WriteLine("people_send-------------------------" + people_send);

                    string people_assign = People_AssignId;
                    Debug.WriteLine("people_assign-------------------------" + people_assign);

                    string base_on = lbBase_NumberId.Text.ToString();
                    Debug.WriteLine("base_on-------------------------" + base_on);

                    string content = edtCause.Text.ToString();
                    Debug.WriteLine("content-------------------------" + content);

                    string strParam = @"{""companyId"":""" + companyId + @""",""typeAppeal"":" + typeAppeal + @",
	""appealId"":""" + appealId + @""",""people_send"":""" + people_send + @""",""people_assign"":""" + people_assign + @""",
    ""base_on"":""" + base_on + @""",""content"":""" + content + @"""}";

                    Debug.WriteLine("strParam-------------------------" + strParam);

                    var data = ContentGlobal.FirebasePOSTFunctions(strSub, strParam);

                    //await Navigation.PopAsync();
                    Page page = new MainPage();
                    NavigationPage.SetHasNavigationBar(page, false);
                    App.Current.MainPage = new NavigationPage(page);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}