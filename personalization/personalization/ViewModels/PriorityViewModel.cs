using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace personalization.ViewModels
{

    public class PriorityViewModel:BindableObject
    {
        private ObservableCollection<int> AvailableNumbers = new ObservableCollection<int> { 1, 2, 3, 4 };
        private int Location;
        private int Program;
        private int Schedule;
        private int Preferences;
        public  PriorityViewModel() {
        }
    }
}
