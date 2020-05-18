using AutoMapper;
using Dvor.Common.Entities;
using Dvor.Web.Models;
using System.Linq;

namespace Dvor.Web.Infrastructure
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<Dish, DishCreationViewModel>()
                .ForMember(creation => creation.Allergies, dish => dish.MapFrom(g => g.Allergies.Select(dc => dc.AllergyId)))
                .ForMember(
                    destination => destination.ImageUrls,
                    source => source.MapFrom(s => s.Images.Select(image => image.Url)));

            CreateMap<DishCreationViewModel, Dish>()
                .ForMember(dish => dish.Allergies, creation => creation.MapFrom(g => g.Allergies.Select(dc => new DishAllergy { AllergyId = dc })));
        }
    }
}