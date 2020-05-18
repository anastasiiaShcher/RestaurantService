using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dvor.Common.Entities;

namespace Dvor.Web.Models
{
    public class DishCreationViewModel
    {
        public string DishId { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public bool IsNew { get; set; }

        public ICollection<string> Allergies { get; set; }

        public IList<string> ImageUrls { get; set; }

        public IList<Allergy> AllAllergies{ get; set; }

        public IList<Category> AllCategories { get; set; }
    }
}