#region

using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Touch.Services;
using Touch.ViewModels;

#endregion

namespace Touch.Views.Pages
{
    internal sealed partial class GalleryPage : IPageWithViewModel<GalleryViewModel>
    {
        public GalleryPage()
        {
            InitializeComponent();
        }

        public GalleryViewModel ViewModel { get; set; }

        /// <summary>
        ///     For async loading, update bindings.
        /// </summary>
        public void UpdateBindings()
        {
            // Must have this! Otherwise, image list show as empty.
            Bindings?.Update();
        }

        private async void GalleryPage_OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadImagesAsync();
        }

        private void AutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // We only want to get results when it was a user typing,
            // otherwise we assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput) return;
            var suggestions = ViewModel.GetSuggestions(sender.Text).ToArray();
            sender.ItemsSource = suggestions.Any() ? suggestions : new[] {"No results"};
        }

        private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender,
            AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // BUG: Don't autocomplete the TextBox when we are showing "no results"
            if (args.SelectedItem is string query && query != "No results" && ViewModel.IsInSuggestions(query))
                sender.Text = query;
        }

        private void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var query = sender.Text;
            if (query != "" && query != "No results") ViewModel.NavigateToSearchPage(query);
        }

        private async void UploadBtn_OnClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.UploadImagesAsync();
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }
    }
}