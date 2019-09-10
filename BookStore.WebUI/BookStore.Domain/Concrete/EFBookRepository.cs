using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
//using System.Data.Entity.ModelConfiguration;

namespace BookStore.Domain.Concrete
{
    public class EFBookRepository: IBookRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Book> Books
        {
            get
            {
                //EFDbContext context = new EFDbContext();
                return context.Books;

            }
        }
        //public IEnumerable<ShippingDetails> Orders
        //{
        //    get
        //    {
        //        return context.Orders
        //            .Include(o => o.OrderLines
        //              .Select(ol => ol.Book));
        //    }
        //}

        //public void SaveOrder(ShippingDetails order)
        //{

        //    ShippingDetails dbOrder = context.Orders.Find(order.ShippingID);
        //    if (dbOrder == null)
        //        context.Orders.Add(order);

        //    else
        //    {
        //        dbOrder.Name = order.Name;
        //        dbOrder.Line1 = order.Line1;
        //        dbOrder.Line2 = order.Line2;
                
        //        dbOrder.City = order.City;
        //        dbOrder.State = order.State;
        //        dbOrder.GiftWrap = order.GiftWrap;
                
        //    }
        //    context.SaveChanges();

        //    ///////////////// from book ////////////////////////
        //    //if (order.OrderId == 0)
        //    //{
        //    //    order = context.Orders.Add(order);
        //    //    foreach (OrderLine line in order.OrderLines)
        //    //    {
        //    //        context.Entry(line.Book).State
        //    //        = System.Data.EntityState.Modified;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    Order dbOrder = context.Orders.Find(order.OrderId);
        //    //    if (dbOrder != null)
        //    //    {
        //    //        dbOrder.Name = order.Name;
        //    //        dbOrder.Line1 = order.Line1;
        //    //        dbOrder.Line2 = order.Line2;
        //    //        dbOrder.Line3 = order.Line3;
        //    //        dbOrder.City = order.City;
        //    //        dbOrder.State = order.State;
        //    //        dbOrder.GiftWrap = order.GiftWrap;
        //    //        dbOrder.Dispatched = order.Dispatched;
        //    //    }
        //    //}
        //    //context.SaveChanges();
        //}


        public void SaveBook(Book book)
        {

            Book dbEntity = context.Books.Find(book.ISBN);
            if(dbEntity==null)
                    context.Books.Add(book);
          
            else
                {
                    dbEntity.Title = book.Title;
                    dbEntity.Specialization = book.Specialization;
                    dbEntity.Price = book.Price;
                    dbEntity.Description = book.Description;
                    dbEntity.ImageData = book.ImageData;
                    dbEntity.ImageMimeType = book.ImageMimeType;
                }
            context.SaveChanges();
        }
        public Book DeleteBook(int ISBN)
        {
            Book dbEntity = context.Books.Find(ISBN);
            if(dbEntity != null)
            {
                context.Books.Remove(dbEntity);
                context.SaveChanges();
            }
            return dbEntity;
        }
    }
}
