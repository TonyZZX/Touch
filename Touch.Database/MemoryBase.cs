#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Touch.Database
{
    public class MemoryBase : IEquatable<MemoryBase>
    {
        public MemoryBase()
        {
            MemoryBaseId = Guid.NewGuid();
        }

        [Key] public Guid MemoryBaseId { get; set; }

        public string Name { get; set; }

        public ImageBase CoverImage { get; set; }

        public StorageItemBase BgmFile { get; set; }

        public IList<ImageBase> Images { get; set; }

        public bool Equals(MemoryBase other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || MemoryBaseId.Equals(other.MemoryBaseId);
        }
    }
}