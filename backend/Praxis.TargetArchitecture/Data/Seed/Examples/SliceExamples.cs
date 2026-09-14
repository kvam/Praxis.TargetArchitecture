namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class SliceExamples
{
    public const string VerticalOverHorizontal = """
            Horizontal, the layout most projects drift into:

                // WRONG
                Controllers/ArchitecturePrinciplesController.cs
                Services/ArchitecturePrincipleService.cs
                Repositories/ArchitecturePrincipleRepository.cs
                Dtos/ArchitecturePrincipleDto.cs

            Adding one field means opening four folders, and every file in them serves a
            dozen unrelated features.

            Vertical, the layout here:

                // RIGHT
                Features/Api/ArchitecturePrinciples/Get/
                    GetArchitecturePrinciplesController.cs
                    GetArchitecturePrinciplesService.cs
                    ArchitecturePrincipleDto.cs

            Adding a field means opening one folder. Everything that changes together sits
            together, and nothing in that folder serves anyone else.
            """;

    public const string ASliceServiceHasOnePublicMethod = """
            One entry point, everything else private:

                // RIGHT
                public class GetArchitecturePrinciplesService(PraxisDbContext dbContext)
                {
                    public async Task<List<ArchitecturePrincipleDto>> GetAsync(
                        CancellationToken cancellationToken) =>
                        await dbContext.ArchitecturePrinciples
                            .AsNoTracking()
                            .OrderBy(principle => principle.Number)
                            .Select(principle => ToDto(principle))
                            .ToListAsync(cancellationToken);

                        private static ArchitecturePrincipleDto ToDto(ArchitecturePrinciple principle) =>
                            new()
                            {
                                ArchitecturePrincipleId = principle.Id,
                                Number = principle.Number,
                                Title = principle.Title,
                                Rationale = principle.Rationale,
                                Layer = principle.Layer,
                                Tags = principle.Tags,
                                Example = principle.Example
                            };
                    }

            The moment a service grows a second public method it is serving two callers,
            and neither can be changed without thinking about the other. Split it: two
            public methods are two slices wearing one coat.
            """;

    public const string NoManagerHelperOrUtilityClasses = """
            A name that describes nothing:

                // WRONG
                public static class PrincipleHelper
                {
                    public static string FormatNumber(int number) => ...
                    public static bool IsValid(string title) => ...
                    public static List<Tag> ParseTags(string raw) => ...
                }

            Nothing connects these three methods except that nobody knew where to put
            them, so the file becomes a landfill and every slice ends up depending on it.

            Put each behaviour where it belongs: formatting next to the component that
            renders it, validation in the slice's validator, parsing on the type that owns
            the tags. If a name needs "Helper", "Manager", or "Utility" to sound like
            something, it is not yet a thing.
            """;

    public const string ValidationHappensAtTheEdge = """
            The controller validates before anything else runs:

                // RIGHT
                [HttpPost("api/architecture-principles")]
                public async Task<ArchitecturePrincipleDto> Create(CreateArchitecturePrincipleDto dto)
                {
                    CreateArchitecturePrincipleDtoValidator.Validate(dto);

                    var architecturePrincipleId = await createService.Create(dto);

                    return await getService.GetById(architecturePrincipleId);
                }

            The validator is a static class of plain ifs, each throwing a ValidationException
            that the middleware turns into a 400 with the message it was given.

            Past that boundary the input is known good, so the service reads as business
            logic instead of a gauntlet of null checks. Validation scattered through the
            call stack means every layer half-trusts the one above it and re-checks the
            same thing three times, differently.
            """;

    public const string NeverCatchWhatYouCannotHandle = """
            A catch that does nothing but lie:

                // WRONG
                try
                {
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Save failed");
                    return null;      // the caller now gets a mystery
                }

            The write failed, the user is told nothing useful, and the stack trace that
            explained why is gone.

            Write the happy path and let it throw. The global exception handler turns the
            exception into a problem response, logs it once with full context, and the bug
            arrives in the log looking exactly like what it is. Catch only when you can
            genuinely do something about it — a retry, a fallback, a specific message.
            """;
}
