namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class BackendCraftExamples
{
    public const string DRYOnlyRealBusinessLogic = """
            Repeating simple checks is often clearer than hiding them behind another name:

                // RIGHT
                var discount = membership switch
                {
                    ArchitectureTag.Performance => 0.10m,
                    ArchitectureTag.Simplicity => 0.05m,
                    _ => 0.00m
                };

            Not:

                // WRONG
                var discount = CalculateDiscount(membership);

            DRY is for real business logic that would otherwise split in different directions.
            It is not a license to extract every repeated line into a helper and make the code
            harder to follow than the duplication ever was.
            """;

    public const string DRYIsForComplexRulesNotEveryLine = """
            Share code when the same business decision would otherwise fork and drift:

                // RIGHT
                var price = basePrice;

                if (isMember)
                {
                    price -= price * 0.10m;
                }

                if (isWeekend)
                {
                    price -= price * 0.05m;
                }

            Not:

                // WRONG
                var price = CalculatePrice(basePrice, isMember, isWeekend);

            The helper hides a few obvious rules behind another name. DRY is worth it when
            the same business logic would otherwise split into different places and drift. It
            is not worth it just to eliminate repetition that is still easiest to read inline.
            """;

    public const string AvoidAdvancedLanguageFeaturesUnlessTheFrameworkNeedsThem = """
            Plain code is easier to read than a pile of indirection:

                // RIGHT
                public static class CreateArchitecturePrincipleDtoValidator
                {
                    public static void Validate(CreateArchitecturePrincipleDto dto)
                    {
                        if (string.IsNullOrWhiteSpace(dto.Title))
                        {
                            throw new ValidationException("Title is required");
                        }
                    }
                }

            Not:

                // WRONG
                public interface IValidator<T>
                {
                    void Validate(T value);
                }

                public abstract class ValidatorBase<T> : IValidator<T> { ... }

            Interfaces, base classes, reflection, and custom generic abstractions all add
            another thing to understand. Keep the shape obvious unless the framework already
            demands the feature. Framework types like List<T> are fine; do not invent your own
            generic layer just to avoid a little repetition.
            """;

    public const string EntityRelationshipsAreBiDirectional = """
            The entity keeps the foreign key and navigation, and the parent keeps the list:

                // RIGHT
                public class ArchitecturePrinciple
                {
                    public required Guid ArchitectureLayerId { get; set; }
                    public ArchitectureLayer Layer { get; set; } = null!;
                }

                public class ArchitectureLayer
                {
                    public List<ArchitecturePrinciple> Principles { get; set; } = [];
                }

            That keeps the relationship explicit in both directions instead of hiding half of
            it behind a one-way navigation property.
            """;

    public const string NoLazyLoading = """
            Load what you need in the query:

                // RIGHT
                var principles = await dbContext.ArchitecturePrinciples
                    .AsNoTracking()
                    .Include(principle => principle.Layer)
                    .ToListAsync();

            Not:

                // WRONG
                var principles = await dbContext.ArchitecturePrinciples.ToListAsync();

                var layer = principle.Layer; // now hidden work happens here

            Lazy loading hides database work behind property access, so the query no longer
            tells the reader what data will be touched.
            """;
}
