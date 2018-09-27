using BankStatementApi.Models;
using System;
using System.Collections.Generic;

namespace BankStatementApi.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        IEnumerable<Transaction> GetTransactionsForDateRange(DateTime start, DateTime end, string userId);
    }
}