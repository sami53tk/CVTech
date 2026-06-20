using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVTech.ActualiteEtAbonnement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abonnements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UtilisateurId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DomaineMetierCode = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    DateCreation = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Titre = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Contenu = table.Column<string>(type: "TEXT", nullable: false),
                    DomaineMetierCode = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    AuteurId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LienExterne = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    DateCreation = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DomainesMetier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Libelle = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    DateCreation = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainesMetier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UtilisateurId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    DomaineMetierCode = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    TypeSource = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    SourceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EstLue = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreation = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abonnements_UtilisateurId_DomaineMetierCode",
                table: "Abonnements",
                columns: new[] { "UtilisateurId", "DomaineMetierCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DomainesMetier_Code",
                table: "DomainesMetier",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abonnements");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "DomainesMetier");

            migrationBuilder.DropTable(
                name: "Notifications");
        }
    }
}
