#region

using System;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

#endregion

namespace Touch.Helpers
{
    public class AnimationHelper
    {
        public static ScalarKeyFrameAnimation CreateScaleAnimation(UIElement element, bool ifShow, double duration)
        {
            var compositor = ElementCompositionPreview.GetElementVisual(element).Compositor;
            var scaleAnimation = compositor.CreateScalarKeyFrameAnimation();
            scaleAnimation.InsertKeyFrame(1f, ifShow ? 1.1f : 1f);
            scaleAnimation.Duration = TimeSpan.FromMilliseconds(duration);
            scaleAnimation.StopBehavior = AnimationStopBehavior.LeaveCurrentValue;
            return scaleAnimation;
        }

        public static ScalarKeyFrameAnimation CreateOpacityAnimation(UIElement element, bool ifShow, double duration)
        {
            var compositor = ElementCompositionPreview.GetElementVisual(element).Compositor;
            var opacityAnimation = compositor.CreateScalarKeyFrameAnimation();
            opacityAnimation.InsertKeyFrame(1f, ifShow ? 1f : 0f);
            opacityAnimation.Duration = TimeSpan.FromMilliseconds(duration);
            return opacityAnimation;
        }

        public static void StartGridViewItemAnimation(Grid grid, int relativeIndex)
        {
            var itemVisual = ElementCompositionPreview.GetElementVisual(grid);
            ElementCompositionPreview.SetIsTranslationEnabled(grid, true);

            var easingFunction =
                Window.Current.Compositor.CreateCubicBezierEasingFunction(new Vector2(0.1f, 0.9f),
                    new Vector2(0.2f, 1f));
            var offsetAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(0f, 100);
            offsetAnimation.InsertKeyFrame(1f, 0, easingFunction);
            offsetAnimation.Target = "Translation.X";
            offsetAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(700);
            offsetAnimation.DelayTime = TimeSpan.FromMilliseconds(relativeIndex * 100);

            var fadeAnimation = Window.Current.Compositor.CreateScalarKeyFrameAnimation();
            fadeAnimation.InsertExpressionKeyFrame(0f, "0");
            fadeAnimation.InsertExpressionKeyFrame(1f, "1");
            fadeAnimation.DelayBehavior = AnimationDelayBehavior.SetInitialValueBeforeDelay;
            fadeAnimation.Duration = TimeSpan.FromMilliseconds(700);
            fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(relativeIndex * 100);

            itemVisual.StartAnimation("Translation.X", offsetAnimation);
            itemVisual.StartAnimation("Opacity", fadeAnimation);
        }
    }
}