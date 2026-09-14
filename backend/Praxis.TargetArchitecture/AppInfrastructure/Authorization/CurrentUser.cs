namespace Praxis.TargetArchitecture.AppInfrastructure.Authorization;

public class CurrentUser
{
    public required Guid AzureAdUserId { get; set; }
    public required string DisplayName { get; set; }
}
