namespace House_Renting_System_Project.Services.Models.Houses
{
	public class HouseFormServiceModel
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public decimal PricePerMonth { get; set; }
		public string ImageUrl { get; set; } = string.Empty;
		public int SelectedCategoryId { get; set; }
	}
}
