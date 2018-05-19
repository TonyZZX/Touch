#region

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Services;

#endregion

namespace Touch.Views.Pages
{
    internal sealed partial class NavRootPage
    {
        private INavigationService _navigationService;

        public NavRootPage()
        {
            InitializeComponent();
        }

        public Frame MainNavFrame => NavFrame;

        public void InitializeNavigationService(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.Navigated += NavigationService_Navigated;
        }

        private void NavigationService_Navigated(object sender, EventArgs e)
        {
            DispatcherHelper.ExecuteOnUIThreadAsync(() => { NavView.IsBackEnabled = _navigationService.CanGoBack; });
        }

        private void NavRootPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            _navigationService.NavigateAsync(typeof(GalleryPage));
        }

        private void NavView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
                _navigationService.NavigateAsync(typeof(SettingsPage));
            else
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (args.InvokedItem as string)
                {
                    // TODO: DataBinding Items
                    case "Gallery":
                        _navigationService.NavigateAsync(typeof(GalleryPage));
                        break;
                    case "Test":
                        _navigationService.NavigateAsync(typeof(TestPage));
                        break;
                }
        }

        private void NavView_OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            _navigationService.GoBackAsync();
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
            }
        }
    }
}