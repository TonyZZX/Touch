#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Touch.Models;

#endregion

namespace Touch.Helpers
{
    internal class Utils
    {
        /// <summary>
        ///     Get <see cref="StorageFile" /> with file path from folder list.
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="folders">folder list</param>
        /// <returns>StorageFile</returns>
        public static async Task<StorageFile> GetFileAsync(string filePath, IEnumerable<Folder> folders)
        {
            var folderList = folders.ToList();
            var folder = folderList[0];
            var relativePath = folder.GetRelativePath(filePath);
            foreach (var f in folderList)
            {
                folder = f;
                relativePath = folder.GetRelativePath(filePath);
                if (relativePath != "") break;
            }

            var storageFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folder.Token);
            var storageFile = await storageFolder.GetFileAsync(relativePath);
            return storageFile;
        }
    }
}