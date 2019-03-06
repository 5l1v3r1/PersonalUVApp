using System;
using Xamarin.Forms;

namespace PersonalUVApp.Pages
{
    public partial class MainPage //: ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }
        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
            Device.BeginInvokeOnMainThread(async () =>
            {
                    base.OnBackButtonPressed();
                    await Navigation.PopAsync();
                
            });
            return true;
        }
    }

}
