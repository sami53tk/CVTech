using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVTech.CatalogueEmploi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Annonces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Titre = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TypeContrat = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    DomaineMetierCode = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Localisation = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    EntrepriseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EstSupprimee = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreation = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annonces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Candidatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnnonceEmploiId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CandidatId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    DateCreation = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurriculumsVitae",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CandidatId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Titre = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Resume = table.Column<string>(type: "TEXT", nullable: false),
                    Competences = table.Column<string>(type: "TEXT", nullable: false),
                    DateDerniereModification = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    DateCreation = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumsVitae", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidatures_AnnonceEmploiId_CandidatId",
                table: "Candidatures",
                columns: new[] { "AnnonceEmploiId", "CandidatId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumsVitae_CandidatId",
                table: "CurriculumsVitae",
                column: "CandidatId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Annonces");

            migrationBuilder.DropTable(
                name: "Candidatures");

            migrationBuilder.DropTable(
                name: "CurriculumsVitae");
        }
    }
}
