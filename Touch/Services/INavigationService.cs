#region

using System;
using System.Threading.Tasks;

#endregion

namespace Touch.Services
{
    internal interface INavigationService
    {
        bool CanGoBack { get; }

        bool IsNavigating { get; }

        event EventHandler<bool> IsNavigatingChanged;

        event EventHandler Navigated;

        Task NavigateAsync(Type sourcePageType);

        Task NavigateAsync(Type sourcePageType, object parameter);

        Task GoBackAsync();
    }
}