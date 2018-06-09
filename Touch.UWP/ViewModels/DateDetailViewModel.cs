#region

using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    public class DateDetailViewModel : BaseImagesViewModel
    {
        public readonly Cover DateCover;

        public DateDetailViewModel(Cover dateCover)
        {
            DateCover = dateCover;
        }

        public void LoadImages()
        {
            LoadImages(image => image.MonthYearDate.ToString() == DateCover.Name);
        }
    }
}