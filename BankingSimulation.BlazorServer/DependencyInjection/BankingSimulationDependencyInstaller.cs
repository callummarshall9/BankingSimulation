using BankingSimulation.Barclays;
using BankingSimulation.BarclaysCard;
using BankingSimulation.BarclaysCard.Services.Orchestration;
using BankingSimulation.Data;
using BankingSimulation.Data.Models;
using BankingSimulation.MBNA;
using BankingSimulation.MBNA.Services.Orchestration;
using BankingSimulation.Nationwide;
using BankingSimulation.Nationwide.Services.Orchestration;
using BankingSimulation.RBS;
using BankingSimulation.Services;
using BankingSimulation.Services.Processing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace BankingSimulation.UI;

public class BankingSimulationDependencyInstaller
{
    public static void AddServices(IServiceCollection services)
    {
        services.AddBankingSimulationServices();
        services.AddBankingSimulationRBSServices();
        services.AddBankingSimulationBarclaysServices();
        services.AddBankingSimulationBarclaysCardServices();
        services.AddBankingSimulationMBNAServices();
        services.AddBankingSimulationNationwideServices();
        services.AddBankingSimulationData();
    }
    
    public static void AddSet<T>(WebApplication app) where T : class
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
    
    public static void AddCategories(WebApplication app)
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
            => ODataDependencyInstaller.HandleOData(options.ApplyTo(service.GetAll())))
            .WithOpenApi()
            .RequireAuthorization();

        app.MapGet($"/Categories/ForPeriod", ([FromServices] ICategoryProcessingService service, DateOnly fromPeriod, DateOnly toPeriod)
            => service.ForPeriod(fromPeriod, toPeriod))
            .WithOpenApi()
            .RequireAuthorization();

        app.MapGet($"/Categories/AccountsForPeriod", ([FromServices] ICategoryProcessingService service, DateOnly fromPeriod, DateOnly toPeriod, string accountIds)
            => service.AccountsForPeriod(fromPeriod, toPeriod, accountIds))
            .WithOpenApi()
            .RequireAuthorization();
    }

    public static void AddCategoryKeywords(WebApplication app)
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
            => ODataDependencyInstaller.HandleOData(options.ApplyTo(service.GetAll())))
            .WithOpenApi()
            .RequireAuthorization();
    }

    public static void AddRoles(WebApplication app)
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
            => ODataDependencyInstaller.HandleOData(options.ApplyTo(service.GetAll())))
            .WithOpenApi()
            .RequireAuthorization();
    }

    public static void AddCalendars(WebApplication app)
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
            => ODataDependencyInstaller.HandleOData(options.ApplyTo(service.GetAll())))
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

    public static void AddCalendarEvents(WebApplication app)
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
            => ODataDependencyInstaller.HandleOData(options.ApplyTo(service.GetAll())))
            .WithOpenApi()
            .RequireAuthorization();
    }

    public static void AddUserRoles(WebApplication app)
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
            => ODataDependencyInstaller.HandleOData(options.ApplyTo(service.GetAll())))
            .WithOpenApi()
            .RequireAuthorization();
    }

    public static void AddTransactions(WebApplication app)
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

        app.MapPost("/Transactions/ImportBarclays",
            async ([FromServices] IHttpContextAccessor context, [FromServices] IBarclaysOrchestrationService orchestrationService)
               => {
                   var requestBody = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
                   await orchestrationService.ImportTransactionsFromRawDataAsync(requestBody);
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
                                   Example = new OpenApiString(@"Number,Date,Account,Amount,Subcategory,Memo
    0,07/05/2024,DUMMY ACCOUNT,-1.00,TEST CARD PURCHASE,EXAMPLE MEMO")
                               }
                           }
                       }
                   },
                   Summary = "Import Transactions",
                   Description = "Import Transactions from CSV for RBS Transaction Statements"
               })
            .WithOpenApi()
            .RequireAuthorization();

        app.MapPost("/Transactions/ImportBarclaysCard",
            async ([FromServices] IHttpContextAccessor context, [FromServices] IBarclaysCardOrchestrationService orchestrationService)
               => {
                   var requestBody = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
                   await orchestrationService.ImportTransactionsFromRawDataAsync(requestBody);
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
                                   Example = new OpenApiString(@"1 May 24,"" Foo "",n/a,TEST CARD HOLDER,,-1.00,")
                               }
                           }
                       }
                   },
                   Summary = "Import Transactions",
                   Description = "Import Transactions from CSV for RBS Transaction Statements"
               })
            .WithOpenApi()
            .RequireAuthorization();

        app.MapPost("/Transactions/ImportMBNA",
            async ([FromServices] IHttpContextAccessor context, [FromServices] IMBNAOrchestrationService orchestrationService)
               => {
                   var requestBody = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
                   await orchestrationService.ImportTransactionsFromRawDataAsync(requestBody);
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
                                   Example = new OpenApiString(@"Date,Date entered,Reference,Description,Amount,
    21/04/2024,21/04/2024,1,INTEREST,1.00,")
                               }
                           }
                       }
                   },
                   Summary = "Import Transactions",
                   Description = "Import Transactions from CSV for RBS Transaction Statements"
               })
            .WithOpenApi()
            .RequireAuthorization();

        app.MapPost("/Transactions/ImportNationwide",
            async ([FromServices] IHttpContextAccessor context, [FromServices] INationwideOrchestrationService orchestrationService)
               => {
                   var requestBody = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
                   await orchestrationService.ImportTransactionsFromRawDataAsync(requestBody);
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
                                   Example = new OpenApiString(@"""Account Name:"",""FlexOne ****12345""
    ""Account Balance:"",""�1.00""
    ""Available Balance: "",""�1.00""

    ""Date"",""Transaction type"",""Description"",""Paid out"",""Paid in"",""Balance""
    ""10 Apr 2024"",""Direct Debit"",""Foo"",""�1.00"","""",""�1.56""")
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
            => ODataDependencyInstaller.HandleOData(options.ApplyTo(service.GetAll())))
            .WithOpenApi()
            .RequireAuthorization();

        app.MapGet($"/Transactions/GetCalendarEventAccountSummaries", (
            [FromServices] ITransactionProcessingService service,
            Guid calendarId)
            => service.GetCalendarEventAccountSummaries(calendarId))
            .WithOpenApi()
            .RequireAuthorization();
    }

    public static void AddTransactionTypes(WebApplication app)
    {
        app.MapGet($"/TransactionTypes", ([FromServices] IFoundationService service, [FromServices] ODataQueryOptions<TransactionType> options)
            => ODataDependencyInstaller.HandleOData(options.ApplyTo(service.GetAll<TransactionType>())))
            .WithOpenApi()
            .RequireAuthorization();
    }
        
    public static void AddAccounts(WebApplication app)
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

        app.MapPost("/Accounts/ImportBarclays",
            async ([FromServices] IHttpContextAccessor context, [FromServices] IBarclaysOrchestrationService orchestrationService)
                => {
                    var requestBody = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
                    await orchestrationService.ImportAccountsFromRawDataAsync(requestBody);
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
                                    Example = new OpenApiString(@"Number,Date,Account,Amount,Subcategory,Memo
    0,07/05/2024,DUMMY ACCOUNT,-1.00,TEST CARD PURCHASE,EXAMPLE MEMO")
                                }
                            }
                        }
                    },
                    Summary = "Import Accounts",
                    Description = "Import Accounts from CSV for RBS Transaction Statements"
                })
            .WithOpenApi()
            .RequireAuthorization();

        app.MapPost("/Accounts/ImportBarclaysCard",
            async ([FromServices] IHttpContextAccessor context, [FromServices] IBarclaysCardOrchestrationService orchestrationService)
                => {
                    var requestBody = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
                    await orchestrationService.ImportAccountsFromRawDataAsync(requestBody);
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
                                    Example = new OpenApiString(@"1 May 24,"" Foo "",n/a,TEST CARD HOLDER,,-1.00,")
                                }
                            }
                        }
                    },
                    Summary = "Import Accounts",
                    Description = "Import Accounts from CSV for RBS Transaction Statements"
                })
            .WithOpenApi()
            .RequireAuthorization();

        app.MapPost("/Accounts/ImportMBNA",
            async ([FromServices] IHttpContextAccessor context, [FromServices] IMBNAOrchestrationService orchestrationService)
                => {
                    await orchestrationService.ImportAccountsAsync();
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
                                    Example = new OpenApiString(@"Date,Date entered,Reference,Description,Amount,
    21/04/2024,21/04/2024,1,INTEREST,1.00,")
                                }
                            }
                        }
                    },
                    Summary = "Import Accounts",
                    Description = "Import Accounts from CSV for RBS Transaction Statements"
                })
            .WithOpenApi()
            .RequireAuthorization();

        app.MapPost("/Accounts/ImportNationwide",
            async ([FromServices] IHttpContextAccessor context, [FromServices] INationwideOrchestrationService orchestrationService)
                => {
                    var requestBody = await new StreamReader(context.HttpContext.Request.Body).ReadToEndAsync();
                    await orchestrationService.ImportAccountsFromRawDataAsync(requestBody);
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
                                    Example = new OpenApiString(@"""Account Name:"",""FlexOne ****12345""
    ""Account Balance:"",""�1.00""
    ""Available Balance: "",""�1.00""

    ""Date"",""Transaction type"",""Description"",""Paid out"",""Paid in"",""Balance""
    ""10 Apr 2024"",""Direct Debit"",""Foo"",""�1.00"","""",""�1.56""")
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
            => ODataDependencyInstaller.HandleOData(options.ApplyTo(service.GetAll())))
            .WithOpenApi()
            .RequireAuthorization();
    }
}