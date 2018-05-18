#region

using Windows.UI.Xaml.Media.Imaging;

#endregion

namespace Touch.Models
{
    /// <summary>
    ///     Image
    /// </summary>
    internal class Image
    {
        /// <summary>
        ///     Image path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     Image thumbnail
        /// </summary>
        public BitmapImage Thumbnail { get; set; }
    }
}