using Praxis.TargetArchitecture.AppInfrastructure.Exceptions;
using Praxis.TargetArchitecture.Data;
using Praxis.TargetArchitecture.Entities;
using Microsoft.EntityFrameworkCore;

namespace Praxis.TargetArchitecture.Features.Api.ArchitecturePrinciples.Get;

public class GetArchitecturePrinciplesService(PraxisDbContext context)
{
    public async Task<List<ArchitecturePrincipleDto>> Get()
    {
        return await context.ArchitecturePrinciples
            .AsNoTracking()
            .OrderBy(principle => principle.Number)
            .Select(principle => ToDto(principle))
            .ToListAsync();
    }

    public async Task<ArchitecturePrincipleDto> GetById(Guid architecturePrincipleId)
    {
        var principle = await context.ArchitecturePrinciples
            .AsNoTracking()
            .SingleOrDefaultAsync(candidate => candidate.Id == architecturePrincipleId);

        if (principle == null)
        {
            throw new NotFoundException($"Architecture principle {architecturePrincipleId} was not found");
        }

        return ToDto(principle);
    }

    private static ArchitecturePrincipleDto ToDto(ArchitecturePrinciple principle) =>
        new()
        {
            ArchitecturePrincipleId = principle.Id,
            Number = principle.Number,
            Title = principle.Title,
            Rationale = principle.Rationale,
            Layer = principle.Layer,
            Tags = principle.Tags,
            IsHighlighted = principle.IsHighlighted,
            Example = principle.Example
        };
}

public class ArchitecturePrincipleDto
{
    public required Guid ArchitecturePrincipleId { get; set; }
    public required int Number { get; set; }
    public required string Title { get; set; }
    public required string Rationale { get; set; }
    public required ArchitectureLayer Layer { get; set; }
    public required List<ArchitectureTag> Tags { get; set; }
    public required bool IsHighlighted { get; set; }
    public required string? Example { get; set; }
}
