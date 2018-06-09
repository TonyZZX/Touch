#region

using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Touch.Database
{
    public class TagBase : IEquatable<TagBase>
    {
        public TagBase()
        {
            TagBaseId = Guid.NewGuid();
        }

        [Key] public Guid TagBaseId { get; set; }

        public string Name { get; set; }

        public bool Equals(TagBase other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TagBase) obj);
        }

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }
    }
}