using PersonalUVApp.Models;
using System;
using System.Linq;
using Xamarin.Forms;

namespace PersonalUVApp.Pages
{
    public partial class RegisterPage :ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            var user = new User()
            {
                Username = UsernameEntry.Text,
                Password = PasswordEntry.Text,
                FirstName = FirstnameEntry.Text,
                LastName = LastnameEntry.Text,
//                Age = int.Parse(AgeEntry.Text),
                SkinType = SkinTypeEntry.Text,
                Location = LocationEntry.Text,
            };
            var signUpSucceeded = AreDetailsValid(user);
            if (signUpSucceeded)
            {
                var rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage != null)
                {
                    App.IsUserLoggedIn = true;
                    Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                    await Navigation.PopToRootAsync();
                }
            }
            else
            {
                Console.WriteLine("Sign up failed");
            }
        }

        protected bool AreDetailsValid(User user)
        {
            return !string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.Password);
        }
    }
}
