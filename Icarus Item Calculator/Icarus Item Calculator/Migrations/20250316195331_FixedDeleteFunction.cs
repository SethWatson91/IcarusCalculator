using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icarus_Item_Calculator.Migrations
{
    /// <inheritdoc />
    public partial class FixedDeleteFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeItems_Items_ItemId",
                table: "RecipeItems");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeItems_Items_ItemId",
                table: "RecipeItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeItems_Items_ItemId",
                table: "RecipeItems");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeItems_Items_ItemId",
                table: "RecipeItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
