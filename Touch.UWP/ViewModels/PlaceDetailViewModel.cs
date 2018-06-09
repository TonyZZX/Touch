#region

using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    public class PlaceDetailViewModel : BaseImagesViewModel
    {
        public readonly Cover PlaceCover;

        public PlaceDetailViewModel(Cover placeCover)
        {
            PlaceCover = placeCover;
        }

        public void LoadImages()
        {
            LoadImages(image => image.Place == PlaceCover.Name);
        }
    }
}