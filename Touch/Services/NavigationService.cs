#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Autofac;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.ViewModels;
using Touch.Views.Pages;

#endregion

namespace Touch.Services
{
    /// <summary>
    ///     NavigationService
    /// </summary>
    internal class NavigationService : INavigationService
    {
        /// <summary>
        ///     Context in which a service can be accessed or a component's dependencies resolved
        /// </summary>
        private readonly IComponentContext _iocResolver;

        /// <summary>
        ///     Navigation frame in root page
        /// </summary>
        private readonly Frame _navFrame;

        /// <summary>
        ///     Dictionary between page and its delegate for initializing view model
        /// </summary>
        private readonly Dictionary<Type, NavigatedToViewModelDelegate> _pageDelegateDict;

        /// <summary>
        ///     Whether it is navigating
        /// </summary>
        private bool _isNavigating;

        public NavigationService(IComponentContext iocResolver, Frame navFrame)
        {
            _iocResolver = iocResolver;
            _navFrame = navFrame;
            _navFrame.Navigated += NavFrame_OnNavigated;

            // Investigate a way to put these mappings into the IOC container
            // so that we don't have a hard dependency on the page types for multiplatform
            _pageDelegateDict = new Dictionary<Type, NavigatedToViewModelDelegate>();
            RegisterPageViewModel<GalleryPage, GalleryViewModel>();
            RegisterPageViewModel<ObjectsPage, ObjectsViewModel>();
            RegisterPageViewModel<DatesPage, DatesViewModel>();
        }

        /// <summary>
        ///     Gets a value that indicates whether there is at least one entry in back navigation history.
        /// </summary>
        public bool CanGoBack => _navFrame.CanGoBack;

        /// <summary>
        ///     Whether it is navigating
        /// </summary>
        public bool IsNavigating
        {
            get => _isNavigating;

            set
            {
                if (value == _isNavigating) return;
                _isNavigating = value;
                IsNavigatingChanged?.Invoke(this, _isNavigating);

                // Check that navigation just finished
                if (!_isNavigating) Navigated?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Whether the state of navigation is changed
        /// </summary>
        public event EventHandler<bool> IsNavigatingChanged;

        /// <summary>
        ///     Navigation finished
        /// </summary>
        public event EventHandler Navigated;

        /// <summary>
        ///     Navigates to the most recent item in back navigation history, if a Frame manages its own navigation history.
        /// </summary>
        /// <returns>Void Task</returns>
        public async Task GoBackAsync()
        {
            if (_navFrame.CanGoBack)
            {
                IsNavigating = true;

                await DispatcherHelper.ExecuteOnUIThreadAsync(() => { _navFrame.GoBack(); });
            }
        }

        /// <summary>
        ///     Navigate to another page without parameter
        /// </summary>
        /// <param name="sourcePageType">Type of source page</param>
        /// <returns>Void task</returns>
        public async Task NavigateAsync(Type sourcePageType)
        {
            await NavigateAsync(sourcePageType, null);
        }

        /// <summary>
        ///     Navigate to another page
        /// </summary>
        /// <param name="sourcePageType">Type of source page</param>
        /// <param name="parameter">Parameter</param>
        /// <returns>Void task</returns>
        public async Task NavigateAsync(Type sourcePageType, object parameter)
        {
            // Early out if already in the middle of a Navigation
            // or destination page is the same page as current one
            if (_isNavigating || _navFrame.CurrentSourcePageType == sourcePageType) return;

            _isNavigating = true;

            await DispatcherHelper.ExecuteOnUIThreadAsync(() => { _navFrame.Navigate(sourcePageType, parameter); });
        }

        #region Page with ViewModel Navigation

        /// <summary>
        ///     Register page with its delegate for initializing view model
        /// </summary>
        /// <typeparam name="TPage">Page</typeparam>
        /// <typeparam name="TViewModel">View model</typeparam>
        private void RegisterPageViewModel<TPage, TViewModel>() where TViewModel : class
        {
            _pageDelegateDict[typeof(TPage)] = page =>
            {
                if (!(page is IPageWithViewModel<TViewModel> pageWithVm)) return;
                pageWithVm.ViewModel = _iocResolver.Resolve<TViewModel>();
                pageWithVm.UpdateBindings();
            };
        }

        /// <summary>
        ///     Navigated event. This event is raised BEFORE <see cref="Page.OnNavigatedTo(NavigationEventArgs)" />
        /// </summary>
        /// <param name="sender">Frame</param>
        /// <param name="e">Args coming from the frame</param>
        private void NavFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            IsNavigating = false;
            if (_pageDelegateDict.TryGetValue(e.SourcePageType, out var navDelegate))
                navDelegate(e.Content);
        }

        /// <summary>
        ///     Delegate for initializing page's view model
        /// </summary>
        /// <param name="page">Page whose view model needs to initialized</param>
        private delegate void NavigatedToViewModelDelegate(object page);

        #endregion
    }
}