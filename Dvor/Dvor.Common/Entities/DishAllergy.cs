namespace Dvor.Common.Entities
{
    public class DishAllergy
    {
        public string DishId { get; set; }

        public Dish Dish { get; set; }

        public string AllergyId { get; set; }

        public Allergy Allergy { get; set; }
    }
}