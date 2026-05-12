namespace House_renting_system_Project.Models.Auth;

using System.ComponentModel.DataAnnotations;

using static Common.Constants.RoleNames;

public class RegisterViewModel
{
	[Required]
	[StringLength(30, MinimumLength = 3)]
	[Display(Name = "Username")]
	public string Username { get; set; } = null!;

	[Required]
	[EmailAddress]
	[Display(Name = "Email")]
	public string Email { get; set; } = null!;

	[Required]
	[DataType(DataType.Password)]
	[StringLength(100, MinimumLength = 6)]
	[Display(Name = "Password")]
	public string Password { get; set; } = null!;

	[Required]
	[AllowedValues(Agent, Client, ErrorMessage = "Invalid Role Name")]
	public string Role { get; init; } = default!;
}
