using BankStatementApi.Controllers;
using BankStatementApi.DTOs;
using BankStatementApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Controllers
{
    [TestClass]
    public class CategoryControllerTest { 

        private CategoryController testSubject;
        private Mock<ICategoryService> mockCategoryService;
        private Mock<ITransactionService> mockTransactionService;

        [TestInitialize]
        public void Setup()
        {
            mockCategoryService = new Mock<ICategoryService>();
            mockTransactionService = new Mock<ITransactionService>();

            testSubject = new CategoryController(mockCategoryService.Object, mockTransactionService.Object);
        }

        [TestMethod]
        public void Post_CallsSaveWithPostData()
        {
            var postData = new CategoryDto();

            testSubject.Post(postData);

            mockCategoryService.Verify(x => x.SaveCategory(postData), Times.Once());
        }

        [TestMethod]
        public void Post_IfSaveOfPostDataFails_ReturnsFalse()
        {
            var postData = new CategoryDto();

            mockCategoryService.Setup(x => x.SaveCategory(postData)).Returns(false);

            var response = testSubject.Post(postData);

            Assert.AreEqual(false, response);
        }

        [TestMethod]
        public void Post_IfSaveOfPostDataSucceeds_CallsReCategoriseTransactions()
        {
            var postData = new CategoryDto();

            mockCategoryService.Setup(x => x.SaveCategory(postData)).Returns(true);

            var response = testSubject.Post(postData);

            mockTransactionService.Verify(x => x.ReCategoriseTransactions(), Times.Once());
        }

    }
}
