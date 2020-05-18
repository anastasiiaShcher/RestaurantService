using System.Collections.Generic;
using System.Security.Claims;
using Dvor.Common.Entities;
using Dvor.Common.Entities.DTO;
using Dvor.Common.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dvor.Web.Controllers
{
    public class BasketController : Controller
    {
        private readonly IOrderService _orderService;

        public BasketController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var currentOrder = _orderService.GetCurrentOrder(User.FindFirst(ClaimTypes.NameIdentifier).Value) ?? new Order{OrderDetails = new List<OrderDetails>()};

            return View(currentOrder);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(string dishId, short quantity)
        {
            var orderDetailsDto = new OrderDetailsDTO
            {
                DishId = dishId,
                Quantity = quantity,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value
            };

            _orderService.AddDetails(orderDetailsDto);

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult RemoveDetails(string id)
        {
            _orderService.RemoveDetails(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangeCount(string id, short quantity)
        {
            _orderService.UpdateDetailsCount(id, quantity);

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Submit()
        {
            _orderService.Submit(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return RedirectToAction("Index", "Home");
        }
    }
}