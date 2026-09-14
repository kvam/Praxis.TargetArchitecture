using Praxis.TargetArchitecture.AppInfrastructure.ControllerAttributes;
using Praxis.TargetArchitecture.Features.Api.ArchitecturePrinciples.Get;
using Microsoft.AspNetCore.Mvc;

namespace Praxis.TargetArchitecture.Features.Api.ArchitecturePrinciples.Create;

public class CreateArchitecturePrincipleController(
    CreateArchitecturePrincipleService createService,
    GetArchitecturePrinciplesService getService) : PraxisControllerBase
{
    [HttpPost("api/architecture-principles")]
    public async Task<ArchitecturePrincipleDto> CreateArchitecturePrinciple(
        [FromBody] CreateArchitecturePrincipleDto dto)
    {
        CreateArchitecturePrincipleDtoValidator.Validate(dto);

        var architecturePrincipleId = await createService.Create(dto);

        return await getService.GetById(architecturePrincipleId);
    }
}
