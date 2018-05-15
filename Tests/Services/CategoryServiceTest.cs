using System.Collections.Generic;
using BankStatementApi.Models;
using BankStatementApi.Repositories;
using BankStatementApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.Services
{
    [TestClass]
    public class CategoryServiceTest
    {
        private Mock<ICategoryRepository> mockCategoryRepository;
        private CategoryService testSubject;

        [TestInitialize]
        public void Setup()
        {
            mockCategoryRepository = new Mock<ICategoryRepository>();
            testSubject = new CategoryService(mockCategoryRepository.Object);
        }

        [TestMethod]
        public void GetCategoryForTransactionName_ReturnsCategoryModel_WhenACategoryMatchesTransactionNames()
        {
            var category = new Category()
            {
                Name = "Test",
                TransactionNames = "Cake, Bakery",
                Target = 100
            };

            mockCategoryRepository.Setup(x => x.GetAll()).Returns(new List<Category>(){
                category
            });
            var response = testSubject.GetCategoryForTransactionName("Cake");

            Assert.AreEqual(category, response);
        }

        [TestMethod]
        public void GetCategoryForTransactionName_ReturnsNull_WhenNoMatchFoundInCategoryList()
        {
            var category = new Category()
            {
                Name = "Test",
                TransactionNames = "No Match",
                Target = 100
            };

            mockCategoryRepository.Setup(x => x.GetAll()).Returns(new List<Category>(){
                category
            });
            var response = testSubject.GetCategoryForTransactionName("Cake");

            Assert.AreEqual(null, response);
        }
    }
}
