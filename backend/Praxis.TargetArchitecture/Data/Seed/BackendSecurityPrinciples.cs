using Praxis.TargetArchitecture.Entities;

using static Praxis.TargetArchitecture.Data.Seed.ArchitecturePrincipleSeed;

namespace Praxis.TargetArchitecture.Data.Seed;

// Continues BackendPrinciples. Seeded next to the other Backend lists so the layer stays
// one contiguous block of numbers.
public static class BackendSecurityPrinciples
{
    public static List<ArchitecturePrinciple> All =>
    [
        // Backend: Authorization
        Principle(
            "Every endpoint declares the access it requires",
            "Each action carries an explicit authorization attribute, or an explicit opt-out with a comment explaining why. An endpoint that says nothing about access is indistinguishable from one where the check was forgotten.",
            ArchitectureLayer.Backend,
            ArchitectureTag.Security, ArchitectureTag.ApiDesign, ArchitectureTag.Review),
        Principle(
            "Access decisions are made in one place",
            "Authorization is resolved by a single handler that combines the caller with the resource being touched, rather than by checks scattered through services. Per-service rules drift apart and make the real policy impossible to audit.",
            ArchitectureLayer.Backend,
            ArchitectureTag.Security, ArchitectureTag.Simplicity, ArchitectureTag.Review),
        Principle(
            "Authorization is scoped to the resource, not just the role",
            "Access is computed from the specific record being requested as well as the caller's role, because a valid token proves who someone is and never which rows they may read. Role-only checks are how one tenant reads another's data.",
            ArchitectureLayer.Backend,
            ArchitectureTag.Security, ArchitectureTag.ApiDesign, ArchitectureTag.Database),
        Principle(
            "The opt-out from a security rule is visible and explained",
            "Bypassing a cross-cutting guard is done with a named marker and a comment giving the reason, so every exception can be found with one search. A silent exception is indistinguishable from a bug.",
            ArchitectureLayer.Backend,
            ArchitectureTag.Security, ArchitectureTag.Review, ArchitectureTag.Readability),

        // Backend: Scheduled work
        Principle(
            "Only one instance runs scheduled jobs",
            "When the application runs as more than one replica, a single instance is elected to run timers and the others stand down. Without that election every scheduled job fires once per replica.",
            ArchitectureLayer.Backend,
            ArchitectureTag.Automation, ArchitectureTag.Database, ArchitectureTag.Performance),
        Principle(
            "Every background job run is recorded",
            "Each run stores when it started, how long it took, and how it failed. A job that only writes to a console log fails silently for months, because nobody is watching a console at three in the morning.",
            ArchitectureLayer.Backend,
            ArchitectureTag.Observability, ArchitectureTag.Automation, ArchitectureTag.Delivery),
    ];
}
