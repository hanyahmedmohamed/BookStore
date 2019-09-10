using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Domain.Entities;
using System.Linq;
using Moq;
using BookStore.Domain.Abstract;
using BookStore.WebUI.Controllers;
using System.Web.Mvc;
using BookStore.WebUI.Models;

namespace BookStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_AddNew_Lines()
        {
            //arrange
            Book b1 = new Book { ISBN = 1, Title = "Asp.net" };
            Book b2 = new Book { ISBN = 2, Title = "oracle" };

            //act
            Cart target = new Cart();
            target.AddItem(b1);
            target.AddItem(b2,3);
            CartLine[] result = target.Lines.ToArray();
            //assert
            Assert.AreEqual(result[0].Book, b1);
            Assert.AreEqual(result[1].Book, b2);
        }
        [TestMethod]
        public void Can_Add_Qty_For_Existing_Lines()
        {
            //arrange
            Book b1 = new Book { ISBN = 1, Title = "Asp.net" };
            Book b2 = new Book { ISBN = 2, Title = "oracle" };

            //act
            Cart target = new Cart();
            target.AddItem(b1);
            target.AddItem(b2, 3);
            target.AddItem(b1,5);

            CartLine[] result = target.Lines.OrderBy(cl=>cl.Book.ISBN).ToArray();
            //assert
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Quantity, 6);
            Assert.AreEqual(result[1].Quantity,3);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            //arrange
            Book b1 = new Book { ISBN = 1, Title = "Asp.net" };
            Book b2 = new Book { ISBN = 2, Title = "oracle" };
            Book b3 = new Book { ISBN = 3, Title = "C#" };
            //act
            Cart target = new Cart();
            target.AddItem(b1);
            target.AddItem(b2, 3);
            target.AddItem(b3, 5);
            target.AddItem(b2, 1);
            target.RemoveLine(b2);
            //assert
            Assert.AreEqual(target.Lines.Where(cl=>cl.Book==b2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);

        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            //arrange
            Book b1 = new Book { ISBN = 1, Title = "Asp.net" ,Price= 100M};
            Book b2 = new Book { ISBN = 2, Title = "oracle", Price = 50M };
            Book b3 = new Book { ISBN = 3, Title = "C#", Price = 75M };
            //act
            Cart target = new Cart();
            target.AddItem(b1,1);
            target.AddItem(b2, 2);
            target.AddItem(b3);
            decimal result = target.ComputeTotalValue();

            //assert
            Assert.AreEqual(result, 275M);
            

        }

        public void Can_Clear_Contents()
        {
            //arrange
            Book b1 = new Book { ISBN = 1, Title = "Asp.net" };
            Book b2 = new Book { ISBN = 2, Title = "oracle" };
            Book b3 = new Book { ISBN = 3, Title = "C#" };
            //act
            Cart target = new Cart();
            target.AddItem(b1);
            target.AddItem(b2, 3);
            target.AddItem(b3, 5);
            target.AddItem(b2, 1);
            target.Clear();
            //assert
            Assert.AreEqual(target.Lines.Count(), 0);

        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m=>m.Books).Returns(
                new Book[]
                {
                      new Book { ISBN = 1, Title = "Asp.net" ,Specialization="programming"},
                      //new Book { ISBN = 2, Title = "oracle" },
                      //new Book { ISBN = 3, Title = "C#" }
                  //kabl l3amlyat el est3lam
                }.AsQueryable()
                );

            Cart cart = new Cart();
            CartController target = new CartController(mock.Object,null);
            //act
            target.AddToCart(cart,1,null);
            //RedirectToRouteResult result = target.AddToCart(cart,2,"myUrl");
            //assert
            Assert.AreEqual(cart.Lines.Count(),1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Book.Title, "Asp.net");
           // Assert.AreEqual(result.RouteValues["returnUrl"],"myUrl");
        }

        [TestMethod]
        public void Adding_Book_To_Cart_Goes_To_Cart_Screen()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(
                new Book[]
                {
                      new Book { ISBN = 1, Title = "Asp.net" ,Specialization="programming"},
                      //new Book { ISBN = 2, Title = "oracle" },
                      //new Book { ISBN = 3, Title = "C#" }
                  //kabl l3amlyat el est3lam
                }.AsQueryable()
                );

            Cart cart = new Cart();
            CartController target = new CartController(mock.Object,null);
            //act
            //target.AddToCart(cart, 1, null);
            RedirectToRouteResult result = target.AddToCart(cart,2,"myUrl");
            //assert
           // Assert.AreEqual(cart.Lines.Count(), 1);
            //Assert.AreEqual(cart.Lines.ToArray()[0].Book.Title, "Asp.net");
            Assert.AreEqual(result.RouteValues["action"], "Index");
             Assert.AreEqual(result.RouteValues["returnUrl"],"myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Content()
        {
            //arrange
            Cart cart = new Cart();
            CartController target = new CartController(null,null);
            //act
            CartIndexViewModel result =(CartIndexViewModel) 
                target.Index(cart, "myUrl").ViewData.Model;
            //assert

            Assert.AreEqual(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            //arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);
            //Act
            ViewResult result = target.Checkout(cart, shippingDetails);
            //Assert
            //mock.Verify(m=>m.ProcessOrder)
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);
            target.ModelState.AddModelError("error", "error");
            //Act
            ViewResult result = target.Checkout(cart, shippingDetails);
            //Assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            //arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);

            //Act
            ViewResult result = target.Checkout(cart, shippingDetails);
            //Assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(),
                It.IsAny<ShippingDetails>()), Times.Once());
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }

    }
}
