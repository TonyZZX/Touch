#region

using System;
using System.ComponentModel.DataAnnotations.Schema;
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
            DateModified = image.DateModified;
            Labels = image.Labels;
        }

        /// <summary>
        ///     Image thumbnail
        /// </summary>
        [NotMapped]
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
            return new Image {Path = Path, Size = Size, DateModified = DateModified, Labels = Labels};
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}