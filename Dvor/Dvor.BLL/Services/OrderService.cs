using Dvor.Common.Entities;
using Dvor.Common.Entities.DTO;
using Dvor.Common.Enums;
using Dvor.Common.Interfaces;
using Dvor.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dvor.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public OrderService(IUnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mailService = mailService;
        }

        public IList<Order> GetAll()
        {
            return _unitOfWork.GetRepository<Order>().GetMany(o => !o.IsDeleted, null, TrackingState.Disabled).ToList();
        }

        public Order Get(string id)
        {
            return _unitOfWork.GetRepository<Order>().Get(
                c => c.OrderId.Equals(id),
                TrackingState.Disabled,
                "OrderDetails.Dish");
        }

        public bool IsExist(string id)
        {
            return _unitOfWork.GetRepository<Order>().IsExist(source => source.OrderId == id);
        }

        public void Create(Order item)
        {
            item.Date = DateTime.UtcNow;
            item.Status = OrderStatus.New;
            _unitOfWork.GetRepository<Order>().Create(item);
            _unitOfWork.Save();
        }

        public void Update(Order item)
        {
            var repository = _unitOfWork.GetRepository<Order>();

            if (repository.IsExist(order => order.OrderId.Equals(item.OrderId)))
            {
                var order = _unitOfWork.GetRepository<Order>().Get(source => source.OrderId == item.OrderId, TrackingState.Enabled, "OrderDetails.Dish");

                if (!order.IsDeleted)
                {
                    if (item.OrderDetails != null)
                    {
                        item.TotalValue = GetOrderValue(item.OrderDetails);
                    }

                    MapEntity(item, order);
                    _unitOfWork.Save();
                }
            }
        }

        public void Delete(string id)
        {
            var order = _unitOfWork.GetRepository<Order>().Get(source => source.OrderId == id);
            order.IsDeleted = true;
            _unitOfWork.Save();
        }

        public Order GetCurrentOrder(string userId)
        {
            return _unitOfWork.GetRepository<Order>()
                .Get(
                    order => !order.IsDeleted 
                             && order.Status == OrderStatus.New 
                             && order.UserId == userId,
                    TrackingState.Disabled,
                    "OrderDetails.Dish");
        }

        public void AddDetails(OrderDetailsDTO orderDetails)
        {
            var currentOrder = GetCurrentOrder(orderDetails.UserId);
            var details = new OrderDetails { DishId = orderDetails.DishId, Quantity = orderDetails.Quantity };

            if (currentOrder == null)
            {
                var order = new Order { UserId = orderDetails.UserId };
                Create(order);
                details.OrderId = order.OrderId;
            }
            else
            {
                details.OrderId = currentOrder.OrderId;
            }

            CreateOrderDetails(details);

            var orderToUpdate = _unitOfWork.GetRepository<Order>().Get(order => order.OrderId == details.OrderId, TrackingState.Enabled, "OrderDetails.Dish");
            orderToUpdate.TotalValue = GetOrderValue(GetDetailsForOrder(orderToUpdate.OrderId));

            _unitOfWork.Save();
        }

        public void RemoveDetails(string id)
        {
            _unitOfWork.GetRepository<OrderDetails>().Delete(id);
            _unitOfWork.Save();
        }

        public void UpdateDetailsCount(string id, short count)
        {
            var orderDetails = _unitOfWork.GetRepository<OrderDetails>().Get(source => source.OrderDetailsId == id);
            orderDetails.Quantity = count;
            _unitOfWork.Save();
            var orderToUpdate = _unitOfWork.GetRepository<Order>().Get(order => order.OrderId == orderDetails.OrderId, TrackingState.Enabled, "OrderDetails.Dish");
            orderToUpdate.TotalValue = GetOrderValue(GetDetailsForOrder(orderToUpdate.OrderId));
            _unitOfWork.Save();
        }

        public IList<OrderDetails> GetDetailsForOrder(string id)
        {
            return _unitOfWork.GetRepository<OrderDetails>()
                .GetMany(source => source.OrderId == id, null, TrackingState.Disabled, "Dish").ToList();
        }

        public void Submit(string userId)
        {
            var currentOrder = GetCurrentOrder(userId);
            var user = _unitOfWork.GetRepository<User>().Get(source => source.UserId == userId, TrackingState.Disabled);
            var mailContent = $"New order for user '{user.Name}' with total of {currentOrder.TotalValue}. Details: ";
            mailContent = currentOrder.OrderDetails.Aggregate(mailContent, (current, detail) => current + $"-{detail.Dish.Name} of count {detail.Quantity}\n");

            var notification = new Notification
            {
                Title = "New Order",
                Content = mailContent
            };

            ChangeStatus(currentOrder.OrderId, OrderStatus.Paid);
            _mailService.Send("sher210400@gmail.com", notification);

            foreach (var currentOrderOrderDetails in currentOrder.OrderDetails)
            {

                _unitOfWork.GetRepository<Dish>().Get(source => source.DishId == currentOrderOrderDetails.DishId).OrderedCount++;
                _unitOfWork.Save();
            }

        }

        public void ChangeStatus(string id, OrderStatus status)
        {
            var order = _unitOfWork.GetRepository<Order>().Get(source => source.OrderId == id);
            order.Status = status;
            _unitOfWork.Save();
        }

        private decimal GetOrderValue(IEnumerable<OrderDetails> orderDetails)
        {
            return orderDetails.Sum(o => o.Dish.Price * o.Quantity);
        }

        private void MapEntity(Order item, Order itemToUpdate)
        {
            itemToUpdate.OrderDetails.Clear();
            itemToUpdate.OrderDetails = item.OrderDetails;
            itemToUpdate.TotalValue = item.TotalValue;
            itemToUpdate.Status = item.Status;
        }

        private void CreateOrderDetails(OrderDetails orderDetails)
        {
            var repository = _unitOfWork.GetRepository<OrderDetails>();
            var details = repository
                .Get(
                    source => source.OrderId == orderDetails.OrderId &&
                              source.DishId == orderDetails.DishId);

            if (details != null)
            {
                details.Quantity += orderDetails.Quantity;
            }
            else
            {
                orderDetails.Quantity = (short)(orderDetails.Quantity > 0 ? orderDetails.Quantity : 1);
                repository.Create(orderDetails);
            }
        }
    }
}