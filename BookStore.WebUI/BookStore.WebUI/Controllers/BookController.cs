using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class BookController : Controller
    {
        //
        private IBookRepository repository;
        
        public int PageSize = 4;
        public BookController(IBookRepository bookRep)
        {
            repository = bookRep;
        }
        
        
        //a5ly ell parameter el egbarya daymn elawal specilization
        public ViewResult List(string specilization, int pageno=1)
        {
            

            BookListViewModel model =
                new BookListViewModel
                {
                    Books = repository.Books
                    .Where(b=>specilization == null || b.Specialization == specilization)
                    .OrderBy(b => b.ISBN)
                    .Skip((pageno - 1) * PageSize)
                    .Take(PageSize).ToList(), 
                     PagingInfo = new Models.PagingInfo 
                     {
                         CurrentPage = pageno,
                         ItemsPerPage = PageSize,
                         TotalItems = specilization==null? 
                                                        repository.Books.Count():
                                                        repository.Books.Where(b=>b.Specialization==specilization).Count()
                     },
                     CurrentSpecilization = specilization
                     
                };
            return View(model);
        }

        public FileContentResult GetImage(int isbn)
        {
            Book book = repository.Books
            .FirstOrDefault(p => p.ISBN == isbn);
            if (book != null)
            {
                //ImageMimeType the type of the image
                return File(book.ImageData, book.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
        public ViewResult ListAll()
        {
            //return View(repository.Books.FirstOrDefault());//34an da list
            return View(repository.Books.ToList());//34an da list
        }
	}
}