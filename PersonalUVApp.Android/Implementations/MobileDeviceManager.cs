using System;
using Android;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Java.Lang;
using PersonalUVApp.DepInj;
using PersonalUVApp.Droid.Implementations;

[assembly: Xamarin.Forms.Dependency(typeof(MobileDeviceManager))]
namespace PersonalUVApp.Droid.Implementations
{
    public class MobileDeviceManager : IMobileDeviceManager
    {
        public void CloseApp()
        {
            JavaSystem.Exit(0);
        }

        public void EnableBluetooth()
        {
            BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (bluetoothAdapter.IsEnabled == false)
                bluetoothAdapter.Enable();
        }

        public void EnableGps()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);

            if (locationManager.IsProviderEnabled(LocationManager.GpsProvider) == false)
            {
                Intent gpsSettingIntent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                Android.App.Application.Context.StartActivity(gpsSettingIntent);
            }
            /*
            var permissionState = ContextCompat.CheckSelfPermission(Android.App.Application.Context as MainActivity, Manifest.Permission.AccessCoarseLocation);
            permissionState == (int)Permission.Granted;
            ActivityCompat.RequestPermissions(Android.App.Application.Context as MainActivity, new[] { Manifest.Permission.AccessCoarseLocation }, 34);
            */
        }


    }
    //https://stackoverflow.com/questions/43752085/prompt-user-to-enable-bluetooth-in-xamarin-forms
}
