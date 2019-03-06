using PersonalUVApp.Models;
using System;
using Xamarin.Forms;
namespace PersonalUVApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }

        public static readonly BindableProperty UserNameProperty = BindableProperty.Create(nameof(UserName), typeof(string), typeof(LoginPage), default(string));

        public Command<PageActionEnum> NavigateCommand { protected set; get; }

        public LoginPage()
        {
            NavigateCommand = new Command<PageActionEnum>(NavigatePage);
            InitializeComponent();
            BindingContext = this;
        }

        private void NavigatePage(PageActionEnum obj)
        {
            Console.WriteLine(obj);
        }
        void Handle_Tapped(object sender, EventArgs e)
        {
            /*PageActionEnum page = (PageActionEnum)e.Parameter;

            if (page == PageActionEnum.ForgetPassword)
                App.UVApp.NavigateToPage(new ForgetPasswordPage());
            else if (page == PageActionEnum.Register)*/
            App.UvApp.NavigateToPage(new RegisterPage());

        }

        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            //            Helper.Settings.GeneralSettings = UsernameEntry.Text;
            //            Helper.Settings.GeneralSettings = PasswordEntry.Text;

            try
            {
                bool verificationUser = Helper.Settings.GeneralSettings.Equals(UsernameEntry.Text);
                if (!verificationUser)
                {

                    DisplayAlert("Something Wrong!", "Username or Password invalid", "OK");
                    PasswordEntry.Text = string.Empty;
                    return;
                }
                //DisplayAlert("Welcome", "Login Success", "OK"); //Bu silinecek
                App.IsUserLoggedIn = true;
                App.UvApp.NavigateToPage(new BluetoothPage());
            }
            catch (Exception ex)
            {
                DisplayAlert("Login", ex.ToString(), "OK");
            }





        }

        /*protected bool AreCredentialsCorrect(User user)
        {
            return true;
        }*/
        /*private void SwitchDataOnChanged(object sender, ToggledEventArgs e)
        {

        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                RememberSwitch.IsEnabled = IsEnabled;
            }
        }*/
    }
}
