using System;
using CoreBluetooth;
using Foundation;
using PersonalUVApp.DepInj;
using PersonalUVApp.iOS.Implementations;
using UIKit;

//[assembly: Xamarin.Forms.Dependency(typeof(MobileDeviceManager))]
namespace PersonalUVApp.iOS.Implementations
{
    public class MobileDeviceManager// : IMobileDeviceManager
    {


        public bool EnableBluetooth()
        {



            NSUrl bluetoothUrl = NSUrl.FromString("App-Prefs:root=Bluetooth");
            NSUrl locationUrl = NSUrl.FromString("App-Prefs:root=LOCATION");

            if (CoreLocation.CLLocationManager.LocationServicesEnabled == false)
            {

            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UIApplication.SharedApplication.OpenUrl(bluetoothUrl);
                UIApplication.SharedApplication.OpenUrl(locationUrl);
            }
            else
            {
                UIApplicationOpenUrlOptions options = new UIApplicationOpenUrlOptions();
                options.OpenInPlace = true;
                UIApplication.SharedApplication.OpenUrl(bluetoothUrl, options, null);
                UIApplication.SharedApplication.OpenUrl(locationUrl, options, null);
            }


            return true;
            /*
            var bluetoothManager = new CoreBluetooth.CBCentralManager();
            // Does not go directly to bluetooth on every OS version though, but opens the Settings on most 
            UIApplication.SharedApplication.OpenUrl(new NSUrl("App-Prefs:root=Bluetooth"));
            if (CoreLocation.CLLocationManager.LocationServicesEnabled == false)
                UIApplication.SharedApplication.OpenUrl(new NSUrl("App-Prefs:root=LOCATION"));
                */               

        }

        public bool EnableGps()
        {
            throw new NotImplementedException();
        }

        public bool IsBluetoothEnabled()
        {
            return true;
            //CBManagerState.PoweredOff
            //var bluetoothManager = new CBCentralManager();
            //if (bluetoothManager.State == CBCentralManagerState.PoweredOff)
            //{
            //    // Does not go directly to bluetooth on every OS version though, but opens the Settings on most
            //    UIApplication.SharedApplication.OpenUrl(new NSUrl("App-Prefs:root=Bluetooth"));
            //}
        }
    }
}
