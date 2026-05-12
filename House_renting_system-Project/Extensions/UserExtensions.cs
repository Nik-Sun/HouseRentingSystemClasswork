namespace House_renting_system_Project.Extensions;

using System.Security.Claims;

using static Common.Constants.RoleNames;

public static class UserExtensions
{
    extension(ClaimsPrincipal user)
    {
        public string? GetId()
            => user.FindFirstValue(ClaimTypes.NameIdentifier);

        public bool IsClient()
            => user.IsInRole(Client);

        public bool IsAgent()
            => user.IsInRole(Agent);
    }
}
