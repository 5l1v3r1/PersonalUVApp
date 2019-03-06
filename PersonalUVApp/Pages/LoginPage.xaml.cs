using PersonalUVApp.Models;
using System;
using System.Linq;
using SQLite;
using Xamarin.Forms;
using PersonalUVApp.Helper;
using Newtonsoft.Json;

namespace PersonalUVApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        public Command<PageActionEnum> NavigateCommand { protected set; get; }

        public LoginPage()
        {
            if (Settings.IsRememberMe == false)
            {
                InitializeComponent();
                NavigateCommand = new Command<PageActionEnum>(NavigatePage);
                BindingContext = this;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Settings.IsRememberMe)
            {
                App.UvApp.ChangeRoot(new BluetoothPage());
            }
            else
            {
                if (Settings.IsUserModelSet == true)
                {
                    UserModel usr = JsonConvert.DeserializeObject<UserModel>(Settings.UserJson);

                    UsernameEntry.Text = usr.Username;
                    PasswordEntry.Text = usr.Password;
                }
            }
        }

        private void NavigatePage(PageActionEnum obj)
        {
            switch (obj)
            {
                case PageActionEnum.Login:

                    UserModel usr = JsonConvert.DeserializeObject<UserModel>(Settings.UserJson);

                    if (UsernameEntry.Text == usr.Username && PasswordEntry.Text == usr.Password)
                    {
                        if (RememberSwitch.On)
                            Settings.IsRememberMe = true;

                        App.UvApp.ChangeRoot(new BluetoothPage());
                    }
                    else
                        DisplayAlert("Error", "There is no user", "OK");

                    break;
                case PageActionEnum.ForgetPassword:
                    App.UvApp.NavigateToPage(new ForgetPasswordPage());
                    break;
                case PageActionEnum.Register:
                    App.UvApp.NavigateToPage(new RegisterPage());
                    break;
            }
        }
       

    }
}
