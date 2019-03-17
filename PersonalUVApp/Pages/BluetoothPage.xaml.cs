using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            BindingContext = this;

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
                BluetoothState state = _bluetoothLE.State;
                Console.WriteLine("state:::" + state);
                _deviceList.Clear();
                _adapter.ScanTimeout = 10000;
                _adapter.ScanMode = ScanMode.LowLatency;
                _adapter.DeviceDiscovered += _adapter_DeviceDiscovered;

                await _adapter.StartScanningForDevicesAsync(allowDuplicatesKey: false);

            }
        }

        void _adapter_DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            if (e.Device != null)
                _deviceList.Add(e.Device);
        }


        private async void DevicesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (DevicesList.SelectedItem == null)
                return;

            _device = DevicesList.SelectedItem as IDevice;

            bool result = await DisplayAlert("Notice", "Do you want to turn on Bluetooth to discover devices", "Connect", "Cancel");

            if (!result)
                return;

            try
            {
                await _adapter.StopScanningForDevicesAsync();

                Device.BeginInvokeOnMainThread(async () =>
                {
                    var parameters = new ConnectParameters(true,true);
                    await _adapter.ConnectToDeviceAsync(_device, parameters);
                });

                Console.WriteLine("sadasd");

                await DisplayAlert("Connected", "Status:" + _device.State, "OK");

            }
            catch (DeviceConnectionException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            DevicesList.SelectedItem = null;
        }
    }
}
//https://github.com/juucustodio/Bluetooth-Xamarin.Forms