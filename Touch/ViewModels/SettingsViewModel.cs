#region

using System.Collections.ObjectModel;
using Touch.Data;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    internal class SettingsViewModel
    {
        private readonly FolderTable _folderTable;

        public SettingsViewModel()
        {
            _folderTable = new FolderTable();
            Folders = new ObservableCollection<Folder>();
        }

        public ObservableCollection<Folder> Folders { get; }

        /// <summary>
        ///     Load folders from database and add them to collections
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
        ///     Add folder to database and collections
        /// </summary>
        /// <param name="folder">Folder</param>
        public void AddFolder(Folder folder)
        {
            _folderTable.Insert(folder.Path, folder.Token);
            Folders.Add(folder);
        }

        /// <summary>
        ///     Remove folder from database and collections
        /// </summary>
        /// <param name="folder">Folder</param>
        public void RemoveFolder(Folder folder)
        {
            _folderTable.Delete(folder.Path);
            Folders.Remove(folder);
        }
    }
}