#region

using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#endregion

namespace Touch.Views.UserControls
{
    public sealed partial class ImageSemanticZoomControl
    {
        public ImageSemanticZoomControl()
        {
            InitializeComponent();
        }

        #region Event

        public event ItemClickEventHandler ItemClick;

        private void GridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ItemClick?.Invoke(sender, e);
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
                typeof(ImageSemanticZoomControl),
                new PropertyMetadata(null));

        #endregion
    }
}