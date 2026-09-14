namespace Praxis.TargetArchitecture.Features.Api.Architecture.GetOverview;

public class GetArchitectureOverviewService
{
    public Task<ArchitectureOverviewDto> Get() =>
        Task.FromResult(new ArchitectureOverviewDto
        {
            Name = "Praxis.TargetArchitecture",
            BackendStyle = BackendArchitectureStyle.VerticalSlice,
            FrontendStyle = FrontendArchitectureStyle.GeneratedModelsFromBackend,
            Principles =
            [
                "Backend owns domain contracts and calculations.",
                "Frontend imports generated models instead of hand-maintaining duplicates.",
                "Slices stay independent except when write flows call their matching get flow."
            ],
            GeneratedArtifacts =
            [
                "frontend/src/models/generated",
                "frontend/src/models/constants.ts"
            ]
        });
}

public class ArchitectureOverviewDto
{
    public required string Name { get; set; }
    public required BackendArchitectureStyle BackendStyle { get; set; }
    public required FrontendArchitectureStyle FrontendStyle { get; set; }
    public required List<string> Principles { get; set; }
    public required List<string> GeneratedArtifacts { get; set; }
}

public enum BackendArchitectureStyle
{
    VerticalSlice = 1,
}

public enum FrontendArchitectureStyle
{
    GeneratedModelsFromBackend = 1,
}

public static class ArchitectureConstants
{
    public const int MaxArchitecturePrinciples = 3;
}
