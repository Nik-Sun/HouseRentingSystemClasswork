using House_renting_system_Project.Models.Auth;
using House_Renting_System_Project.Services.Contracts;
using House_Renting_System_Project.Services.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace House_renting_system_Project.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService authService;

		public AuthController(IAuthService authService)
		{
			this.authService = authService;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid == false)
			{
				return View(model);
			}

			var isLoggedIn = await authService.LoginAsync(new LoginServiceModel
			{
				Email = model.Email,
				Password = model.Password,
				RememberMe = model.RememberMe
			});

			if (isLoggedIn)
			{
				return RedirectToAction("Index", "Home");
			}

			ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			return View(model);
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid == false)
			{
				return View(model);
			}

			var result = await authService.RegisterAsync(new RegisterServiceModel
			{
				Username = model.Username,
				Email = model.Email,
				Password = model.Password
			});

			if (result.Succeeded)
			{
				TempData["Success"] = "User Created Successfully!";
				return RedirectToAction(nameof(Login));
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error);
			}

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await authService.LogoutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
