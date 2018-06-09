#region

using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    public class TagDetailViewModel : BaseImagesViewModel
    {
        public readonly Cover TagCover;

        public TagDetailViewModel(Cover tagCover)
        {
            TagCover = tagCover;
        }

        public void LoadImages()
        {
            LoadImages(image => image.IfContainsTag(TagCover.Name));
        }
    }
}