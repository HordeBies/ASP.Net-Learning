using Assignment_ModelBindingAndValidations.CustomValidators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Assignment_ModelBindingAndValidations.Models
{
    public class Order
    {
        [BindNever]
        public int? OrderNo { get; set; }
        [Required]
        [MinimumYear(2000)]
        public DateTime OrderDate { get; set; }
        [Required]
        public double InvoicePrice { get; set; }
        [Required]
        [MinItemCount(1)]
        [Sum("InvoicePrice")]
        public List<Product> Products { get; set; }
    }
}
