using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Abstract
{
   public  interface IOrderProcessor
    {
       IEnumerable<ShippingDetails> Orders { get; }
        void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
        void SaveOrder(ShippingDetails order);
    }
}
