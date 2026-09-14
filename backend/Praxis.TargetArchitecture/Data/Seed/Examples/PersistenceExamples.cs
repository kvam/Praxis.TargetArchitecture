namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class PersistenceExamples
{
    public const string ProjectToTheDTOInTheQuery = """
            Select into the DTO so the database returns only what is sent:

                // RIGHT
                await context.ArchitecturePrinciples
                    .AsNoTracking()
                    .OrderBy(principle => principle.Number)
                    .Select(principle => new ArchitecturePrincipleDto
                    {
                        ArchitecturePrincipleId = principle.Id,
                        Title = principle.Title
                    })
                    .ToListAsync();

            Not:

                // WRONG
                var principles = await context.ArchitecturePrinciples.ToListAsync();
                return principles.Select(Project).ToList();

            The second version reads every column of every row, including the long rationale
            and example text, to throw most of it away in memory.
            """;

    public const string AuditFieldsAreNeverSetByHand = """
            The DbContext stamps every IDateTrackedEntity on save:

                // RIGHT
                await context.SaveChangesAsync();

            Not:

                // WRONG
                entity.CreatedUtc = DateTime.UtcNow;
                entity.UpdatedUtc = DateTime.UtcNow;
                await context.SaveChangesAsync();

            The context owns the audit timestamps, so every slice gets the same rule and no
            one has to remember to set them by hand.
            """;

    public const string NoQueriesInsideLoops = """
            One round trip:

                // RIGHT
                var principleIds = dtos.Select(dto => dto.Id).ToList();

                var principles = await context.ArchitecturePrinciples
                    .Where(principle => principleIds.Contains(principle.Id))
                    .ToDictionaryAsync(principle => principle.Id);

            Not one per iteration:

                // WRONG
                foreach (var dto in dtos)
                {
                    var principle = await context.ArchitecturePrinciples
                        .SingleAsync(candidate => candidate.Id == dto.Id);
                }

            With ten rows in a test database both feel instant. With ten thousand rows in
            production the second is ten thousand round trips.
            """;

    public const string QueriesMustTranslateToSQL = """
            Filter in the database:

                // RIGHT
                await context.ArchitecturePrinciples
                    .Where(principle => principle.Layer == ArchitectureLayer.Backend)
                    .ToListAsync();

            Not in memory:

                // WRONG
                var all = await context.ArchitecturePrinciples.ToListAsync();
                return all.Where(principle => IsInteresting(principle)).ToList();

            Calling a C# method inside Where forces the whole table into memory first,
            because the provider cannot translate your method into SQL. The tell is an
            await before the filtering, or a client-evaluation warning at runtime.
            """;

    public const string EnumValuesAreNeverRenumbered = """
            The database stores the number, not the name:

                public enum ArchitectureLayer
                {
                    Backend = 1,
                    Frontend = 2,
                    Contract = 3
                }

            Insert Collaboration in the middle and every stored 3 silently becomes something
            else. Rows that said Contract now say Collaboration, and nothing fails.

            So values are only ever appended:

                // RIGHT
                Collaboration = 7

            Retiring a value means leaving the number reserved and stopping its use, never
            handing it to something new.
            """;
}
