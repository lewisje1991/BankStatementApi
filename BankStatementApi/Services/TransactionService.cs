using BankStatementApi.DTOs;
using BankStatementApi.Models;
using BankStatementApi.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BankStatementApi.Services
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;

        private ICsvService _csvService;

        private ICategoryService _categoryService;

        public TransactionService(ITransactionRepository transactionRepository, ICsvService csvService, ICategoryService categoryService)
        {
            _transactionRepository = transactionRepository;
            _csvService = csvService;
            _categoryService = categoryService;
        }

        public void ProcessTransactions(MemoryStream postedFile, string bankName)
        {
            List<TransactionDto> transactionDtos = _csvService.CsvToDtoList<TransactionDto>(postedFile, bankName);

            List<Transaction> transactionModels = new List<Transaction>();

            transactionDtos.ForEach(transactionDto => {

                Transaction model = new Transaction()
                {
                    Type = transactionDto.Type,
                    Date = transactionDto.Date,
                    Credit = transactionDto.Credit,
                    Debit = transactionDto.Debit,
                    Description = transactionDto.Description,
                    UserId = 1,
                    Category = _categoryService.GetCategoryForTransactionName(transactionDto.Description)
            };
                transactionModels.Add(model);
            });

            _transactionRepository.AddRange(transactionModels);
            _transactionRepository.Save();
        } 

        public bool ReCategoriseTransactions()
        {
            var transactionModels = _transactionRepository.GetAll().ToList();

            transactionModels.ForEach(t => t.Category = _categoryService.GetCategoryForTransactionName(t.Description));

            if (_transactionRepository.Save() == 0)
            {
                return false;
            }

            return true;
        }
    }
}