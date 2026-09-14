using Praxis.TargetArchitecture.Entities;
using Praxis.TargetArchitecture.Data.Seed.Examples;

using static Praxis.TargetArchitecture.Data.Seed.ArchitecturePrincipleSeed;

namespace Praxis.TargetArchitecture.Data.Seed;

// Continues BackendPrinciples. Seeded immediately after it so the Backend layer stays
// one contiguous block of numbers.
public static class BackendCraftPrinciples
{
    public static List<ArchitecturePrinciple> All =>
    [
        // Backend: Plain code over machinery
        Principle(
            "Services are concrete classes",
            "No interface is introduced for a single implementation. Indirection is added when a second implementation actually exists.",
            ArchitectureLayer.Backend,
            ArchitectureTag.Simplicity, ArchitectureTag.DotNet, ArchitectureTag.Readability),
        Principle(
            "Map by hand",
            "Projecting an entity into a DTO is a list of assignments any reader can verify. A mapper turns a rename into a silent null and hides which fields actually cross the wire.",
            BackendExamples.MapByHand,
            ArchitectureLayer.Backend,
            ArchitectureTag.Simplicity, ArchitectureTag.Dependencies, ArchitectureTag.DotNet),
        Principle(
            "DRY only real business logic",
            "Duplicate simple code when it keeps the reader closer to the work. DRY is for complex business logic that would otherwise fork, not for every repeated line in a file.",
            BackendCraftExamples.DRYOnlyRealBusinessLogic,
            ArchitectureLayer.Backend,
            ArchitectureTag.Simplicity, ArchitectureTag.Readability, ArchitectureTag.Learning),
        Principle(
            "DRY is for complex rules, not every line",
            "Share code when the same business decision would otherwise fork and drift. Do not chase every repeated line into a helper if the duplication is still the clearest code.",
            BackendCraftExamples.DRYIsForComplexRulesNotEveryLine,
            ArchitectureLayer.Backend,
            ArchitectureTag.Simplicity, ArchitectureTag.Readability, ArchitectureTag.Learning),
        Principle(
            "Avoid advanced language features unless the framework needs them",
            "Interfaces, base classes, reflection, and custom generic abstractions add moving parts that the reader has to unpack. Use the simplest shape that still works, and only reach for language features when the framework already requires them.",
            BackendCraftExamples.AvoidAdvancedLanguageFeaturesUnlessTheFrameworkNeedsThem,
            ArchitectureLayer.Backend,
            ArchitectureTag.Simplicity, ArchitectureTag.Readability, ArchitectureTag.DotNet),
        Principle(
            "Entity relationships are bi-directional",
            "The dependent entity keeps both ArchitectureLayerId and ArchitectureLayer, and the principal side keeps List<ArchitecturePrinciple>, so the relationship shape is always explicit in the model and the object graph stays honest.",
            BackendCraftExamples.EntityRelationshipsAreBiDirectional,
            ArchitectureLayer.Backend,
            ArchitectureTag.EntityFramework, ArchitectureTag.DotNet, ArchitectureTag.Readability),
        Principle(
            "No lazy loading",
            "Load what you need in the query. Lazy loading hides database work behind property access and makes the data shape harder to reason about.",
            BackendCraftExamples.NoLazyLoading,
            ArchitectureLayer.Backend,
            ArchitectureTag.EntityFramework, ArchitectureTag.Database, ArchitectureTag.Readability),
        Principle(
            "No in-process message bus",
            "MediatR and friends add a dispatch layer between a controller and the service one folder away. The slice is already the handler, and go-to-definition should reach it.",
            ArchitectureLayer.Backend,
            ArchitectureTag.Dependencies, ArchitectureTag.Simplicity, ArchitectureTag.DotNet),
        Principle(
            "Validate with plain code",
            "A static validator with guard clauses reads top to bottom and debugs like any other method. A fluent rule chain is a second language for expressing an if statement.",
            CodeStyleExamples.ValidateWithPlainCode,
            ArchitectureLayer.Backend,
            ArchitectureTag.Dependencies, ArchitectureTag.Simplicity, ArchitectureTag.ErrorHandling),
        Principle(
            "Prefer the base class library",
            "System.Text.Json over Newtonsoft, HttpClient over a client wrapper, LINQ over a query builder. The platform version is already installed, already patched, and already familiar.",
            ArchitectureLayer.Backend,
            ArchitectureTag.Dependencies, ArchitectureTag.DotNet, ArchitectureTag.Simplicity),

        // Backend: Readability
        Principle(
            "Guard clauses instead of else",
            "Preconditions are handled and returned from at the top of a method, which keeps the happy path at one indentation level and removes branches nobody reads.",
            BackendExamples.GuardClausesInsteadOfElse,
            ArchitectureLayer.Backend,
            ArchitectureTag.Readability, ArchitectureTag.Simplicity, ArchitectureTag.DotNet),
        Principle(
            "Names are never abbreviated",
            "Full names cost keystrokes once and save comprehension forever. Abbreviations are a private dialect the next developer has to learn.",
            CodeStyleExamples.NamesAreNeverAbbreviated,
            ArchitectureLayer.Backend,
            ArchitectureTag.Naming, ArchitectureTag.Readability, ArchitectureTag.Learning),
        Principle(
            "Our own platform types carry the solution name",
            "PraxisIocConfiguration, not IocConfiguration; PraxisDbContext, not AppDbContext. The prefix tells the reader at a glance that this is ours to change rather than something the framework shipped, and it keeps our types from colliding with the ones it did.",
            BackendExamples.OurOwnPlatformTypesCarryTheSolutionName,
            ArchitectureLayer.Backend,
            ArchitectureTag.Naming, ArchitectureTag.Readability, ArchitectureTag.DotNet),
        Principle(
            "Comments explain why, not what",
            "Code already states what happens. A comment earns its place by recording the reason, constraint, or rejected alternative behind the code.",
            CodeStyleExamples.CommentsExplainWhyNotWhat,
            ArchitectureLayer.Backend,
            ArchitectureTag.Readability, ArchitectureTag.Simplicity, ArchitectureTag.Learning),
        Principle(
            "Warnings are errors",
            "The build fails on warnings and they are fixed rather than suppressed, because a tolerated warning quickly becomes a wall of them.",
            CodeStyleExamples.WarningsAreErrors,
            ArchitectureLayer.Backend,
            ArchitectureTag.Automation, ArchitectureTag.DotNet, ArchitectureTag.Simplicity),

        // Backend: Configuration
        Principle(
            "Configuration holds secrets and environment differences, nothing else",
            "If a value is the same everywhere and is not sensitive, it is a constant in code. Moving it to a config file only hides it from the reader and the compiler.",
            ConfigurationExamples.ConfigurationHoldsSecretsAndEnvironmentDifferencesNothingElse,
            ArchitectureLayer.Backend,
            ArchitectureTag.Configuration, ArchitectureTag.Security, ArchitectureTag.Simplicity),
        Principle(
            "Switches are computed in a static environments file",
            "PraxisEnvironments decides what is on and off in one readable place. A flag expressed as C# can be grepped, navigated to, and type-checked; a key in a settings file can only be hoped for.",
            ConfigurationExamples.SwitchesAreComputedInAStaticEnvironmentsFile,
            ArchitectureLayer.Backend,
            ArchitectureTag.Configuration, ArchitectureTag.DotNet, ArchitectureTag.Readability),
        Principle(
            "Keep the reasoning next to the code",
            "A condition you can read beats a value you must go looking for. Logic buried in configuration turns 'why did it do that' into an archaeology exercise across environments.",
            ArchitectureLayer.Backend,
            ArchitectureTag.Configuration, ArchitectureTag.Readability, ArchitectureTag.Simplicity),
        Principle(
            "Config files stay small",
            "A settings file that grows into hundreds of keys has become an untyped, untested second program. Keep appsettings for logging, connection strings, and genuine per-environment values.",
            ArchitectureLayer.Backend,
            ArchitectureTag.Configuration, ArchitectureTag.Simplicity, ArchitectureTag.Readability),
        Principle(
            "A missing setting fails loudly at startup",
            "Required configuration is read once when the application starts, not lazily in the middle of a request where it fails as a confusing null.",
            ArchitectureLayer.Backend,
            ArchitectureTag.Configuration, ArchitectureTag.ErrorHandling, ArchitectureTag.DotNet),
    ];
}
