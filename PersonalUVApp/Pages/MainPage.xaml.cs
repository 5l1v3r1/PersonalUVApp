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

        public MainPage()
        {
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

                    //UserDialogs.Instance.ShowLoading();

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
                                                        }
                                                        lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
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
                        UserDialogs.Instance.HideLoading();
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
