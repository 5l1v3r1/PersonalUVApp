using Newtonsoft.Json;
using PersonalUVApp.Helper;
using PersonalUVApp.Models;
using System;

namespace PersonalUVApp.Pages
{
    public partial class ForgetPasswordPage
    {
        public ForgetPasswordPage()
        {
            InitializeComponent();
        }

        private void ForgetPasswordAsync(object sender, EventArgs e)
        {
            bool isSuccess = GetNewPassword();
            if (!isSuccess)
                return;
            Tools.ShowMessage("Password is changed!", "Success", "OK");
            App.UvApp.NavigateToPage(new LoginPage());
        }

        private bool GetNewPassword()
        {
            UserModel usr = JsonConvert.DeserializeObject<UserModel>(Settings.UserJson);

            if (UsernameEntry.Text.Trim() == usr.Username && FirstNameEntry.Text.Trim() == usr.FirstName)
            {
                usr.Password = NewPasswordEntry.Text.Trim();
                usr = JsonConvert.DeserializeObject<UserModel>(Settings.UserJson);
                return true;
            }
            return false;
        }
    }
}
