using Dvor.Common.Entities;
using System.Collections.Generic;

namespace Dvor.Common.Interfaces.Services
{
    public interface IDishService : IService<Dish>
    {
        IList<Dish> GetSorted(DishSorting parameters);
        IList<Category> GetCategories();
        IList<Allergy> GetAllergies();
    }
}