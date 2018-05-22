#region

using System;
using System.Threading.Tasks;

#endregion

namespace Touch.Services
{
    /// <summary>
    ///     Interface of NavigationService
    /// </summary>
    internal interface INavigationService
    {
        /// <summary>
        ///     Gets a value that indicates whether there is at least one entry in back navigation history.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        ///     Whether it is navigating
        /// </summary>
        bool IsNavigating { get; }

        /// <summary>
        ///     Whether the state of navigation is changed
        /// </summary>
        event EventHandler<bool> IsNavigatingChanged;

        /// <summary>
        ///     Navigation finished
        /// </summary>
        event EventHandler Navigated;

        /// <summary>
        ///     Navigate to another page without parameter
        /// </summary>
        /// <param name="sourcePageType">Type of source page</param>
        /// <returns>Void Task</returns>
        Task NavigateAsync(Type sourcePageType);

        /// <summary>
        ///     Navigate to another page
        /// </summary>
        /// <param name="sourcePageType">Type of source page</param>
        /// <param name="parameter">Parameter</param>
        /// <returns>Void Task</returns>
        Task NavigateAsync(Type sourcePageType, object parameter);

        /// <summary>
        ///     Navigates to the most recent item in back navigation history, if a Frame manages its own navigation history.
        /// </summary>
        /// <returns>Void Task</returns>
        Task GoBackAsync();
    }
}