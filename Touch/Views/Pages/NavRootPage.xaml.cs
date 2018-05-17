#region

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

#endregion

namespace Touch.Views.Pages
{
    internal sealed partial class NavRootPage
    {
        public NavRootPage()
        {
            InitializeComponent();
        }

        private void NavRootPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            NavFrame.Navigate(typeof(GalleryPage));
        }

        private void NavView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
                NavFrame.Navigate(typeof(SettingsPage));
            else
                switch (args.InvokedItem as string)
                {
                    // TODO: DataBinding Items
                    case "Gallery":
                        NavFrame.Navigate(typeof(GalleryPage));
                        break;
                    case "Test":
                        NavFrame.Navigate(typeof(TestPage));
                        break;
                    default:
                        // TODO: Throw exception: unexpected item
                        break;
                }
        }

        private void NavFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            switch (e.SourcePageType)
            {
                case Type _ when e.SourcePageType == typeof(GalleryPage):
                    ((NavigationViewItem) NavView.MenuItems[0]).IsSelected = true;
                    break;
                case Type _ when e.SourcePageType == typeof(TestPage):
                    ((NavigationViewItem) NavView.MenuItems[1]).IsSelected = true;
                    break;
                default:
                    // TODO: Throw exception: unexpected item
                    break;
            }
        }
    }
}