using BankingSimulation.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSimulation.Data;

public class BankSimulationContext(DbContextOptions<BankSimulationContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountBankingSystemReference> AccountSystemReferences { get; set; }
    public DbSet<BankingSystem> Systems { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionType> TransactionTypes { get; set; }
    public DbSet<Calendar> Calendars { get; set; }
    public DbSet<CalendarEvent> CalendarEvents { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryKeyword> CategoryKeywords { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BankingSystem>().HasKey(s => s.Id);
        modelBuilder.Entity<AccountBankingSystemReference>().HasKey(c => new { c.BankingSystemId, c.AccountId });

        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.ChildCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Role>()
            .HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Role>()
            .HasMany(r => r.Categories)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Account>()
            .HasMany(a => a.Roles)
            .WithOne(ar => ar.Account)
            .HasForeignKey(ar => ar.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AccountRole>()
            .HasOne(ar => ar.Role)
            .WithMany(r => r.Accounts)
            .HasForeignKey(ar => ar.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Role>().ToTable("Roles", "Security");

        modelBuilder.Entity<UserRole>().ToTable("UserRoles", "Security")
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<AccountRole>().ToTable("AccountRoles", "Security")
            .HasKey(ar => new { ar.AccountId, ar.RoleId });

        modelBuilder.Entity<AccountRole>()
            .HasOne(ar => ar.Account)
            .WithMany(a => a.Roles)
            .HasForeignKey(ar => ar.AccountId);

        modelBuilder.Entity<AccountRole>()
            .HasOne(ar => ar.Role)
            .WithMany(r => r.Accounts)
            .HasForeignKey(ar => ar.RoleId);
    }

    public async Task UpdateTransactionsForCategory(Guid categoryId)
    {
        await Transactions
            .IgnoreQueryFilters()
            .Where(c => c.CategoryId == categoryId)
            .ExecuteUpdateAsync(t => t.SetProperty(t => t.CategoryId, _ => null));

        var dbKeywords = CategoryKeywords
            .IgnoreQueryFilters()
            .Where(ck => ck.CategoryId == categoryId)
            .Select(ck => ck.Keyword)
            .ToArray();

        foreach(var keyword in dbKeywords)
            await Transactions
                .Where(t => t.Description.Contains(keyword))
                .ExecuteUpdateAsync(t => t.SetProperty(t => t.CategoryId, categoryId));
    }
}