using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaAcademia_2._0.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaSeries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Series",
                table: "ItensTreino",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Series",
                table: "ItensTreino");
        }
    }
}
