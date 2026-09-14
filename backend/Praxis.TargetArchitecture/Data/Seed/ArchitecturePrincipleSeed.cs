using Praxis.TargetArchitecture.Entities;

namespace Praxis.TargetArchitecture.Data.Seed;

public static class ArchitecturePrincipleSeed
{
    public static ArchitecturePrinciple Principle(
        string title,
        string rationale,
        ArchitectureLayer layer,
        params ArchitectureTag[] tags) =>
        new()
        {
            Id = Guid.NewGuid(),
            Number = 0,
            Title = title,
            Rationale = rationale,
            Layer = layer,
            Tags = [.. tags],
            IsHighlighted = false,
            Example = null
        };

    public static ArchitecturePrinciple Principle(
        string title,
        string rationale,
        string example,
        ArchitectureLayer layer,
        params ArchitectureTag[] tags) =>
        new()
        {
            Id = Guid.NewGuid(),
            Number = 0,
            Title = title,
            Rationale = rationale,
            Layer = layer,
            Tags = [.. tags],
            IsHighlighted = false,
            Example = example
        };
}
