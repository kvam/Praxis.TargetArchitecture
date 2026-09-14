namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class BackendExamples
{
    public const string EveryEndpointIsAVerticalSlice = """
            One endpoint, one folder, everything it owns:

                // RIGHT
                Features/Api/ArchitecturePrinciples/Create/
                    CreateArchitecturePrincipleController.cs
                    CreateArchitecturePrincipleService.cs   (+ its DTO and validator)

            Adding a second action means a second folder, never a second method here:

                // RIGHT
                Features/Api/ArchitecturePrinciples/Get/
                    GetArchitecturePrinciplesController.cs
                    GetArchitecturePrinciplesService.cs

            The test for the create slice lives at the mirrored path under the test project.
            Deleting the feature is deleting the folder plus its service registration.
            """;

    public const string ControllersAreThin = """
            Validate, call one service, return:

                // RIGHT
                [HttpPost("api/architecture-principles")]
                public async Task<ArchitecturePrincipleDto> CreateArchitecturePrinciple(
                    [FromBody] CreateArchitecturePrincipleDto dto)
                {
                    CreateArchitecturePrincipleDtoValidator.Validate(dto);

                    var architecturePrincipleId = await createService.Create(dto);

                    return await getService.GetById(architecturePrincipleId);
                }

            No branching on results, no try/catch, no business rules. Logic put here cannot
            be unit tested without spinning up the transport layer around it.
            """;

    public const string ExceptionsAreForErrorsNotFlow = """
            Throw a domain exception to surface a real error:

                // RIGHT
                if (titleIsTaken)
                {
                    throw new ConflictException($"A principle named '{dto.Title}' exists");
                }

            PraxisExceptionHandlingMiddleware turns it into a sensible API response:

                ConflictException   -> 409 { "message": "..." }
                NotFoundException   -> 404 { "message": "..." }
                ValidationException -> 400 { "message": "..." }

            Do not use exceptions as program flow, and do not catch them just to invent a
            second path through the method.
            """;

    public const string TryCatchIsALastResort = """
            The whole application has one catch, and it does work:

                // RIGHT
                // Middleware: the one place that turns an exception into a response.
                catch (PraxisDomainException exception)
                {
                    await WriteProblem(context, exception);
                }

            A form posting to that endpoint does not need its own:

                // WRONG
                try {
                    await createArchitecturePrinciple(form)
                    setForm(initialForm)
                } catch (submitError) {
                    setFormError(submitError instanceof Error ? submitError.message : "...")
                }

            The mutation already tracks the failure, so ask it instead of catching:

                // RIGHT
                createArchitecturePrinciple(form, { onSuccess: () => setForm(initialForm) })

                const { createError } = useCreateArchitecturePrincipleMutation()

            Everything else lets the exception travel. A catch that logs and rethrows adds a
            line to a log nobody reads and removes nothing from the problem.
            """;

    public const string GuardClausesInsteadOfElse = """
            Handle the exceptional case and leave:

                // RIGHT
                if (principle == null)
                {
                    throw new NotFoundException($"Principle {id} was not found");
                }

                return Project(principle);

            Not:

                // WRONG
                if (principle != null)
                {
                    return Project(principle);
                }
                else
                {
                    throw new NotFoundException(...);
                }

            The happy path stays at one indentation level and reads straight down.
            """;

    public const string OurOwnPlatformTypesCarryTheSolutionName = """
            Ours:

                PraxisControllerBase       PraxisDbContext
                PraxisIocConfiguration     PraxisEnvironments

            Not ours:

                ControllerBase           DbContext

            Reading PraxisDbContext, you know immediately it is in this repository and can be
            changed. Reading AppDbContext or plain IocConfiguration, you cannot tell whether
            you are looking at something the framework shipped or something a colleague
            wrote. Slice types are named after the action instead: GetArchitecturePrinciplesService.
            """;

    public const string MapByHand = """
            The mapping is a list of assignments anyone can verify:

                // RIGHT
                new ArchitecturePrincipleDto
                {
                    ArchitecturePrincipleId = principle.Id,
                    Title = principle.Title,
                    Tags = principle.Tags
                }

            Not:

                // WRONG
                mapper.Map<ArchitecturePrincipleDto>(principle);

            Rename a property and the hand-written version fails to compile. The mapped
            version keeps running and quietly sends null, and you find out from a user.
            """;

    public const string SlicesDoNotReferenceEachOthersTypes = """
            A DTO defined in one slice stays in that slice:

                // WRONG
                public async Task<ArchitecturePrincipleDto> CreateArchitecturePrinciple(
                    [FromBody] CreateArchitecturePrincipleDto dto)

            The only allowed cross-slice call is in the controller, where GetService can
            return the created value:

                // RIGHT
                return await getService.GetById(architecturePrincipleId);

            The create service itself still must not depend on the Get slice DTO, because
            that couples two slices to each other's public shapes.
            """;
}
