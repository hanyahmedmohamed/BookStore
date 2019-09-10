using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Domain.Entities
{
   public class Book
    {
       [Key]
       //[HiddenInput(DisplayValue= false)]
        public int ISBN { get; set; }
       [Required(ErrorMessage="please enter book title")]
        public string Title { get; set; }
       [DataType(DataType.MultilineText)]
       [Required(ErrorMessage = "please enter book Description")]
        public string Description { get; set; }
       [Required(ErrorMessage = "please enter book price")]
       [Range(0.1,9999.99,ErrorMessage="Please enter a positive price")]
        public decimal Price { get; set; }
       [Required]
        public string Specialization { get; set; }
        //public string Author { get; set; }

       public byte[] ImageData { get; set; }
       public string ImageMimeType { get; set; }
    }
}
