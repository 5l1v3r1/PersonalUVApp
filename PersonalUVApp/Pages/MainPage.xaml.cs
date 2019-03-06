using System;

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
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}
