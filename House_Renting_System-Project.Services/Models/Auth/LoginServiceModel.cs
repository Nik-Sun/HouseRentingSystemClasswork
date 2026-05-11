namespace House_Renting_System_Project.Services.Models.Auth
{
	public class LoginServiceModel
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public bool RememberMe { get; set; }
	}
}
