using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaAcademia_2._0.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaDiarioTreino : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrosTreino",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TreinoId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataTreino = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosTreino", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosTreino_Treinos_TreinoId",
                        column: x => x.TreinoId,
                        principalTable: "Treinos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosItens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RegistroTreinoId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExercicioId = table.Column<int>(type: "INTEGER", nullable: false),
                    Carga = table.Column<string>(type: "TEXT", nullable: false),
                    SeriesRealizadas = table.Column<string>(type: "TEXT", nullable: true),
                    RepeticoesRealizadas = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosItens_Exercicios_ExercicioId",
                        column: x => x.ExercicioId,
                        principalTable: "Exercicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistrosItens_RegistrosTreino_RegistroTreinoId",
                        column: x => x.RegistroTreinoId,
                        principalTable: "RegistrosTreino",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosItens_ExercicioId",
                table: "RegistrosItens",
                column: "ExercicioId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosItens_RegistroTreinoId",
                table: "RegistrosItens",
                column: "RegistroTreinoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosTreino_TreinoId",
                table: "RegistrosTreino",
                column: "TreinoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrosItens");

            migrationBuilder.DropTable(
                name: "RegistrosTreino");
        }
    }
}
