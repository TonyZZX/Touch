#region

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Navigation;
using Touch.Models;
using Touch.ViewModels;

#endregion

namespace Touch.Views.Pages
{
    internal sealed partial class ObjectDetailsPage
    {
        private ObjectDetailsViewModel _viewModel;

        public ObjectDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var labelObject = e.Parameter as LabelObject;
            _viewModel = new ObjectDetailsViewModel(labelObject);
            TitleText.Text = labelObject?.Name ?? throw new InvalidOperationException();
        }

        private async void ObjectDetailsPage_OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            // Hide mask befor showing
            var maskVisual = ElementCompositionPreview.GetElementVisual(Mask);
            maskVisual.Opacity = 0f;

            await _viewModel.LoadImagesAsync();

            // Show mask of cover image
            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            var fadeAnimation = compositor.CreateScalarKeyFrameAnimation();
            fadeAnimation.InsertKeyFrame(1, 1);
            fadeAnimation.Duration = TimeSpan.FromMilliseconds(2000);
            maskVisual.StartAnimation("Opacity", fadeAnimation);
        }
    }
}