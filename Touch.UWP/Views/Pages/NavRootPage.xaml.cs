#region

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Api;
using Touch.Database;
using Touch.Services;

#endregion

namespace Touch.Views.Pages
{
    public sealed partial class NavRootPage : INavRootPage
    {
        private readonly Mgml _mgml;

        public NavRootPage()
        {
            InitializeComponent();

            _mgml = new Mgml();

            // Listen to the window directly so we will respond to hotkeys regardless of which element has focus.
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += CoreDispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
        }

        public async Task UploadImagesAsync()
        {
            UploadingGrid.Visibility = Visibility.Visible;
            await _mgml.UploadImagesAsync();
            UploadingGrid.Visibility = Visibility.Collapsed;
        }

        private void NavRootPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            ((NavigationViewItem) NavView.MenuItems[0]).IsSelected = true;
        }

        private void NavView_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                NavFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                var selectedItem = (args.SelectedItem as NavigationViewItem)?.Content;
                switch (selectedItem)
                {
                    case "Home":
                        NavFrame.Navigate(typeof(GalleryPage));
                        break;
                    case "Dates":
                        NavFrame.Navigate(typeof(DatesPage));
                        break;
                    case "Places":
                        NavFrame.Navigate(typeof(PlacesPage));
                        break;
                    case "Tags":
                        NavFrame.Navigate(typeof(TagsPage));
                        break;
                    default:
                        Debug.WriteLine("NotImplementedException: " + selectedItem);
                        break;
                }
            }
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            OnBackRequested();
        }

        private void NavFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = NavFrame.CanGoBack;

            var sourcePage = NavFrame.SourcePageType;
            switch (sourcePage)
            {
                case Type _ when sourcePage == typeof(SettingsPage):
                    NavView.SelectedItem = NavView.SettingsItem as NavigationViewItem;
                    break;
                case Type _ when sourcePage == typeof(GalleryPage):
                    ((NavigationViewItem) NavView.MenuItems[0]).IsSelected = true;
                    break;
                case Type _ when sourcePage == typeof(DatesPage):
                    ((NavigationViewItem) NavView.MenuItems[3]).IsSelected = true;
                    break;
                case Type _ when sourcePage == typeof(PlacesPage):
                    ((NavigationViewItem) NavView.MenuItems[4]).IsSelected = true;
                    break;
                case Type _ when sourcePage == typeof(TagsPage):
                    ((NavigationViewItem) NavView.MenuItems[5]).IsSelected = true;
                    break;
            }
        }

        private async void AutoSuggestBox_OnTextChangedAsync(AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            // We only want to get results when it was a user typing,
            // otherwise we assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput) return;

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                var queryStr = sender.Text;
                using (var db = new Context())
                {
                    var suggestions = db.Tags.AsEnumerable().Distinct()
                        .Where(tag => tag.Name.IndexOf(queryStr, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        .Select(tag => tag.Name).OrderBy(str => str).ToArray();
                    sender.ItemsSource = suggestions.Any() ? suggestions : new[] {"No results"};
                }
            });
        }

        private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender,
            AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem is string query)
                sender.Text = query;
        }

        private void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var queryStr = sender.Text;
            if (queryStr != "" && queryStr != "No results")
                NavFrame.Navigate(typeof(SearchPage), queryStr);
        }

        private void OnBackRequested()
        {
            if (NavFrame.CanGoBack) NavFrame.GoBack();
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
            OnBackRequested();
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
            if (backPressed) OnBackRequested();
        }

        #endregion
    }
}