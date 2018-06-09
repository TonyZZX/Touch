#region

using System;
using System.Collections.Generic;
using System.Linq;
using Touch.Database;
using Touch.Helpers;

#endregion

namespace Touch.Models
{
    public class Memory : PropertyChangedHelper, IEquatable<Memory>
    {
        private StorageItemBase _bgmFile;
        private Image _coverImage;
        private Guid _id;
        private IList<Image> _images;
        private string _name;

        public Memory()
        {
        }

        public Memory(MemoryBase memory)
        {
            _id = memory.MemoryBaseId;
            Name = memory.Name;
            CoverImage = new Image(memory.CoverImage);
            BgmFile = memory.BgmFile;
            Images = memory.Images == null
                ? new List<Image>()
                : memory.Images.Select(image => new Image(image)).ToList();
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public Image CoverImage
        {
            get => _coverImage;
            set => SetProperty(ref _coverImage, value);
        }

        public StorageItemBase BgmFile
        {
            get => _bgmFile;
            set => SetProperty(ref _bgmFile, value);
        }

        public IList<Image> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }

        public bool Equals(Memory other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || _id.Equals(other._id);
        }

        public MemoryBase ToMemoryBase()
        {
            var memory = new MemoryBase
            {
                Name = Name,
                CoverImage = CoverImage.ToImageBase(),
                BgmFile = BgmFile,
                Images = Images == null ? new List<ImageBase>() : Images.Select(image => image.ToImageBase()).ToList()
            };
            if (_id != Guid.Empty) memory.MemoryBaseId = _id;
            return memory;
        }
    }
}