using BankAccounts.AppplicationData.Records;
using BankAccounts.Records;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.AppplicationData.Db
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<TransactionsEntity> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasMany(user => user.Accounts)
                .WithOne(account => account.User)
                .HasForeignKey(account => account.UserId);

            modelBuilder.Entity<AccountEntity>()
        .HasMany(account => account.Transactions)
        .WithOne(transaction => transaction.Account)
        .HasForeignKey(transaction => transaction.AccountId);



            base.OnModelCreating(modelBuilder);
        }
    }
}
