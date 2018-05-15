using System;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;
using Touch.Models;

namespace Touch.Views.Pages
{
    public sealed partial class MainPage
    {
        private StorageFile _file;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void SelectButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".bmp");
            _file = await picker.PickSingleFileAsync();
            if (_file == null) return;
            PathText.Text = _file.Path;
            using (var fileStream = await _file.OpenAsync(FileAccessMode.Read))
            {
                var bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(fileStream);
                MyImage.Source = bitmapImage;
            }
        }

        private async void UploadButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (PathText.Text == "") return;
            using (var httpClient = new HttpClient())
            {
                using (var fileStream = await _file.OpenAsync(FileAccessMode.Read))
                {
                    var streamContent = new HttpStreamContent(fileStream);
                    var result = await httpClient.PostAsync(new Uri("http://59.110.137.131:1696"), streamContent);
                    var content = await result.Content.ReadAsStringAsync();
                    var trueContent = string.Join("", content.Split('\r', '\n').Skip(2));
                    var labels = Label.FromJson(trueContent)[0];
                    LabelResultText.Text = labels.Count == 0
                        ? "unrecognized"
                        : string.Join(", ", labels.Select(label => label.Name));
                }
            }
        }
    }
}