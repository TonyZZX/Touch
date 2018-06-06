#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Touch.Models
{
    public class Memory
    {
        public Memory()
        {
            LabelId = Guid.NewGuid();
        }

        [Key] public Guid LabelId { get; set; }

        public string Name { get; set; }

        public IList<Image> Images { get; set; }
    }
}