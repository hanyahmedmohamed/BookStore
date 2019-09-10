using System;
using Moq;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace SportsStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Arrange - create a Product with image data
            Book book = new Book
            {
                ISBN = 2,
                Title = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };
            // Arrange - create the mock repository
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[] {
                new Book {ISBN = 1, Title = "P1"},
                book,
                new Book {ISBN = 3, Title = "P3"}
                }.AsQueryable());
            // Arrange - create the controller
            BookController target = new BookController(mock.Object);
            // Act - call the GetImage action method
            ActionResult result = target.GetImage(2);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(book.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Arrange - create the mock repository
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[] {
            new Book {ISBN = 1, Title = "P1"},
            new Book {ISBN = 2, Title = "P2"}
            }.AsQueryable());
            // Arrange - create the controller
            BookController target = new BookController(mock.Object);
            // Act - call the GetImage action method
            ActionResult result = target.GetImage(100);
            // Assert
            Assert.IsNull(result);
        }
    }
}
