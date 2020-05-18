namespace Dvor.Common.Entities
{
    public class OrderDetails
    {
        public string OrderDetailsId { get; set; }

        public short Quantity { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }

        public string DishId { get; set; }

        public Dish Dish { get; set; }
    }
}