namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class ConfigurationExamples
{
    public const string SwitchesAreComputedInAStaticEnvironmentsFile = """
            PraxisEnvironments is the one place that decides what is on:

                // RIGHT
                public static class PraxisEnvironments
                {
                    public static string CurrentEnvironmentName =>
                        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

                    public static bool IsProduction => CurrentEnvironmentName == "Production";

                    public static bool ExposeOpenApiDocument => !IsProduction;
                    public static bool SeedArchitecturePrinciples => !IsProduction;
                }

            Used directly where it matters:

                // RIGHT
                if (PraxisEnvironments.ExposeOpenApiDocument)
                {
                    app.MapOpenApi();
                }

            You can go-to-definition on the switch, the compiler checks it, and the reason
            it is off in production is one line away. A "Features:ExposeOpenApi": false key
            in appsettings can only be grepped for and hoped about.
            """;

    public const string ConfigurationHoldsSecretsAndEnvironmentDifferencesNothingElse = """
            Belongs in configuration (or user-secrets):

                // RIGHT
                ConnectionStrings:PraxisDatabase     differs per environment, sensitive
                AzureAd:ClientSecret               secret

            Belongs in code:

                // RIGHT
                public const int MaxPageSize = 100;          // same everywhere
                public static bool SeedData => !IsProduction; // derived, not configured

            Moving MaxPageSize into appsettings.json does not make it configurable in any
            useful sense — nobody will change it without a deployment anyway. It only makes
            it untyped, untested, and invisible to the reader of the code that uses it.
            """;

    public const string LocalSecretsLiveInDotnetUserSecrets = """
            Set it once, outside the repository:

                // RIGHT
                cd backend/Praxis.TargetArchitecture
                dotnet user-secrets init
                dotnet user-secrets set "ConnectionStrings:PraxisDatabase" "Server=..."

            Read it exactly like any other setting:

                // RIGHT
                builder.Configuration.GetConnectionString("PraxisDatabase")

            The value lives in your user profile, so it cannot be committed. The same secret
            in appsettings.Development.json is one `git add -A` away from being permanent —
            and deleting it later does not remove it from history.
            """;
}
