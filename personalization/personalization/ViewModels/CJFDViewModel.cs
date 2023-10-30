using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using personalization.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using personalization.Views;

namespace personalization.ViewModels
{
   
    public class CJFDViewModel : BindableObject
    {
        public List<Preference> AvailableServices { get; set; }
        public List<string> TopicsList { get; set; }
        public ObservableCollection<string> SelectedTopics { get; set; }
        public ICommand SaveCommand { get; set; }
        public CJFDViewModel()
        {
            AvailableServices = new List<Preference>
            {
                new Preference { Id_preference = 1, Topic = "Futbol", Center_id = "Deportivo" },
                new Preference { Id_preference = 2, Topic = "Rugby", Center_id = "Deportivo" },
                new Preference { Id_preference = 3, Topic = "Tenis", Center_id = "Deportivo" },
                new Preference { Id_preference = 4, Topic = "Voleibol", Center_id = "Deportivo" },
                new Preference { Id_preference = 5, Topic = "Ultimate", Center_id = "Deportivo" },
                new Preference { Id_preference = 6, Topic = "Baloncesto", Center_id = "Deportivo" },
                new Preference { Id_preference = 7, Topic = "Tenis Mesa", Center_id = "Deportivo" },
                new Preference { Id_preference = 8, Topic = "Cardio", Center_id = "Deportivo" },
                new Preference { Id_preference = 9, Topic = "Rumba", Center_id = "Deportivo" },
                new Preference { Id_preference = 10, Topic = "ASCUN", Center_id = "Deportivo" },
                new Preference { Id_preference = 11, Topic = "COPA PUJ", Center_id = "Deportivo" },
                new Preference { Id_preference = 12, Topic = "Cheers", Center_id = "Deportivo" },
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

            await Shell.Current.GoToAsync($"{nameof(CGCView)}");
        }

    }
}
