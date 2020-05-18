using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dvor.Common.Entities;
using Dvor.Common.Interfaces;
using Dvor.Common.Interfaces.Services;
using Dvor.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dvor.Web.Controllers
{
    public class DishesController : Controller
    {
        private readonly IDishService _dishService;
        private readonly IMapper _mapper;
        private readonly IService<Image> _imageService;
        private readonly Cloudinary _cloudinary;

        public DishesController(IDishService dishService, IMapper mapper, IService<Image> imageService)
        {
            _dishService = dishService;
            _mapper = mapper;
            _imageService = imageService;
            var account = new Account(
                "dlqhmwg8b",
                "226824121788653",
                "Frnmg4R8irqhybtwY_Sz-_Kn5zs");

            _cloudinary = new Cloudinary(account);
        }

        [Authorize]
        public IActionResult Index()
        {
            var dishes = _dishService.GetAll();

            return View(dishes);
        }

        [Authorize]
        public IActionResult Add()
        {
            var categories = _dishService.GetCategories();
            var allergies = _dishService.GetAllergies();
            var dishCreationViewModel = new DishCreationViewModel { AllCategories = categories, AllAllergies = allergies};

            return View(dishCreationViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(DishCreationViewModel model, IEnumerable<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                if (images.Any())
                {
                    var imageLinks = new List<string>();
                    foreach (var formFile in images)
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(formFile.FileName, formFile.OpenReadStream())
                        };
                        var uploadResult = _cloudinary.Upload(uploadParams);
                        var url = uploadResult.Uri.ToString();
                        imageLinks.Add(url);
                    }

                    model.ImageUrls = imageLinks;
                }

                var dish = _mapper.Map<Dish>(model);
                _dishService.Create(dish);

                var imageList = model.ImageUrls.Select(source => new Image
                {
                    DishId=  dish.DishId,
                    Url = source
                }).ToList();

                imageList.ForEach(source => _imageService.Create(source));

                return RedirectToAction("Index");
            }

            var categories = _dishService.GetCategories();
            var allergies = _dishService.GetAllergies();
            var dishCreationViewModel = new DishCreationViewModel { AllCategories = categories, AllAllergies = allergies };

            return View(dishCreationViewModel);
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            var dish = _dishService.Get(id);
            if (dish != null)
            {
                var creationViewModel = _mapper.Map<DishCreationViewModel>(dish);
                creationViewModel.AllCategories = _dishService.GetCategories();
                creationViewModel.AllAllergies = _dishService.GetAllergies();

                return View("Add", creationViewModel);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(DishCreationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dish = _mapper.Map<Dish>(model);
                _dishService.Update(dish);

                return RedirectToAction("Index");
            }

            model.AllCategories = _dishService.GetCategories();
            model.AllAllergies = _dishService.GetAllergies();

            return View("Add", model);
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
            var dish = _dishService.Get(id);
            if (dish != null)
            {
                _dishService.Delete(id);

                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}