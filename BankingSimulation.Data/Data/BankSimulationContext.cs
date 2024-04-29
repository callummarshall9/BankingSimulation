using BankingSimulation.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace BankingSimulation.Data;

public class BankSimulationContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountBankingSystemReference> AccountSystemReferences { get; set; }
    public DbSet<BankingSystem> Systems { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
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

        
    }
}

public class ODataContext : BankSimulationContext
{
    private string userId;
    public ODataContext(DbContextOptions options, ClaimsPrincipal principal) : base(options)
    {
        userId = principal?.Identity?.Name ?? "Guest";
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>()
            .HasQueryFilter(a => a.Roles.Any());

        modelBuilder.Entity<AccountRole>()
            .HasQueryFilter(ar => ar.Role != null);

        modelBuilder.Entity<Role>()
            .HasQueryFilter(r => r.UserRoles.Any());

        modelBuilder.Entity<UserRole>()
            .HasQueryFilter(ur => ur.UserId == userId);

        modelBuilder.Entity<Transaction>()
            .HasQueryFilter(t => t.Account != null);
    }
}