#region

using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
            var suggestions = ViewModel.GetSuggestions(sender.Text);
            sender.ItemsSource = suggestions.Count > 0 ? suggestions : new[] {"No results"};
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

        private void GridViewItem_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!(sender is Grid rootGrid))
                return;
            var img = rootGrid.Children[0] as FrameworkElement;
            ToggleItemPointAnimation(img, true);
        }

        private void GridViewItem_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!(sender is Grid rootGrid))
                return;
            var img = rootGrid.Children[0] as FrameworkElement;
            ToggleItemPointAnimation(img, false);
        }

        private void GridViewItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is Grid rootGrid)
                rootGrid.Clip = new RectangleGeometry
                {
                    Rect = new Rect(0, 0, rootGrid.ActualWidth, rootGrid.ActualHeight)
                };
        }

        private void ToggleItemPointAnimation(FrameworkElement img, bool show)
        {
            var imgVisual = ElementCompositionPreview.GetElementVisual(img);
            var scaleAnimation = CreateScaleAnimation(show);
            imgVisual.CenterPoint = new Vector3((float) img.ActualWidth / 2, (float) img.ActualHeight / 2, 0f);
            imgVisual.StartAnimation("Scale.x", scaleAnimation);
            imgVisual.StartAnimation("Scale.y", scaleAnimation);
        }

        private ScalarKeyFrameAnimation CreateScaleAnimation(bool show)
        {
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var scaleAnimation = compositor.CreateScalarKeyFrameAnimation();
            scaleAnimation.InsertKeyFrame(1f, show ? 1.1f : 1f);
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            scaleAnimation.StopBehavior = AnimationStopBehavior.LeaveCurrentValue;
            return scaleAnimation;
        }
    }
}