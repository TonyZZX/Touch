#region

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Touch.ViewModels;

#endregion

namespace Touch.Views.Pages
{
    internal sealed partial class GallerySearchPage
    {
        private GallerySearchViewModel _viewModel;

        public GallerySearchPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _viewModel = new GallerySearchViewModel(e.Parameter as string);
        }

        private async void GallerySearchPage_OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadImagesAsync();
        }
    }
}