using System.Collections.Generic;
using Dvor.Common.Entities;
using Dvor.Common.Entities.DTO;
using Dvor.Common.Enums;

namespace Dvor.Common.Interfaces.Services
{
    public interface IOrderService : IService<Order>
    {
        Order GetCurrentOrder(string userId);

        void AddDetails(OrderDetailsDTO orderDetails);

        void RemoveDetails(string id);

        void UpdateDetailsCount(string id, short count);

        IList<OrderDetails> GetDetailsForOrder(string id);

        void Submit(string userId);

        void ChangeStatus(string id, OrderStatus status);
    }
}