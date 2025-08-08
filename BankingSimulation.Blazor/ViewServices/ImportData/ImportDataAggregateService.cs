using BankingSimulation.Barclays;
using BankingSimulation.BarclaysCard.Services.Orchestration;
using BankingSimulation.MBNA.Services.Orchestration;
using BankingSimulation.Nationwide.Services.Orchestration;
using BankingSimulation.RBS;
using System.ComponentModel.DataAnnotations;

namespace BankingSimulation.Blazor.ViewServices.ImportData
{
    public class ImportDataAggregateService(IRBSOrchestrationService rbsOrchestrationService,
        IBarclaysOrchestrationService barclaysOrchestrationService,
        IBarclaysCardOrchestrationService barclaysCardOrchestrationService,
        IMBNAOrchestrationService mbnaOrchestrationService,
        INationwideOrchestrationService nationwideOrchestrationService) : IImportDataAggregateService
    {
        public async Task ImportAsync(string? option, string? rawData)
        {
            if (string.IsNullOrEmpty(rawData))
                throw new ValidationException("Please supply valid data to import");

            switch(option)
            {
                case "rbs":
                    await ImportRBS(rawData);
                    break;
                case "barclays":
                    await ImportBarclays(rawData);
                    break;
                case "barclaysCard":
                    await ImportBarclaysCard(rawData);
                    break;
                case "mbna":
                    await ImportMBNA(rawData);
                    break;
                case "nationwide":
                    await ImportNationwide(rawData);
                    break;
                default:
                    throw new ArgumentException("Invalid import option");
            }
        }

        private async Task ImportRBS(string rawData)
        {
            await rbsOrchestrationService.ImportAccountsFromRawDataAsync(rawData);
            await rbsOrchestrationService.ImportTransactionsFromRawDataAsync(rawData);
        }

        private async Task ImportBarclays(string rawData)
        {
            await barclaysOrchestrationService.ImportAccountsFromRawDataAsync(rawData);
            await barclaysOrchestrationService.ImportTransactionsFromRawDataAsync(rawData);
        }

        private async Task ImportBarclaysCard(string rawData)
        {
            await barclaysCardOrchestrationService.ImportAccountsFromRawDataAsync(rawData);
            await barclaysCardOrchestrationService.ImportTransactionsFromRawDataAsync(rawData);
        }

        private async Task ImportMBNA(string rawData)
        {
            await mbnaOrchestrationService.ImportAccountsAsync();
            await mbnaOrchestrationService.ImportTransactionsFromRawDataAsync(rawData);
        }

        private async Task ImportNationwide(string rawData)
        {
            await nationwideOrchestrationService.ImportAccountsFromRawDataAsync(rawData);
            await nationwideOrchestrationService.ImportTransactionsFromRawDataAsync(rawData);
        }
    }
}
