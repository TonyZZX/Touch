#region

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#endregion

namespace Touch.Models.Migrations
{
    [DbContext(typeof(Database))]
    internal class DatabaseModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

            modelBuilder.Entity("Touch.Models.Folder", b =>
            {
                b.Property<string>("Path")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Token");

                b.HasKey("Path");

                b.ToTable("Folders");
            });

            modelBuilder.Entity("Touch.Models.Image", b =>
            {
                b.Property<string>("Path");

                b.Property<ulong>("Size");

                b.Property<DateTimeOffset>("Date");

                b.HasKey("Path", "Size", "Date");

                b.ToTable("Images");
            });

            modelBuilder.Entity("Touch.Models.Label", b =>
            {
                b.Property<Guid>("LabelId")
                    .ValueGeneratedOnAdd();

                b.Property<DateTimeOffset?>("ImageDate");

                b.Property<string>("ImagePath");

                b.Property<ulong?>("ImageSize");

                b.Property<int>("Index");

                b.HasKey("LabelId");

                b.HasIndex("ImagePath", "ImageSize", "ImageDate");

                b.ToTable("Labels");
            });

            modelBuilder.Entity("Touch.Models.Label", b =>
            {
                b.HasOne("Touch.Models.Image")
                    .WithMany("Labels")
                    .HasForeignKey("ImagePath", "ImageSize", "ImageDate");
            });
#pragma warning restore 612, 618
        }
    }
}