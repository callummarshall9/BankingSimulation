using BankingSimulation.UI.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.UI.ViewServices;

public interface IBankAccountsViewService
{
    BankAccountsIndexViewModel Index();
}