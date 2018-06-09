#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Microsoft.EntityFrameworkCore;
using Touch.Database;
using Touch.Models;
using Buffer = Windows.Storage.Streams.Buffer;

#endregion

namespace Touch.Services
{
    public class ScanImageTask : IScanImageTask
    {
        private bool _isRunning;

        public event EventHandler Running;

        public event EventHandler ContentChanged;

        public event EventHandler Completed;

        public async Task Start()
        {
            if (_isRunning) return;
            _isRunning = true;
            await Task.Run(LoadImages);
            _isRunning = false;
        }

        private async Task LoadImages()
        {
            Running?.Invoke(this, EventArgs.Empty);

            // Set fast access to file properties
            var queryOptions = new QueryOptions(CommonFileQuery.OrderByName,
                new List<string> {".jpg", ".jpeg", ".png", ".bmp"});
            queryOptions.SetThumbnailPrefetch(ThumbnailMode.SingleItem, Constants.ThumbnailSize,
                ThumbnailOptions.ReturnOnlyIfCached);
            queryOptions.SetPropertyPrefetch(
                PropertyPrefetchOptions.BasicProperties | PropertyPrefetchOptions.ImageProperties, null);

            using (var db = new Context())
            {
                // Old images in database
                var oldImages = db.Images.Include(image => image.Tags);
                var oldImagesSet = oldImages.ToHashSet();

                // Load all images from folders
                var newImages = new List<Image>();
                var folders = db.Folders.ToList();
                foreach (var folder in folders)
                {
                    var storageFolder =
                        await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folder.Token);
                    var storageFiles = await storageFolder.CreateFileQueryWithOptions(queryOptions).GetFilesAsync();
                    // TODO: query.ContentsChanged += OnContentsChanged;
                    foreach (var storageFile in storageFiles)
                    {
                        var basicProperties = await storageFile.GetBasicPropertiesAsync();
                        var imageProperties = await storageFile.Properties.GetImagePropertiesAsync();
                        var newImage = new Image
                        {
                            Name = Path.GetFileNameWithoutExtension(storageFile.Name),
                            FileType = storageFile.FileType,
                            Path = storageFile.Path,
                            Size = basicProperties.Size,
                            DateModified = basicProperties.DateModified,
                            Height = imageProperties.Height,
                            Width = imageProperties.Width,
                            DateTaken = imageProperties.DateTaken,
                            Latitude = imageProperties.Latitude,
                            Longitude = imageProperties.Longitude
                        };

                        var newImageBase = newImage.ToImageBase();
                        if (oldImagesSet.Contains(newImageBase))
                        {
                            oldImagesSet.Remove(newImageBase);
                            continue;
                        }

                        newImage.Id = Guid.NewGuid();

                        // Save Thumbnail to LocalFolder
                        using (var thumbnail =
                            await storageFile.GetThumbnailAsync(ThumbnailMode.SingleItem, Constants.ThumbnailSize))
                        {
                            var saveFile =
                                await ApplicationData.Current.LocalFolder.CreateFileAsync(newImage.Id.ToString(),
                                    CreationCollisionOption.ReplaceExisting);
                            using (var destFileStream = await saveFile.OpenAsync(FileAccessMode.ReadWrite))
                            {
                                var inputBuffer = new Buffer(2048);
                                IBuffer buf;
                                while ((buf = await thumbnail.ReadAsync(inputBuffer, inputBuffer.Capacity,
                                           InputStreamOptions.None)).Length > 0)
                                    await destFileStream.WriteAsync(buf);
                            }
                        }

                        // Save location information
                        if (newImage.Latitude != null && newImage.Longitude != null)
                        {
                            var location = new BasicGeoposition
                            {
                                Latitude = (double) newImage.Latitude,
                                Longitude = (double) newImage.Longitude
                            };
                            var result = await MapLocationFinder.FindLocationsAtAsync(new Geopoint(location));
                            if (result.Status == MapLocationFinderStatus.Success)
                            {
                                newImage.Region = result.Locations[0].Address.Region;
                                newImage.District = result.Locations[0].Address.District;
                                newImage.Town = result.Locations[0].Address.Town;
                            }
                        }

                        newImages.Add(newImage);
                    }
                }

                var hasOldImages = oldImagesSet.Any();
                var hasNewImages = newImages.Any();

                // Delete unexisted images in database
                if (hasOldImages)
                    try
                    {
                        var unexistedImages = oldImages.AsEnumerable().Intersect(oldImagesSet.ToList());
                        db.Images.RemoveRange(unexistedImages);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        throw;
                    }

                // Add new images to database
                // ReSharper disable once InvertIf
                if (hasNewImages)
                {
                    var addImages = newImages.Select(image => image.ToImageBase());
                    db.Images.AddRange(addImages);
                    db.SaveChanges();
                }

                if (hasOldImages || hasNewImages) ContentChanged?.Invoke(this, EventArgs.Empty);
            }

            Completed?.Invoke(this, EventArgs.Empty);
        }
    }
}