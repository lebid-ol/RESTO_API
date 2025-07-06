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
    }
}
