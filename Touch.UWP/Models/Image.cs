#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml.Media.Imaging;
using Touch.Database;
using Touch.Helpers;

#endregion

namespace Touch.Models
{
    public class Image : PropertyChangedHelper, IEquatable<Image>
    {
        private BitmapImage _originalImage;
        private IList<Tag> _tags;

        public Image()
        {
        }

        public Image(ImageBase image)
        {
            Id = image.ImageBaseId;
            Name = image.Name;
            FileType = image.FileType;
            Path = image.Path;
            Size = image.Size;
            DateModified = image.DateModified;
            Height = image.Height;
            Width = image.Width;
            DateTaken = image.DateTaken;
            Latitude = image.Latitude;
            Longitude = image.Longitude;
            Town = image.Town;
            District = image.District;
            Region = image.Region;
            Tags = image.Tags == null ? new List<Tag>() : image.Tags.Select(tag => new Tag(tag)).ToList();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FileType { get; set; }

        public string Path { get; set; }

        public ulong Size { get; set; }

        public DateTimeOffset DateModified { get; set; }

        public uint Height { get; set; }

        public uint Width { get; set; }

        public DateTimeOffset DateTaken { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public string Town { get; set; }

        public string District { get; set; }

        public string Region { get; set; }

        public IList<Tag> Tags
        {
            get => _tags;
            set => SetProperty(ref _tags, value);
        }

        // Most time it is the date that photo was took.
        // If the taken time is empty (the year in taken date is smaller than 1601),
        // it will be the date that file was last modified.
        public DateTimeOffset DisplayDate => DateTaken.Year <= 1601 ? DateModified : DateTaken;

        public MonthYear MonthYearDate => new MonthYear(DisplayDate);

        public string Place => Town + ", " + Region;

        public string ThumbnailSource => "ms-appdata:///local/" + Id;

        public BitmapImage ThumbnailImage => new BitmapImage(new Uri(ThumbnailSource));

        public BitmapImage OriginalImage
        {
            get => _originalImage;
            private set => SetProperty(ref _originalImage, value);
        }

        public bool Equals(Image other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Path, other.Path) && Size == other.Size && DateModified.Equals(other.DateModified) &&
                   Height == other.Height && Width == other.Width && DateTaken.Equals(other.DateTaken) &&
                   Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        public ImageBase ToImageBase()
        {
            var image = new ImageBase
            {
                Name = Name,
                FileType = FileType,
                Path = Path,
                Size = Size,
                DateModified = DateModified,
                Height = Height,
                Width = Width,
                DateTaken = DateTaken,
                Latitude = Latitude,
                Longitude = Longitude,
                Town = Town,
                District = District,
                Region = Region,
                Tags = Tags == null ? new List<TagBase>() : Tags.Select(tag => tag.ToTagBase()).ToList()
            };
            if (Id != Guid.Empty) image.ImageBaseId = Id;
            return image;
        }

        public bool IfContainsTag(string tagName)
        {
            return Tags != null && Tags.Any(tag => tag.Name == tagName);
        }

        public bool IfContainsTags(IList<string> tagNames)
        {
            return tagNames != null &&
                   !tagNames.Except(Tags.Select(tag => tag.Name), StringComparer.OrdinalIgnoreCase).Any();
        }

        public async Task SetOriginalImageAsync(IList<Folder> folders)
        {
            if (OriginalImage != null) return;
            var storageFile = await TryGetStorageFileAsync(Path, folders);
            if (storageFile != null)
                using (var stream = await storageFile.OpenAsync(FileAccessMode.Read))
                {
                    OriginalImage = new BitmapImage();
                    await OriginalImage.SetSourceAsync(stream);
                }
            else
                throw new FileNotFoundException();
        }

        /// <summary>
        ///     Get <see cref="StorageFile" /> from folder list based on file path.
        /// </summary>
        public async Task<StorageFile> TryGetStorageFileAsync(string filePath, IList<Folder> folders)
        {
            Folder fileFolder = null;
            var relativePath = "";
            foreach (var folder in folders)
            {
                relativePath = folder.GetRelativePath(filePath);
                if (relativePath == "") continue;
                fileFolder = folder;
                break;
            }

            if (fileFolder == null || relativePath == "") return null;
            var storageFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(fileFolder.Token);
            var storageFile = await storageFolder.GetFileAsync(relativePath);
            return storageFile;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Image) obj);
        }

        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            unchecked
            {
                var hashCode = Path != null ? Path.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ Size.GetHashCode();
                hashCode = (hashCode * 397) ^ DateModified.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Height;
                hashCode = (hashCode * 397) ^ (int) Width;
                hashCode = (hashCode * 397) ^ DateTaken.GetHashCode();
                hashCode = (hashCode * 397) ^ Latitude.GetHashCode();
                hashCode = (hashCode * 397) ^ Longitude.GetHashCode();
                return hashCode;
            }
        }
    }
}