using Praxis.TargetArchitecture.AppInfrastructure.ControllerAttributes;
using Microsoft.AspNetCore.Mvc;

namespace Praxis.TargetArchitecture.Features.Api.Architecture.GetOverview;

public class GetArchitectureOverviewController(GetArchitectureOverviewService service) : PraxisControllerBase
{
    [HttpGet("api/architecture/overview")]
    public async Task<ArchitectureOverviewDto> GetArchitectureOverview() =>
        await service.Get();
}
