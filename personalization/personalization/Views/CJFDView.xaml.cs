using personalization.ViewModels;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace personalization.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CJFDView : ContentPage
    {
        public CJFDView()
        {
            InitializeComponent();

            var viewModel=new CJFDViewModel();
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