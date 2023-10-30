using personalization.Services;
using personalization.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace personalization.ViewModels
{
    public class EventsMainPageViewModel : INotifyPropertyChanged
    {
        private EventService eventService;

        public ObservableCollection<Events> Events { get; private set; }

        public ICommand SaveCommand { get; set; }

        public EventsMainPageViewModel()
        {
            eventService = new EventService();
            Events = new ObservableCollection<Events>();
            LoadEventsAsync();
            SaveCommand = new Command(() =>ContinuePage());
        }

        private async void LoadEventsAsync()
        {
            var events = await eventService.GetEvents();
            foreach (var ev in events)
            {
                Events.Add(ev);

            }
            Debug.WriteLine($"Cantidad de eventos cargados: {Events.Count}");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void ContinuePage() {
            await Shell.Current.GoToAsync($"{nameof(BeaconLocationView)}");
        }
    }
}

