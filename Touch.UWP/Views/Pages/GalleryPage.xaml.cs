#region

using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using Autofac;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Services;
using Touch.ViewModels;

#endregion

namespace Touch.Views.Pages
{
    public sealed partial class GalleryPage
    {
        private readonly GalleryViewModel _galleryViewModel;
        private readonly SettingsViewModel _settingsViewModel;

        public GalleryPage()
        {
            InitializeComponent();

            _galleryViewModel = new GalleryViewModel();
            _settingsViewModel = new SettingsViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await LoadImageGroups();

            await Task.Run(() => _settingsViewModel.LoadFolders());

            using (var scope = App.Container.BeginLifetimeScope())
            {
                var scanImageTask = scope.Resolve<IScanImageTask>();
                scanImageTask.ContentChanged += async (_, __) => { await LoadImageGroups(); };
            }
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                scope.Resolve<IScanImageTask>().Start();
            }
        }

        private async void AddFolderButton_OnClickAsync(object sender, RoutedEventArgs e)
        {
            var folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            folderPicker.FileTypeFilter.Add("*");
            var storageFolder = await folderPicker.PickSingleFolderAsync();
            if (storageFolder == null) return;
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                _settingsViewModel.AddFolder(storageFolder);
                using (var scope = App.Container.BeginLifetimeScope())
                {
                    scope.Resolve<IScanImageTask>().Start();
                }
            });
        }

        private async void UploadButton_OnClickAsync(object sender, RoutedEventArgs e)
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                using (var scope = App.Container.BeginLifetimeScope())
                {
                    await scope.Resolve<INavRootPage>().UploadImagesAsync();
                }

                FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
            });
        }

        private async Task LoadImageGroups()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                _galleryViewModel.LoadImageGroups();
                NoPhotosGrid.Visibility =
                    _galleryViewModel.Images.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            });
        }
    }
}