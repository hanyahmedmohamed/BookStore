using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> LineCollection = new List<CartLine>() ;
        public void AddItem(Book book,int quantity =1)
        {
            //34an adef 3la elly maowgod
            //34an at2kd eny ma5tartch elktab da 2bl kda
            CartLine line = LineCollection
                .Where(b=>b.Book.ISBN==book.ISBN)
                //yrg3 awl 7aga
                .FirstOrDefault();

            if(line==null)
            {
                LineCollection.Add(new CartLine{
                    Book= book, Quantity=quantity});
            }else
                line.Quantity +=quantity;
        }//end add item

        public void RemoveLine(Book book)
        {
            LineCollection.RemoveAll(b=>b.Book.ISBN==book.ISBN);
        }

        public decimal ComputeTotalValue()
        {
            return LineCollection.Sum(cl=>cl.Book.Price*cl.Quantity);
        }

        public void Clear()
        {
            LineCollection.Clear();
        }
        //property
        public IEnumerable<CartLine> Lines
        {
            get {return LineCollection;} 
        }
    }

    public class CartLine
    {
        public Book Book {get; set;}
        public int Quantity {get; set;}
    }
}
