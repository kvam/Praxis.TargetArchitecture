using Praxis.TargetArchitecture.Entities;

using static Praxis.TargetArchitecture.Data.Seed.ArchitecturePrincipleSeed;

namespace Praxis.TargetArchitecture.Data.Seed;

// Continues ProcessPrinciples. Seeded next to the other Process lists so the layer stays
// one contiguous block of numbers.
public static class ProcessGuardrailPrinciples
{
    public static List<ArchitecturePrinciple> All =>
    [
        // Process: Quantified limits
        Principle(
            "Complexity limits are numbers, not taste",
            "Method length, nesting depth, and constructor dependencies have agreed ceilings, and a change that breaks one is refactored before merge. Asking people to keep it simple gives a reviewer nothing to point at.",
            ArchitectureLayer.Process,
            ArchitectureTag.Readability, ArchitectureTag.Simplicity, ArchitectureTag.Review),

        // Process: Secrets beyond the repository
        Principle(
            "Prefer platform identity over stored credentials",
            "Where the host can prove the application's identity, use that instead of a password or key held in configuration. A credential that does not exist cannot be leaked, rotated late, or committed by accident.",
            ArchitectureLayer.Process,
            ArchitectureTag.Security, ArchitectureTag.Configuration, ArchitectureTag.Automation),
        Principle(
            "Secrets are never passed as command arguments",
            "A secret is read from an environment variable or a hidden prompt, never typed as an argument, because arguments land in shell history and are visible to anyone listing processes. The exposure outlives the command by months.",
            ArchitectureLayer.Process,
            ArchitectureTag.Security, ArchitectureTag.Automation, ArchitectureTag.Configuration),
    ];
}
