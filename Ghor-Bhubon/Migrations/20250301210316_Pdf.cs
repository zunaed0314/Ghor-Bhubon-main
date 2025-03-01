using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ghor_Bhubon.Migrations
{
    /// <inheritdoc />
    public partial class Pdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyPendings",
                table: "PropertyPendings");

            migrationBuilder.RenameTable(
                name: "PropertyPendings",
                newName: "PropertyPending");

            migrationBuilder.RenameColumn(
                name: "PdfPath",
                table: "PropertyPending",
                newName: "PdfFilePath");

            migrationBuilder.RenameColumn(
                name: "PropertyPendingID",
                table: "PropertyPending",
                newName: "ID");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "PropertyPending",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "PropertyPending",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "PropertyPending",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyPending",
                table: "PropertyPending",
                column: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyPending",
                table: "PropertyPending");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "PropertyPending");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "PropertyPending");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "PropertyPending");

            migrationBuilder.RenameTable(
                name: "PropertyPending",
                newName: "PropertyPendings");

            migrationBuilder.RenameColumn(
                name: "PdfFilePath",
                table: "PropertyPendings",
                newName: "PdfPath");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "PropertyPendings",
                newName: "PropertyPendingID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyPendings",
                table: "PropertyPendings",
                column: "PropertyPendingID");
        }
    }
}
