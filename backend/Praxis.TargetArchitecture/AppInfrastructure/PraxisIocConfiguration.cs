using Praxis.TargetArchitecture.Features.Api.Architecture.GetOverview;
using Praxis.TargetArchitecture.Features.Api.ArchitecturePrinciples.Create;
using Praxis.TargetArchitecture.Features.Api.ArchitecturePrinciples.Get;

namespace Praxis.TargetArchitecture.AppInfrastructure;

public static class PraxisIocConfiguration
{
    public static void AddPraxisServices(this IServiceCollection services)
    {
        services.AddScoped<GetArchitectureOverviewService>();
        services.AddScoped<GetArchitecturePrinciplesService>();
        services.AddScoped<CreateArchitecturePrincipleService>();
    }
}
