using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ghor_Bhubon.Migrations
{
    /// <inheritdoc />
    public partial class bhaihoiseee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LandlordID",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LandlordID",
                table: "Transactions");
        }
    }
}
