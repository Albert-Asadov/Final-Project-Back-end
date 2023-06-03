using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Razor_Final_Project_Code_Academy.Migrations
{
    public partial class DeliveryInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryInformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Shipping = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AboutReturnRequest = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryInformations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryInformations");
        }
    }
}
