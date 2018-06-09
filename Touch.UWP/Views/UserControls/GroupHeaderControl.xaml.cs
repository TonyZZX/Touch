#region

using Windows.UI.Xaml;

#endregion

namespace Touch.Views.UserControls
{
    public sealed partial class GroupHeaderControl
    {
        public GroupHeaderControl()
        {
            InitializeComponent();
        }

        #region DependencyProperties

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(GroupHeaderControl),
                new PropertyMetadata(""));

        #endregion
    }
}