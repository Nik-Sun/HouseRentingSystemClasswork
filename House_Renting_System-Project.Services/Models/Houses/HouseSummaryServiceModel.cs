namespace House_Renting_System_Project.Services.Models.Houses
{
	public class HouseSummaryServiceModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public string ImageUrl { get; set; } = string.Empty;
		public bool CurrentUserIsOwner { get; set; }
	}
}
