using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LootGenerator.Migrations
{
    public partial class CollectionRecordIdNameChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CollectionRecords",
                newName: "RecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecordId",
                table: "CollectionRecords",
                newName: "Id");
        }
    }
}
