#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Touch.Database;
using Touch.Helpers;
using Touch.Models;

#endregion

namespace Touch.Api
{
    /// <summary>
    ///     Label category
    /// </summary>
    public class Mgml : PropertyChangedHelper
    {
        private double _maxValue;
        private double _progress;

        public double MaxValue
        {
            get => _maxValue;
            private set => SetProperty(ref _maxValue, value);
        }

        public double Progress
        {
            get => _progress;
            private set => SetProperty(ref _progress, value);
        }

        /// <summary>
        ///     23 kinds of different labels
        /// </summary>
        public string[] Data => new[]
        {
            "Building", "Grass", "Tree", "Cow", "Horse", "Sheep", "Sky", "Mountain", "Airplane", "Water", "Face", "Car",
            "Bicycle", "Flower", "Sign", "Bird", "Book", "Chair", "Road", "Cat", "Dog", "Body", "Boat"
        };

        /// <summary>
        ///     Get label in category based on index
        /// </summary>
        /// <param name="index">Label index</param>
        /// <returns>Return label if exists, otherwise return empty string</returns>
        public string Get(int index)
        {
            return index < Data.Length ? Data[index] : "";
        }

        public async Task UploadImagesAsync()
        {
            MaxValue = 1;
            Progress = 0;
            using (var db = new Context())
            {
                var imageBases = db.Images.Include(image => image.Tags);
                var unlabeledImageBases = imageBases.Where(image => image.Tags.Count <= 0).ToList();
                var folders = db.Folders.Select(folder => new Folder(folder)).ToList();

                MaxValue = unlabeledImageBases.Count + 1;
                foreach (var imageBase in unlabeledImageBases)
                {
                    Progress++;
                    var storageFile = await new Image(imageBase).TryGetStorageFileAsync(imageBase.Path, folders);
                    var fileSize = (await storageFile.GetBasicPropertiesAsync()).Size;
                    IRandomAccessStream stream;
                    if (fileSize > 300 * 1024)
                        stream = await storageFile.GetThumbnailAsync(ThumbnailMode.SingleItem, Constants.ThumbnailSize);
                    else
                        stream = await storageFile.OpenAsync(FileAccessMode.Read);
                    // Upload image to predict labels
                    Debug.WriteLine("File: " + imageBase.Path);
                    imageBase.Tags = (await PredictTagsAsync(stream)).ToList();
                    db.Images.Update(imageBase);
                    db.SaveChanges();
                    stream.CloneStream();
                }

                Progress++;
            }
        }

        /// <summary>
        ///     Predict tags for one image based on MGML
        /// </summary>
        private static async Task<IList<TagBase>> PredictTagsAsync(IInputStream stream)
        {
            using (var httpClient = new HttpClient())
            {
                using (var httpResponse =
                    await httpClient.PostAsync(new Uri("http://59.110.137.131:1696"), new HttpStreamContent(stream)))
                {
                    if (httpResponse.StatusCode != HttpStatusCode.Ok) throw new NetworkInformationException();
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    var trueContent = string.Join("", content.Split('\r', '\n').Skip(2));
                    Debug.WriteLine("Tags: " + trueContent);
                    var tagIndices = JsonConvert.DeserializeObject<IList<IList<int>>>(trueContent)
                        .First();
                    return tagIndices.Select(index => new TagBase {Name = new Mgml().Get(index)}).ToList();
                }
            }
        }
    }
}