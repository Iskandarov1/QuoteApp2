using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quote.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoryImplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category",
                table: "Quote");

            migrationBuilder.AlterColumn<string>(
                name: "quote_text",
                table: "Quote",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(400)",
                oldMaxLength: 400);

            migrationBuilder.AddColumn<Guid>(
                name: "category_id",
                table: "Quote",
                type: "uuid",
                maxLength: 100,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "Quote",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriber",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    telegram_user = table.Column<long>(type: "bigint", nullable: true),
                    PreferredNotificationMethod = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriber", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quote_category_id",
                table: "Quote",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Category_category_name",
                table: "Category",
                column: "category_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_created_at",
                table: "Category",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriber_created_at",
                table: "Subscriber",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriber_email",
                table: "Subscriber",
                column: "email",
                unique: true,
                filter: "email IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriber_telegram_user",
                table: "Subscriber",
                column: "telegram_user",
                unique: true,
                filter: "telegram_user IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Quote_Category_category_id",
                table: "Quote",
                column: "category_id",
                principalTable: "Category",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quote_Category_category_id",
                table: "Quote");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Subscriber");

            migrationBuilder.DropIndex(
                name: "IX_Quote_category_id",
                table: "Quote");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "Quote");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "Quote");

            migrationBuilder.AlterColumn<string>(
                name: "quote_text",
                table: "Quote",
                type: "character varying(400)",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "Quote",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
