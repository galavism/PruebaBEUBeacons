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
    public partial class CGCView : ContentPage
    {
        public CGCView()
        {
            InitializeComponent();
            var viewModel = new CGCViewModel();
            BindingContext = viewModel;

            collectionView.SelectionChanged += (sender, e) =>
            {
                var selectedItems = ((CollectionView)sender).SelectedItems;
                viewModel.SelectedTopics.Clear(); // Limpiar la colección actual

                foreach (var selectedItem in selectedItems)
                {
                    viewModel.SelectedTopics.Add((string)selectedItem); // Agregar elementos seleccionados a la colección
                }
            };
        }
    }
}