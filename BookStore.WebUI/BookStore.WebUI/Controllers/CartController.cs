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
    public class CartController : Controller
    {

        private IBookRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IBookRepository repo,IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int isbn, string returnUrl)
        {
            Book book=repository.Books.FirstOrDefault(b=>b.ISBN==isbn);
            if(book!=null)
            {
                //getcart().additem(book);
                cart.AddItem(book);
            }

            return RedirectToAction("Index", new { returnUrl }); //Index
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int isbn, string returnUrl)
        {
            Book book = repository.Books.FirstOrDefault(b => b.ISBN == isbn);
            if (book != null)
            {
                cart.RemoveLine(book);
                //getcart().removeline(book);
            }

            return RedirectToAction("Index", new { returnUrl }); //Index
        }
        //mmok a7tag a7zfha
        //public  Cart GetCart()
        //{
        //    Cart cart =(Cart) Session["Cart"];
        //    if(cart==null)
        //    {
        //        cart = new Cart();
        //        Session["Cart"] = cart;
                
        //    }
        //    return cart;
            
        //}

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }
        [HttpPost]
        public ViewResult Checkout(Cart cart,ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
                ModelState.AddModelError("", "Sorry your cart is empty");
            if(ModelState.IsValid)
            {
                orderProcessor.SaveOrder(shippingDetails);
               
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }else
                //shipingDetails elly hea mab3otaly
                return View(shippingDetails);
        }

	}
}

//if(ModelState.IsValid)
//            {
//                repository.SaveBook(book);
//                //7aga 4bh el session leha w2t w ynthy
//                TempData["message"] = book.Title + "has been saved";
//                return RedirectToAction("Index");

//            }
//            else
//            {
//                return View(book);
//            }