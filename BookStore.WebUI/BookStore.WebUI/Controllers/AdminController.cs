using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IBookRepository repository;
        public AdminController(IBookRepository repo)
        {
            repository = repo;
        }

        //mogard 3rd
        public ViewResult Index()
        {
            return View(repository.Books);
        }
        public ViewResult Edit(int ISBN)
        {
            Book book = repository.Books.FirstOrDefault(b => b.ISBN == ISBN);
            return View(book);
        }
        [HttpPost]
        public ActionResult Edit(Book book , HttpPostedFileBase image = null)
        {
            //bt2kd en el data salema
            if(ModelState.IsValid)
            {
                if (image != null)
                {
                    book.ImageMimeType =  image.ContentType;
                    book.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(book.ImageData, 0, image.ContentLength);
                }
                repository.SaveBook(book);
                //7aga 4bh el session leha w2t w ynthy
                TempData["message"] = book.Title + "has been saved";
                return RedirectToAction("Index");

            }
            else
            {
                return View(book);
            }
        }
        public ViewResult Create()
        {
            return View("Edit", new Book());
        }
        [HttpGet]
        public ActionResult Delete(int ISBN)
        {
            Book deletedBook = repository.DeleteBook(ISBN);
            if(deletedBook !=null)
            {
                TempData["message"] = deletedBook.Title + "was deleted";
            }
            return RedirectToAction("Index");
        }
	}
}