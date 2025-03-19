using System.Text.Json.Serialization;
using BankingSimulation.Data;
using BankingSimulation.Data.Brokers;
using BankingSimulation.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped((provider) => provider.GetService<IHttpContextAccessor>().HttpContext.User);
builder.Services.AddScoped<IAuthorisationBroker, AuthorisationBroker>();

builder.Services.AddAuthentication("bearer")
    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("bearer", opts => { });

builder.Services.AddAuthorization();

var useSQLServer = builder.Configuration.GetSection("ConnectionStrings")
    .GetChildren()
    .Any(c => c.Key == "BankSimulationContextSQLServer");

builder.Services.AddDbContext<BankSimulationContext>(opts =>
{
    if (useSQLServer)
        opts.UseSqlServer(builder.Configuration.GetConnectionString("BankSimulationSQLServer"));
    else
        opts.UseSqlite(builder.Configuration.GetConnectionString("BankingSimulationSQLite"));
});

builder.Services.AddDbContext<ODataContext>(opts =>
{
    if (useSQLServer)
        opts.UseSqlServer(builder.Configuration.GetConnectionString("BankSimulationSQLServer"));
    else
        opts.UseSqlite(builder.Configuration.GetConnectionString("BankingSimulationSQLite"));
});


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(jsonOptions => jsonOptions.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddOData((opts) => opts.EnableQueryFeatures(100));

ODataDependencyInstaller.RegisterOData(builder.Services);

BankingSimulationDependencyInstaller.AddServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

BankingSimulationDependencyInstaller.AddAccounts(app);
BankingSimulationDependencyInstaller.AddSet<AccountBankingSystemReference>(app);
BankingSimulationDependencyInstaller.AddSet<BankingSystem>(app);
BankingSimulationDependencyInstaller.AddCalendars(app);
BankingSimulationDependencyInstaller.AddCalendarEvents(app);
BankingSimulationDependencyInstaller.AddCategories(app);
BankingSimulationDependencyInstaller.AddCategoryKeywords(app);
BankingSimulationDependencyInstaller.AddRoles(app);
BankingSimulationDependencyInstaller.AddUserRoles(app);
BankingSimulationDependencyInstaller.AddTransactions(app);
BankingSimulationDependencyInstaller.AddTransactionTypes(app);

app.MapGet("/health/check", () => DateTimeOffset.UtcNow);

app.UseAuthentication();
app.UseAuthorization();

 app.UseCors(x => x
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowAnyHeader()
    .AllowCredentials()); // allow credentials

 using var scope = app.Services.CreateScope();

 scope.ServiceProvider.GetService<BankSimulationContext>().Database.EnsureCreated();

app.Run();


