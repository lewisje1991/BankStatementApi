using BankStatementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankStatementApi.Models
{
    public class BankStatementApiContext : DbContext
    {
        public BankStatementApiContext(DbContextOptions<BankStatementApiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
    }
}