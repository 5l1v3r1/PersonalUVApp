using System;

using Xamarin.Forms;

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
            App.UVApp.NavigateToPage(new MainPage());
        }
    }
}
