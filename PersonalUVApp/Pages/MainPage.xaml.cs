using Newtonsoft.Json;
using PersonalUVApp.Helper;
using PersonalUVApp.Models;
using System;
using Xamarin.Forms;

namespace PersonalUVApp.Pages
{
    public partial class MainPage //: ContentPage
    {
        public UserModel User
        {
            get => (UserModel)GetValue(UserProperty);
            set => SetValue(UserProperty, value);
        }

        public static readonly BindableProperty UserProperty = BindableProperty.Create(nameof(User), typeof(UserModel), typeof(MainPage));

        public MainPage()
        {
            User = JsonConvert.DeserializeObject<UserModel>(Settings.UserJson);
            InitializeComponent();
            BindingContext = this;
        }
        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }



    }
}
