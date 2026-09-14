namespace Praxis.TargetArchitecture.AppInfrastructure;

public static class PraxisEnvironments
{
    private const string ProductionEnvironmentName = "Production";

    private static string CurrentEnvironmentName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

    private static bool IsProduction => CurrentEnvironmentName == ProductionEnvironmentName;

    public static bool ExposeOpenApiDocument => !IsProduction;

    public static bool SeedArchitecturePrinciples => !IsProduction;

    public static bool WriteTypescriptFiles => !IsProduction;
}
