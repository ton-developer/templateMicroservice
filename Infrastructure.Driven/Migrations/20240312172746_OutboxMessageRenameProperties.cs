using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Driven.Migrations
{
    /// <inheritdoc />
    public partial class OutboxMessageRenameProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoutingKey",
                table: "outbox_messages",
                newName: "AggregateName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AggregateName",
                table: "outbox_messages",
                newName: "RoutingKey");
        }
    }
}
