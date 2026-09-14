using Praxis.TargetArchitecture.AppInfrastructure.ControllerAttributes;
using Microsoft.AspNetCore.Mvc;

namespace Praxis.TargetArchitecture.Features.Api.ArchitecturePrinciples.Get;

public class GetArchitecturePrinciplesController(GetArchitecturePrinciplesService service) : PraxisControllerBase
{
    [HttpGet("api/architecture-principles")]
    public async Task<List<ArchitecturePrincipleDto>> GetArchitecturePrinciples() =>
        await service.Get();
}
