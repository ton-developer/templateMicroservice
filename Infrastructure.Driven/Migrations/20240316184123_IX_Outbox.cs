using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Driven.Migrations
{
    /// <inheritdoc />
    public partial class IX_Outbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_outbox_messages_processed_on_utc_null",
                table: "outbox_messages",
                column: "ProcessedOnUtc",
                filter: " \"ProcessedOnUtc\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_outbox_messages_processed_on_utc_null",
                table: "outbox_messages");
        }
    }
}
