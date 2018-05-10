using BankStatementApi.Repositories;
using BankStatementApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.Services
{
    [TestClass]
    class CategoryServiceTest
    {
        private Mock<CategoryRepository> mockCategoryRepository;
        private CategoryService testSubject;

        [TestInitialize]
        public void Setup()
        {
            mockCategoryRepository = new Mock<CategoryRepository>();
            testSubject = new CategoryService(mockCategoryRepository.Object);
        }

        [TestMethod]
        public void GetCategoryForTransactionName_ReturnsACategoryForATransactionName_WhenACategoryIsFound()
        {
            var response = testSubject.GetCategoryForTransactionName("Cake");
        }
    }
}
