using System.Collections.Generic;

namespace Dvor.Common.Entities
{
    public class Allergy
    {
        public string AllergyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public ICollection<DishAllergy> Dishes { get; set; }

        public ICollection<UserAllergy> Users { get; set; }
    }
}