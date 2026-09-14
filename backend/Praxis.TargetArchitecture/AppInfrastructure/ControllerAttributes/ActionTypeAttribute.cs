using Microsoft.AspNetCore.Authorization;

namespace Praxis.TargetArchitecture.AppInfrastructure.ControllerAttributes;

public enum ActionType
{
    Read,
    Edit
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeActionTypeAttribute(ActionType actionType) : AuthorizeAttribute
{
    public ActionType ActionType { get; } = actionType;
}
