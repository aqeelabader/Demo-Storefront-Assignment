using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prog7311poeTask2.Models
{
    public class ProductModel
    {
        [Display(Name = "ProductId")]
        public int PId { get; set; }

        [Required(ErrorMessage = "full name required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "description required")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "category required")]
        public string ProductCategory { get; set; }

        [Required(ErrorMessage = "price required")]
        public int ProductPrice { get; set; }

        public string ProductPic { get; set; }

    }
}