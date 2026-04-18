using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LotTrace_MES.Migrations
{
    /// <inheritdoc />
    public partial class Update_Lot_And_Line_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Products",
                newName: "ProductCode");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeNumber",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Lots",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentState",
                table: "Lines",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeNumber",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "Lines");

            migrationBuilder.RenameColumn(
                name: "ProductCode",
                table: "Products",
                newName: "ProductName");
        }
    }
}
