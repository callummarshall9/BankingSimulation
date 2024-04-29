using BankingSimulation.Data;
using BankingSimulation.Data.Models;
using BankingSimulation.RBS;
using BankingSimulation.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


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
    builder.EntitySet<Transaction>("Transactions");
    builder.EntitySet<TransactionType>("TransactionTypes");

    return builder.GetEdmModel();
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BankSimulationContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("BankSimulationContext")));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOData((opts) => opts.EnableQueryFeatures(100));

var model = GetModel();

builder.Services.AddODataOptions<Account>(model);
builder.Services.AddODataOptions<AccountBankingSystemReference>(model);
builder.Services.AddODataOptions<BankingSystem>(model);
builder.Services.AddODataOptions<Calendar>(model);
builder.Services.AddODataOptions<CalendarEvent>(model);
builder.Services.AddODataOptions<Category>(model);
builder.Services.AddODataOptions<CategoryKeyword>(model);
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

AddSet<Account>(app);
AddSet<AccountBankingSystemReference>(app);
AddSet<BankingSystem>(app);
AddSet<Calendar>(app);
AddSet<CalendarEvent>(app);
AddSet<Category>(app);
AddSet<CategoryKeyword>(app);
AddSet<Transaction>(app);
AddSet<TransactionType>(app);

app.MapGet("/health/check", () => DateTimeOffset.UtcNow);

app.MapPost("/Accounts/ImportRBS", 
    async ([FromServices] IHttpContextAccessor context, [FromServices] IRBSOrchestrationService rbsOrchestrationService) 
        => {
            var requestBody = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
            await rbsOrchestrationService.ImportAccountsFromRawDataAsync(requestBody);
            return "";
        }).WithOpenApi((operation) => new(operation) {
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
        });

app.MapPost("/Transactions/ImportRBS", 
    async ([FromServices] IHttpContextAccessor context, [FromServices] IRBSOrchestrationService rbsOrchestrationService) 
       => {
            var requestBody = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
            await rbsOrchestrationService.ImportTransactionsFromRawDataAsync(requestBody);
            return "";
        }).WithOpenApi((operation) => new(operation) {
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
        });;

 app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.Run();

void AddSet<T>(WebApplication app) where T : class
{
    app.MapPost($"/{typeof(T).Name}s", ([FromServices] IFoundationService foundationService, [FromBody] T entity) 
        => foundationService.AddAsync<T>(entity)).WithOpenApi();
    app.MapPut($"/{typeof(T).Name}s", ([FromServices] IFoundationService foundationService, [FromBody] T entity) 
        => foundationService.UpdateAsync<T>(entity)).WithOpenApi();
    app.MapDelete($"/{typeof(T).Name}s", ([FromServices] IFoundationService foundationService, [FromBody] T entity) 
        => foundationService.UpdateAsync<T>(entity)).WithOpenApi();
    app.MapGet($"/{typeof(T).Name}s", ([FromServices] IFoundationService foundationService, [FromServices] ODataQueryOptions<T> options)
        => options.ApplyTo(foundationService.GetAll<T>()).Cast<T>()).WithOpenApi();
}
