using Microsoft.EntityFrameworkCore;

namespace BankingSimulation.Data;

public class BankSimulationContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountBankingSystemReference> AccountSystemReferences { get; set; }
    public DbSet<BankingSystem> Systems { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<TransactionType> TransactionTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=BankingSimulation;User Id=sa;Password=Npfzstyk325;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BankingSystem>().HasKey(s => s.Id);
        modelBuilder.Entity<AccountBankingSystemReference>().HasKey(c => new { c.BankingSystemId, c.AccountId });
    }
}