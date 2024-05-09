using BankingSimulation.Authentication;
using BankingSimulation.Data;
using BankingSimulation.Data.Brokers;
using BankingSimulation.Data.Models;
using BankingSimulation.RBS;
using BankingSimulation.Services;
using BankingSimulation.Services.Processing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;


IEdmModel GetModel()
{
    var builder = new ODataConventionModelBuilder();

    builder.EntitySet<Account>("Accounts");
    builder.EntitySet<AccountBankingSystemReference>("AccountBankingSystemReferences").EntityType.HasKey(t => new { t.BankingSystemId, t.AccountId });
    builder.EntitySet<BankingSystem>("BankingSystems");
    builder.EntitySet<Calendar>("Calendars");
    builder.EntitySet<CalendarEvent>("CalendarEvents");
    builder.EntitySet<Category>("Categories");
    builder.EntitySet<CategoryKeyword>("CategoryKeywords");
    builder.EntitySet<Role>("Roles");
    builder.EntitySet<UserRole>("UserRoles").EntityType.HasKey(ur => new { ur.UserId, ur.RoleId });
    builder.EntitySet<Transaction>("Transactions");
    builder.EntitySet<TransactionType>("TransactionTypes");

    return builder.GetEdmModel();
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("bearer")
    .AddScheme<AuthenticationSchemeOptions, SSOAuthenticationHandler>("bearer", opts => { });

builder.Services.AddScoped((provider) => provider.GetService<IHttpContextAccessor>().HttpContext.User);
builder.Services.AddScoped<IAuthorisationBroker, AuthorisationBroker>();

builder.Services.AddAuthorization();

builder.Services.AddDbContext<BankSimulationContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("BankSimulationContext")));

