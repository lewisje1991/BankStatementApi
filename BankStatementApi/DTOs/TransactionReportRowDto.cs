using System;
using System.Collections.Generic;

namespace BankStatementApi.DTOs
{
    public class TransactionReportRowDto
    {
        public string CategoryName;
        public Decimal CategoryGoalTarget;
        public Decimal TotalSpent;
        public Decimal Delta;
        public List<TransactionDto> Transactions;
    }
}