using Newtonsoft.Json;
using PersonalUVApp.Helper;
using PersonalUVApp.Models;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace PersonalUVApp.Pages
{
    public partial class RegisterPage : ContentPage
    {
        public UserModel User
        {
            get => (UserModel)GetValue(UserProperty);
            set => SetValue(UserProperty, value);
        }

        public static readonly BindableProperty UserProperty = BindableProperty.Create(nameof(User), typeof(UserModel), typeof(RegisterPage), null);

        public Command RegisterCommand { protected set; get; }

        public RegisterPage()
        {
            User = new UserModel() { 
                Username = "deneme",
                Password = "123",
                FirstName = "Bismillah",
                LastName = "Her hayrın başı",
                Age = 12,

            };
            InitializeComponent();

            RegisterCommand = new Command(Register);

            BindingContext = this;
        }

        private void Register(object obj)
        {
            string serialized = JsonConvert.SerializeObject(User);
            Settings.UserJson = serialized;

            DisplayAlert("Success", "Registriation has been done successfully", "OK");

            App.UvApp.PopPage();
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {

                //App.Db.Insert(new User
                //{
                //    Username = UsernameEntry.Text,
                //    Password = PasswordEntry.Text,
                //    FirstName = FirstnameEntry.Text,
                //    LastName = LastnameEntry.Text,
                //    //                Age = Convert.ToInt32(AgeEntry.Text),
                //    SkinType = SkinTypeEntry.Text,
                //    Location = LocationEntry.Text,
                //});
                ////                await Navigation.PopAsync();

                var rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage == null) return;
                App.IsUserLoggedIn = true;
                Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                await Navigation.PopToRootAsync();
        }


        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == UserProperty.PropertyName)
                Console.WriteLine("denemeasd");
        }

    }
}
