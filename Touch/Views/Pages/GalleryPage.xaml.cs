#region

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            var suggestions = ViewModel.GetSuggestions(sender.Text);
            sender.ItemsSource = suggestions.Count > 0 ? suggestions : new[] {"No results"};
        }

        private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender,
            AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // BUG: Don't autocomplete the TextBox when we are showing "no results"
            if (ViewModel.IsInSuggestions(args.SelectedItem as string)) sender.Text = (string) args.SelectedItem;
        }

        private void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.NavigateToSearchPage(sender.Text);
        }
    }
}