using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Charger
{
    class BCaster : BroadcastReceiver
    {
        MainActivity activity;
        public List<string> dNames;
        public List<string> dAddress;
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action== BluetoothDevice.ActionFound )
            {

                BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                dNames.Add(device.Name);
                dAddress.Add(device.Address);
            }
            if (intent.Action == BluetoothAdapter.ActionStateChanged)
            {
                if (!activity.BlueAdapter.IsEnabled)
                {
                    activity.BlueConection();
                }
                if (activity.BlueAdapter.IsEnabled)
                {
                }
            }
        }
        public string Find(string nameDevice)
        {
            string addres = "";
            for (int i = 0; i < dNames.Count; i++)
            {
                if (dNames[i]==nameDevice)
                {
                    addres = dAddress[i];
                }
            }
            return addres;
        }
        public BCaster(MainActivity main)
        {
            activity = main;
            dNames = new List<string>();
            dAddress = new List<string>();
        }
    }
}