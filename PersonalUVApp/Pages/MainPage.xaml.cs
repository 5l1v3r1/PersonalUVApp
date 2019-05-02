using System;
using Newtonsoft.Json;
using PersonalUVApp.Helper;
using PersonalUVApp.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using Acr.UserDialogs;
using PersonalUVApp.DepInj;
using Plugin.BluetoothLE;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Diagnostics;
using Plugin.Geolocator;
using System.Reactive.Linq;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Globalization;
using Plugin.Geolocator.Abstractions;
using Plugin.Connectivity;

namespace PersonalUVApp.Pages
{
    public partial class MainPage 
    {
        public UserModel User
        {
            get => (UserModel)GetValue(UserProperty);
            set => SetValue(UserProperty, value);
        }

        public static readonly BindableProperty UserProperty = BindableProperty.Create(nameof(User), typeof(UserModel), typeof(MainPage));

        const int parallaxSpeed = 4;

        double lastScroll;

        Dictionary<int, string> info = new Dictionary<int, string>();
        Dictionary<int, string> recommendedProtection = new Dictionary<int, string>();


        public MainPage()
        {
            info.Add(3, "A UV Index reading of 0 to 2 means low danger from the sun\\'s UV rays for the average person.");
            info.Add(6, "A UV Index reading of 3 to 5 means moderate risk of harm from unprotected sun exposure.");
            info.Add(8, "A UV Index reading of 6 to 7 means high risk of harm from unprotected sun exposure. Protection against skin and eye damage is needed.");
            info.Add(11, "A UV Index reading of 8 to 10 means very high risk of harm from unprotected sun exposure. Take extra precautions because unprotected skin and eyes will be damaged and can burn quickly.");
            info.Add(20, "A UV Index reading of 11 or more means extreme risk of harm from unprotected sun exposure. Take all precautions because unprotected skin and eyes can burn in minutes.");

            recommendedProtection.Add(3, "Wear sunglasses on bright days. If you burn easily, cover up and use broad spectrum SPF 30+ sunscreen. Bright surfaces, such as sand, water and snow, will increase UV exposure.");
            recommendedProtection.Add(6, "Stay in shade near midday when the sun is strongest. If outdoors, wear sun protective clothing, a wide-brimmed hat, and UV-blocking sunglasses. Generously apply broad spectrum SPF 30+ sunscreen every 2 hours, even on cloudy days, and after swimming or sweating. Bright surfaces, such as sand, water and snow, will increase UV exposure.");
            recommendedProtection.Add(8, "Reduce time in the sun between 10 a.m. and 4 p.m. If outdoors, seek shade and wear sun protective clothing, a wide-brimmed hat, and UV-blocking sunglasses. Generously apply broad spectrum SPF 30+ sunscreen every 2 hours, even on cloudy days, and after swimming or sweating. Bright surfaces, such sand, water and snow, will increase UV exposure.");
            recommendedProtection.Add(11, "Minimize sun exposure between 10 a.m. and 4 p.m. If outdoors, seek shade and wear sun protective clothing, a wide-brimmed hat, and UV-blocking sunglasses. Generously apply broad spectrum SPF 30+ sunscreen every 2 hours, even on cloudy days, and after swimming or sweating. Bright surfaces, such as sand, water and snow, will increase UV exposure.");
            recommendedProtection.Add(20, " Try to avoid sun exposure between 10 a.m. and 4 p.m. If outdoors, seek shade and wear sun protective clothing, a wide-brimmed hat, and UV-blocking sunglasses. Generously apply broad spectrum SPF 30+ sunscreen every 2 hours, even on cloudy days, and after swimming or sweating. Bright surfaces, such as sand, water and snow, will increase UV exposure.");

            User = JsonConvert.DeserializeObject<UserModel>(Settings.UserJson);
            InitializeComponent();

            ParallaxScroll.ParallaxView = headerView;
            ParallaxScroll.Scrolled += OnParallaxScrollScrolled;

            BindingContext = this;

            EstablishConnectionReadData();

        }

