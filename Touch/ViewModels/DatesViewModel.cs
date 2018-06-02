#region

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Models;
using Touch.Services;
using Touch.Views.Pages;

#endregion

namespace Touch.ViewModels
{
    internal class DatesViewModel : AcrylicGridViewModel
    {
        private readonly INavigationService _navigationService;

        public DatesViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        /// <summary>
        ///     Load available objects from database
        /// </summary>
        /// <returns>Void Task</returns>
        public async Task LoadObjectsAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                using (var db = new Database())
                {
                    var distinctDates = db.Images.Select(image => new ThumbnailImage(image).MonthYear).ToHashSet();
                    await LoadObjectsAsync(distinctDates, (image, date) => new ThumbnailImage(image).MonthYear == date,
                        date => date);
                }
            });
        }

        /// <summary>
        ///     Navigate to <see cref="DateDetailsPage" />
        /// </summary>
        /// <param name="labelObject">Classification object</param>
        public void NavigateToDetailsage(object labelObject)
        {
            _navigationService.NavigateAsync(typeof(DateDetailsPage), labelObject);
        }
    }
}