using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
//using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BookStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod] 
        public void Index_Contains_All_Products()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(M => M.Books).Returns(new Book[]
                {
                    new Book{ISBN=1,Title="Book1"},
                    new Book{ISBN=2,Title="Book2"},
                    new Book{ISBN=3,Title="Book3"}
                });
            AdminController target=new AdminController(mock.Object);
            //act
            Book[] result=((IEnumerable<Book>) target.Index().
                ViewData.Model).ToArray();
            //assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("Book1", result[0].Title);
            Assert.AreEqual("Book2", result[1].Title);
            Assert.AreEqual("Book3", result[2].Title);
        }
        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            AdminController target = new AdminController(mock.Object);
            Book book = new Book { Title = "Test Book" };
            //act
            ActionResult result = target.Edit(book);
            
            //assert
            mock.Verify(b => b.SaveBook(book));
            Assert.IsNotInstanceOfType(result,typeof( ViewResult));
           

        }
        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            AdminController target = new AdminController(mock.Object);
            Book book = new Book { Title = "Test Book" };
            target.ModelState.AddModelError("error", "error");
            //act
            ActionResult result = target.Edit(book);

            //assert
            //at2kd ino m4 hay7fz el book wla mra
            mock.Verify(b => b.SaveBook(It.IsAny<Book>()),Times.Never());
            Assert.IsInstanceOfType(result, typeof(ViewResult));


        }

        [TestMethod]
        public void Can_Delete_Valid_Books()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            Book book = new Book { ISBN = 1, Title = "Test Book" };
            mock.Setup(b=>b.Books).Returns(new Book[]
             {
                   
                    new Book{ISBN=2,Title="Test2"},
                    new Book{ISBN=3,Title="Test3"},
                    book
                });
               
                
            AdminController target = new AdminController(mock.Object);
            
            
            //act
            target.Delete(book.ISBN);

            //assert
            mock.Verify(b => b.DeleteBook(book.ISBN));
            


        }
        
    }
}
