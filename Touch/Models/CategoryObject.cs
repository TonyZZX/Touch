#region

using Windows.UI.Xaml.Media.Imaging;

#endregion

namespace Touch.Models
{
    /// <summary>
    ///     Classification object
    /// </summary>
    public class CategoryObject
    {
        /// <summary>
        ///     Object name in category
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Thumbnail of the image which contains this object
        /// </summary>
        public BitmapImage Thumbnail { get; set; }
    }
}