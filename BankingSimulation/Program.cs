using BankingSimulation.Data;
using BankingSimulation.Services;
using BankingSimulation.RBS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddODataQueryFilter();

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
AddSet<Transaction>(app);
AddSet<TransactionType>(app);

app.MapPost("/Accounts/Import", 
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

app.MapPost("/Transactions/Import", 
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
    app.MapGet($"/{typeof(T).Name}s", ([FromServices] IFoundationService foundationService) => foundationService.GetAll<T>()).WithOpenApi();
}
