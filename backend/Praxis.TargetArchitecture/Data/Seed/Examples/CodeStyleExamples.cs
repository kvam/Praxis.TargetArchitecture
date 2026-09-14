namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class CodeStyleExamples
{
    public const string NamesAreNeverAbbreviated = """
            Abbreviations that only the author can expand:

                // WRONG
                var arcPrincs = await ctx.ArchPrinc
                    .Where(p => p.Lyr == lyr)
                    .ToListAsync(ct);

            Spelled out, the line explains itself:

                // RIGHT
                var principles = await dbContext.ArchitecturePrinciples
                    .Where(principle => principle.Layer == layer)
                    .ToListAsync(cancellationToken);

            The abbreviation saves the author a few keystrokes once and costs every
            reader a guess forever. Editors complete long names; nobody completes your
            private shorthand. The only accepted short names are the ones the platform
            itself made universal, such as `id` in a URL template.
            """;

    public const string CommentsExplainWhyNotWhat = """
            A comment that restates the code:

                // WRONG
                // Order by number
                .OrderBy(principle => principle.Number)

            It adds nothing, and it becomes a lie the day someone reorders by title.

            A comment that carries information the code cannot:

                // RIGHT
                // DefaultIfEmpty(0).MaxAsync() throws on the in-memory provider, so the
                // nullable cast is load-bearing rather than defensive.
                var highest = await dbContext.ArchitecturePrinciples
                    .MaxAsync(principle => (int?)principle.Number, cancellationToken) ?? 0;

            The second comment survives refactoring because it explains a decision. If you
            feel the urge to explain *what* a line does, rename something instead.
            """;

    public const string ObviousBeatsShort = """
            Clever, and three readings deep:

                // WRONG
                var grouped = principles
                    .GroupBy(p => p.Layer)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.Title).ToList());

                var result = grouped.SelectMany(kv => kv.Value.Select((t, i) =>
                    new { kv.Key, Title = t, Index = i + 1 })).ToList();

            Obvious, and readable at a glance:

                // RIGHT
                var numbered = new List<NumberedPrinciple>();

                foreach (var layer in principles.GroupBy(principle => principle.Layer))
                {
                    var number = 1;

                    foreach (var principle in layer)
                    {
                        numbered.Add(new NumberedPrinciple(layer.Key, principle.Title, number));
                        number++;
                    }
                }

            The second version is longer and better. Code is read far more often than it
            is written, and usually by someone tired, at speed, looking for a bug.
            """;

    public const string WarningsAreErrors = """
            The project file makes it impossible to ignore them:

                // RIGHT
                <PropertyGroup>
                    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
                    <Nullable>enable</Nullable>
                </PropertyGroup>

            A warning nobody has to fix is a warning nobody ever fixes. Within a month the
            build prints forty of them, the real one scrolls past unread, and the null
            reference it predicted ships to production.

            Failing the build keeps the count at zero, which is the only number anyone
            actually notices changing.
            """;

    public const string ValidateWithPlainCode = """
            Neither a validation framework nor a custom attribute is needed here. Both hide
            the rule somewhere other than where it applies:

                // WRONG
                [ValidTagCount(Min = 2, Max = 3)]        // now go read the attribute
                public required List<ArchitectureTag> Tags { get; set; }

                RuleFor(dto => dto.Tags)                 // a chain that means "if"
                    .Must(tags => tags.Distinct().Count() == tags.Count)
                    .WithMessage("Tags must be distinct");

            The attribute runs through reflection and cannot be stepped through. The chain
            is a second language for expressing a condition, with its own operators to
            learn before you can read what is a one-line rule.

            The real validator in this repository is a list of ifs:

                // RIGHT
                public static class CreateArchitecturePrincipleDtoValidator
                {
                    public static void Validate(CreateArchitecturePrincipleDto dto)
                    {
                        if (string.IsNullOrWhiteSpace(dto.Title))
                        {
                            throw new ValidationException("Title is required");
                        }

                        if (dto.Tags.Count is < 2 or > 3)
                        {
                            throw new ValidationException("Between two and three tags are required");
                        }

                        if (dto.Tags.Distinct().Count() != dto.Tags.Count)
                        {
                            throw new ValidationException("Tags must be distinct");
                        }
                    }
                }

            It reads top to bottom, every branch takes a breakpoint, and adding a rule means
            writing another if. Nobody has to learn anything to change it.
            """;
}
