using Praxis.TargetArchitecture.Entities;

using static Praxis.TargetArchitecture.Data.Seed.ArchitecturePrincipleSeed;

namespace Praxis.TargetArchitecture.Data.Seed;

// Continues FrontendPrinciples. Seeded next to it so the Frontend layer stays one
// contiguous block of numbers.
public static class FrontendCraftPrinciples
{
    public static List<ArchitecturePrinciple> All =>
    [
        // Frontend: Styling
        Principle(
            "Styling lives in named styled-components",
            "Presentation goes into a named styled component rather than an inline style object in the markup. A name explains intent, can be reused, and is not rebuilt on every render.",
            ArchitectureLayer.Frontend,
            ArchitectureTag.React, ArchitectureTag.Readability, ArchitectureTag.TypeScript),

        // Frontend: The backend owns the numbers
        Principle(
            "The frontend never computes a domain number",
            "Totals, conversions, and any figure a user might quote are calculated in the backend and sent as their own field. A sum written in both languages is two answers waiting to disagree.",
            ArchitectureLayer.Frontend,
            ArchitectureTag.ApiDesign, ArchitectureTag.React, ArchitectureTag.Readability),

        // Frontend: Rendering cost
        Principle(
            "Rows carry a stable identity",
            "Lists and grids key their rows on a durable domain identifier so a refetch updates nodes in place instead of rebuilding them. Keying on array position throws away selection and half-finished edits on every refresh.",
            ArchitectureLayer.Frontend,
            ArchitectureTag.React, ArchitectureTag.Performance, ArchitectureTag.Readability),
    ];
}
