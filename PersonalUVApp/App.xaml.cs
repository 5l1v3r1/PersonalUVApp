using System.Collections.ObjectModel;
using PersonalUVApp.Helper;
using PersonalUVApp.Models;
using PersonalUVApp.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PersonalUVApp
{
    public partial class App : Application
    {
        public static App UvApp => Current as App;
        public static bool IsUserLoggedIn { get; set; }

        public App()
        {
            InitializeComponent();

            //if (!IsUserLoggedIn)
            //{
            //    MainPage = new NavigationPage(new MainPage())
            //    {
            //        BarTextColor = Color.White,
            //        BarBackgroundColor = (Color)Resources["BarBackgroundColor"]
            //    };
            //}
            //else
            //{
            MainPage = new NavigationPage(new LoginPage()); //boyle kalsin simdilik
            //MainPage = new NavigationPage(new DisplayAlarmPage("sa", "sa", null)); //boyle kalsin simdilik
            //}
        }

        public async void NavigateToPage(ContentPage page, bool animation = true)
        {
            await MainPage.Navigation.PushAsync(page, animation);
        }
        public async void PopPage()
        {
            await MainPage.Navigation.PopAsync();
        }


        public void ChangeRoot(ContentPage newRoot)
        {
            MainPage = new NavigationPage(newRoot)
            {
                BarTextColor = Color.White,
                BarBackgroundColor = (Color)Resources["BarBackgroundColor"]
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
