using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ReplacementRecords_EquipmentId",
                table: "ReplacementRecords",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplacementRecords_InventoryId",
                table: "ReplacementRecords",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReplacementRecord_Equipment",
                table: "ReplacementRecords",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReplacementRecord_Inventory",
                table: "ReplacementRecords",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReplacementRecord_Equipment",
                table: "ReplacementRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_ReplacementRecord_Inventory",
                table: "ReplacementRecords");

            migrationBuilder.DropIndex(
                name: "IX_ReplacementRecords_EquipmentId",
                table: "ReplacementRecords");

            migrationBuilder.DropIndex(
                name: "IX_ReplacementRecords_InventoryId",
                table: "ReplacementRecords");
        }
    }
}
