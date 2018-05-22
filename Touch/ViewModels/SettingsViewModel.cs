#region

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    internal class SettingsViewModel
    {
        public SettingsViewModel()
        {
            Folders = new ObservableCollection<Folder>();
        }

        /// <summary>
        ///     Folder list
        /// </summary>
        public ObservableCollection<Folder> Folders { get; }

        /// <summary>
        ///     Load folders from database and add them to collections.
        /// </summary>
        /// <returns>Void Task</returns>
        public async Task LoadFoldersAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                using (var db = new Database())
                {
                    foreach (var folder in db.Folders) Folders.Add(folder);
                }
            });
        }

        /// <summary>
        ///     Add folder to <see cref="StorageApplicationPermissions.FutureAccessList" />, database and collections.
        /// </summary>
        /// <param name="storageFolder">StorageFolder</param>
        /// <returns>Void Task</returns>
        public async Task AddFolderAsync(StorageFolder storageFolder)
        {
            if (Folders.Any(f => f.Path == storageFolder.Path)) return;
            var token = StorageApplicationPermissions.FutureAccessList.Add(storageFolder);
            var folder = new Folder {Path = storageFolder.Path, Token = token};
            using (var db = new Database())
            {
                db.Folders.Add(folder);
                db.SaveChanges();
            }

            await DispatcherHelper.ExecuteOnUIThreadAsync(() => { Folders.Add(folder); });
        }

        /// <summary>
        ///     Remove folder from database, <see cref="StorageApplicationPermissions.FutureAccessList" /> and collections.
        /// </summary>
        /// <param name="folder">Folder</param>
        /// <returns>Void Task</returns>
        public async Task RemoveFolderAsync(Folder folder)
        {
            using (var db = new Database())
            {
                db.Folders.Remove(folder);
                db.SaveChanges();
            }

            StorageApplicationPermissions.FutureAccessList.Remove(folder.Token);
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => { Folders.Remove(folder); });
        }
    }
}