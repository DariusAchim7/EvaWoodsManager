using EvaWoods.Web.Components;
using EvaWoods.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using EvaWoods.Application.Clienti;
using EvaWoods.Domain.Abstractions;
using EvaWoods.Domain.Clienti;
using EvaWoods.Infrastructure.Clienti;
using EvaWoods.Infrastructure.Persistence;
using EvaWoods.Application.Proiecte;
using EvaWoods.Domain.Proiecte;
using EvaWoods.Infrastructure.Proiecte;
using EvaWoods.Application.Cheltuieli;
using EvaWoods.Domain.Cheltuieli;
using EvaWoods.Infrastructure.Cheltuieli;
using EvaWoods.Application.Produse;
using EvaWoods.Domain.Produse;
using EvaWoods.Infrastructure.Produse;
using EvaWoods.Application.Piese;
using EvaWoods.Domain.Piese;
using EvaWoods.Infrastructure.Piese;
using EvaWoods.Application.Materiale;
using EvaWoods.Domain.Materiale;
using EvaWoods.Infrastructure.Materiale;  
using EvaWoods.Application.Oferte;
using EvaWoods.Domain.Oferte;
using EvaWoods.Infrastructure.Oferte;  
using MudBlazor;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProiectRepository, ProiectRepository>();
builder.Services.AddScoped<IProiectService, ProiectService>();
builder.Services.AddScoped<IIstoricStatusProiectRepository, IstoricStatusProiectRepository>();
builder.Services.AddScoped<ISpatiuProiectRepository, SpatiuProiectRepository>();
builder.Services.AddScoped<INotaInternaProiectRepository, NotaInternaProiectRepository>();
builder.Services.AddScoped<ICheltuialaRepository, CheltuialaRepository>();
builder.Services.AddScoped<ICheltuialaService, CheltuialaService>();
builder.Services.AddScoped<IFurnizorRepository, FurnizorRepository>();
builder.Services.AddScoped<IProdusRepository, ProdusRepository>();
builder.Services.AddScoped<IProdusService, ProdusService>();
builder.Services.AddScoped<ICorpProiectRepository, CorpProiectRepository>();
builder.Services.AddScoped<IPiesaRepository, PiesaRepository>();
builder.Services.AddScoped<IPiesaService, PiesaService>();
builder.Services.AddScoped<ICalculMaterialRepository, CalculMaterialRepository>();
builder.Services.AddScoped<ICalculMaterialService, CalculMaterialService>();
builder.Services.AddScoped<ILinieOfertaRepository, LinieOfertaRepository>();
builder.Services.AddScoped<IOfertaService, OfertaService>();

var pgHost = Environment.GetEnvironmentVariable("PGHOST");
if (!string.IsNullOrEmpty(pgHost))
{
    var pgPort = Environment.GetEnvironmentVariable("PGPORT") ?? "5432";
    var pgDatabase = Environment.GetEnvironmentVariable("PGDATABASE");
    var pgUser = Environment.GetEnvironmentVariable("PGUSER");
    var pgPassword = Environment.GetEnvironmentVariable("PGPASSWORD");

    builder.Configuration["ConnectionStrings:DefaultConnection"] =
        $"Host={pgHost};Port={pgPort};Database={pgDatabase};Username={pgUser};Password={pgPassword};SSL Mode=Require;Trust Server Certificate=true";
}


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.AddAuthorization();

builder.Services.AddCascadingAuthenticationState();

MudGlobal.InputDefaults.Variant = Variant.Outlined;

var railwayPort = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(railwayPort))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{railwayPort}");
}

var app = builder.Build();


app.UseForwardedHeaders(new Microsoft.AspNetCore.Builder.ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true); 
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();