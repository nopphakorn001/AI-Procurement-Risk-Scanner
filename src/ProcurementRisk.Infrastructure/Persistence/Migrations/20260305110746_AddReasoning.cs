using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcurementRisk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddReasoning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reasoning",
                table: "Suppliers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reasoning",
                table: "Suppliers");
        }
    }
}
