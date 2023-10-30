using personalization.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace personalization
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CJFDView), typeof(CJFDView));

            Routing.RegisterRoute(nameof(CGCView), typeof(CGCView));

            Routing.RegisterRoute(nameof(PriorityView), typeof(PriorityView));

            Routing.RegisterRoute(nameof(EventsMainPageView), typeof(EventsMainPageView));

            Routing.RegisterRoute(nameof(BeaconLocationView), typeof(BeaconLocationView));
        }


    }
}
