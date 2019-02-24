using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PersonalUVApp.Pages
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void RegisterAsync(object sender, EventArgs e)
        {
            App.UVApp.NavigateToPage(new LoginPage());
        }
    }
}
