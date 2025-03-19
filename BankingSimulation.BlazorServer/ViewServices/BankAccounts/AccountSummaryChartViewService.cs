using BankingSimulation.Data.Models;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.UI.ViewServices;

public class AccountSummaryChartViewService(ITransactionProcessingService transactionProcessingService) : IAccountSummaryChartViewService
{
    public IEnumerable<PeriodAccountSummary> GetPeriodAccountSummaries(Guid calendarId)
        => transactionProcessingService.GetCalendarEventAccountSummaries(calendarId);
}