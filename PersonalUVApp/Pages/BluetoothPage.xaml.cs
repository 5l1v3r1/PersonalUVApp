using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PersonalUVApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothPage
    {
        IAdapter _adapter;
        IBluetoothLE _bluetoothLE;
        ObservableCollection<IDevice> _deviceList;
        IDevice _device;

        public BluetoothPage()
        {
            InitializeComponent();
            _bluetoothLE = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;
            _deviceList = new ObservableCollection<IDevice>();
            DevicesList.ItemsSource = _deviceList;
        }

        private async void SearchDeviceBtnClicked(object sender, EventArgs e)
        {

            if (_bluetoothLE.State == BluetoothState.Off)
            {
                await DisplayAlert("Attention", "Bluetooth disabled.", "OK");
            }
            else
            {
                _deviceList.Clear();
                _adapter.ScanTimeout = 10000;
                _adapter.ScanMode = ScanMode.Balanced;
                _adapter.DeviceDiscovered += (obj, a) =>
                {
                    if (!_deviceList.Contains(a.Device))
                        _deviceList.Add(a.Device);
                };
                if (!_adapter.IsScanning)
                    await _adapter.StartScanningForDevicesAsync();
            }
        }

        private async void DevicesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _device = DevicesList.SelectedItem as IDevice;

            bool result = await DisplayAlert("Notice", "Do you want to turn on Bluetooth to discover devices",
                "Connect", "Cancel");

            if (!result)
                return;
            //Stop Scanner
            await _adapter.StopScanningForDevicesAsync();

            try
            {
                Device.BeginInvokeOnMainThread(new Action(async () =>
                {
                    ConnectParameters parameters = new ConnectParameters(forceBleTransport: true);
                    await _adapter.ConnectToDeviceAsync(_device, parameters);
                }));
                await DisplayAlert("Connected", "Status:" + _device.State, "OK");

            }
            catch (DeviceConnectionException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        protected override bool OnBackButtonPressed()
        {
            // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
            Device.BeginInvokeOnMainThread(async () =>
            {
                base.OnBackButtonPressed();
                await Navigation.PopAsync();
                
            });
            return true;
        }

    }
}
//https://github.com/juucustodio/Bluetooth-Xamarin.Forms