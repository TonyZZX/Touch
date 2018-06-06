#region

using Microsoft.EntityFrameworkCore;

#endregion

namespace Touch.Models
{
    /// <summary>
    ///     Database context using <see cref="Microsoft.EntityFrameworkCore" />
    /// </summary>
    public class Database : DbContext
    {
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Label> Labels { get; set; }

        /// <summary>
        ///     Override this method to configure the database (and other options) to be used for this context.
        ///     This method is called for each instance of the context that is created.
        /// </summary>
        /// <param name="optionsBuilder">
        ///     A builder used to create or modify options for this context. Databases (and other extensions)
        ///     typically define extension methods on this object that allow you to configure the context.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=Touch.db");
        }

        /// <summary>
        ///     Override this method to further configure the model that was discovered by convention from the entity types exposed
        ///     in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context.
        /// </summary>
        /// <param name="modelBuilder">
        ///     The builder being used to construct the model for this context. Databases (and other extensions)
        ///     typically define extension methods on this object that allow you to configure aspects of the model that are
        ///     specific to a given database.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Image>().HasKey(image => new {image.Path, image.Size, image.Date});
        }
    }
}