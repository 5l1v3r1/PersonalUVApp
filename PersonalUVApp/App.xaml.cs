using PersonalUVApp.Models;
using PersonalUVApp.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PersonalUVApp
{
    public partial class App //: Application
    {
        public static string DbName { get; set; } = "UserDb";
        public static App UvApp => Current as App;
        public static bool IsUserLoggedIn { get; set; }
        //        public static SQLiteConnection Db { get; set; }
        public static User NewUser { get; set; }

        public App()
        {
            InitializeComponent();
            if (!IsUserLoggedIn)
            {
                MainPage = new NavigationPage(new LoginPage())
                {
                    BarTextColor = Color.White,
                    BarBackgroundColor = (Color)Resources["BarBackgroundColor"]
                };
            }
            else
            {
                MainPage = new NavigationPage(new MainPage()); //boyle kalsin simdilik

            }
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
