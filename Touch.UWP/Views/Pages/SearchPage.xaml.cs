#region

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.ViewModels;

#endregion

namespace Touch.Views.Pages
{
    public sealed partial class SearchPage
    {
        private SearchViewModel _viewModel;

        public SearchPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var queryStr = e.Parameter as string;
            _viewModel = new SearchViewModel(queryStr);

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                TitleText.Text = "Results for \"" + queryStr + "\"";
                _viewModel.LoadImageGroups();
                NoPhotosGrid.Visibility = _viewModel.Images.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            });
        }
    }
}