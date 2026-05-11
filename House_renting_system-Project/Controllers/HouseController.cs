using House_renting_system_Project.Models.House;
using House_renting_system_Project.Models.Query;
using House_Renting_System_Project.Services.Contracts;
using House_Renting_System_Project.Services.Models.Houses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace House_renting_system_Project.Controllers
{
	public class HouseController : Controller
	{
		private readonly IHouseService houseService;

		public HouseController(IHouseService houseService)
		{
			this.houseService = houseService;
		}

		[HttpGet]
		public async Task<IActionResult> AllHouses([FromQuery] QueryViewModel model)
		{
			if (model.CategoryId.HasValue && model.CategoryId.Value < 0)
			{
				model.CategoryId = 0;
			}

			model.SearchText = model.SearchText?.Trim();
			model.SortingType = model.SortingType == "Desc" ? "Desc" : "Asc";

			ViewBag.Title = "All Houses";
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var houses = await houseService.GetAllAsync(new HouseQueryServiceModel
			{
				CategoryId = model.CategoryId,
				SearchText = model.SearchText,
				SortingType = model.SortingType
			}, currentUserId);

			var viewModel = new AllHousesViewModel
			{
				Query = model,
				Houses = houses.Select(MapHouse).ToList(),
				Categoryies = (await houseService.GetCategoriesAsync()).Select(MapCategory).ToList()
			};

			return View(viewModel);
		}

		public async Task<IActionResult> Details(int id)
		{
			var house = await houseService.GetDetailsAsync(id);
			if (house == null)
			{
				return BadRequest();
			}

			return View(new HouseDetailViewModel
			{
				Id = house.Id,
				Name = house.Name,
				Address = house.Address,
				ImageUrl = house.ImageUrl,
				CurrentUserIsOwner = house.CurrentUserIsOwner,
				Description = house.Description,
				CreatedBy = house.CreatedBy,
				Price = house.Price
			});
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var model = new CreateHouseModel
			{
				Categories = (await houseService.GetCategoriesAsync()).Select(MapCategory).ToList()
			};

			return View(model);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Create(CreateHouseModel model)
		{
			if (ModelState.IsValid == false)
			{
				model.Categories = (await houseService.GetCategoriesAsync()).Select(MapCategory).ToList();
				return View(model);
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrWhiteSpace(userId))
			{
				return Unauthorized();
			}

			await houseService.CreateAsync(MapHouseForm(model), userId);

			return RedirectToAction(nameof(AllHouses));
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> MyHouses()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrWhiteSpace(userId))
			{
				return Unauthorized();
			}

			ViewBag.Title = "My Houses";
			var houses = await houseService.GetUserHousesAsync(userId);
			var model = new AllHousesViewModel
			{
				Houses = houses.Select(MapHouse).ToList(),
				Categoryies = (await houseService.GetCategoriesAsync()).Select(MapCategory).ToList()
			};

			return View(nameof(AllHouses), model);
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Edit(int id)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrWhiteSpace(currentUserId))
			{
				return Unauthorized();
			}

			var house = await houseService.GetEditModelAsync(id, currentUserId);
			if (house == null)
			{
				return Unauthorized();
			}

			var model = new CreateHouseModel
			{
				Id = house.Id,
				Address = house.Address,
				Description = house.Description,
				ImageUrl = house.ImageUrl,
				PricePerMonth = house.PricePerMonth,
				Title = house.Title,
				SelectedCategoryId = house.SelectedCategoryId,
				Categories = (await houseService.GetCategoriesAsync()).Select(MapCategory).ToList()
			};

			return View(model);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Edit(CreateHouseModel model)
		{
			if (ModelState.IsValid == false)
			{
				model.Categories = (await houseService.GetCategoriesAsync()).Select(MapCategory).ToList();
				return View(model);
			}

			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrWhiteSpace(currentUserId))
			{
				return Unauthorized();
			}

			var isUpdated = await houseService.UpdateAsync(MapHouseForm(model), currentUserId);
			if (!isUpdated)
			{
				return Unauthorized();
			}

			return RedirectToAction(nameof(MyHouses));
		}

		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrWhiteSpace(currentUserId))
			{
				return Unauthorized();
			}

			var isDeleted = await houseService.DeleteAsync(id, currentUserId);
			if (!isDeleted)
			{
				return Unauthorized();
			}

			return RedirectToAction(nameof(MyHouses));
		}

		private static HousesViewModel MapHouse(HouseSummaryServiceModel house)
		{
			return new HousesViewModel
			{
				Id = house.Id,
				Name = house.Name,
				Address = house.Address,
				ImageUrl = house.ImageUrl,
				CurrentUserIsOwner = house.CurrentUserIsOwner
			};
		}

		private static CategoryViewModel MapCategory(HouseCategoryServiceModel category)
		{
			return new CategoryViewModel
			{
				Id = category.Id,
				Name = category.Name
			};
		}

		private static HouseFormServiceModel MapHouseForm(CreateHouseModel model)
		{
			return new HouseFormServiceModel
			{
				Id = model.Id,
				Title = model.Title,
				Description = model.Description,
				Address = model.Address,
				PricePerMonth = model.PricePerMonth,
				ImageUrl = model.ImageUrl,
				SelectedCategoryId = model.SelectedCategoryId
			};
		}
	}
}
