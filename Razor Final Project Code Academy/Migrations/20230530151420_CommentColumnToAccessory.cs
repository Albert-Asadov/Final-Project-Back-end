using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Razor_Final_Project_Code_Academy.Migrations
{
    public partial class CommentColumnToAccessory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessoryId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AccessoryId",
                table: "Comments",
                column: "AccessoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Accessories_AccessoryId",
                table: "Comments",
                column: "AccessoryId",
                principalTable: "Accessories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Accessories_AccessoryId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AccessoryId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AccessoryId",
                table: "Comments");
        }
    }
}
