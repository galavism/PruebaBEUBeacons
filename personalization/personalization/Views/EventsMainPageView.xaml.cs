using personalization.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace personalization.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventsMainPageView : ContentPage
    {
        public EventsMainPageView()
        {
            InitializeComponent();
            BindingContext = new EventsMainPageViewModel();
        }

    }
}