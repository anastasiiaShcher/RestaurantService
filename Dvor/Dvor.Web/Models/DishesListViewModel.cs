using System.Collections.Generic;
using Dvor.Common.Entities;

namespace Dvor.Web.Models
{
    public class DishesListViewModel
    {
        public IList<Dish> Dishes { get; set; }

        public DishSorting Filter { get; set; }
    }
}