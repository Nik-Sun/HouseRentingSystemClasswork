using House_Renting_System_Project.Services.Contracts;

namespace House_renting_system_Project.Middlewares
{
	public class CustomMiddleware
	{
		private RequestDelegate next;

		public CustomMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext, IStatisticsService service)
		{
			service.RegisterRequest();
			await next(httpContext);
		}
	}
}
