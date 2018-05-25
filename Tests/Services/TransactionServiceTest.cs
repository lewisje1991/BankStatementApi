using BankStatementApi.Repositories;
using BankStatementApi.Services;
using BankStatementApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

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

        public Category GetCategory(string Name)
        {
            return new Category()
            {
                Name = "Name"
            };
        }

        public Transaction GetTransaction(string description, string categoryName, string transactionNames)
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
