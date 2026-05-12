namespace House_renting_system_Project.Controllers;

using House_renting_system_Project.Extensions;
using House_Renting_System_Project.Services.Contracts;
using House_Renting_System_Project.Services.Models.Houses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.House;
using Models.Query;
using System.Security.Claims;

using static Common.Constants.RoleNames;

public class HouseController(IHouseService service) : Controller
{
    [HttpGet]
	public async Task<IActionResult> AllHouses(
		[FromQuery] QueryViewModel model)
	{
		if (model.CategoryId.HasValue && model.CategoryId.Value < 0)
		{
			model.CategoryId = 0;
		}

		model.SearchText = model.SearchText?.Trim();
		model.SortingType =
			model.SortingType == "Desc"
				? "Desc"
				: "Asc";

		this.ViewBag.Title = "All Houses";

		var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
		var houseQueryServiceModel = new HouseQueryServiceModel
		{
			CategoryId = model.CategoryId,
			SearchText = model.SearchText,
			SortingType = model.SortingType
		};

        var houses = await service.GetAllAsync(
			houseQueryServiceModel,
			currentUserId);

		var viewModel = new AllHousesViewModel
		{
			Query = model,
			Houses = [.. houses.Select(MapHouse)],
			Categories = [.. (await service.GetCategoriesAsync()).Select(MapCategory)]
        };

		return this.View(viewModel);
	}

	public async Task<IActionResult> Details(int id)
	{
		var house = await service.GetDetailsAsync(
			id,
			this.User.GetId()!);

		if (house is null)
		{
			return this.BadRequest();
		}

		var houseDetailViewModel = new HouseDetailViewModel
		{
			Id = house.Id,
			Name = house.Name,
			Address = house.Address,
			ImageUrl = house.ImageUrl,
			CurrentUserIsOwner = house.CurrentUserIsOwner,
			Description = house.Description,
			CreatedBy = house.CreatedBy,
			Price = house.Price
		};

        return this.View(houseDetailViewModel);
	}

	[Authorize(Roles = Agent)]
	[HttpGet]
	public async Task<IActionResult> Create()
	{
		var model = new CreateHouseModel
		{
			Categories = [.. (await service.GetCategoriesAsync()).Select(MapCategory)]
        };

		return this.View(model);
	}

    [Authorize(Roles = Agent)]
    [HttpPost]
	public async Task<IActionResult> Create(CreateHouseModel model)
	{
		if (!this.ModelState.IsValid)
        {
            model.Categories = [.. (await service.GetCategoriesAsync()).Select(MapCategory)];
			return this.View(model);
		}

		var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (string.IsNullOrWhiteSpace(userId))
		{
			return Unauthorized();
		}

		await service.CreateAsync(
			MapHouseForm(model),
			userId);

		return this.RedirectToAction(nameof(this.AllHouses));
	}

    [Authorize(Roles = Agent)]
    [HttpGet]
	public async Task<IActionResult> MyHouses()
	{
		var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (string.IsNullOrWhiteSpace(userId))
		{
			return this.Unauthorized();
		}

		this.ViewBag.Title = "My Houses";
		var houses = await service.GetUserHousesAsync(userId);

		var model = new AllHousesViewModel
		{
			Houses = [.. houses.Select(MapHouse)],
			Categories = [.. (await service.GetCategoriesAsync()).Select(MapCategory)]
        };

		return this.View(
			nameof(this.AllHouses),
			model);
	}

	[HttpGet]
    [Authorize(Roles = Agent)]
    public async Task<IActionResult> Edit(int id)
	{
		var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (string.IsNullOrWhiteSpace(currentUserId))
		{
			return this.Unauthorized();
		}

		var house = await service.GetEditModelAsync(
			id,
			currentUserId);

		if (house is null)
		{
			return this.Unauthorized();
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
            Categories = [.. (await service.GetCategoriesAsync()).Select(MapCategory)]
        };

		return this.View(model);
	}

	[HttpPost]
    [Authorize(Roles = Agent)]
    public async Task<IActionResult> Edit(CreateHouseModel model)
	{
		if (!this.ModelState.IsValid)
		{
			model.Categories = [.. (await service.GetCategoriesAsync()).Select(MapCategory)];
			return this.View(model);
		}

		var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (string.IsNullOrWhiteSpace(currentUserId))
		{
			return Unauthorized();
		}

		var isUpdated = await service.UpdateAsync(
			MapHouseForm(model),
			currentUserId);

		if (!isUpdated)
		{
			return this.Unauthorized();
		}

		return this.RedirectToAction(nameof(this.MyHouses));
	}

	[HttpPost]
    [Authorize(Roles = Agent)]
    public async Task<IActionResult> Delete(int id)
	{
		var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (string.IsNullOrWhiteSpace(currentUserId))
		{
			return this.Unauthorized();
		}

		var isDeleted = await service.DeleteAsync(
			id,
			currentUserId);

		if (!isDeleted)
		{
			return this.Unauthorized();
		}

		return this.RedirectToAction(nameof(this.MyHouses));
	}

	[HttpPost]
	[Authorize(Roles = Client)]
	public async Task<IActionResult> Rent(int id)
	{
		var currentUserId = this.User.GetId();
        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return this.Unauthorized();
        }

		var success = await service.RentAsync(
			id,
			currentUserId);

		if (success)
		{
            return this.RedirectToAction(nameof(this.AllHouses));
        }

        return this.Unauthorized();
    }

    [HttpPost]
    [Authorize(Roles = Client)]
    public async Task<IActionResult> Leave(int id)
    {
        var currentUserId = this.User.GetId();
        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return this.Unauthorized();
        }

        var success = await service.LeaveAsync(
            id,
            currentUserId);

        if (success)
        {
            return this.RedirectToAction(nameof(this.AllHouses));
        }

        return this.Unauthorized();
    }

    private static HousesViewModel MapHouse(HouseSummaryServiceModel house)
		=> new()
		{
			Id = house.Id,
			Name = house.Name,
			Address = house.Address,
			ImageUrl = house.ImageUrl,
			CurrentUserIsOwner = house.CurrentUserIsOwner,
			CurrentUserIsRenter = house.CurrentUserIsRenter,
			IsRented = house.IsRented
		};

	private static CategoryViewModel MapCategory(HouseCategoryServiceModel category)
		=> new()
		{
			Id = category.Id,
			Name = category.Name
		};

	private static HouseFormServiceModel MapHouseForm(CreateHouseModel model)
		=> new()
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
