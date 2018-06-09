#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Touch.Helpers;

#endregion

namespace Touch.Models
{
    public class Cover : PropertyChangedHelper
    {
        private readonly Image _image;
        private BitmapImage _originalImage;

        public Cover(Image image)
        {
            _image = image;
            // BUG: Why have to new a BitmapImage and set it setable???
            ThumbnailImage = new BitmapImage(new Uri(_image.ThumbnailSource));
        }

        public string Name { get; set; }

        public BitmapImage ThumbnailImage { get; set; }

        public BitmapImage OriginalImage
        {
            get => _originalImage;
            private set => SetProperty(ref _originalImage, value);
        }

        public async Task SetOriginalImageAsync(IList<Folder> folders)
        {
            await _image.SetOriginalImageAsync(folders);
            OriginalImage = _image.OriginalImage;
        }
    }
}