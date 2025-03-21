using System.Collections;
using System.Text.Json;
using BankingSimulation.Data;
using BankingSimulation.Data.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query.Wrapper;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace BankingSimulation.UI;

public static class ODataDependencyInstaller
{
    public static void RegisterOData(IServiceCollection services)
    {
        var model = GetModel();
        
        services.AddODataOptions<Account>(model);
        services.AddODataOptions<AccountBankingSystemReference>(model);
        services.AddODataOptions<BankingSystem>(model);
        services.AddODataOptions<Calendar>(model);
        services.AddODataOptions<CalendarEvent>(model);
        services.AddODataOptions<Category>(model);
        services.AddODataOptions<CategoryKeyword>(model);
        services.AddODataOptions<Role>(model);
        services.AddODataOptions<UserRole>(model);
        services.AddODataOptions<Transaction>(model);
        services.AddODataOptions<TransactionType>(model);
    }
    
    private static IEdmModel GetModel()
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
    
    public static object HandleOData(IEnumerable result)
    {
        if (result is ISelectExpandWrapper castedWrapper)
        {
            var newDictionary = new Dictionary<string, object>();

            var dictionaryResult = castedWrapper.ToDictionary();

            foreach (var (key, value) in dictionaryResult)
            {
                string actualKey = JsonNamingPolicy.CamelCase.ConvertName(key);

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

            var entities = (result as IEnumerable<ISelectExpandWrapper> ?? Array.Empty<ISelectExpandWrapper>()).ToList();

            foreach(var entity in entities)
            {
                var newDictionary = new Dictionary<string, object>();

                var dictionaryResult = entity.ToDictionary();

                foreach(var (key, value) in dictionaryResult)
                {
                    string actualKey = JsonNamingPolicy.CamelCase.ConvertName(key);

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

    private static object HandleODataDictionary(ISelectExpandWrapper castedWrapper)
    {
        var newDictionary = new Dictionary<string, object>();

        var dictionaryResult = castedWrapper.ToDictionary();

        foreach (var (key, value) in dictionaryResult)
        {
            string actualKey = JsonNamingPolicy.CamelCase.ConvertName(key);

            newDictionary[actualKey] = value;

            if (value is IEnumerable<ISelectExpandWrapper> castedValue)
                newDictionary[actualKey] = HandleOData(castedValue);

            if (value is ISelectExpandWrapper expandWrapper)
                newDictionary[actualKey] = HandleODataDictionary(expandWrapper);
        }


        return newDictionary;
    }
}