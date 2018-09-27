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
        private IUserService _userService;

        public TransactionService(ITransactionRepository transactionRepository, ICsvService csvService, ICategoryService categoryService, IUserService userService)
        {
            _transactionRepository = transactionRepository;
            _csvService = csvService;
            _categoryService = categoryService;
            _userService = userService;
        }

        public bool ProcessTransactions(MemoryStream postedFile, string bankName)
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
                    UserId = _userService.GetCurrentUserId(),
                };
                transactionModels.Add(model);
            });

          CategoriseTransactions(transactionModels);
          return SaveTransactions();
        }

        public bool ReCategoriseTransactions()
        {
            CategoriseTransactions(_transactionRepository.GetAll().ToList());
            return SaveTransactions();
        }

        private void CategoriseTransactions(List<Transaction> transactionModels)
        {
            transactionModels.ForEach(t => t.Category = _categoryService.GetCategoryForTransactionName(t.Description));
        }

        private bool SaveTransactions()
        {
            if (_transactionRepository.Save() == 0)
            {
                return false;
            }

            return true;
        }
    }
}