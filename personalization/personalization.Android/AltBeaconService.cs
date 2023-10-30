using AltBeaconOrg.BoundBeacon;
using Android.App;
using Android.Bluetooth;
using personalization.Droid;
using personalization.InterfaceBeacons;
using personalization.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(AltBeaconService))]
namespace personalization.Droid
{
    public class AltBeaconService : Java.Lang.Object, IbeaconAndroid
    {
        private readonly MonitorNotifier _monitorNotifier;
        private readonly RangeNotifier _rangeNotifier;
        private BeaconManager _beaconManager;

        Region _tagRegion;
        Region _emptyRegion;


        List<BeaconDetection> _BeaconDetection = new List<BeaconDetection>();

        object _lock = new object();


        public AltBeaconService()
        {
            _monitorNotifier = new MonitorNotifier();
            _rangeNotifier = new RangeNotifier();
        }

        public BeaconManager BeaconManagerImpl
        {
            get
            {
                if (_beaconManager == null)
                    _beaconManager = InitializeBeaconManager();
                return _beaconManager;
            }
        }
        public void StartMonitoring()
        {
            BeaconManagerImpl.ForegroundBetweenScanPeriod = 5000;
            BeaconManagerImpl.BackgroundBetweenScanPeriod = 5000;

            BeaconManagerImpl.AddMonitorNotifier(_monitorNotifier);
            BeaconManagerImpl.StartMonitoringBeaconsInRegion(_tagRegion);
            BeaconManagerImpl.StartMonitoringBeaconsInRegion(_emptyRegion);
        }


        public void InitializeService()
        {
            if (_beaconManager == null)
                _beaconManager = InitializeBeaconManager();
        }


        private BeaconManager InitializeBeaconManager()
        {
            // Enable the BeaconManager 
            BeaconManager bm = BeaconManager.GetInstanceForApplication(Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity);

            var iBeaconParser = new BeaconParser();
            //	Estimote > 2013
            iBeaconParser.SetBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24");
            bm.BeaconParsers.Add(iBeaconParser);

            _monitorNotifier.EnterRegionComplete += EnteredRegion;
            _monitorNotifier.ExitRegionComplete += ExitedRegion;
            _monitorNotifier.DetermineStateForRegionComplete += DeterminedStateForRegionComplete;
            _rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;

            _tagRegion = new Region("Beacon 2", Identifier.Parse("8E6DBFBB-489D-418A-9560-1BA1CE6301AA"), null, null);
            _tagRegion = new Region("myUniqueBeaconId", Identifier.Parse("B9407F30-F5F8-466E-AFF9-25556B57FE6D"), null, null);
            _emptyRegion = new Region("myEmptyBeaconId", null, null, null);

            //bm.SetBackgroundMode(false);

            bm.BackgroundMode = false;
            bm.Bind((IBeaconConsumer)Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity);

            return bm;
        }

        public void StartRanging()
        {
            BeaconManagerImpl.ForegroundBetweenScanPeriod = 100;
            BeaconManagerImpl.BackgroundScanPeriod = 500;
            BeaconManagerImpl.BackgroundBetweenScanPeriod = 30000;
            BeaconManagerImpl.ForegroundScanPeriod = 200;

            BeaconManagerImpl.AddRangeNotifier(_rangeNotifier);
            try
            {
                BeaconManagerImpl.StartRangingBeaconsInRegion(_tagRegion);
                BeaconManagerImpl.StartRangingBeaconsInRegion(_emptyRegion);
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine("StartRangingException: " + ex.Message);
            }

        }


        public void StopRanging()
        {
            if (_beaconManager != null)
            {
                try
                {
                    BeaconManagerImpl.StopRangingBeaconsInRegion(_tagRegion);
                    BeaconManagerImpl.StopRangingBeaconsInRegion(_emptyRegion);
                    BeaconManagerImpl.RemoveRangeNotifier(_rangeNotifier);
                }
                catch (Exception ex)
                {

                    System.Diagnostics.Debug.WriteLine("StopRangingException: " + ex.Message);
                }
            }
        }


        private void EnteredRegion(object sender, MonitorEventArgs e)
        {
            string region = "???";
            if (e.Region != null)
            {
                if (e.Region.Id1 == null)
                    region = "null region";
                else
                    region = e.Region.Id1.ToString().ToUpper();
            }

            Xamarin.Forms.MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "EnteredRegion", region);

            Console.WriteLine("EnteredRegion");
        }

        private void ExitedRegion(object sender, MonitorEventArgs e)
        {
            string region = "???";
            if (e.Region != null)
            {
                if (e.Region.Id1 == null)
                    region = "null region";
                else
                    region = e.Region.Id1.ToString().ToUpper();
            }

            Xamarin.Forms.MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "ExitedRegion", region);
            Console.WriteLine("ExitedRegion");
        }

        private void DeterminedStateForRegionComplete(object sender, MonitorEventArgs e)
        {
            Console.WriteLine("DeterminedStateForRegionComplete");
        }

        void RangingBeaconsInRegion(object sender, RangeEventArgs e)
        {

            _BeaconDetection = new List<BeaconDetection>();

            lock (_lock)
            {

                // Get all beacons and create the BeaconDetection
                foreach (Beacon beacon in e.Beacons)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("NAME {0} - IP {1} - {2}dB", beacon.BluetoothName, beacon.BluetoothAddress, beacon.Rssi));
                    _BeaconDetection.Add(new BeaconDetection(beacon.BluetoothName, beacon.BluetoothAddress, beacon.Id1.ToString(), beacon.Id2.ToString(), beacon.Id3.ToString(), beacon.Distance, beacon.Rssi));
                };


                Task.Run(() =>
                {
                    // I send beacons to XF project
                    if (_BeaconDetection.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("I SEND TO XF " + _BeaconDetection.Count + " BEACONS");
                        Xamarin.Forms.MessagingCenter.Send<App, List<BeaconDetection>>((App)Xamarin.Forms.Application.Current, "BeaconsReceived", _BeaconDetection);
                    }
                });

            }

        }

        public void BluetoothEnable()
        {
            BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (!bluetoothAdapter.IsEnabled)
                bluetoothAdapter.Enable();

        }
        public void SetBackgroundMode(bool isBackground)
        {
            if (_beaconManager != null)
                BeaconManagerImpl.BackgroundMode = isBackground;

        }

        public void OnDestroy()
        {

            if (_beaconManager != null && BeaconManagerImpl.IsBound((IBeaconConsumer)Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity))
                BeaconManagerImpl.Unbind((IBeaconConsumer)Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity);

        }

    }
}