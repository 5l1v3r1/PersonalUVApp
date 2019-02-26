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

        void Handle_Tapped(object sender, TappedEventArgs e)
        {
            PageActionEnum page = (PageActionEnum)e.Parameter;

            if (page == PageActionEnum.ForgetPassword)
                App.UVApp.NavigateToPage(new ForgetPasswordPage());
            else if (page == PageActionEnum.Register)
                App.UVApp.NavigateToPage(new RegisterPage());

        }

        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            User user = new User
            {
                Username = UsernameEntry.Text,
                Password = PasswordEntry.Text
            };
            var isValid = AreCredentialsCorrect(user);
            if (!isValid)
            {
                Console.WriteLine("Login failed");
                PasswordEntry.Text = string.Empty;
                return;
            }

            App.IsUserLoggedIn = true;
            App.UVApp.NavigateToPage(new MainPage());
        }

        protected bool AreCredentialsCorrect(User user)
        {
            return true;
        }
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
