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
            App.UvApp.NavigateToPage(new MainPage());
        }
    }
}
