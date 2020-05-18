using System.Linq;
using System.Security.Claims;
using Dvor.Common.Entities;
using Dvor.Common.Interfaces.Services;
using Dvor.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dvor.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IDishService _dishService;
        private readonly IUserService _userService;

        public CategoryController(IDishService dishService, IUserService userService)
        {
            _dishService = dishService;
            _userService = userService;
        }

        [Authorize]
        public IActionResult Get(DishSorting parameters)
        {
            var user = _userService.Get(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var allergies = user.Allergies.Select(source => source.AllergyId);
            parameters.Allergies = allergies;
            var items = _dishService.GetSorted(parameters);

            var dishesListViewModel = new DishesListViewModel
            {
                Filter = parameters,
                Dishes = items
            };

            return View(dishesListViewModel);
        }
    }
}