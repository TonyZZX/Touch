#region

using System;
using Windows.System;
using Windows.UI.Core;
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

            // Listen to the window directly so we will respond to hotkeys regardless of which element has focus.
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += CoreDispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
        }

        /// <summary>
        ///     Expose NavFrame to App
        /// </summary>
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
                    case "Objects":
                        _navigationService.NavigateAsync(typeof(ObjectsPage));
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
            }
        }

        #region Keyboard Navigation

        /// <summary>
        ///     Invoked on every keystroke, including system keys such as Alt key combinations.
        ///     Used to detect keyboard navigation between pages even when the page itself
        ///     doesn't have focus.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the event.</param>
        private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs e)
        {
            var virtualKey = e.VirtualKey;

            // Only investigate further when Left, Right, or the dedicated Previous or Next keys are pressed
            if (e.EventType != CoreAcceleratorKeyEventType.SystemKeyDown &&
                e.EventType != CoreAcceleratorKeyEventType.KeyDown ||
                virtualKey != VirtualKey.Left && virtualKey != VirtualKey.Right && (int) virtualKey != 166 &&
                (int) virtualKey != 167) return;
            var coreWindow = Window.Current.CoreWindow;
            const CoreVirtualKeyStates downState = CoreVirtualKeyStates.Down;
            var menuKey = (coreWindow.GetKeyState(VirtualKey.Menu) & downState) == downState;
            var controlKey = (coreWindow.GetKeyState(VirtualKey.Control) & downState) == downState;
            var shiftKey = (coreWindow.GetKeyState(VirtualKey.Shift) & downState) == downState;
            var noModifiers = !menuKey && !controlKey && !shiftKey;
            var onlyAlt = menuKey && !controlKey && !shiftKey;

            if (((int) virtualKey != 166 || !noModifiers) && (virtualKey != VirtualKey.Left || !onlyAlt)) return;
            _navigationService.GoBackAsync();
            e.Handled = true;
        }

        /// <summary>
        ///     Invoked on every mouse click, touch screen tap, or equivalent interaction.
        ///     Used to detect browser-style next and previous mouse button clicks to navigate between pages.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the event.</param>
        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs e)
        {
            var properties = e.CurrentPoint.Properties;

            // Ignore button chords with the left, right, and middle buttons
            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed || properties.IsMiddleButtonPressed)
                return;

            // If back or foward are pressed (but not both) navigate appropriately
            var backPressed = properties.IsXButton1Pressed;
            if (backPressed)
                _navigationService.GoBackAsync();
        }

        #endregion
    }
}