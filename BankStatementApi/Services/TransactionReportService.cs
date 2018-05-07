
using System;
using System.Linq;
using System.Collections.Generic;
using BankStatementApi.DTOs;
using BankStatementApi.Repositories;
using BankStatementApi.Models;

namespace BankStatementApi.Services
{
    public class TransactionReportService : ITransactionReportService
    {
        private ITransactionRepository _transactionRepository;
        private ICategoryRepository _categoryRepository;

        public TransactionReportService(ITransactionRepository transactionRepository, ICategoryRepository categoryRepository)
        {
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
        }

        public TransactionReportDto GenerateTransactionReport(DateTime start, DateTime end, int userId)
        {
            var transactionReportDto = new TransactionReportDto()
            {
                Start = start,
                End = end,
                Rows = GetReportRows(start, end)
            };

            return transactionReportDto;
        }

        private List<TransactionReportRowDto> GetReportRows(DateTime start, DateTime end)
        {
            var rows = new List<TransactionReportRowDto>();

            GetCategorisedRows(start, end, rows);
            GetUncategorisedRow(start, end, rows);

            return rows;
        }

        private void GetCategorisedRows(DateTime start, DateTime end, List<TransactionReportRowDto> rows)
        {
            foreach (var category in _categoryRepository.GetAll())
            {
                //TODO refactor this.
                var categoriesWithTransactions = category.Transactions.Where(t => t.Date >= start && t.Date <= end && t.Type != "CPT").ToList();

                var categoryTotalSpent = categoriesWithTransactions.Sum(t => t.Debit);

                rows.Add(new TransactionReportRowDto()
                {
                    CategoryName = category.Name,
                    TotalSpent = categoryTotalSpent,
                    CategoryGoalTarget = category.Target,
                    Delta = category.Target - categoryTotalSpent,
                    Transactions = FormatTransactions(categoriesWithTransactions)
                });
            }
        }

        private void GetUncategorisedRow(DateTime start, DateTime end, List<TransactionReportRowDto> rows)
        {
            var uncategorisedTransactions = _transactionRepository.GetAll().Where(t => t.Date >= start && t.Date <= end && (t.Type == "DEB" || t.Type == "DD") && t.Category == null).ToList();
            var uncategorisedTotalSpent = uncategorisedTransactions.Sum(t => t.Debit);

            rows.Add(new TransactionReportRowDto()
            {
                CategoryName = "Uncategorised",
                TotalSpent = uncategorisedTotalSpent,
                Transactions = FormatTransactions(uncategorisedTransactions)
            });
        }

        private List<TransactionDto> FormatTransactions(ICollection<Transaction> transactions)
        {
            //Todo make this into a mapper;
            var dtos = new List<TransactionDto>();
            foreach(var transaction in transactions)
            {
                dtos.Add(new TransactionDto()
                {
                    Type = transaction.Type,
                    Description = transaction.Description,
                    Debit = transaction.Debit,
                    Credit = transaction.Credit,
                    Date = transaction.Date
                });
            }

            return dtos;
        }
    }
}
