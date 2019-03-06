using PersonalUVApp.Models;
using System;
using System.Linq;
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

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            if (AreDetailsValid())
            {

                App.NewUser = new User
                {
                    Username = UsernameEntry.Text,
                    Password = PasswordEntry.Text,
                    FirstName = FirstnameEntry.Text,
                    LastName = LastnameEntry.Text,
                    //                Age = Convert.ToInt32(AgeEntry.Text),
                    SkinType = SkinTypeEntry.Text,
                    Location = LocationEntry.Text,
                };
                //                await Navigation.PopAsync();


                Page rootPage = Navigation.NavigationStack.FirstOrDefault();
                if (rootPage == null) return;
                App.IsUserLoggedIn = true;
                Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
                await Navigation.PopToRootAsync();
            }
            else
            {
                Console.WriteLine("Sign up failed");
            }
        }

        protected bool AreDetailsValid()
        {
            return !string.IsNullOrWhiteSpace(UsernameEntry.Text) && !string.IsNullOrWhiteSpace(PasswordEntry.Text);
        }
    }
}
