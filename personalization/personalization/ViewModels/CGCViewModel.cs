using personalization.Models;
using personalization.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace personalization.ViewModels
{
    public class CGCViewModel: BindableObject
    {
        public List<Preference> AvailableServices { get; set; }
        public List<string> TopicsList { get; set; }
        public ObservableCollection<string> SelectedTopics { get; set; }
        public ICommand SaveCommand { get; set; }

        public CGCViewModel() {
           
           AvailableServices = new List<Preference>
            {
                new Preference { Id_preference = 21, Topic = "Salsa", Center_id = "Cultural" },
                new Preference { Id_preference = 22, Topic = "Tango", Center_id = "Cultural" },
                new Preference { Id_preference = 23, Topic = "Escritura", Center_id = "Cultural" },
                new Preference { Id_preference = 24, Topic = "Coro", Center_id = "Cultural" },
                new Preference { Id_preference = 25, Topic = "Javeriana Canta", Center_id = "Cultural" }
            };
            TopicsList = AvailableServices.Select(preference => preference.Topic).ToList();
            Console.WriteLine("Contenido lista TopicsList :");
            foreach (var topic in TopicsList)
            {
                Console.WriteLine(topic);
            }
            SelectedTopics = new ObservableCollection<string>();
            SaveCommand = new Command(() => SavePreferences());
        }
        private async void SavePreferences()
        {
            var selectedTopics = SelectedTopics;
            // Llamaría al método del back
            if (selectedTopics != null && selectedTopics.Count > 0)
            {
                List<int> selectedIds = new List<int>();

                foreach (var selectedTopic in selectedTopics)
                {
                    var preference = AvailableServices.FirstOrDefault(p => p.Topic == selectedTopic);
                    if (preference != null)
                    {
                        selectedIds.Add(preference.Id_preference);
                    }
                }

                foreach (var id in selectedIds)
                {
                    Console.WriteLine($"ID seleccionado: {id}");
                }
            }
            else
            {
                // No se seleccionaron elementos
                Console.WriteLine("Ningún elemento seleccionado.");
            }
            await Shell.Current.GoToAsync($"{nameof(EventsMainPageView)}");
        }
    }
}
