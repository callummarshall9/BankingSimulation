using BankingSimulation.Data.Models;
using BankingSimulation.UI.ViewServices;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Components;

public partial class AccountSummaryChart : ComponentBase
{
    [Parameter]
    public Guid CalendarId { get; set; }
    
    public IEnumerable<PeriodAccountSummary> Summaries { get; set; }
    
    [Inject]
    public IAccountSummaryChartViewService AccountSummaryChartViewService { get; set; }

    protected override void OnInitialized()
    {
        Summaries = AccountSummaryChartViewService.GetPeriodAccountSummaries(CalendarId);
    }
}