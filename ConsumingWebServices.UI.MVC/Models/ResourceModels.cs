using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

//Consuming API - Step 1 - Create the DTO ViewModels 
namespace ConsumingWebServices.UI.MVC.Models
{
    public class ResourceViewModel
    {
        [Key]
        public int ResourceId { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "** Max 25 characters")]
        public string Name { get; set; }
        [StringLength(50, ErrorMessage = "** Max 50 characters")]
        [UIHint("MultilineText")]
        public string Description { get; set; }
        [Required]
        [StringLength(75, ErrorMessage = "** Max 75 characters")]
        public string Url { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "** Max 25 characters")]
        public string LinkText { get; set; }
        [Required]
        public int CategoryId { get; set; }

        public virtual CategoryViewModel Category { get; set; }
    }

    public class CategoryViewModel
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(25, ErrorMessage = "** Max 25 characters")]
        public string CategoryName { get; set; }
        [StringLength(50, ErrorMessage = "** Max 50 characters")]
        public string CategoryDescription { get; set; }
    }
}
