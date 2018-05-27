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
        private Mock<ITransactionRepository> mockTransactionRepository;
        private Mock<ICsvService> mockCsvService;
        private Mock<ICategoryService> mockCategoryService;
        private TransactionService testSubject;

        [TestInitialize]
        public void Setup()
        {
            mockTransactionRepository = new Mock<ITransactionRepository>();
            mockCsvService = new Mock<ICsvService>();
            mockCategoryService = new Mock<ICategoryService>();
            testSubject = new TransactionService(mockTransactionRepository.Object, mockCsvService.Object, mockCategoryService.Object);
        }

        [TestMethod]
        public void ReCategorise_ChangesCategoryOfSingleTransaction()
        {
            var transactionList = new List<Transaction>()
            {
                GetTransaction("T1", "OriginalCategory", "T1"),

            };

            var category = GetCategory("FirstCategory");

            mockTransactionRepository.Setup(x => x.GetAll()).Returns(transactionList);

            mockCategoryService.Setup(x => x.GetCategoryForTransactionName(transactionList[0].Description)).Returns(category);

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

            mockTransactionRepository.Setup(x => x.GetAll()).Returns(transactionList);

            mockCategoryService.Setup(x => x.GetCategoryForTransactionName(transactionList[0].Description)).Returns(firstCategory);

            mockCategoryService.Setup(x => x.GetCategoryForTransactionName(transactionList[1].Description)).Returns(secondCategory);

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

            mockCsvService.Setup(x => x.CsvToDtoList<TransactionDto>(memorySteam, "BankOfScotland")).Returns(transactionDtoList);

            var firstCategory = GetCategory("FirstCategory");
            var secondCategory = GetCategory("SecondCategory");

            mockCategoryService.Setup(x => x.GetCategoryForTransactionName(transactionDtoList[0].Description)).Returns(firstCategory);
            mockCategoryService.Setup(x => x.GetCategoryForTransactionName(transactionDtoList[1].Description)).Returns(secondCategory);

            mockTransactionRepository.Setup(x => x.Save()).Returns(1);

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
