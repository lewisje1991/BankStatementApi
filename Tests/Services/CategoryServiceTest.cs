using System.Collections.Generic;
using BankStatementApi.Models;
using BankStatementApi.Repositories;
using BankStatementApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BankStatementApi.DTOs;

namespace Tests.Services
{
    [TestClass]
    public class CategoryServiceTest
    {
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private Mock<IUserService> _mockUserService;
        private CategoryService _testSubject;

        [TestInitialize]
        public void Setup()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockUserService = new Mock<IUserService>();
            _testSubject = new CategoryService(_mockCategoryRepository.Object, _mockUserService.Object);
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

            _mockCategoryRepository.Setup(x => x.GetAll()).Returns(new List<Category>(){
                category
            });
            var response = _testSubject.GetCategoryForTransactionName("Cake");

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

            _mockCategoryRepository.Setup(x => x.GetAll()).Returns(new List<Category>(){
                category
            });
            var response = _testSubject.GetCategoryForTransactionName("Cake");

            Assert.AreEqual(null, response);
        }

        [TestMethod]
        public void SaveCategory_SavesASingleCategory()
        {
            var categoryDto = new CategoryDto()
            {
                Name = "Test",
                TransactionNames = "No Match",
                Target = 100
            };

            _mockCategoryRepository.Setup(x => x.Save()).Returns(1);

            var response = _testSubject.SaveCategory(categoryDto);

            _mockCategoryRepository.Verify(x => x.Add(It.IsAny<Category>()), Times.Once);
        }

        [TestMethod]
        public void SaveCategory_FormatsCertainPropertiesBeforeSaving()
        {
            var categoryDto = new CategoryDto()
            {
                Name = "Test",
                TransactionNames = "No Match",
                Target = 100
            };

            _mockCategoryRepository.Setup(x => x.Save()).Returns(1);

            var response = _testSubject.SaveCategory(categoryDto);

            _mockCategoryRepository.Verify(x => x.Add(It.Is<Category>(p => p.Name == "test" && p.TransactionNames == "no match")), Times.Once);
        }

        [TestMethod]
        public void SaveCategory_ReturnsTrueOnSuccessfulSave()
        {
            var categoryDto = new CategoryDto()
            {
                Name = "Test",
                TransactionNames = "No Match",
                Target = 100
            };

            _mockCategoryRepository.Setup(x => x.Save()).Returns(1);

            var response = _testSubject.SaveCategory(categoryDto);

            Assert.IsTrue(response);
        }

        [TestMethod]
        public void SaveCategory_SaveCategory_ReturnsFalseOnUnsuccessfulSave()
        {
            var categoryDto = new CategoryDto()
            {
                Name = "Test",
                TransactionNames = "No Match",
                Target = 100
            };

            _mockCategoryRepository.Setup(x => x.Save()).Returns(0);

            var response = _testSubject.SaveCategory(categoryDto);

            Assert.IsFalse(response);
        }
    }
}
