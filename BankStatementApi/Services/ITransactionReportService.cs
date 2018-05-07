using BankStatementApi.DTOs;
using System;

namespace BankStatementApi.Services
{
    public interface ITransactionReportService
    {
        TransactionReportDto GenerateTransactionReport(DateTime start, DateTime end, int userId);
    }
}