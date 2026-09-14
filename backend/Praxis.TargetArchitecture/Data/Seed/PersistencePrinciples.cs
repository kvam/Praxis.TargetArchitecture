using Praxis.TargetArchitecture.Entities;
using Praxis.TargetArchitecture.Data.Seed.Examples;

using static Praxis.TargetArchitecture.Data.Seed.ArchitecturePrincipleSeed;

namespace Praxis.TargetArchitecture.Data.Seed;

public static class PersistencePrinciples
{
    public static List<ArchitecturePrinciple> All =>
    [
        // Persistence: Entity Framework is the data layer
        Principle(
            "The DbContext is the repository",
            "Entity Framework already implements the repository and unit-of-work patterns. Wrapping it adds a layer that only re-exposes what it hides.",
            QueryExamples.TheDbContextIsTheRepository,
            ArchitectureLayer.Persistence,
            ArchitectureTag.EntityFramework, ArchitectureTag.Simplicity, ArchitectureTag.Dependencies),
        Principle(
            "Queries live in the slice that needs them",
            "There is no shared query layer. A query is written against the DbContext where it is used, so changing one endpoint cannot break another.",
            ArchitectureLayer.Persistence,
            ArchitectureTag.EntityFramework, ArchitectureTag.VerticalSlices, ArchitectureTag.Database),
        Principle(
            "Entities are shared, DTOs are not",
            "Entities live in one place because the schema is genuinely one thing. This is the deliberate exception to slice isolation, not a precedent for sharing anything else.",
            ArchitectureLayer.Persistence,
            ArchitectureTag.EntityFramework, ArchitectureTag.ApiDesign, ArchitectureTag.VerticalSlices),
        Principle(
            "The DbContext is scoped and never shared",
            "It is resolved per request and never captured in a singleton, static, or parallel task. It is not thread-safe and fails unpredictably when treated as if it were.",
            ArchitectureLayer.Persistence,
            ArchitectureTag.EntityFramework, ArchitectureTag.DotNet, ArchitectureTag.ErrorHandling),

        // Persistence: Queries
        Principle(
            "Project to the DTO in the query",
            "Select into the DTO so the database returns only the columns that are sent. Materializing entities and mapping afterwards reads the whole row to discard most of it.",
            PersistenceExamples.ProjectToTheDTOInTheQuery,
            ArchitectureLayer.Persistence,
            ArchitectureTag.EntityFramework, ArchitectureTag.Performance, ArchitectureTag.ApiDesign),
        Principle(
            "Reads are untracked",
            "AsNoTracking on every read path, and project in the database instead of materializing entities. A read that builds a change-tracking graph pays for a save that will never happen.",
            QueryExamples.ReadsAreUntracked,
            ArchitectureLayer.Persistence,
            ArchitectureTag.EntityFramework, ArchitectureTag.Performance, ArchitectureTag.Database),
        Principle(
            "Loading is explicit",
            "Lazy-loading proxies stay off and related data is pulled with an explicit Include or projection, so a query's cost is visible where it is written.",
            QueryExamples.LoadingIsExplicit,
            ArchitectureLayer.Persistence,
            ArchitectureTag.EntityFramework, ArchitectureTag.Performance, ArchitectureTag.Readability),
        Principle(
            "No queries inside loops",
            "Related data is fetched in one round trip before iterating. A query in a loop is an N+1 that only shows up once the table is large.",
            PersistenceExamples.NoQueriesInsideLoops,
            ArchitectureLayer.Persistence,
            ArchitectureTag.EntityFramework, ArchitectureTag.Performance, ArchitectureTag.Database),
        Principle(
            "Queries must translate to SQL",
            "Filtering and ordering happen in the database. Pulling rows into memory to finish the work in C# turns a query into a table scan.",
            PersistenceExamples.QueriesMustTranslateToSQL,
            ArchitectureLayer.Persistence,
            ArchitectureTag.EntityFramework, ArchitectureTag.Performance, ArchitectureTag.Database),

        // Persistence: Writes and schema
        Principle(
            "One SaveChanges per unit of work",
            "A request saves once so the whole change is atomic. Multiple saves leave a half-applied operation behind when the second one fails.",
            QueryExamples.OneSaveChangesPerUnitOfWork,
            ArchitectureLayer.Persistence,
            ArchitectureTag.EntityFramework, ArchitectureTag.Database, ArchitectureTag.ErrorHandling),
        Principle(
            "Audit fields are never set by hand",
            "The DbContext stamps every IDateTrackedEntity on save, so no slice can forget to record a change and none of them can disagree about when it happened.",
            PersistenceExamples.AuditFieldsAreNeverSetByHand,
            ArchitectureLayer.Persistence,
            ArchitectureTag.EntityFramework, ArchitectureTag.Database, ArchitectureTag.Automation),
        Principle(
            "Enum values are never renumbered",
            "Persisted enums are stored as integers, so reusing or reordering a value silently rewrites the meaning of existing rows. Append instead.",
            PersistenceExamples.EnumValuesAreNeverRenumbered,
            ArchitectureLayer.Persistence,
            ArchitectureTag.Database, ArchitectureTag.ApiDesign, ArchitectureTag.EntityFramework),
        Principle(
            "Migrations are append-only",
            "A migration that has been applied anywhere is never edited. Correcting it means a new migration, because history is what other databases have already replayed.",
            QueryExamples.MigrationsAreAppendOnly,
            ArchitectureLayer.Persistence,
            ArchitectureTag.Database, ArchitectureTag.EntityFramework, ArchitectureTag.Delivery),
        Principle(
            "Schema changes are reviewed as carefully as code",
            "Generated migrations are read before they are committed. The tool infers intent from a model diff and is confidently wrong about renames.",
            ArchitectureLayer.Persistence,
            ArchitectureTag.Database, ArchitectureTag.Review, ArchitectureTag.EntityFramework),
        Principle(
            "The database connection retries transient failures",
            "The connection is configured to retry brief network and failover blips, because a managed database is occasionally unreachable for a second by design. Without it, routine maintenance surfaces to users as an error page.",
            ArchitectureLayer.Persistence,
            ArchitectureTag.Database, ArchitectureTag.ErrorHandling, ArchitectureTag.Performance),
    ];
}
