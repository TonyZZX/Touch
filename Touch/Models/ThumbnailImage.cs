#region

using System;
using Windows.Globalization.DateTimeFormatting;
using Windows.UI.Xaml.Media.Imaging;

#endregion

namespace Touch.Models
{
    /// <summary>
    ///     Image thumbnail
    /// </summary>
    internal class ThumbnailImage : Image, IEquatable<ThumbnailImage>
    {
        /// <summary>
        ///     Image thumbnail
        /// </summary>
        public ThumbnailImage()
        {
        }

        /// <summary>
        ///     Image thumbnail
        /// </summary>
        public ThumbnailImage(Image image)
        {
            Path = image.Path;
            Size = image.Size;
            Date = image.Date;
            Labels = image.Labels;
        }

        public string MonthYear => new DateTimeFormatter("month year").Format(Date);

        /// <summary>
        ///     Image thumbnail
        /// </summary>
        public BitmapImage Thumbnail { get; set; }

        public bool Equals(ThumbnailImage other)
        {
            return base.Equals(other);
        }

        /// <summary>
        ///     Convert to <see cref="Image" />
        /// </summary>
        /// <returns>Image</returns>
        public Image ConvertToImage()
        {
            return new Image {Path = Path, Size = Size, Date = Date, Labels = Labels};
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}