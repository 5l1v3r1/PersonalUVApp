using PersonalUVApp.Models;
using System;
using System.Linq;
using SQLite;
using Xamarin.Forms;
using PersonalUVApp.Helper;
using Newtonsoft.Json;
using Acr.UserDialogs;

namespace PersonalUVApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        //public Command<PageActionEnum> NavigateCommand { protected set; get; }

        public Command LoginCommand { protected set; get; }
        public Command ForgotPasswordCommand { protected set; get; }
        public Command RegisterCommand { protected set; get; }


        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            if (Settings.IsRememberMe == false)
            {
                InitializeComponent();
                LoginCommand = new Command(Login);
                ForgotPasswordCommand = new Command(ForgotPassword);
                RegisterCommand = new Command(Register);
                //NavigateCommand = new Command<PagForgotPassword);
                BindingContext = this;
            }
        }

        private void Register(object obj)
        {
            App.UvApp.NavigateToPage(new RegisterPage());
        }

        private void ForgotPassword(object obj)
        {
            App.UvApp.NavigateToPage(new ForgetPasswordPage());
        }

        private void Login(object obj)
        {
            UserModel usr = JsonConvert.DeserializeObject<UserModel>(Settings.UserJson);

            if (usr == null)
            {
                UserDialogs.Instance.Alert("Username is not valid!");
                return;
            }

            if (UsernameEntry.Text == usr.Username && PasswordEntry.Text == usr.Password)
            {
                Settings.IsRememberMe = true;

                App.UvApp.ChangeRoot(new MainPage());
            }
            else
                DisplayAlert("Error", "There is no user", "OK");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Settings.IsRememberMe)
            {
                App.UvApp.ChangeRoot(new MainPage());
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

                    if (usr == null)
                    {
                        UserDialogs.Instance.Alert("Username is not valid!");
                        return;
                    }

                    if (UsernameEntry.Text == usr.Username && PasswordEntry.Text == usr.Password)
                    {
                        Settings.IsRememberMe = true;

                        App.UvApp.ChangeRoot(new MainPage());
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
