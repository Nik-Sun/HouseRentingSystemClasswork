namespace House_renting_system_Project.Common;

public static class Constants
{
    public static class RoleNames
    {
        public static readonly string[] Names = [Agent, Client];

        public const string Agent = "Agent";
        public const string Client = "Client";
    }

    public static class UserCredentials
    {
        public const string DefaultAgentUsername = "agent-def";
        public const string DefaultAgentEmail = "agent@mail.com";
        public const string DefaultAgentPassword = "aA3456!";
    }
}
