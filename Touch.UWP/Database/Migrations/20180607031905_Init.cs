#region

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#endregion

namespace Touch.Database.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Folders",
                table => new
                {
                    StorageItemBaseId = table.Column<Guid>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Folders", x => x.StorageItemBaseId); });

            migrationBuilder.CreateTable(
                "Memories",
                table => new
                {
                    MemoryBaseId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CoverImageImageBaseId = table.Column<Guid>(nullable: true),
                    BgmFileStorageItemBaseId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memories", x => x.MemoryBaseId);
                    table.ForeignKey(
                        "FK_Memories_Folders_BgmFileStorageItemBaseId",
                        x => x.BgmFileStorageItemBaseId,
                        "Folders",
                        "StorageItemBaseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Images",
                table => new
                {
                    ImageBaseId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Size = table.Column<ulong>(nullable: false),
                    DateModified = table.Column<DateTimeOffset>(nullable: false),
                    Height = table.Column<uint>(nullable: false),
                    Width = table.Column<uint>(nullable: false),
                    DateTaken = table.Column<DateTimeOffset>(nullable: false),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Town = table.Column<string>(nullable: true),
                    District = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    MemoryBaseId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageBaseId);
                    table.ForeignKey(
                        "FK_Images_Memories_MemoryBaseId",
                        x => x.MemoryBaseId,
                        "Memories",
                        "MemoryBaseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Tags",
                table => new
                {
                    TagBaseId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ImageBaseId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagBaseId);
                    table.ForeignKey(
                        "FK_Tags_Images_ImageBaseId",
                        x => x.ImageBaseId,
                        "Images",
                        "ImageBaseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_Images_MemoryBaseId",
                "Images",
                "MemoryBaseId");

            migrationBuilder.CreateIndex(
                "IX_Memories_BgmFileStorageItemBaseId",
                "Memories",
                "BgmFileStorageItemBaseId");

            migrationBuilder.CreateIndex(
                "IX_Memories_CoverImageImageBaseId",
                "Memories",
                "CoverImageImageBaseId");

            migrationBuilder.CreateIndex(
                "IX_Tags_ImageBaseId",
                "Tags",
                "ImageBaseId");

            migrationBuilder.AddForeignKey(
                "FK_Memories_Images_CoverImageImageBaseId",
                "Memories",
                "CoverImageImageBaseId",
                "Images",
                principalColumn: "ImageBaseId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Images_Memories_MemoryBaseId",
                "Images");

            migrationBuilder.DropTable(
                "Tags");

            migrationBuilder.DropTable(
                "Memories");

            migrationBuilder.DropTable(
                "Folders");

            migrationBuilder.DropTable(
                "Images");
        }
    }
}