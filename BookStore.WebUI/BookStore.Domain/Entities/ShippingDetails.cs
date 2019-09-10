using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required( ErrorMessage ="Please enter a name")]
        public string Name { get; set; }
        [Key]
        public int ShippingID { get; set; }
        //awel address
        [Required(ErrorMessage = "Please enter a first address line")]
        [Display(Name="Address Line 1")]
        public string Line1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string Line2 { get; set; }
         [Required(ErrorMessage = "Please enter City Name")]
        public string City { get; set; }
        public string State { get; set; }
         [Required(ErrorMessage = "Please enter Country Name")]
        public string Country { get; set; }
        public bool GiftWrap { get; set; }
        public virtual List<OrderLine> OrderLines { get; set; }


    }

    public class OrderLine
    {
        public int OrderLineId { get; set; }
        public ShippingDetails Order { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
