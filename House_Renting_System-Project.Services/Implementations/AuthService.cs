using House_renting_system_Project.Data.Data.Entities;
using House_Renting_System_Project.Services.Contracts;
using House_Renting_System_Project.Services.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace House_Renting_System_Project.Services.Implementations
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AuthService(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		public async Task<bool> LoginAsync(LoginServiceModel model)
		{
			var user = await userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				return false;
			}

			var isPasswordValid = await userManager.CheckPasswordAsync(user, model.Password);
			if (!isPasswordValid)
			{
				return false;
			}

			await signInManager.SignInAsync(user, model.RememberMe);
			return true;
		}

		public async Task<AuthOperationResult> RegisterAsync(RegisterServiceModel model)
		{
			var existingUser = await userManager.FindByEmailAsync(model.Email);
			if (existingUser != null)
			{
				return new AuthOperationResult
				{
					Errors = new[] { "User already exists" }
				};
			}

			var newUser = new ApplicationUser
			{
				UserName = model.Username,
				Email = model.Email
			};

			var result = await userManager.CreateAsync(newUser, model.Password);

			return new AuthOperationResult
			{
				Succeeded = result.Succeeded,
				Errors = result.Errors.Select(error => error.Description)
			};
		}

		public Task LogoutAsync()
		{
			return signInManager.SignOutAsync();
		}
	}
}
