#region

using System;
using System.Collections;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

#endregion

namespace Touch.Views.Controls
{
    internal sealed partial class SemanticZoomControl
    {
        public SemanticZoomControl()
        {
            InitializeComponent();
        }

        #region ZoomInOut Animation

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

        #endregion

        #region DependencyProperties

        public IEnumerable Source
        {
            get => (IEnumerable) GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                "Source",
                typeof(IEnumerable),
                typeof(SemanticZoomControl),
                new PropertyMetadata(null));

        #endregion
    }
}