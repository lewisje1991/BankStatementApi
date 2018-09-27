using BankStatementApi.Repositories;
using BankStatementApi.Services;
using BankStatementApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using BankStatementApi.DTOs;

namespace Tests.Services
{
    [TestClass]
    public class TransactionServiceTest
    {
        private Mock<ITransactionRepository> _mockTransactionRepository;
        private Mock<ICsvService> _mockCsvService;
        private Mock<ICategoryService> _mockCategoryService;
        private Mock<IUserService> _mockUserService;
        private TransactionService testSubject;

        [TestInitialize]
        public void Setup()
        {
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockCsvService = new Mock<ICsvService>();
            _mockCategoryService = new Mock<ICategoryService>();
            _mockUserService = new Mock<IUserService>();
            testSubject = new TransactionService(_mockTransactionRepository.Object, _mockCsvService.Object, _mockCategoryService.Object, _mockUserService.Object);
        }

        [TestMethod]
        public void ReCategorise_ChangesCategoryOfSingleTransaction()
        {
            var transactionList = new List<Transaction>()
            {
                GetTransaction("T1", "OriginalCategory", "T1"),

            };

            var category = GetCategory("FirstCategory");

            _mockTransactionRepository.Setup(x => x.GetAll()).Returns(transactionList);

            _mockCategoryService.Setup(x => x.GetCategoryForTransactionName(transactionList[0].Description)).Returns(category);

            var response = testSubject.ReCategoriseTransactions();

            Assert.AreEqual(category.Name, transactionList[0].Category.Name);
        }

        [TestMethod]
        public void ReCategorise_ChangesCategoryOfMultipleTransactions()
        {
            var transactionList = new List<Transaction>()
            {
                GetTransaction("T1", "OriginalCategory", "T1"),
                GetTransaction("T2", "OriginalCategory2", "T2"),
            };

            var firstCategory = GetCategory("FirstCategory");
            var secondCategory = GetCategory("SecondCategory");

            _mockTransactionRepository.Setup(x => x.GetAll()).Returns(transactionList);

            _mockCategoryService.Setup(x => x.GetCategoryForTransactionName(transactionList[0].Description)).Returns(firstCategory);

            _mockCategoryService.Setup(x => x.GetCategoryForTransactionName(transactionList[1].Description)).Returns(secondCategory);

            var response = testSubject.ReCategoriseTransactions();

            Assert.AreEqual(firstCategory.Name, transactionList[0].Category.Name);
            Assert.AreEqual(secondCategory.Name, transactionList[1].Category.Name);
        }

        [TestMethod]
        public void ProcessTransactions_SavesTransactionWithCategorySet()
        {
            var memorySteam = new MemoryStream();

            var transactionList = new List<Transaction>()
            {
                GetTransaction("T1", "OriginalCategory", "T1"),
                GetTransaction("T2", "OriginalCategory2", "T2"),
            };

            var transactionDtoList = new List<TransactionDto>()
            {
                GetTransactionDto("t1"),
                GetTransactionDto("t2"),
            };

            _mockCsvService.Setup(x => x.CsvToDtoList<TransactionDto>(memorySteam, "BankOfScotland")).Returns(transactionDtoList);

            var firstCategory = GetCategory("FirstCategory");
            var secondCategory = GetCategory("SecondCategory");

            _mockCategoryService.Setup(x => x.GetCategoryForTransactionName(transactionDtoList[0].Description)).Returns(firstCategory);
            _mockCategoryService.Setup(x => x.GetCategoryForTransactionName(transactionDtoList[1].Description)).Returns(secondCategory);

            _mockTransactionRepository.Setup(x => x.Save()).Returns(1);

            testSubject.ProcessTransactions(memorySteam, "BankOfScotland");

        }

        private Category GetCategory(string Name)
        {
            return new Category()
            {
                Name = "Name"
            };
        }

        private TransactionDto GetTransactionDto(string description)
        {
            return new TransactionDto()
            {
                Description = description
            };
        }

        private Transaction GetTransaction(string description, string categoryName, string transactionNames)
        {
            return new Transaction()
            {
                Description = description,
                Category = new Category()
                {
                    Name = categoryName,
                    TransactionNames = transactionNames,
                }
            };
        }
    }
}
