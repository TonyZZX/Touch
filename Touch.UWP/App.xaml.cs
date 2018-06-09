#region

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Touch.Database;
using Touch.Models;
using Touch.Services;
using Touch.Views.Pages;

#endregion

namespace Touch
{
    /// <summary>
    ///     Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App
    {
        public static IContainer Container;

        /// <summary>
        ///     Initializes the singleton application object.  This is the first line of authored code
        ///     executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;

            using (var db = new Context())
            {
                db.Database.Migrate();
            }

            MapService.ServiceToken = Constants.MapServiceToken;
        }

        /// <summary>
        ///     Invoked when the application is launched normally by the end user.  Other entry points
        ///     will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            SetTitleBar();

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (e.PrelaunchActivated || Window.Current.Content != null) return;
            // Create a Frame to act as the navigation context and navigate to the first page
            var navRootPage = new NavRootPage();
            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            var builder = new ContainerBuilder();
            builder.RegisterInstance(navRootPage).AsImplementedInterfaces();
            builder.RegisterType<ScanImageTask>().AsImplementedInterfaces().SingleInstance();
            Container = builder.Build();
            Container.Resolve<IScanImageTask>().Start();

            // Place the frame in the current Window
            Window.Current.Content = navRootPage;
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        ///     Invoked when application execution is being suspended.  Application state is saved
        ///     without knowing whether the application will be terminated or resumed with the contents
        ///     of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private static void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        /// <summary>
        ///     Set up transparent TitleBar
        /// </summary>
        private void SetTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = (Color) Resources["SystemBaseHighColor"];
        }
    }
}