        private async void EstablishConnectionReadData()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                    await DisplayAlert("Need location", "In order to use this functionality, you must enable your gps", "OK");

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Location))
                    status = results[Permission.Location];

                if (status == PermissionStatus.Granted)
                {
                    if (CrossGeolocator.Current.IsGeolocationAvailable == false)
                    {
                        DependencyService.Get<IMobileDeviceManager>().EnableGps();
                    }

                    DependencyService.Get<IMobileDeviceManager>().EnableBluetooth();


                    try
                    {

                        CrossBleAdapter.Current.Scan().Subscribe(scanResult =>
                        {
                            Debug.WriteLine(scanResult.Device.NativeDevice + scanResult.Device.Name + " UUID: " + scanResult.Device.Uuid);
                            if (scanResult.Device.Name == "HMSoft")
                            {
                                CrossBleAdapter.Current.StopScan();

                                IDevice device = scanResult.Device;

                                device.Connect();
                                UserDialogs.Instance.HideLoading();

                                scanResult.Device.WhenAnyCharacteristicDiscovered().Subscribe(chs =>
                                {
                                    if (chs.Uuid == new Guid("0000FFE1-0000-1000-8000-00805F9B34FB"))
                                    {
                                        chs.EnableNotifications().Subscribe(
                                            result =>
                                            {
                                                Debug.WriteLine(result.Data);
                                                chs.WhenNotificationReceived().Subscribe(res =>
                                                {
                                                    string ret = System.Text.Encoding.UTF8.GetString(res.Data);
                                                    Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        var arr = ret.Split(',');
                                                        if (arr.Length == 3)
                                                        {
                                                            lblUvIndex.Text = arr[0];
                                                            lblTemp.Text = arr[1];
                                                            lblHum.Text = arr[2].Substring(0, 5);
                                                            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");

                                                            MapIndexValueToTexts(arr[0]);
                                                        }
                                                    });
                                                });
                                            }
                                        );
                                    }
                                });
                            }
                        });
                        }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.Alert(ex.Message);

                        UserDialogs.Instance.Confirm(new ConfirmConfig
                        {
                            Title = "Bluetooth LE Scan Failure!",
                            CancelText = "No, Close App!",
                            OkText = "Try Again!",
                            OnAction = (obj) =>
                            {
                                if (obj == true)
                                {
                                    EstablishConnectionReadData();
                                }
                                else
                                {
                                    DependencyService.Get<IMobileDeviceManager>().CloseApp();
                                }
                            }
                        });

                    }
                    finally
                    {
                   
                        //CrossBleAdapter.Current.StopScan();
                        //UserDialogs.Instance.HideLoading();
                    }

                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
        }

        private void MapIndexValueToTexts(string uvIndexString)
        {
            try
            {
                double uvIndex = double.Parse(uvIndexString, new NumberFormatInfo() { NumberDecimalSeparator = "." });

                if (uvIndex <= 3)
                {
                    lblUvIndexMeans.Text = info[3];
                    lblRecommendProtection.Text = recommendedProtection[3];
                }
                else if (uvIndex <= 6)
                {
                    lblUvIndexMeans.Text = info[6];
                    lblRecommendProtection.Text = recommendedProtection[6];
                }
                else if (uvIndex <= 8)
                {
                    lblUvIndexMeans.Text = info[8];
                    lblRecommendProtection.Text = recommendedProtection[8];
                }
                else if (uvIndex <= 11)
                {
                    lblUvIndexMeans.Text = info[11];
                    lblRecommendProtection.Text = recommendedProtection[11];
                }
                else
                {
                    lblUvIndexMeans.Text = info[20];
                    lblRecommendProtection.Text = recommendedProtection[20];
                }

                aiUvIndexMeans.IsVisible = false;
                aiRecommendProtection.IsVisible = false;
                lblCautionaryNotes.IsVisible = false;

                lblUvIndexMeans.IsVisible = true;
                lblRecommendProtection.IsVisible = true;
                lblCautionaryNotes.IsVisible = true;

            }
            catch (FormatException e)
            {
                lblUvIndexMeans.Text = info[6];
                lblRecommendProtection.Text = recommendedProtection[6];
                UserDialogs.Instance.Alert(e.Message);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ParallaxScroll.Scrolled -= OnParallaxScrollScrolled;
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            Settings.IsRememberMe = false;
            User = null;
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }

        private async void OnShareClicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = $"UV Index: {lblUvIndex.Text}, Temperature: {lblTemp.Text}, Humidity: {lblHum.Text} ",
                Title = "Share Sensor Values"
            });
        }

        async void ChangeLocationAsync(object sender, System.EventArgs e)
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

        void OnParallaxScrollScrolled(object sender, ScrolledEventArgs e)
        {
            double translation = 0;

            if (lastScroll == 0)
            {
                lastScroll = e.ScrollY;
            }

            if (lastScroll < e.ScrollY)
            {
                translation = 0 - ((e.ScrollY / parallaxSpeed));

                if (translation > 0) translation = 0;
            }
            else
            {
                translation = 0 + ((e.ScrollY / parallaxSpeed));

                if (translation > 0) translation = 0;
            }

            //SubHeaderView.FadeTo(translation < -40 ? 0 : 1);

            lastScroll = e.ScrollY;
        }

    }
}
