#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Touch.Models
{
    /// <summary>
    ///     Image label
    /// </summary>
    public class Label
    {
        /// <summary>
        ///     Image label
        /// </summary>
        public Label()
        {
            // Auto-generate distinct key
            LabelId = Guid.NewGuid();
        }

        /// <summary>
        ///     Primary key for database
        /// </summary>
        [Key]
        public Guid LabelId { get; set; }

        /// <summary>
        ///     Label index in category
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     Label name in category
        /// </summary>
        [NotMapped]
        public string Name => new Category().Get(Index);
    }
}