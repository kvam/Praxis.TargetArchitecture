using Praxis.TargetArchitecture.Entities;
using Praxis.TargetArchitecture.Data.Seed.Examples;

using static Praxis.TargetArchitecture.Data.Seed.ArchitecturePrincipleSeed;

namespace Praxis.TargetArchitecture.Data.Seed;

// Continues ProcessPrinciples. Seeded immediately after it so the Process layer stays
// one contiguous block of numbers.
public static class ProcessDeliveryPrinciples
{
    public static List<ArchitecturePrinciple> All =>
    [
        // Process: Dependencies
        Principle(
            "A new package must clear a high bar",
            "Adding a dependency means adopting its upgrades, its breakages, its supply chain, and its idea of how things should be done. Ten lines you own can be read, debugged, and deleted; a dependency must be evaluated, audited, and eventually migrated away from.",
            ProcessDeliveryExamples.ANewPackageMustClearAHighBar,
            ArchitectureLayer.Process,
            ArchitectureTag.Dependencies, ArchitectureTag.Simplicity, ArchitectureTag.Security),
        Principle(
            "Prefer the platform",
            ".NET and the browser already do most of it. A library is worth it when it solves a problem that is genuinely hard, not one that is merely tedious.",
            ArchitectureLayer.Process,
            ArchitectureTag.Dependencies, ArchitectureTag.DotNet, ArchitectureTag.Simplicity),
        Principle(
            "Avoid popular libraries that do very little",
            "Popularity is not a reason. AutoMapper replaces an obvious assignment block with configuration that fails at runtime instead of compile time, and you still have to understand the mapping afterwards.",
            ArchitectureLayer.Process,
            ArchitectureTag.Dependencies, ArchitectureTag.Simplicity, ArchitectureTag.Readability),
        Principle(
            "A wrapper you must still understand is not an abstraction",
            "If using the library means learning its DSL and the thing it wraps, it has added a layer without removing one.",
            ArchitectureLayer.Process,
            ArchitectureTag.Dependencies, ArchitectureTag.Simplicity, ArchitectureTag.Learning),
        Principle(
            "No framework inside the framework",
            "Home-grown abstraction layers over ASP.NET, EF, or React force every newcomer to learn a private dialect before they can read the code.",
            ArchitectureLayer.Process,
            ArchitectureTag.Dependencies, ArchitectureTag.Simplicity, ArchitectureTag.Learning),

        // Process: The repository is the specification
        Principle(
            "The code is the specification",
            "This repository demonstrates the standard by running it. A rule that is not visible in working code is a suggestion nobody will follow.",
            ArchitectureLayer.Process,
            ArchitectureTag.Learning, ArchitectureTag.Automation),
        Principle(
            "Documentation changes with the code",
            "Conventions, README, and AI configuration are updated in the same change as the behaviour they describe, because documentation is only trusted while it is true.",
            ArchitectureLayer.Process,
            ArchitectureTag.Learning, ArchitectureTag.Review),
        Principle(
            "The AI configuration is part of the architecture",
            "Instructions and skills encode the same rules as the docs, so an assistant is constrained by the standard rather than guessing at it.",
            ArchitectureLayer.Process,
            ArchitectureTag.Automation, ArchitectureTag.Learning),
        Principle(
            "Seed data makes a fresh clone useful",
            "Startup seeding is idempotent and meaningful, so the first run of the application demonstrates the system instead of an empty list.",
            ArchitectureLayer.Process,
            ArchitectureTag.Learning, ArchitectureTag.Automation, ArchitectureTag.Database),
        Principle(
            "Regeneration is a human action",
            "Assistants never run model generation or start the backend to enable it. Contract changes are surfaced to a person who chooses when to propagate them.",
            ProcessDeliveryExamples.RegenerationIsAHumanAction,
            ArchitectureLayer.Process,
            ArchitectureTag.CodeGeneration, ArchitectureTag.Automation),

        // Process: Workflow
        Principle(
            "Work happens on a branch",
            "The default branch is never modified directly and nothing is pushed without being asked, so history stays reviewable and reversible.",
            ArchitectureLayer.Process,
            ArchitectureTag.Flow, ArchitectureTag.Review, ArchitectureTag.Delivery),
        Principle(
            "Green build before done",
            "Backend build and tests plus frontend lint and typecheck all pass before work is reported as finished. Unverified work is unfinished work.",
            ProcessDeliveryExamples.GreenBuildBeforeDone,
            ArchitectureLayer.Process,
            ArchitectureTag.Automation, ArchitectureTag.Delivery),
        Principle(
            "Continuous integration enforces the standard",
            "The same build, test, lint, and typecheck commands developers run locally run on every pull request, so agreement is mechanical rather than social.",
            ArchitectureLayer.Process,
            ArchitectureTag.Automation, ArchitectureTag.Delivery),

        // Process: Secrets
        Principle(
            "Secrets never enter the repository",
            "Credentials, connection strings, and tokens are configuration, not source. A secret in history is compromised even after it is deleted.",
            ArchitectureLayer.Process,
            ArchitectureTag.Security, ArchitectureTag.Configuration),
        Principle(
            "Local secrets live in dotnet user-secrets",
            "Connection strings, keys, and tokens are set with dotnet user-secrets, which stores them outside the repository. A secret in appsettings.json is one git add away from being permanent.",
            ConfigurationExamples.LocalSecretsLiveInDotnetUserSecrets,
            ArchitectureLayer.Process,
            ArchitectureTag.Security, ArchitectureTag.Configuration, ArchitectureTag.DotNet),

        // Process: Definition of done
        Principle(
            "Done means running in production with users",
            "Not written, not merged, not deployed to test. Until real people are using it, the work has produced cost and no value and is still in progress.",
            ArchitectureLayer.Process,
            ArchitectureTag.Ownership, ArchitectureTag.Delivery),
        Principle(
            "Merged is not done",
            "Code sitting on the main branch waiting for a release is inventory. It has been paid for, it is aging, and it is teaching nobody anything.",
            ArchitectureLayer.Process,
            ArchitectureTag.Flow, ArchitectureTag.Delivery),
        Principle(
            "Ship small and often",
            "Frequent small releases make each one boring. Batching changes into a big release concentrates the risk and hides which change caused the problem.",
            ArchitectureLayer.Process,
            ArchitectureTag.Flow, ArchitectureTag.Delivery, ArchitectureTag.Automation),
        Principle(
            "Releasing is routine, not an event",
            "A deployment that needs a plan, a window, and an audience is too rare. Make it dull enough that shipping is never the reason to delay finishing.",
            ArchitectureLayer.Process,
            ArchitectureTag.Delivery, ArchitectureTag.Automation),
        Principle(
            "Only users can tell you it works",
            "Passing tests prove the code does what you expected. Whether what you expected was worth building is a question only production can answer.",
            ArchitectureLayer.Process,
            ArchitectureTag.Delivery, ArchitectureTag.Learning),
        Principle(
            "Zero bug policy",
            "Discovered bugs are fixed before new work is added. If a defect is known, it gets handled before the team starts something else.",
            ProcessDeliveryExamples.ZeroBugPolicy,
            ArchitectureLayer.Process,
            ArchitectureTag.Delivery, ArchitectureTag.Review, ArchitectureTag.Readability),
        Principle(
            "You own it after it ships",
            "Shipping is the start of the feedback loop, not the end of the task. Watch it run, and treat what you learn as part of the same piece of work.",
            ArchitectureLayer.Process,
            ArchitectureTag.Ownership, ArchitectureTag.Delivery),
    ];
}
