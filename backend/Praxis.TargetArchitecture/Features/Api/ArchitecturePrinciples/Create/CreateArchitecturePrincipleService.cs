using Praxis.TargetArchitecture.AppInfrastructure.Exceptions;
using Praxis.TargetArchitecture.Data;
using Praxis.TargetArchitecture.Entities;
using Microsoft.EntityFrameworkCore;

namespace Praxis.TargetArchitecture.Features.Api.ArchitecturePrinciples.Create;

public class CreateArchitecturePrincipleService(PraxisDbContext context)
{
    public async Task<Guid> Create(CreateArchitecturePrincipleDto dto)
    {
        var titleIsTaken = await context.ArchitecturePrinciples
            .AnyAsync(principle => principle.Title == dto.Title);

        if (titleIsTaken)
        {
            throw new ConflictException($"An architecture principle named '{dto.Title}' already exists");
        }

        var highestNumber = await context.ArchitecturePrinciples
            .MaxAsync(principle => (int?)principle.Number) ?? 0;

        var principle = new ArchitecturePrinciple
        {
            Id = Guid.NewGuid(),
            Number = highestNumber + 1,
            Title = dto.Title,
            Rationale = dto.Rationale,
            Layer = dto.Layer,
            Tags = dto.Tags,
            IsHighlighted = dto.IsHighlighted,
            Example = dto.Example
        };

        context.ArchitecturePrinciples.Add(principle);

        await context.SaveChangesAsync();

        return principle.Id;
    }
}

public class CreateArchitecturePrincipleDto
{
    public required string Title { get; set; }
    public required string Rationale { get; set; }
    public required ArchitectureLayer Layer { get; set; }
    public required List<ArchitectureTag> Tags { get; set; }
    public required bool IsHighlighted { get; set; }
    public required string? Example { get; set; }
}

public static class CreateArchitecturePrincipleDtoValidator
{
    public static void Validate(CreateArchitecturePrincipleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ValidationException("Title is required");
        }

        if (string.IsNullOrWhiteSpace(dto.Rationale))
        {
            throw new ValidationException("Rationale is required");
        }

        if (dto.Tags.Count is < 2 or > 3)
        {
            throw new ValidationException("Between two and three tags are required");
        }

        if (dto.Tags.Distinct().Count() != dto.Tags.Count)
        {
            throw new ValidationException("Tags must be distinct");
        }
    }
}
