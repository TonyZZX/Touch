#region

using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Touch.Data;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    internal class SettingsViewModel
    {
        /// <summary>
        ///     Folder table in database
        /// </summary>
        private readonly FolderTable _folderTable;

        public SettingsViewModel()
        {
            _folderTable = new FolderTable();
            Folders = new ObservableCollection<Folder>();
        }

        public ObservableCollection<Folder> Folders { get; }

        /// <summary>
        ///     Load folders from database and add them to collections.
        /// </summary>
        public void LoadFolders()
        {
            using (var query = _folderTable.SelectAll())
            {
                while (query.Read())
                    Folders.Add(new Folder {Path = query.GetString(0), Token = query.GetString(1)});
            }
        }

        /// <summary>
        ///     Add folder to <see cref="StorageApplicationPermissions.FutureAccessList" />, database and collections.
        /// </summary>
        /// <param name="folder">Folder</param>
        public void AddFolder(StorageFolder folder)
        {
            var token = StorageApplicationPermissions.FutureAccessList.Add(folder);
            _folderTable.Insert(folder.Path, token);
            Folders.Add(new Folder {Path = folder.Path, Token = token});
        }

        /// <summary>
        ///     Remove folder from database, <see cref="StorageApplicationPermissions.FutureAccessList" /> and collections.
        /// </summary>
        /// <param name="folder">Folder</param>
        public void RemoveFolder(Folder folder)
        {
            _folderTable.Delete(folder.Path);
            StorageApplicationPermissions.FutureAccessList.Remove(folder.Token);
            Folders.Remove(folder);
        }
    }
}