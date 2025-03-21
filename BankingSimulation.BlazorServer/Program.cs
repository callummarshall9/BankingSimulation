using System.Security.Claims;
using System.Text.Json.Serialization;
using ApexCharts;
using BankingSimulation.BlazorServer.Views;
using BankingSimulation.Data;
using BankingSimulation.Data.Brokers;
using BankingSimulation.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ClaimsPrincipal>(provider => provider.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? new ClaimsPrincipal());
builder.Services.AddScoped<IAuthorisationBroker, AuthorisationBroker>();

builder.Services.AddAuthentication("bearer")
    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("bearer", opts => { });

builder.Services.AddAuthorization();

var useSQLServer = builder.Configuration.GetSection("ConnectionStrings")
    .GetChildren()
    .Any(c => c.Key == "BankSimulationSQLServer");

builder.Services.AddDbContext<BankSimulationContext>(opts =>
{
    if (useSQLServer)
        opts.UseSqlServer(builder.Configuration.GetConnectionString("BankSimulationSQLServer"));
    else
        opts.UseSqlite(builder.Configuration.GetConnectionString("BankingSimulationSQLite"))
            .EnableSensitiveDataLogging();
});

builder.Services.AddDbContext<ODataContext>(opts =>
{
    if (useSQLServer)
        opts.UseSqlServer(builder.Configuration.GetConnectionString("BankSimulationSQLServer"));
    else
        opts.UseSqlite(builder.Configuration.GetConnectionString("BankingSimulationSQLite"))
            .EnableSensitiveDataLogging();
});


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddApexCharts();
builder.Services.AddBlazorBootstrap();
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureHttpJsonOptions(jsonOptions => jsonOptions.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddOData((opts) => opts.EnableQueryFeatures(100));

ODataDependencyInstaller.RegisterOData(builder.Services);

BankingSimulationUIDependencyInstaller.AddServices(builder.Services);
BankingSimulationDependencyInstaller.AddServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/health/check", () => DateTimeOffset.UtcNow);

app.UseAuthentication();
app.UseAuthorization();

 app.UseCors(x => x
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowAnyHeader()
    .AllowCredentials()); // allow credentials

 using var scope = app.Services.CreateScope();

 scope.ServiceProvider.GetService<BankSimulationContext>()?.Database.EnsureCreated();

app.Run();


