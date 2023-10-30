using personalization.Models;
using personalization.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using personalization.InterfaceBeacons;

namespace personalization.ViewModels
{
    public class BeaconLocationViewModel : INotifyPropertyChanged
    {
        public bool IsStartedRanging { get; set; }

        public bool IsTransmitting { get; set; }

        public ObservableCollection<BeaconDetection> ReceivedBeacons { get; set; } = new ObservableCollection<BeaconDetection>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand OnAppearingCommand { get; protected set; }
        public ICommand OnDisappearingCommand { get; protected set; }

        public ICommand StartRangingCommand { get; protected set; }

        public BeaconLocationViewModel()
        {
            this.StartRangingCommand = new Command(() => {
                startRangingBeacon();
            });

            this.OnAppearingCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

                    if (status != PermissionStatus.Granted)
                        status = await Util.Permissions.CheckPermissions(Permission.Location);
                });

                MessagingCenter.Subscribe<App>(this, "CleanBeacons", (sender) =>
                {
                    List<BeaconDetection> receivedBeacons = new List<BeaconDetection>(ReceivedBeacons);

                    UpdateBeaconCurrentDateTime(receivedBeacons, DateTime.Now);
                    DeleteOldBeacons(receivedBeacons);

                    ReceivedBeacons = ConvertToObservableCollection(receivedBeacons);
                });

                MessagingCenter.Subscribe<App, List<BeaconDetection>>(this, "BeaconsReceived", (sender, arg) =>
                {
                    if (arg != null && arg is List<BeaconDetection>)
                    {
                        System.Diagnostics.Debug.WriteLine("Received: " + ((List<BeaconDetection>)arg).Count);
                        List<BeaconDetection> temp = arg;
                        List<BeaconDetection> receivedBeacons = new List<BeaconDetection>(ReceivedBeacons);

                        if (arg != null && arg.Count > 0)
                        {

                            DateTime now = DateTime.Now;

                            UpdateBeaconCurrentDateTime(receivedBeacons, now);

                            foreach (BeaconDetection sharedBeacon in arg)
                            {

                                // Is the beacon already in list?
                                var ret = receivedBeacons.Where(o => o.BluetoothAddress == sharedBeacon.BluetoothAddress).FirstOrDefault();
                                if (ret != null) // Is present
                                {
                                    var index = receivedBeacons.IndexOf(ret);
                                    receivedBeacons[index].Update(now, sharedBeacon.Distance, sharedBeacon.Rssi); // Update last received date time
                                }
                                else
                                {
                                    receivedBeacons.Insert(0, sharedBeacon);
                                }
                            }

                            DeleteOldBeacons(receivedBeacons);

                            ReceivedBeacons = ConvertToObservableCollection(receivedBeacons);
                        }
                    }
                });

            });

            this.OnDisappearingCommand = new Command(() =>
            {
                MessagingCenter.Unsubscribe<App, List<BeaconDetection>>(this, "BeaconsReceived");
                MessagingCenter.Unsubscribe<App>(this, "CleanBeacons");
            });
        }

        private void startRangingBeacon()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                var beaconService = Xamarin.Forms.DependencyService.Get<IbeaconAndroid>();
                beaconService.BluetoothEnable();

                if (!IsStartedRanging)
                    beaconService.StartRanging();
                else
                    beaconService.StopRanging();

                IsStartedRanging = !IsStartedRanging;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                if (!IsStartedRanging)
                    Xamarin.Forms.DependencyService.Get<iOSScan>().startranging();
                else
                    Xamarin.Forms.DependencyService.Get<iOSScan>().stopranging();

                IsStartedRanging = !IsStartedRanging;
            }
        }
        private ObservableCollection<BeaconDetection> ConvertToObservableCollection(List<BeaconDetection> receivedBeacons)
        {
            if (receivedBeacons != null)
                return new ObservableCollection<BeaconDetection>(receivedBeacons.OrderByDescending(o => o.Rssi));

            return null;
        }

        private void UpdateBeaconCurrentDateTime(List<BeaconDetection> receivedBeacons, DateTime now)
        {
            foreach (BeaconDetection shared in receivedBeacons)
                shared.CurrentDateTime = now;
        }

        private void DeleteOldBeacons(List<BeaconDetection> receivedBeacons)
        {
            if (receivedBeacons != null)
            {
                int count = receivedBeacons.Count;

                for (int ii = count - 1; ii >= 0; ii--)
                {
                    try
                    {
                        if (receivedBeacons[ii].ForceDelete)
                            receivedBeacons.RemoveAt(ii);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }
        }


        public DataTemplate ViewCellBeaconTemplate
        {
            get
            {
                return new DataTemplate(typeof(BeaconLocationView.ViewCellBeacon));
            }
        }

    }
}
