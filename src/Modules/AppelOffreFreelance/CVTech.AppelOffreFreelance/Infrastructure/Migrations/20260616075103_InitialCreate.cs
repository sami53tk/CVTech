using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVTech.AppelOffreFreelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppelsOffres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Titre = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DomaineMetierCode = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Localisation = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    BudgetMax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EntrepriseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EstSupprime = table.Column<bool>(type: "INTEGER", nullable: false),
                    LaureatId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DateCreation = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppelsOffres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Propositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AppelOffreId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FreelanceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TauxJournalier = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DureeEstimeeJours = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreation = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propositions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Propositions_AppelOffreId_FreelanceId",
                table: "Propositions",
                columns: new[] { "AppelOffreId", "FreelanceId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppelsOffres");

            migrationBuilder.DropTable(
                name: "Propositions");
        }
    }
}
