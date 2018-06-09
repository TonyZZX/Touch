#region

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.ViewModels;

#endregion

namespace Touch.Views.Pages
{
    public sealed partial class TagsPage
    {
        private static int _persistedItemIndex = -1;

        private static TagsViewModel _viewModel = new TagsViewModel();


        public TagsPage()
        {
            InitializeComponent();

            CoverGridView.ItemsSource = _viewModel.Covers;
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private int PersistedItemIndex
        {
            get => _persistedItemIndex;
            set => _persistedItemIndex = value;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // If it does not come back from detail page, refresh it
            if (e.NavigationMode != NavigationMode.Back)
            {
                PersistedItemIndex = -1;
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    _viewModel = new TagsViewModel();
                    CoverGridView.ItemsSource = _viewModel.Covers;
                    _viewModel.LoadCovers();
                });
            }

            NoTagsGrid.Visibility = _viewModel.Covers.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CoverGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (CoverGridView.Items != null) PersistedItemIndex = CoverGridView.Items.IndexOf(e.ClickedItem);
            Frame.Navigate(typeof(TagDetailPage), e.ClickedItem);
        }
    }
}