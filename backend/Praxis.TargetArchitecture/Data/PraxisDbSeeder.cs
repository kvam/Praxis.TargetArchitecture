using Praxis.TargetArchitecture.Data.Seed;
using Praxis.TargetArchitecture.Entities;
using Microsoft.EntityFrameworkCore;

namespace Praxis.TargetArchitecture.Data;

public static class PraxisDbSeeder
{
    public static async Task Seed(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PraxisDbContext>();

        var existingPrinciples = await context.ArchitecturePrinciples
            .Select(principle => new
            {
                principle.Title,
                principle.IsHighlighted
            })
            .ToListAsync();

        var principles = SeedPrinciples()
            .Select((principle, index) => WithNumber(principle, index + 1))
            .Select(WithHighlight)
            .ToList();

        GuardAgainstUnknownHighlights(principles);

        var missingPrinciples = principles
            .Where(principle => existingPrinciples.All(existing => existing.Title != principle.Title))
            .ToList();

        var highlightWasChanged = false;

        foreach (var principle in context.ArchitecturePrinciples)
        {
            var shouldBeHighlighted = IsHighlighted(principle.Title);

            if (principle.IsHighlighted == shouldBeHighlighted)
            {
                continue;
            }

            principle.IsHighlighted = shouldBeHighlighted;
            highlightWasChanged = true;
        }

        if (missingPrinciples.Count == 0 && !highlightWasChanged)
        {
            return;
        }

        context.ArchitecturePrinciples.AddRange(missingPrinciples);

        await context.SaveChangesAsync();
    }

    /// <summary>
    /// A highlight is matched by title, so a rename or typo would otherwise just stop
    /// highlighting that principle with nothing to notice. Fail loudly instead.
    /// </summary>
    private static void GuardAgainstUnknownHighlights(List<ArchitecturePrinciple> principles)
    {
        var titles = principles.Select(principle => principle.Title).ToHashSet();
        var unknown = HighlightedPrinciples.Where(title => !titles.Contains(title)).ToList();

        if (unknown.Count == 0)
        {
            return;
        }

        throw new InvalidOperationException(
            $"Highlighted principles match no seeded title: {string.Join(", ", unknown)}.");
    }

    private static ArchitecturePrinciple WithNumber(ArchitecturePrinciple principle, int number)
    {
        principle.Number = number;

        return principle;
    }

    private static ArchitecturePrinciple WithHighlight(ArchitecturePrinciple principle)
    {
        principle.IsHighlighted = IsHighlighted(principle.Title);

        return principle;
    }

    private static bool IsHighlighted(string title) =>
        HighlightedPrinciples.Contains(title);

    /// <summary>
    /// Roughly one principle in five, spread across every layer so filtering to the key set
    /// still describes the whole standard rather than one corner of it.
    /// </summary>
    private static readonly HashSet<string> HighlightedPrinciples =
    [
        // Contract
        "Backend owns the contract",
        "Frontend models are generated, never written",

        // Backend
        "Every endpoint is a vertical slice",
        "Slices do not reference each other's types",
        "A slice can be deleted by deleting its folder",
        "Map by hand",
        "Never catch what you cannot handle",
        "Guard clauses instead of else",
        "Names are never abbreviated",
        "Every endpoint declares the access it requires",

        // Persistence
        "The DbContext is the repository",
        "Project to the DTO in the query",
        "Enum values are never renumbered",

        // Frontend
        "Server state lives in the query cache",
        "Domain rules are not reimplemented in the UI",
        "The browser is the standard library",

        // Testing
        "Test against a real database, not a mock",

        // Process
        "One year of experience is the bar",
        "Obvious beats short",
        "A file is at most 200 lines",
        "A new package must clear a high bar",
        "Avoid popular libraries that do very little",
        "No framework inside the framework",
        "Unhandled exceptions are persisted for debugging",
        "Green build before done",

        // Collaboration
        "Everyone works across the stack",
        "A feature is delivered end to end",
        "Small teams outperform large ones",
        "Trust is the multiplier",
        "Limit work in progress to an absolute minimum",
        "Finish before you start",
        "Doing nothing beats doing the wrong thing",
        "Pair on anything hard",
    ];

    private static List<ArchitecturePrinciple> SeedPrinciples() =>
    [
        .. ContractPrinciples.All,
        .. BackendPrinciples.All,
        .. BackendCraftPrinciples.All,
        .. BackendSecurityPrinciples.All,
        .. PersistencePrinciples.All,
        .. FrontendPrinciples.All,
        .. FrontendCraftPrinciples.All,
        .. TestingPrinciples.All,
        .. ProcessPrinciples.All,
        .. ProcessDeliveryPrinciples.All,
        .. ProcessGuardrailPrinciples.All,
        .. TeamPrinciples.All,
        .. FlowPrinciples.All
    ];
}
