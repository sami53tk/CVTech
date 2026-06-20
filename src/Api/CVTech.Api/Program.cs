using System.Text;
using CVTech.ActualiteEtAbonnement;
using CVTech.Api.Middlewares;
using CVTech.AppelOffreFreelance;
using CVTech.BuildingBlocks.Evenements;
using CVTech.CatalogueEmploi;
using CVTech.GestionIdentite;
using CVTech.GestionIdentite.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// --- Répertoire des fichiers SQLite (un par module) ---
Directory.CreateDirectory(Path.Combine(builder.Environment.ContentRootPath, "data"));

// --- Bus d'événements interne (partagé par tous les modules) ---
builder.Services.AddSingleton<IBusEvenements, BusEvenementsEnMemoire>();

// --- Modules métier (ordre important : GestionIdentite expose IVerificateurPermission,
// ActualiteEtAbonnement expose IReferentielDomaineMetier) ---
builder.Services.AjouterModuleGestionIdentite(builder.Configuration);
builder.Services.AjouterModuleActualiteEtAbonnement(builder.Configuration);
builder.Services.AjouterModuleCatalogueEmploi(builder.Configuration);
builder.Services.AjouterModuleAppelOffreFreelance(builder.Configuration);

// --- Authentification JWT ---
var jwt = builder.Configuration.GetSection(OptionsJwt.Section);
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Emetteur"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["CleSecrete"]!)),
        };
    });

builder.Services.AddAuthorization();

// --- Gestion centralisée des exceptions métier ---
builder.Services.AddExceptionHandler<GestionnaireExceptionsGlobal>();
builder.Services.AddProblemDetails();

// --- Documentation API ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- CORS pour le frontend React (Vite dev server) ---
var origineFrontend = builder.Configuration["Cors:OrigineFrontend"] ?? "http://localhost:5173";
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy => policy
        .WithOrigins(origineFrontend)
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("Frontend");
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

// --- Routes des modules ---
app.MapperEndpointsGestionIdentite();
app.MapperEndpointsCatalogueEmploi();
app.MapperEndpointsAppelOffreFreelance();
app.MapperEndpointsActualiteEtAbonnement();

// --- Migrations & données de démarrage ---
await app.Services.MigrerGestionIdentiteAsync();
await app.Services.SeedGestionIdentiteAsync();
await app.Services.MigrerActualiteEtAbonnementAsync();
await app.Services.SeedActualiteEtAbonnementAsync();
await app.Services.MigrerCatalogueEmploiAsync();
await app.Services.MigrerAppelOffreFreelanceAsync();

app.Run();
