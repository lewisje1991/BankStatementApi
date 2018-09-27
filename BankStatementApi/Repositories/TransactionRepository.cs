using BankStatementApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BankStatementApi.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(BankStatementApiContext context) : base(context)
        {
            
        }
        
        public IEnumerable<Transaction> GetTransactionsForDateRange(DateTime start, DateTime end, string userId)
        {
            return base.Find(t => t.Date >= start && t.Date <= end && t.UserId == userId);
        }
    }
}