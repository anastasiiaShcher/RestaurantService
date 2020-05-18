namespace Dvor.Common.Entities
{
    public class Image
    {
        public string ImageId { get; set; }

        public string Url { get; set; }

        public string DishId { get; set; }

        public Dish Dish { get; set; }
    }
}