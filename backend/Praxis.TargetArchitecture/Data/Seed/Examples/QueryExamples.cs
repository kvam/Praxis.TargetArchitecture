namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class QueryExamples
{
    public const string TheDbContextIsTheRepository = """
            The layer nobody needs:

                // WRONG
                public interface IArchitecturePrincipleRepository
                {
                    Task<List<ArchitecturePrinciple>> GetAllAsync();
                    Task<ArchitecturePrinciple?> GetByIdAsync(Guid id);
                    Task AddAsync(ArchitecturePrinciple principle);
                }

            Every method forwards to `DbSet`, drops the ability to project, compose, or
            page, and exists to abstract a database that is never going to be swapped.

            Inject the context and write the query where it is needed:

                // RIGHT
                public class GetArchitecturePrinciplesService(PraxisDbContext dbContext)

            `DbSet<T>` is already a repository and `SaveChangesAsync` is already a unit of
            work. Wrapping them only removes capability.
            """;

    public const string ReadsAreUntracked = """
            A read that asks the change tracker to remember everything:

                // WRONG
                var principles = await dbContext.ArchitecturePrinciples
                    .ToListAsync(cancellationToken);     // 134 tracked entities

            Nothing here will ever be saved, so the tracking snapshots are pure cost — and
            worse, a stray edit to one of them could be written by an unrelated
            SaveChanges later in the request.

            State the intent instead:

                // RIGHT
                .AsNoTracking()
                .Select(Projection)

            Untracked, projected to a DTO in the database, and impossible to accidentally
            persist. Tracking is for the entities you loaded in order to change.
            """;

    public const string LoadingIsExplicit = """
            Lazy loading turns one query into a hundred and hides it behind a property
            access:

                // WRONG
                foreach (var principle in principles)
                {
                    Console.WriteLine(principle.Author.Name);   // a query, every lap
                }

            Ask for what you need, once:

                // RIGHT
                await dbContext.ArchitecturePrinciples
                    .AsNoTracking()
                    .Include(principle => principle.Author)
                    .ToListAsync(cancellationToken);

            Or better, project only the columns the DTO actually uses and let the database
            do the join. Either way the cost is visible in the code rather than in a
            production trace three months later.
            """;

    public const string OneSaveChangesPerUnitOfWork = """
            Saving in a loop makes each iteration its own transaction:

                // WRONG
                foreach (var principle in principles)
                {
                    dbContext.ArchitecturePrinciples.Add(principle);
                    await dbContext.SaveChangesAsync(cancellationToken);   // 134 round trips
                }

            Fail on number 87 and you are left with 86 rows committed and no way to tell
            the caller what happened.

            One save, one transaction, all or nothing:

                // RIGHT
                dbContext.ArchitecturePrinciples.AddRange(principles);
                await dbContext.SaveChangesAsync(cancellationToken);

            That is what a unit of work means: the request either happened or it did not.
            """;

    public const string MigrationsAreAppendOnly = """
            Once a migration has run anywhere but your own machine, it is history. Editing
            it means your database and everyone else's silently disagree about what the
            schema is, and nothing will tell you until a deploy fails.

            Wrong, after the fact:

                // WRONG
                // Opening 20260901_AddTags.cs and adding another column to it

            Right:

                // RIGHT
                dotnet ef migrations add AddPrincipleExample

            A new migration, reviewed like any other code, applied in order. If the last
            migration is wrong, the fix is a migration that corrects it — not a rewrite of
            the one people have already run.
            """;
}
