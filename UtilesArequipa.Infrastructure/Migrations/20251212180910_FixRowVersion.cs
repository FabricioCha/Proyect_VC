using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtilesArequipa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_kit_items_kit_id",
                table: "kit_items");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "products",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "kit_items",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_kit_items_kit_id_product_id",
                table: "kit_items",
                columns: new[] { "kit_id", "product_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_kit_items_kit_id_product_id",
                table: "kit_items");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "products");

            migrationBuilder.AlterColumn<int>(
                name: "quantity",
                table: "kit_items",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_kit_items_kit_id",
                table: "kit_items",
                column: "kit_id");
        }
    }
}
