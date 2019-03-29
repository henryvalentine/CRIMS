using Crims.Core.Utils.Serializer;
using Crims.UI.Win.Enroll.Classes;
using Crims.UI.Win.Enroll.DataServices;
using Crims.UI.Win.Enroll.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Crims.UI.Win.Enroll.Forms
{
    public partial class Login : Form
    {
        private LoginNotifyer _LoginNotifyer;
        private UserAccountModel _UserProfile;
        private string _LoginUrl = Settings.Default.LoginServiceUrl;

        public Login()
        {
            InitializeComponent();
        }

        public Login(LoginNotifyer _LoginNotifyer)
        {
            this._LoginNotifyer = _LoginNotifyer;
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            if (Settings.Default.AuthoriseOnlyUserName)
            {
                textBoxPassword.Enabled = false;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _LoginNotifyer(_UserProfile);
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            if (textBoxUserName.Text.Trim() == string.Empty)
            {
                labelMsg.Text = "Please Enter a Username";
                return;
            }
            if (!Settings.Default.AuthoriseOnlyUserName && textBoxPassword.Text.Trim() == String.Empty)
            {
                labelMsg.Text = "Please Enter a Username";
                return;
            }
            string email = textBoxUserName.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            labelMsg.Text = "Logging In, Please wait...";
            buttonLogin.Enabled = false;


            if (Settings.Default.AuthoriseOnlyUserName)
            {
                var result = await Task.Run(() =>
                {
                    return DatabaseOpperations.AuthoriseOnlyUserName(email);
                });

                if (result.Rows.Count > 0)
                {
                    _UserProfile = new UserAccountModel
                    {
                        UserName = result.Rows[0]["Email"].ToString(),
                        UserId = result.Rows[0]["Id"].ToString(),
                        ProfileId = result.Rows[0]["UserInfo_Id"].ToString(),
                        Email = result.Rows[0]["Email"].ToString()
                    };
                    _LoginNotifyer(_UserProfile, true);
                }
                else
                {
                    labelMsg.Text = "Invalid Username";
                    buttonLogin.Enabled = true;
                    return;
                }
            }
            else
            {

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(_LoginUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, _LoginUrl + "ApiLogin?");
                var keyValues = new List<KeyValuePair<string, string>>();
                keyValues.Add(new KeyValuePair<string, string>("email", email));
                keyValues.Add(new KeyValuePair<string, string>("password", password));
                msg.Content = new FormUrlEncodedContent(keyValues);
                HttpResponseMessage response = await client.SendAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    _UserProfile = Newtonsoft.Json.JsonConvert.DeserializeObject<UserAccountModel>(result);
                }
                else
                {
                    var errorMessage = response.StatusCode.ToString();
                    labelMsg.Text = errorMessage + " - Unable to login. Please contact your Admin.";
                    buttonLogin.Enabled = true;
                    textBoxPassword.Text = string.Empty;
                    return;
                }
                if (_UserProfile.Code == 5)
                {
                    //After Succesfully Loging In...
                    _LoginNotifyer(_UserProfile, true);
                }
                else
                {
                    buttonLogin.Enabled = true;
                    textBoxPassword.Text = string.Empty;
                    labelMsg.Text = _UserProfile.Message;
                }
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
        }

        private void linkLabelSettigns_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new FormSettings().ShowDialog();
        }
    
    }
}
