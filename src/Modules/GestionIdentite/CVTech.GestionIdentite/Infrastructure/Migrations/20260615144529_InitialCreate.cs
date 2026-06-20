using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVTech.GestionIdentite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    MotDePasseHache = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    EstBloque = table.Column<bool>(type: "INTEGER", nullable: false),
                    TypeUtilisateur = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    Administrateur_Nom = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Nom = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Prenom = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    RaisonSociale = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    DateCreation = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_Email",
                table: "Utilisateurs",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Utilisateurs");
        }
    }
}
