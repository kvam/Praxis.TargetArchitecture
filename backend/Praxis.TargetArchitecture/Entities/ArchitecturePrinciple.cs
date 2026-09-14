using Praxis.TargetArchitecture.Entities.Interfaces;

namespace Praxis.TargetArchitecture.Entities;

public class ArchitecturePrinciple : IChangeTrackable, IDateTrackedEntity
{
    public required Guid Id { get; set; }
    public required int Number { get; set; }
    public required string Title { get; set; }
    public required string Rationale { get; set; }
    public required ArchitectureLayer Layer { get; set; }
    public required List<ArchitectureTag> Tags { get; set; }
    public required bool IsHighlighted { get; set; }

    /// <summary>
    /// Optional worked example. Null means the principle stands on its rationale alone.
    /// </summary>
    public required string? Example { get; set; }

    #region Audit

    public DateTime CreatedUtc { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime UpdatedUtc { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;

    #endregion
}

public enum ArchitectureLayer
{
    Contract = 1,
    Backend = 2,
    Persistence = 3,
    Frontend = 4,
    Testing = 5,
    Process = 6,
    Collaboration = 7
}

/// <summary>
/// A principle belongs to exactly one layer but usually touches several themes. Tags are the
/// cross-cutting view: "show me everything about dependencies" regardless of where it lives.
/// </summary>
public enum ArchitectureTag
{
    // Stack
    DotNet = 1,
    EntityFramework = 2,
    React = 3,
    TypeScript = 4,

    // Shape of the system
    CodeGeneration = 5,
    ApiDesign = 6,
    VerticalSlices = 7,
    Database = 8,

    // Craft
    Simplicity = 9,
    Readability = 10,
    Naming = 11,
    Dependencies = 12,

    // Running it
    Configuration = 13,
    Security = 14,
    ErrorHandling = 15,
    Observability = 16,
    Performance = 17,
    Automation = 18,

    // Working
    Flow = 19,
    Delivery = 20,
    Communication = 21,
    Ownership = 22,
    Learning = 23,
    Review = 24
}
