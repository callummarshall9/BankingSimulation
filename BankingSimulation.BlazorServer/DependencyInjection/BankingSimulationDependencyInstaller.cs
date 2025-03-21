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
}