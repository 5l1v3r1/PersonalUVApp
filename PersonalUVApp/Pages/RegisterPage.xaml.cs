using PersonalUVApp.Helper;
using PersonalUVApp.Models;
using System;
using System.Linq;
using SQLite;
using Xamarin.Forms;

namespace PersonalUVApp.Pages
{
    public partial class RegisterPage : ContentPage
    {
        
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = this;
            App.db = DependencyService.Get<ISQLiteConnection>().CreateConnection();
            App.db.CreateTable<User>();
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            if (AreDetailsValid())
            {
                int _test = App.db.Insert(new User
                {
                    Username = UsernameEntry.Text,
                    Password = PasswordEntry.Text,
                    FirstName = FirstnameEntry.Text,
                    LastName = LastnameEntry.Text,
                    //                Age = Convert.ToInt32(AgeEntry.Text),
                    SkinType = SkinTypeEntry.Text,
                    Location = LocationEntry.Text,
                });
                Console.WriteLine("Kayit yapildi mi?"+_test);
                //                await Navigation.PopAsync();

                Page rootPage = Navigation.NavigationStack.FirstOrDefault();
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

        protected bool AreDetailsValid()
        {
            return !string.IsNullOrWhiteSpace(UsernameEntry.Text); //&& !string.IsNullOrWhiteSpace(PasswordEntry.Text);
        }
    }
}
