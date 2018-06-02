#region

using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    internal class DateDetailsViewModel : SelectedImagesViewModel
    {
        public DateDetailsViewModel(LabelObject labelObject)
        {
            LabelObject = labelObject;
        }

        /// <summary>
        ///     Classification object
        /// </summary>
        public LabelObject LabelObject { get; }

        public override async Task LoadImagesAsync()
        {
            var date = LabelObject.Name;
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                await LoadImagesAsync(date, image => new ThumbnailImage(image).MonthYear == date);
            });
        }
    }
}