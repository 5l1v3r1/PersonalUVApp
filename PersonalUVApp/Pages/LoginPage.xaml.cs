using System;
using System.Collections.Generic;

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

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}
