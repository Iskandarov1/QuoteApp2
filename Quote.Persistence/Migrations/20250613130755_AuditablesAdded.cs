using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quote.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AuditablesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "Subscriber",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "Quote",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "Category",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Quote_is_deleted",
                table: "Quote",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Category_is_deleted",
                table: "Category",
                column: "is_deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quote_is_deleted",
                table: "Quote");

            migrationBuilder.DropIndex(
                name: "IX_Category_is_deleted",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "Subscriber");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "Quote");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "Category");
        }
    }
}
