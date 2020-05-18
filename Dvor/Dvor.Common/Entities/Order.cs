using Dvor.Common.Enums;
using System;
using System.Collections.Generic;

namespace Dvor.Common.Entities
{
    public class Order
    {
        public string OrderId { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalValue { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }

        public OrderStatus Status { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public bool IsDeleted { get; set; }
    }
}