builder.Services.AddDbContext<ODataContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("BankSimulationContext")));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(jsonOptions => jsonOptions.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddOData((opts) => opts.EnableQueryFeatures(100));

var model = GetModel();

builder.Services.AddODataOptions<Account>(model);
builder.Services.AddODataOptions<AccountBankingSystemReference>(model);
builder.Services.AddODataOptions<BankingSystem>(model);
builder.Services.AddODataOptions<Calendar>(model);
builder.Services.AddODataOptions<CalendarEvent>(model);
builder.Services.AddODataOptions<Category>(model);
builder.Services.AddODataOptions<CategoryKeyword>(model);
builder.Services.AddODataOptions<Role>(model);
builder.Services.AddODataOptions<UserRole>(model);
builder.Services.AddODataOptions<Transaction>(model);
builder.Services.AddODataOptions<TransactionType>(model);

builder.Services.AddBankingSimulationServices();
builder.Services.AddBankingSimulationRBSServices();
builder.Services.AddBankingSimulationData();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

AddAccounts(app);
AddSet<AccountBankingSystemReference>(app);
AddSet<BankingSystem>(app);
AddCalendars(app);
AddCalendarEvents(app);
AddCategories(app);
AddCategoryKeywords(app);
AddRoles(app);
AddUserRoles(app);
AddTransactions(app);
AddTransactionTypes(app);

app.MapGet("/health/check", () => DateTimeOffset.UtcNow);

 app.UseCors(x => x
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowAnyHeader()
    .AllowCredentials()); // allow credentials

app.Run();

object HandleOData(IEnumerable result)
{
    if (result is ISelectExpandWrapper castedWrapper)
    {
        var newDictionary = new Dictionary<string, object>();

        var dictionaryResult = castedWrapper.ToDictionary();

        foreach (var (key, value) in dictionaryResult)
        {
            string actualKey = System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(key);

            newDictionary[actualKey] = value;

            if (value is IEnumerable<ISelectExpandWrapper> castedValue)
                newDictionary[actualKey] = HandleOData(castedValue);

            if (value is ISelectExpandWrapper selectExpandWrapper)
                newDictionary[actualKey] = HandleOData(new[] { selectExpandWrapper }.ToList().AsQueryable());
        }


        return newDictionary;
    }

    if (result is IEnumerable<ISelectExpandWrapper>)
    {
        var results = new List<IDictionary<string, object>>();

        var entities = (result as IEnumerable<ISelectExpandWrapper>).ToList();

        foreach(var entity in entities)
        {
            var newDictionary = new Dictionary<string, object>();

            var dictionaryResult = entity.ToDictionary();

            foreach(var (key, value) in dictionaryResult)
            {
                string actualKey = System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(key);

                newDictionary[actualKey] = value;

                if (value is IEnumerable<ISelectExpandWrapper> castedValue)
                    newDictionary[actualKey] = HandleOData(castedValue);

                if (value is ISelectExpandWrapper expandWrapper)
                    newDictionary[actualKey] = HandleODataDictionary(expandWrapper);
            }

            results.Add(newDictionary);
        }

        return results;
    }

    return result;
}

object HandleODataDictionary(ISelectExpandWrapper castedWrapper)
{
    var newDictionary = new Dictionary<string, object>();

    var dictionaryResult = castedWrapper.ToDictionary();

    foreach (var (key, value) in dictionaryResult)
    {
        string actualKey = System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(key);

        newDictionary[actualKey] = value;

        if (value is IEnumerable<ISelectExpandWrapper> castedValue)
            newDictionary[actualKey] = HandleOData(castedValue);

        if (value is ISelectExpandWrapper expandWrapper)
            newDictionary[actualKey] = HandleODataDictionary(expandWrapper);
    }


    return newDictionary;
}

void AddSet<T>(WebApplication app) where T : class
{
    app.MapPost($"/{typeof(T).Name}s", ([FromServices] IFoundationService foundationService, [FromBody] T entity) 
        => foundationService.AddAsync<T>(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapPut($"/{typeof(T).Name}s", ([FromServices] IFoundationService foundationService, [FromBody] T entity) 
        => foundationService.UpdateAsync<T>(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapDelete($"/{typeof(T).Name}s", ([FromServices] IFoundationService foundationService, [FromBody] T entity) 
        => foundationService.DeleteAsync<T>(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/{typeof(T).Name}s", ([FromServices] IFoundationService foundationService, [FromServices] ODataQueryOptions<T> options)
        => {
            var result = foundationService.GetAll<T>();

            var appliedResult = options.ApplyTo(result);

            return appliedResult;
        }
    )
        .WithOpenApi()
        .RequireAuthorization();
}

void AddAccounts(WebApplication app)
{
    app.MapPost($"/Accounts", ([FromServices] IAccountProcessingService service, [FromBody] Account entity)
        => service.AddAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapPost("/Accounts/ImportRBS",
        async ([FromServices] IHttpContextAccessor context, [FromServices] IRBSOrchestrationService rbsOrchestrationService)
            => {
                var requestBody = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
                await rbsOrchestrationService.ImportAccountsFromRawDataAsync(requestBody);
                return "";
            }).WithOpenApi((operation) => new(operation)
            {
                RequestBody = new()
                {
                    Required = true,
                    Content = new Dictionary<string, OpenApiMediaType>()
                    {
                        ["text/csv"] = new()
                        {
                            Schema = new OpenApiSchema()
                            {
                                Type = "text/csv",
                                Example = new OpenApiString(@"Date,Type,Description,Value,Balance,Account Name,Account Number
19 Dec 2022,DPC,""Test Account"",2.01,432.38,AccountReference1,AccountNumber1")
                            }
                        }
                    }
                },
                Summary = "Import Accounts",
                Description = "Import Accounts from CSV for RBS Transaction Statements"
            })
        .WithOpenApi()
        .RequireAuthorization();

    app.MapPut($"/Accounts", ([FromServices] IAccountProcessingService service, [FromBody] Account entity)
        => service.UpdateAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapDelete($"/Accounts", ([FromServices] IAccountProcessingService service, [FromBody] Account entity)
        => service.DeleteAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/Accounts", ([FromServices] IAccountProcessingService service, [FromServices] ODataQueryOptions<Account> options)
        => HandleOData(options.ApplyTo(service.GetAll())))
        .WithOpenApi()
        .RequireAuthorization();
}

void AddCategories(WebApplication app)
{
    app.MapPost($"/Categories", ([FromServices] ICategoryProcessingService service, [FromBody] Category entity)
        => service.AddAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapPut($"/Categories", ([FromServices] ICategoryProcessingService service, [FromBody] Category entity)
        => service.UpdateAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapDelete($"/Categories", ([FromServices] ICategoryProcessingService service, [FromBody] Category entity)
        => service.DeleteAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/Categories", ([FromServices] ICategoryProcessingService service, [FromServices] ODataQueryOptions<Category> options)
        => HandleOData(options.ApplyTo(service.GetAll())))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/Categories/ForPeriod", ([FromServices] ICategoryProcessingService service, DateOnly fromPeriod, DateOnly toPeriod)
        => service.ForPeriod(fromPeriod, toPeriod))
        .WithOpenApi()
        .RequireAuthorization();
}

void AddCategoryKeywords(WebApplication app)
{
    app.MapPost($"/CategoryKeywords", ([FromServices] ICategoryKeywordsProcessingService service, [FromBody] CategoryKeyword entity)
        => service.AddAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapPut($"/CategoryKeywords", ([FromServices] ICategoryKeywordsProcessingService service, [FromBody] CategoryKeyword entity)
        => service.UpdateAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapDelete($"/CategoryKeywords", ([FromServices] ICategoryKeywordsProcessingService service, [FromBody] CategoryKeyword entity)
        => service.DeleteAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/CategoryKeywords", ([FromServices] ICategoryKeywordsProcessingService service, [FromServices] ODataQueryOptions<CategoryKeyword> options)
        => HandleOData(options.ApplyTo(service.GetAll())))
        .WithOpenApi()
        .RequireAuthorization();
}

void AddRoles(WebApplication app)
{
    app.MapPost($"/Roles", ([FromServices] IRoleProcessingService service, [FromBody] Role entity)
        => service.AddAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapPut($"/Roles", ([FromServices] IRoleProcessingService service, [FromBody] Role entity)
        => service.UpdateAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapDelete($"/Roles", ([FromServices] IRoleProcessingService service, [FromBody] Role entity)
        => service.DeleteAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/Roles", ([FromServices] IRoleProcessingService service, [FromServices] ODataQueryOptions<Role> options)
        => HandleOData(options.ApplyTo(service.GetAll())))
        .WithOpenApi()
        .RequireAuthorization();
}

void AddCalendars(WebApplication app)
{
    app.MapPost($"/Calendars", ([FromServices] ICalendarProcessingService service, [FromBody] Calendar entity)
        => service.AddAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapPut($"/Calendars", ([FromServices] ICalendarProcessingService service, [FromBody] Calendar entity)
        => service.UpdateAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapDelete($"/Calendars", ([FromServices] ICalendarProcessingService service, [FromBody] Calendar entity)
        => service.DeleteAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/Calendars", ([FromServices] ICalendarProcessingService service, [FromServices] ODataQueryOptions<Calendar> options)
        => HandleOData(options.ApplyTo(service.GetAll())))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/Calendars/ComputeCalendarStats", ([FromServices] ICalendarProcessingService service, Guid calendarId)
        => service.ComputeCalendarCategoryStats(calendarId))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/Calendars/ComputeCalendarAccountStats", ([FromServices] ICalendarProcessingService service, 
        Guid calendarId,
        string accountIds)
        => service.ComputeCalendarCategoryStatsForAccounts(calendarId, accountIds))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/Calendars/ComputeNetCalendarStats", ([FromServices] ICalendarProcessingService service, Guid calendarId)
        => service.ComputeNetCalendarStats(calendarId))
        .WithOpenApi()
        .RequireAuthorization();
}

void AddCalendarEvents(WebApplication app)
{
    app.MapPost($"/CalendarEvents", ([FromServices] ICalendarEventProcessingService service, [FromBody] CalendarEvent entity)
        => service.AddAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapPut($"/CalendarEvents", ([FromServices] ICalendarEventProcessingService service, [FromBody] CalendarEvent entity)
        => service.UpdateAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapDelete($"/CalendarEvents", ([FromServices] ICalendarEventProcessingService service, [FromBody] CalendarEvent entity)
        => service.DeleteAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/CalendarEvents", ([FromServices] ICalendarEventProcessingService service, [FromServices] ODataQueryOptions<CalendarEvent> options)
        => HandleOData(options.ApplyTo(service.GetAll())))
        .WithOpenApi()
        .RequireAuthorization();
}

void AddUserRoles(WebApplication app)
{
    app.MapPost($"/UserRoles", ([FromServices] IUserRoleProcessingService service, [FromBody] UserRole entity)
        => service.AddAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapDelete($"/UserRoles", ([FromServices] IUserRoleProcessingService service, [FromBody] UserRole entity)
        => service.DeleteAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/UserRoles", ([FromServices] IUserRoleProcessingService service, [FromServices] ODataQueryOptions<UserRole> options)
        => HandleOData(options.ApplyTo(service.GetAll())))
        .WithOpenApi()
        .RequireAuthorization();
}

void AddTransactions(WebApplication app)
{
    app.MapPost($"/Transactions", ([FromServices] ITransactionProcessingService service, [FromBody] Transaction entity)
        => service.AddAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapPost("/Transactions/ImportRBS",
        async ([FromServices] IHttpContextAccessor context, [FromServices] IRBSOrchestrationService rbsOrchestrationService)
           => {
               var requestBody = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
               await rbsOrchestrationService.ImportTransactionsFromRawDataAsync(requestBody);
               return "";
           }).WithOpenApi((operation) => new(operation)
           {
               RequestBody = new()
               {
                   Required = true,
                   Content = new Dictionary<string, OpenApiMediaType>()
                   {
                       ["text/csv"] = new()
                       {
                           Schema = new OpenApiSchema()
                           {
                               Type = "text/csv",
                               Example = new OpenApiString(@"Date,Type,Description,Value,Balance,Account Name,Account Number
19 Dec 2022,DPC,""Test Account"",2.01,432.38,AccountReference1,AccountNumber1")
                           }
                       }
                   }
               },
               Summary = "Import Transactions",
               Description = "Import Transactions from CSV for RBS Transaction Statements"
           })
        .WithOpenApi()
        .RequireAuthorization();

    app.MapPut($"/Transactions", ([FromServices] ITransactionProcessingService service, [FromBody] Transaction entity)
        => service.UpdateAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapDelete($"/Transactions", ([FromServices] ITransactionProcessingService service, [FromBody] Transaction entity)
        => service.DeleteAsync(entity))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/Transactions", ([FromServices] ITransactionProcessingService service, [FromServices] ODataQueryOptions<Transaction> options)
        => HandleOData(options.ApplyTo(service.GetAll())))
        .WithOpenApi()
        .RequireAuthorization();

    app.MapGet($"/Transactions/GetCalendarEventAccountSummaries", (
        [FromServices] ITransactionProcessingService service,
        Guid calendarId)
        => service.GetCalendarEventAccountSummaries(calendarId))
        .WithOpenApi()
        .RequireAuthorization();
}

void AddTransactionTypes(WebApplication app)
{
    app.MapGet($"/TransactionTypes", ([FromServices] IFoundationService service, [FromServices] ODataQueryOptions<TransactionType> options)
        => HandleOData(options.ApplyTo(service.GetAll<TransactionType>())))
        .WithOpenApi()
        .RequireAuthorization();
}