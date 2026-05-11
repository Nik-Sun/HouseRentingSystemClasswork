using House_Renting_System_Project.Services.Contracts;

namespace House_Renting_System_Project.Services.Implementations
{
	public class StatisticsService : IStatisticsService
	{
		public int TotalRequests { get; private set; }
		public int HouseSearches { get; private set; }

		public void RegisterRequest()
		{
			TotalRequests++;
		}

		public void RegisterHouseSearch()
		{
			HouseSearches++;
		}
	}
}
