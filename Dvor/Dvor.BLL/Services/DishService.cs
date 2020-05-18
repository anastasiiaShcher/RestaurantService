using Dvor.BLL.Infrastructure;
using Dvor.Common.Entities;
using Dvor.Common.Enums;
using Dvor.Common.Interfaces;
using Dvor.Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dvor.BLL.Services
{
    public class DishService : IDishService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DishService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Dish> GetAll()
        {
            return _unitOfWork.GetRepository<Dish>().GetMany(source => !source.IsDeleted, null, TrackingState.Disabled, "Category").ToList();
        }

        public Dish Get(string id)
        {
            return _unitOfWork.GetRepository<Dish>().Get(source => source.DishId == id, TrackingState.Disabled, "Category", "Allergies");
        }

        public bool IsExist(string id)
        {
            return _unitOfWork.GetRepository<Dish>().IsExist(Dish => Dish.DishId == id);
        }

        public void Create(Dish item)
        {
            _unitOfWork.GetRepository<Dish>().Create(item);
            _unitOfWork.Save();
        }

        public void Update(Dish item)
        {
            var DishToUpdate = _unitOfWork.GetRepository<Dish>().Get(source => source.DishId == item.DishId, TrackingState.Enabled, "Allergies");
            MapEntity(item, DishToUpdate);
            _unitOfWork.Save();
        }

        public void Delete(string id)
        {
            var dish = _unitOfWork.GetRepository<Dish>().Get(source => source.DishId == id);
            dish.IsDeleted = true;
            _unitOfWork.Save();
        }

        public IList<Dish> GetSorted(DishSorting parameters)
        {
            Expression<Func<Dish, bool>> param = g => !g.IsDeleted;
            var conditionExpressions = new List<Expression<Func<Dish, bool>>>
            {
                param
            };

            if (!string.IsNullOrEmpty(parameters.CategoryId))
            {
                param = dish => dish.CategoryId == parameters.CategoryId;
                conditionExpressions.Add(param);
            }

            if (parameters.NewOnly)
            {
                param = dish => dish.IsNew;
                conditionExpressions.Add(param);
            }

            if (parameters.Allergies != null && parameters.Allergies.Any())
            {
                param = g => !g.Allergies
                    .Any(gn => parameters.Allergies
                        .Contains(gn.AllergyId));

                conditionExpressions.Add(param);
            }

            var condition = ExpressionActions.CombinePredicates(conditionExpressions, Expression.AndAlso);
            var repository = _unitOfWork.GetRepository<Dish>();
            var result = condition != null 
                ? repository.GetMany(condition, null, TrackingState.Disabled, "Allergies.Allergy", "Category", "Images")
                : repository.GetMany(source => !source.IsDeleted, null, TrackingState.Disabled, "Allergies.Allergy", "Category", "Images");

            switch (parameters.SortingMethod)
            {
                case SortingMethod.PriceAsc:
                    result = result.OrderBy(g => g.Price);
                    break;
                case SortingMethod.PriceDesc:
                    result = result.OrderByDescending(g => g.Price);
                    break;
                case SortingMethod.Popular:
                    result = result.OrderByDescending(g => g.OrderedCount);
                    break;
            }

            return result.ToList();

        }

        public IList<Category> GetCategories()
        {
            return _unitOfWork.GetRepository<Category>().GetMany(source => true, source => source.Name, TrackingState.Disabled).ToList();
        }

        public IList<Allergy> GetAllergies()
        {
            return _unitOfWork.GetRepository<Allergy>().GetMany(source => true, source => source.Name, TrackingState.Disabled).ToList();
        }

        private void MapEntity(Dish item, Dish itemToUpdate)
        {
            itemToUpdate.Name = item.Name;
            itemToUpdate.Price = item.Price;
            itemToUpdate.CategoryId = item.CategoryId;

            itemToUpdate.Allergies?.Clear();
            itemToUpdate.Allergies = item.Allergies;
        }
    }
}