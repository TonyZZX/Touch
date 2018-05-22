#region

using System;
using Windows.ApplicationModel;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Models;
using Touch.ViewModels;

#endregion

namespace Touch.Views.Pages
{
    internal sealed partial class SettingsPage
    {
        private readonly SettingsViewModel _viewModel;

        public SettingsPage()
        {
            InitializeComponent();
            _viewModel = new SettingsViewModel();
        }

        private async void SettingsPage_OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadFoldersAsync();

            // Update "About this app" section
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                var package = Package.Current;
                var version = package.Id.Version;
                VersionText.Text = package.DisplayName + " " +
                                   $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            });
        }

        private async void AddFolderBtn_OnClickAsync(object sender, RoutedEventArgs e)
        {
            var folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            folderPicker.FileTypeFilter.Add("*");
            var storageFolder = await folderPicker.PickSingleFolderAsync();
            if (storageFolder == null) return;
            await _viewModel.AddFolderAsync(storageFolder);
        }

        private async void DeleteBtn_OnClickAsync(object sender, RoutedEventArgs e)
        {
            if (!((sender as Button)?.DataContext is Folder clickedfolder)) return;
            await _viewModel.RemoveFolderAsync(clickedfolder);
        }

        private async void FeedbackBtn_OnClickAsync(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(@"https://github.com/TonyZZX/Touch.UWP/issues"));
        }
    }
}