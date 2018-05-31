#region

using Windows.UI.Xaml;
using Touch.ViewModels;

#endregion

namespace Touch.Views.Pages
{
    internal sealed partial class ObjectsPage
    {
        private readonly ObjectsViewModel _viewModel;

        public ObjectsPage()
        {
            InitializeComponent();

            _viewModel = new ObjectsViewModel();
        }

        private async void ObjectsPage_OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadObjectsAsync();
        }
    }
}