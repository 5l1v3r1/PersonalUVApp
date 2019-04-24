using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using PersonalUVApp.DepInj;
using Plugin.BluetoothLE;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PersonalUVApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothPage
    {
        public Command SearchDevicesCommand { protected set; get; }
        public Command ReadFileCommand { protected set; get; }




        public BluetoothPage()
        {
            InitializeComponent();
            SearchDevicesCommand = new Command(async () => await SearchDevicesAsync());
            ReadFileCommand = new Command(() => ReadFileAsync());

            BindingContext = this;
        }

        private void ReadFileAsync()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(BluetoothPage)).Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        static string BytesToStringConverted(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }


        private int readerId = 1;

        private async Task SearchDevicesAsync()
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
                    DependencyService.Get<IMobileDeviceManager>().EnableGps();
                    DependencyService.Get<IMobileDeviceManager>().EnableBluetooth();

                    if (CrossBleAdapter.Current.Status == AdapterStatus.PoweredOn)
                    {
                        Debug.WriteLine("Girdi.");

                        UserDialogs.Instance.ShowLoading();
                        try
                        {
                            CrossBleAdapter.Current.Scan(new ScanConfig {ScanType = BleScanType.Balanced }).Subscribe(scanResult =>
                            {
                                Debug.WriteLine(scanResult.Device.NativeDevice + scanResult.Device.Name + " UUID: " + scanResult.Device.Uuid);
                                if (scanResult.Device.Name == "HMSoft")
                                {
                                    CrossBleAdapter.Current.StopScan();
                                    UserDialogs.Instance.HideLoading();

                                    IDevice device = scanResult.Device;

                                    device.Connect();

                                    scanResult.Device.WhenAnyCharacteristicDiscovered().Subscribe(chs => {
                                       if (chs.Uuid == new Guid("0000FFE1-0000-1000-8000-00805F9B34FB"))
                                        {
                                            chs.EnableNotifications().Subscribe(
                                                result =>
                                                {
                                                    Debug.WriteLine(result.Data);
                                                    chs.WhenNotificationReceived().Subscribe(res => {
                                                        string ret = System.Text.Encoding.UTF8.GetString(res.Data);
                                                        Device.BeginInvokeOnMainThread(() =>
                                                        {
                                                            //lblUvIndexVal.Text = ret;
                                                        });
                                                        Debug.WriteLine(ret);
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
                        }
                        finally
                        {
                            //CrossBleAdapter.Current.StopScan();
                            UserDialogs.Instance.HideLoading();
                        }
                    }
                    else if (status != PermissionStatus.Unknown)
                    {
                        await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                    }
                }

                /*
               Random rnd = new Random();
               string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "values.txt");

               for (int i = 0; i < 100; i++)
               {
                   await Task.Delay(10);

                   string values = $"Reader-ID: {readerId}, Time: {DateTime.Now}, Value: { rnd.Next(8,12) } \n";

                   File.AppendAllText(fileName, values);
               }

               Console.WriteLine(File.ReadAllText(fileName));
               */

              
            }

        }
    }
}






//https://github.com/juucustodio/Bluetooth-Xamarin.Forms




//    IAdapter _adapter;
//    IBluetoothLE _bluetoothLE;
//    ObservableCollection<IDevice> _deviceList;
//    IDevice _device;



//    public BluetoothPage()
//    {
//        InitializeComponent();
//        BindingContext = this;

//        _bluetoothLE = CrossBluetoothLE.Current;
//        _adapter = CrossBluetoothLE.Current.Adapter;
//        _deviceList = new ObservableCollection<IDevice>();
//        DevicesList.ItemsSource = _deviceList;
//    }

//    private async void SearchDeviceBtnClicked(object sender, EventArgs e)
//    {
//        if (_bluetoothLE.State == BluetoothState.Off)
//        {
//            await DisplayAlert("Attention", "Bluetooth disabled.", "OK");
//        }
//        else
//        {
//            BluetoothState state = _bluetoothLE.State;
//            Console.WriteLine("state:::" + state);
//            _deviceList.Clear();
//            _adapter.ScanTimeout = 10000;
//            _adapter.ScanMode = ScanMode.LowLatency;
//            _adapter.DeviceDiscovered += _adapter_DeviceDiscovered;

//            await _adapter.StartScanningForDevicesAsync(allowDuplicatesKey: false);

//        }
//    }

//    void _adapter_DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
//    {
//        if (e.Device != null)
//            _deviceList.Add(e.Device);
//    }


//    private async void DevicesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
//    {

//        if (DevicesList.SelectedItem == null)
//            return;

//        _device = DevicesList.SelectedItem as IDevice;

//        bool result = await DisplayAlert("Notice", "Do you want to turn on Bluetooth to discover devices", "Connect", "Cancel");

//        if (!result)
//            return;

//        try
//        {
//            await _adapter.StopScanningForDevicesAsync();

//            Device.BeginInvokeOnMainThread(async () =>
//            {
//                var parameters = new ConnectParameters(true,true);
//                await _adapter.ConnectToDeviceAsync(_device, parameters);
//            });

//            Console.WriteLine("sadasd");

//            await DisplayAlert("Connected", "Status:" + _device.State, "OK");

//        }
//        catch (DeviceConnectionException ex)
//        {
//            await DisplayAlert("Error", ex.Message, "OK");
//        }
//        DevicesList.SelectedItem = null;
//    }