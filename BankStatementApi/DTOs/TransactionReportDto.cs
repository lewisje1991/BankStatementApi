using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankStatementApi.DTOs
{
    public class TransactionReportDto
    {
        public string Title = "Transaction Report";
        public DateTime Start;
        public DateTime End;
        public List<TransactionReportRowDto> Rows;
    }
}