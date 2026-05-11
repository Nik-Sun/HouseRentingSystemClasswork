using House_Renting_System_Project.Services.Models.Auth;

namespace House_Renting_System_Project.Services.Contracts
{
	public interface IAuthService
	{
		Task<bool> LoginAsync(LoginServiceModel model);
		Task<AuthOperationResult> RegisterAsync(RegisterServiceModel model);
		Task LogoutAsync();
	}
}
