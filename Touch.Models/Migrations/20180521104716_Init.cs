#region

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#endregion

namespace Touch.Models.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Folders",
                table => new
                {
                    Path = table.Column<string>(nullable: false),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Folders", x => x.Path); });

            migrationBuilder.CreateTable(
                "Images",
                table => new
                {
                    Path = table.Column<string>(nullable: false),
                    Size = table.Column<ulong>(nullable: false),
                    DateModified = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Images", x => new {x.Path, x.Size, x.DateModified}); });

            migrationBuilder.CreateTable(
                "Labels",
                table => new
                {
                    LabelId = table.Column<Guid>(nullable: false),
                    ImageDateModified = table.Column<DateTimeOffset>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    ImageSize = table.Column<ulong>(nullable: true),
                    Index = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labels", x => x.LabelId);
                    table.ForeignKey(
                        "FK_Labels_Images_ImagePath_ImageSize_ImageDateModified",
                        x => new {x.ImagePath, x.ImageSize, x.ImageDateModified},
                        "Images",
                        new[] {"Path", "Size", "DateModified"},
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_Labels_ImagePath_ImageSize_ImageDateModified",
                "Labels",
                new[] {"ImagePath", "ImageSize", "ImageDateModified"});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Folders");

            migrationBuilder.DropTable(
                "Labels");

            migrationBuilder.DropTable(
                "Images");
        }
    }
}