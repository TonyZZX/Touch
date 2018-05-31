#region

using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Touch.Helpers;

#endregion

namespace Touch.Views.Controls
{
    /// <summary>
    ///     Adaptive GridView based on width.
    ///     <see cref="Microsoft.Toolkit.Uwp.UI.Controls.AdaptiveGridView" /> may fill the width of the page even if there is a
    ///     few items, so the width of items will be stretched which is not very ideal.
    /// </summary>
    public class AdaptiveGridView : GridView
    {
        public AdaptiveGridView()
        {
            if (ItemContainerStyle == null)
                ItemContainerStyle = new Style(typeof(GridViewItem));

            ItemContainerStyle.Setters.Add(new Setter(HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch));

            Loaded += (s, a) =>
            {
                if (ItemsPanelRoot != null)
                    InvalidateMeasure();
            };
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (!(ItemsPanelRoot is ItemsWrapGrid panel))
                return base.MeasureOverride(availableSize);
            if (Math.Abs(MinItemWidth) < Utils.Tolerance)
                throw new DivideByZeroException("You need to have a MinItemWidth greater than zero");

            var availableWidth = availableSize.Width - (Padding.Right + Padding.Left);

            var numColumns = Math.Floor(availableWidth / MinItemWidth);
            numColumns = Math.Abs(numColumns) < Utils.Tolerance ? 1 : numColumns;
            if (Items != null)
            {
                // ReSharper disable once UnusedVariable
                var numRows = Math.Ceiling(Items.Count / numColumns);
            }

            var itemWidth = availableWidth / numColumns;
            var aspectRatio = MinItemHeight / MinItemWidth;
            var itemHeight = itemWidth * aspectRatio;

            panel.ItemWidth = itemWidth;
            panel.ItemHeight = itemHeight;

            return base.MeasureOverride(availableSize);
        }

        #region DependencyProperties

        /// <summary>
        ///     Minimum height for item
        /// </summary>
        public double MinItemHeight
        {
            get => (double) GetValue(MinItemHeightProperty);
            set => SetValue(MinItemHeightProperty, value);
        }

        public static readonly DependencyProperty MinItemHeightProperty =
            DependencyProperty.Register(
                "MinItemHeight",
                typeof(double),
                typeof(AdaptiveGridView),
                new PropertyMetadata(1.0, (s, a) =>
                {
                    if (!double.IsNaN((double) a.NewValue))
                        ((AdaptiveGridView) s).InvalidateMeasure();
                }));

        /// <summary>
        ///     Minimum width for item (must be greater than zero)
        /// </summary>
        public double MinItemWidth
        {
            get => (double) GetValue(MinimumItemWidthProperty);
            set => SetValue(MinimumItemWidthProperty, value);
        }

        public static readonly DependencyProperty MinimumItemWidthProperty =
            DependencyProperty.Register(
                "MinItemWidth",
                typeof(double),
                typeof(AdaptiveGridView),
                new PropertyMetadata(1.0, (s, a) =>
                {
                    if (!double.IsNaN((double) a.NewValue))
                        ((AdaptiveGridView) s).InvalidateMeasure();
                }));

        #endregion
    }
}