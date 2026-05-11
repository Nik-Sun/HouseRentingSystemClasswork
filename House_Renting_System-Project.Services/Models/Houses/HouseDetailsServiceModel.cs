namespace House_Renting_System_Project.Services.Models.Houses
{
	public class HouseDetailsServiceModel : HouseSummaryServiceModel
	{
		public string Description { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public string CreatedBy { get; set; } = string.Empty;
	}
}
