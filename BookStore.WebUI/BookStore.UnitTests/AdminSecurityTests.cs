using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Models;
using BookStore.WebUI.Controllers;
using System.Web.Mvc;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            //arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);
            LoginViewModel model = new LoginViewModel
            {
                Username="admin",
                Password="secret"
            };
            AccountController target = new AccountController(mock.Object);
            //act
            ActionResult result = target.Login(model, "/MyUrl");
            //assert
            Assert.IsInstanceOfType(result,typeof( RedirectResult));
            Assert.AreEqual("/MyUrl", ((RedirectResult) result).Url);
        }

        [TestMethod]
        public void Can_Login_With_Invalid_Credentials()
        {
            //arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("userx", "passx")).Returns(false);
            LoginViewModel model = new LoginViewModel
            {
                Username = "userx",
                Password = "passx"
            };
            AccountController target = new AccountController(mock.Object);
            //act
            ActionResult result = target.Login(model, "/MyUrl");
            //assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
        [TestMethod]
        public void Can_Edit_Book()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]
                {
                    new Book{ISBN=1,Title="Book1"},
                    new Book{ISBN=2,Title="Book2"},
                    new Book{ISBN=3,Title="Book3"}
                });
            AdminController target = new AdminController(mock.Object);

            //act
            Book b1 = target.Edit(1).ViewData.Model as Book;
            Book b2 = target.Edit(2).ViewData.Model as Book;
            Book b3 = target.Edit(3).ViewData.Model as Book;
            //assert
            
            Assert.AreEqual("Book1", b1.Title);
            Assert.AreEqual("Book2", b2.Title);
            Assert.AreEqual(2, b2.ISBN);
            //Assert.AreEqual("Book2", b2.Title);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexist_Book()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]
                {
                    new Book{ISBN=1,Title="Book1"},
                    new Book{ISBN=2,Title="Book2"},
                    new Book{ISBN=3,Title="Book3"}
                });
            AdminController target = new AdminController(mock.Object);

            //act

            Book b4 = target.Edit(4).ViewData.Model as Book;
            //assert

            Assert.IsNull(b4);

        }
    }
}
