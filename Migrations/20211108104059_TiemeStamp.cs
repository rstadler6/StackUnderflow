using Microsoft.EntityFrameworkCore.Migrations;

namespace StackUnderflow.Migrations
{
    public partial class TiemeStamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TiemeStamp",
                table: "Comments",
                newName: "TimeStamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Comments",
                newName: "TiemeStamp");
        }
    }
}
