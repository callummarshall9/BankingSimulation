using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BankingSimulation.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSimulation.Data;

public class ODataContext : BankSimulationContext
{
    private string userId;
    public ODataContext(DbContextOptions<BankSimulationContext> options, ClaimsPrincipal principal) : base(options)
    {
        userId = principal?.Identity?.Name ?? "Guest";
    }

    private IEnumerable<Guid> roleIds
    {
        get => Roles.IgnoreQueryFilters()
            .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
            .Select(r => r.Id)
            .ToArray();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>()
            .HasQueryFilter(a => a.Roles.Any());

        modelBuilder.Entity<AccountBankingSystemReference>()
            .HasQueryFilter(absr => absr.Account != null);

        modelBuilder.Entity<AccountRole>()
            .HasQueryFilter(ar => ar.Role != null);

        modelBuilder.Entity<Role>()
            .HasQueryFilter(r => roleIds.Contains(r.Id));

        modelBuilder.Entity<UserRole>()
            .HasQueryFilter(ur => ur.Role != null);

        modelBuilder.Entity<Transaction>()
            .HasQueryFilter(t => t.Account != null);

        modelBuilder.Entity<Category>()
            .HasQueryFilter(c => c.Role != null);

        modelBuilder.Entity<CategoryKeyword>()
            .HasQueryFilter(ck => ck.Category != null);

        modelBuilder.Entity<Calendar>()
            .HasQueryFilter(c => c.Role != null);

        modelBuilder.Entity<CalendarEvent>()
            .HasQueryFilter(ce => ce.Calendar != null);
    }
}