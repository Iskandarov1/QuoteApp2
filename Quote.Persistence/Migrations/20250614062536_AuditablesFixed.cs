using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quote.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AuditablesFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriber_email",
                table: "Subscriber");

            migrationBuilder.DropIndex(
                name: "IX_Subscriber_telegram_user",
                table: "Subscriber");

            migrationBuilder.DropIndex(
                name: "IX_Quote_is_deleted",
                table: "Quote");

            migrationBuilder.DropIndex(
                name: "IX_Category_is_deleted",
                table: "Category");

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "Subscriber",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "Subscriber",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "Quote",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "Category",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriber_email",
                table: "Subscriber",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriber_telegram_user",
                table: "Subscriber",
                column: "telegram_user",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriber_email",
                table: "Subscriber");

            migrationBuilder.DropIndex(
                name: "IX_Subscriber_telegram_user",
                table: "Subscriber");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "Subscriber");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "Quote");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "Category");

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "Subscriber",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

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

            migrationBuilder.CreateIndex(
                name: "IX_Quote_is_deleted",
                table: "Quote",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Category_is_deleted",
                table: "Category",
                column: "is_deleted");
        }
    }
}
