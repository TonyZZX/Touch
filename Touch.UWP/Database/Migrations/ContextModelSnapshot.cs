#region

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#endregion

namespace Touch.Database.Migrations
{
    [DbContext(typeof(Context))]
    internal class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799");

            modelBuilder.Entity("Touch.Database.ImageBase", b =>
            {
                b.Property<Guid>("ImageBaseId")
                    .ValueGeneratedOnAdd();

                b.Property<DateTimeOffset>("DateModified");

                b.Property<DateTimeOffset>("DateTaken");

                b.Property<string>("District");

                b.Property<string>("FileType");

                b.Property<uint>("Height");

                b.Property<double?>("Latitude");

                b.Property<double?>("Longitude");

                b.Property<Guid?>("MemoryBaseId");

                b.Property<string>("Name");

                b.Property<string>("Path");

                b.Property<string>("Region");

                b.Property<ulong>("Size");

                b.Property<string>("Town");

                b.Property<uint>("Width");

                b.HasKey("ImageBaseId");

                b.HasIndex("MemoryBaseId");

                b.ToTable("Images");
            });

            modelBuilder.Entity("Touch.Database.MemoryBase", b =>
            {
                b.Property<Guid>("MemoryBaseId")
                    .ValueGeneratedOnAdd();

                b.Property<Guid?>("BgmFileStorageItemBaseId");

                b.Property<Guid?>("CoverImageImageBaseId");

                b.Property<string>("Name");

                b.HasKey("MemoryBaseId");

                b.HasIndex("BgmFileStorageItemBaseId");

                b.HasIndex("CoverImageImageBaseId");

                b.ToTable("Memories");
            });

            modelBuilder.Entity("Touch.Database.StorageItemBase", b =>
            {
                b.Property<Guid>("StorageItemBaseId")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Path");

                b.Property<string>("Token");

                b.HasKey("StorageItemBaseId");

                b.ToTable("Folders");
            });

            modelBuilder.Entity("Touch.Database.TagBase", b =>
            {
                b.Property<Guid>("TagBaseId")
                    .ValueGeneratedOnAdd();

                b.Property<Guid?>("ImageBaseId");

                b.Property<string>("Name");

                b.HasKey("TagBaseId");

                b.HasIndex("ImageBaseId");

                b.ToTable("Tags");
            });

            modelBuilder.Entity("Touch.Database.ImageBase", b =>
            {
                b.HasOne("Touch.Database.MemoryBase")
                    .WithMany("Images")
                    .HasForeignKey("MemoryBaseId");
            });

            modelBuilder.Entity("Touch.Database.MemoryBase", b =>
            {
                b.HasOne("Touch.Database.StorageItemBase", "BgmFile")
                    .WithMany()
                    .HasForeignKey("BgmFileStorageItemBaseId");

                b.HasOne("Touch.Database.ImageBase", "CoverImage")
                    .WithMany()
                    .HasForeignKey("CoverImageImageBaseId");
            });

            modelBuilder.Entity("Touch.Database.TagBase", b =>
            {
                b.HasOne("Touch.Database.ImageBase")
                    .WithMany("Tags")
                    .HasForeignKey("ImageBaseId");
            });
#pragma warning restore 612, 618
        }
    }
}