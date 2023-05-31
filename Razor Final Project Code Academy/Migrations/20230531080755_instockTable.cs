using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Razor_Final_Project_Code_Academy.Migrations
{
    public partial class instockTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Accessories");

            migrationBuilder.AddColumn<bool>(
                name: "InStock",
                table: "Accessories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InStock",
                table: "Accessories");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Accessories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
