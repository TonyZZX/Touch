#region

using Windows.UI.Xaml.Navigation;

#endregion

namespace Touch.Views.Pages
{
    internal sealed partial class GallerySearchPage
    {
        public GallerySearchPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string query) TestText.Text = query;
        }
    }
}