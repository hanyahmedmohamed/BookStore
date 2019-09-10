using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using BookStore.WebUI.HtmlHelper;
//using System.Web.WebPages.Html;
using BookStore.WebUI.Models;
using System.Web.Mvc;
//using System.Web.Mvc;
using System.Web;


namespace BookStore.UnitTests
{
    [TestClass]
    public class ProductCatalog
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[] 
            {
                new  Book{ISBN=1, Title="book1"},
                new  Book{ISBN=2, Title="book2"},
                new  Book{ISBN=3, Title="book3"},
                new  Book{ISBN=4, Title="book4"},
                new  Book{ISBN=5, Title="book5"}
                
            });
            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;
            //act
            BookListViewModel result =
                (BookListViewModel)controller.List(null,3).Model;

            //assert
            Book[] bookArray = result.Books.ToArray();
            Assert.IsTrue(bookArray.Length==0);
            //Assert.AreEqual(bookArray[0].Title, "book4");
            //Assert.AreEqual(bookArray[1].Title, "book5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //arrange
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 14,
                ItemsPerPage = 5

            };
            //hayktb masln page 5
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            String expectedResult = 
                "<a class= \"btn btn-default\"  href = \"Page1\">1</a> "
                + "<a class= \"btn btn-default btn-primary selected\"  href = \"Page2\">2</a> "
               + "<a class= \"btn btn-default\"  href = \"Page3\">3</a> ";


            //act
            String result = myHelper.PageLinks(pagingInfo,pageUrlDelegate).ToString();
            //assert
            Assert.AreEqual( expectedResult,  result );
        }
        [TestMethod]
        public void Can_Send_Pagination_view_Model()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(
                new Book[]{
                new Book { ISBN=1, Title ="Operating system"},
                new Book { ISBN=2, Title ="web application asp.net"},
                new Book { ISBN=3, Title ="android mobil application"},
                new Book { ISBN=4, Title ="database system"},
                new Book { ISBN=5, Title ="mis"}
                });
            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;
            //act
            BookListViewModel result =(BookListViewModel)controller.List(null,2).Model;

            //assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);

        }

        [TestMethod]
        public void Can_Filter_Books()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(
                new Book[]{
                new Book { ISBN=1, Title ="Operating system",Specialization="cs"},
                new Book { ISBN=2, Title ="web application asp.net",Specialization="is"},
                new Book { ISBN=3, Title ="android mobil application",Specialization="is"},
                new Book { ISBN=4, Title ="database system",Specialization="is"},
                new Book { ISBN=5, Title ="mis",Specialization="is"}
                });
            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;
            //act
            //han5tar el book bs mn el model
            Book[] result =((BookListViewModel)controller.List("is", 2).Model).Books.ToArray();
            //assert
            Assert.AreEqual(result.Length, 1);
            Assert.IsTrue(result[0].Title == "mis" &&result[0].Specialization=="is");
        }

        [TestMethod]
        public void Can_Create_Specilization()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(
                new Book[]{
                new Book { ISBN=1, Title ="Operating system",Specialization="cs"},
                new Book { ISBN=2, Title ="web application asp.net",Specialization="is"},
                new Book { ISBN=3, Title ="android mobil application",Specialization="is"},
                new Book { ISBN=4, Title ="database system",Specialization="is"},
                new Book { ISBN=5, Title ="mis",Specialization="is"}
                });
            NavController controller = new NavController(mock.Object);
            
            //act
            string[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();
            //assert
            //result hea 3dd el aksam elly 3andy
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0] == "cs" && result[1] == "is");
        }

        [TestMethod]
        public void Indicates_Selected_Spec()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(
                new Book[]{
                new Book { ISBN=1, Title ="Operating system",Specialization="cs"},
                new Book { ISBN=2, Title ="web application asp.net",Specialization="is"},
                new Book { ISBN=3, Title ="android mobil application",Specialization="is"},
                new Book { ISBN=4, Title ="database system",Specialization="is"},
                new Book { ISBN=5, Title ="mis",Specialization="is"}
                });
            NavController controller = new NavController(mock.Object);

            //act
            string result = controller.Menu("is").ViewBag.SelectedSpec;
            //assert
            //result hea 3dd el aksam elly 3andy
            Assert.AreEqual("is",result);
           

        }

        [TestMethod]
        public void Generate_Spec_Specific_Book_Count()
        {
            //arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(
                new Book[]{
                new Book { ISBN=1, Title ="Operating system",Specialization="cs"},
                new Book { ISBN=2, Title ="web application asp.net",Specialization="is"},
                new Book { ISBN=3, Title ="android mobil application",Specialization="is"},
                new Book { ISBN=4, Title ="database system",Specialization="cs"},
                new Book { ISBN=5, Title ="mis",Specialization="is"}
                });
            BookController controller = new BookController(mock.Object);

            //act
            int result1 = ((BookListViewModel)controller.List("is").
                Model).PagingInfo.TotalItems;
            int result2 = ((BookListViewModel)controller.List("cs").
                Model).PagingInfo.TotalItems;

            //assert
            //result hea 3dd el aksam elly 3andy
            Assert.AreEqual(result1, 3);
            Assert.AreEqual(result2, 2);

        }



    }
}
