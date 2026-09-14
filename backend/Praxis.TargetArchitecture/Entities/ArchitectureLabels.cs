namespace Praxis.TargetArchitecture.Entities;

/// <summary>
/// The human-readable name for each enum member, used by the register UI. A member added
/// without a label throws here, which fails PraxisTypescriptGeneratorTests rather than
/// shipping a blank chip to the UI.
/// </summary>
public static class ArchitectureLabels
{
    public static string For(ArchitectureLayer layer) => layer switch
    {
        ArchitectureLayer.Contract => "Contract",
        ArchitectureLayer.Backend => "Backend",
        ArchitectureLayer.Persistence => "Persistence",
        ArchitectureLayer.Frontend => "Frontend",
        ArchitectureLayer.Testing => "Testing",
        ArchitectureLayer.Process => "Process",
        ArchitectureLayer.Collaboration => "Collaboration",
        _ => throw new ArgumentOutOfRangeException(nameof(layer), layer, "No label for this layer."),
    };

    public static string For(ArchitectureTag tag) => tag switch
    {
        ArchitectureTag.DotNet => ".NET",
        ArchitectureTag.EntityFramework => "Entity Framework",
        ArchitectureTag.React => "React",
        ArchitectureTag.TypeScript => "TypeScript",

        ArchitectureTag.CodeGeneration => "code generation",
        ArchitectureTag.ApiDesign => "API design",
        ArchitectureTag.VerticalSlices => "vertical slices",
        ArchitectureTag.Database => "database",

        ArchitectureTag.Simplicity => "simplicity",
        ArchitectureTag.Readability => "readability",
        ArchitectureTag.Naming => "naming",
        ArchitectureTag.Dependencies => "dependencies",

        ArchitectureTag.Configuration => "configuration",
        ArchitectureTag.Security => "security",
        ArchitectureTag.ErrorHandling => "error handling",
        ArchitectureTag.Observability => "observability",
        ArchitectureTag.Performance => "performance",
        ArchitectureTag.Automation => "automation",

        ArchitectureTag.Flow => "flow",
        ArchitectureTag.Delivery => "delivery",
        ArchitectureTag.Communication => "communication",
        ArchitectureTag.Ownership => "ownership",
        ArchitectureTag.Learning => "learning",
        ArchitectureTag.Review => "review",
        _ => throw new ArgumentOutOfRangeException(nameof(tag), tag, "No label for this tag."),
    };
}
