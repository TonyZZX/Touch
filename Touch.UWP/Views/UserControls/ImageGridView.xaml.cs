#region

using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Touch.Helpers;

#endregion

namespace Touch.Views.UserControls
{
    public sealed partial class ImageGridView
    {
        public ImageGridView()
        {
            InitializeComponent();
        }

        #region DependencyProperties

        public int PersistedItemIndex
        {
            get => (int) GetValue(PersistedItemIndexProperty);
            set => SetValue(PersistedItemIndexProperty, value);
        }

        public static readonly DependencyProperty PersistedItemIndexProperty =
            DependencyProperty.Register(
                "PersistedItemIndex",
                typeof(int),
                typeof(ImageGridView),
                new PropertyMetadata(-1));

        #endregion

        #region Animation

        private void GridView_OnContainerContentChanging(ListViewBase sender,
            ContainerContentChangingEventArgs args)
        {
            args.ItemContainer.Loaded += ItemContainer_Loaded;
        }

        private void ItemContainer_Loaded(object sender, RoutedEventArgs e)
        {
            var itemsPanel = (ItemsWrapGrid) ItemsPanelRoot;
            var itemContainer = (GridViewItem) sender;
            var itemIndex = IndexFromContainer(itemContainer);

            if (itemsPanel != null)
            {
                var relativeIndex = itemIndex - itemsPanel.FirstVisibleIndex;
                var grid = itemContainer.ContentTemplateRoot as Grid;

                if (itemIndex != PersistedItemIndex && itemIndex >= 0 && itemIndex >= itemsPanel.FirstVisibleIndex &&
                    itemIndex <= itemsPanel.LastVisibleIndex)
                    AnimationHelper.StartGridViewItemAnimation(grid, relativeIndex);
            }

            itemContainer.Loaded -= ItemContainer_Loaded;
        }

        private void GridViewItem_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!(sender is Grid rootGrid)) return;
            var image = rootGrid.Children[0] as FrameworkElement;
            ToggleItemPointAnimation(image, true);
        }

        private void GridViewItem_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!(sender is Grid rootGrid)) return;
            var image = rootGrid.Children[0] as FrameworkElement;
            ToggleItemPointAnimation(image, false);
        }

        private void GridViewItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!(sender is Grid grid)) return;
            grid.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, grid.ActualWidth, grid.ActualHeight)
            };
        }

        private static void ToggleItemPointAnimation(FrameworkElement image, bool ifShow)
        {
            var imageVisual = ElementCompositionPreview.GetElementVisual(image);
            var scaleAnimation = AnimationHelper.CreateScaleAnimation(image, ifShow, 1000);
            imageVisual.CenterPoint = new Vector3((float) image.ActualWidth / 2, (float) image.ActualHeight / 2, 0f);
            imageVisual.StartAnimation("Scale.x", scaleAnimation);
            imageVisual.StartAnimation("Scale.y", scaleAnimation);
        }

        #endregion
    }
}