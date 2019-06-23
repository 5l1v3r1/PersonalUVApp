using Newtonsoft.Json;
using PersonalUVApp.Helper;
using PersonalUVApp.Models;
using Plugin.Geolocator;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Position = Plugin.Geolocator.Abstractions.Position;

using Xamarin.Forms;
using Plugin.Connectivity;
using Acr.UserDialogs;

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
                Username = "",
                Password = "",
                FirstName = "",
                LastName = "",
                Age = "",
            };
            InitializeComponent();

            RegisterCommand = new Command(Register);

            BindingContext = this;
        }

        private void Register(object obj)
        {
            string serialized = JsonConvert.SerializeObject(User);
            Settings.UserJson = serialized;

            UserDialogs.Instance.Alert("Registriation has been done successfully!", "Success", "Ok");

            App.UvApp.PopPage();
        }



        private void OnSkinTypeClicked(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;

            if (clicked == null) return;

            switch (clicked.Text)
            {
                case "Fair":
                    btnNormalSkin.Opacity = btnDarkSkin.Opacity = .3;
                    break;
                case "Normal":
                    btnFairSkin.Opacity = btnDarkSkin.Opacity = .3;
                    break;
                case "Dark":
                    btnFairSkin.Opacity = btnNormalSkin.Opacity = .3;
                    break;
                default:
                    break;
            }
            clicked.Opacity = 1;
            User.SkinType = clicked.Text;
        }


        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            var rootPage = Navigation.NavigationStack.FirstOrDefault();
            if (rootPage == null) return;
            App.IsUserLoggedIn = true;
            Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack.First());
            await Navigation.PopToRootAsync();
        }

        private async void Location_Button_OnClicked(object obj, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                UserDialogs.Instance.ShowLoading();

                Position position = null;
                Plugin.Geolocator.Abstractions.IGeolocator locator = CrossGeolocator.Current;
                try
                {
                    Debug.WriteLine("1");
                    locator.DesiredAccuracy = 100;
                    position = await locator.GetLastKnownLocationAsync();
                    //                if (position != null)
                    //                    //got a cahched position, so let's use it.
                    //                    return;
                    if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                        return;
                    position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

                    if (position == null)
                        return;

                    Console.WriteLine("POSITION VALUES");
                    string output =
                        $"Time: {position.Timestamp} \nLat: {position.Latitude} \nLong: {position.Longitude} \nAltitude: {position.Altitude} \nAltitude Accuracy: {position.AltitudeAccuracy} \nAccuracy: {position.Accuracy} \nHeading: {position.Heading} \nSpeed: {position.Speed}";
                    Debug.WriteLine($"POSITION = {output}");

                    var reversePosition = new Position(position.Latitude, position.Longitude);
                    var possibleAddresses = await locator.GetAddressesForPositionAsync(reversePosition);
                    foreach (var addresses in possibleAddresses)
                    {
                        User.Location = addresses.AdminArea + " - " + addresses.SubAdminArea + " - " + addresses.SubLocality;
                        break;
                    }
                    btnGetMyLocation.Text = User.Location;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to get location: " + ex);
                }
                finally
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
            else
            {
                UserDialogs.Instance.Alert("There is no internet connection!", "Error", "Ok");
            }


        }


    }
}
