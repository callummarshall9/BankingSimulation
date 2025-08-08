using BankingSimulation.Barclays;
using BankingSimulation.BarclaysCard;
using BankingSimulation.Blazor;
using BankingSimulation.Blazor.Views;
using BankingSimulation.Blazor.ViewServices.Accounts;
using BankingSimulation.Blazor.ViewServices.Calendars;
using BankingSimulation.Blazor.ViewServices.Categories;
using BankingSimulation.Blazor.ViewServices.Categorys;
using BankingSimulation.Blazor.ViewServices.ImportData;
using BankingSimulation.Blazor.ViewServices.Roles;
using BankingSimulation.Blazor.ViewServices.Transactions;
using BankingSimulation.Data;
using BankingSimulation.MBNA;
using BankingSimulation.Nationwide;
using BankingSimulation.RBS;
using BankingSimulation.Services;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBankingSimulationData();
builder.Services.AddBankingSimulationServices();
builder.Services.AddBankingSimulationBarclaysServices();
builder.Services.AddBankingSimulationBarclaysCardServices();
builder.Services.AddBankingSimulationMBNAServices();
builder.Services.AddBankingSimulationNationwideServices();
builder.Services.AddBankingSimulationRBSServices();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped((provider) => {
    var httpContextAccessor = provider.GetService<IHttpContextAccessor>();

    return httpContextAccessor?.HttpContext?.User ?? new System.Security.Claims.ClaimsPrincipal(new DefaultAuthInfo());
});

var useSQLServer = builder.Configuration.GetSection("ConnectionStrings")
    .GetChildren()
    .Any(c => c.Key == "BankSimulationContextSQLServer");

builder.Services.AddDbContext<ODataContext>(opts =>
{
    if (useSQLServer)
        opts.UseSqlServer(builder.Configuration.GetConnectionString("BankSimulationSQLServer"));
    else
        opts.UseSqlite(builder.Configuration.GetConnectionString("BankingSimulationSQLite"));
});

builder.Services.AddScoped<IAccountViewService, AccountViewService>();
builder.Services.AddScoped<BankingSimulation.Blazor.ViewServices.Accounts.ICalendarViewService, BankingSimulation.Blazor.ViewServices.Accounts.CalendarViewService>();
builder.Services.AddScoped<IPeriodAccountSummaryViewService, PeriodAccountSummaryViewService>();

builder.Services.AddScoped<BankingSimulation.Blazor.ViewServices.Calendars.ICalendarViewService, BankingSimulation.Blazor.ViewServices.Calendars.CalendarViewService>();
builder.Services.AddScoped<IAddCalendarViewService, AddCalendarViewService>();
builder.Services.AddScoped<IAddCalendarEventViewService, AddCalendarEventViewService>();

builder.Services.AddScoped<IEditCalendarViewService, EditCalendarViewService>();
builder.Services.AddScoped<IComputeCalendarStatsViewService, ComputeCalendarStatsViewService>();

builder.Services.AddScoped<IImportDataAggregateService, ImportDataAggregateService>();
builder.Services.AddScoped<ITotalTransactionViewService, TotalTransactionViewService>();

builder.Services.AddScoped<IAddRoleViewService, AddRoleViewService>();
builder.Services.AddScoped<IEditRoleViewService, EditRoleViewService>();
builder.Services.AddScoped<IRoleViewService, RoleViewService>();

builder.Services.AddScoped<IAddCategoryViewService, AddCategoryViewService>();
builder.Services.AddScoped<IEditCategoryViewService, EditCategoryViewService>();
builder.Services.AddScoped<ICategoryViewService, CategoryViewService>();

//Force global authorization
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

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

using var scope = app.Services.CreateScope();

scope.ServiceProvider.GetService<ODataContext>()?.Database.EnsureCreated();

app.Run();
