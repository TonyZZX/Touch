#region

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Touch.ViewModels;

#endregion

namespace Touch.Views.Pages
{
    internal sealed partial class GalleryPage
    {
        private readonly GalleryViewModel _viewModel;

        public GalleryPage()
        {
            InitializeComponent();
            _viewModel = new GalleryViewModel();
        }

        private async void GalleryPage_OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadImagesAsync();
        }

        private void AutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // We only want to get results when it was a user typing,
            // otherwise we assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput) return;
            var suggestions = _viewModel.GetSuggestions(sender.Text);
            sender.ItemsSource = suggestions.Count > 0 ? suggestions : new[] {"No results"};
        }

        private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender,
            AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // BUG: Don't autocomplete the TextBox when we are showing "no results"
            if (_viewModel.IsInSuggestions(args.SelectedItem as string)) sender.Text = (string) args.SelectedItem;
        }

        private void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (_viewModel.IsInSuggestions(sender.Text)) sender.Text += "!";
        }
    }
}