using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amazonBooks.Migrations
{
    /// <inheritdoc />
    public partial class AddPdfPathToBooksEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PdfPath",
                table: "BooksEntity",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfPath",
                table: "BooksEntity");
        }
    }
}
