using System;
using System.Collections.Generic;
using System.Windows.Input;
using PersonalUVApp.Models;
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
            Console.WriteLine(obj + " asdasdas ");
        }

        void Handle_Tapped(object sender, TappedEventArgs e)
        {
            PageActionEnum page = (PageActionEnum)e.Parameter;

            if (page == PageActionEnum.ForgetPassword)
                App.UVApp.NavigateToPage(new ForgetPasswordPage());
            else if (page == PageActionEnum.Register)
                App.UVApp.NavigateToPage(new RegisterPage());

            Console.WriteLine(e.Parameter +  " asdad_" );
        }

        void LoginAsync(object sender, System.EventArgs e)
        {
            App.UVApp.NavigateToPage(new MainPage());
        }
    }
}
