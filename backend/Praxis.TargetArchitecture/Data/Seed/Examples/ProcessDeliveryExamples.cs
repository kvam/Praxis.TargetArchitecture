namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class ProcessDeliveryExamples
{
    public const string UnhandledExceptionsArePersistedForDebugging = """
            Production failures should still be inspectable after the request is over:

                // RIGHT
                Persist the exception message, stack trace, request path, and useful context
                to the database through the exception pipeline.

            Not:

                // WRONG
                Hope somebody copied the log line before it rolled away.
                Leave debugging dependent on reproducing the same failure live.

            Storing exception details in the database makes production debugging much simpler,
            because the failure can still be inspected after the request and its logs are gone.
            """;

    public const string HTTPRequestsArePersistedForDebugging = """
            A production request should leave a trail:

                // RIGHT
                Persist the route, method, user, correlation details, and request body shape
                needed to understand what the user sent.

            Not:

                // WRONG
                Depend entirely on memory or transient logs to understand what triggered the
                failure.

            Storing HTTP requests in the database makes it much easier to inspect what really
            happened after the request has already completed.
            """;

    public const string DevelopmentAPIsReturnRichExceptionDetails = """
            Developers should see what failed immediately:

                // RIGHT
                In development, return the exception message, stack trace, and inner
                exception details in the API response.

            Not:

                // WRONG
                Hide the real failure behind a generic "something went wrong" response while
                developing locally.

            Development error responses are for diagnosis, not presentation. Rich failure data
            makes local debugging fast, and production can stay much stricter.
            """;

    public const string RequestAndExceptionLogsTellOneStory = """
            The request log and the exception log should connect:

                // RIGHT
                Read the stored request and the stored exception together to reconstruct what
                the user did and where the system failed.

            Not:

                // WRONG
                Keep request and exception data so disconnected that debugging becomes guesswork.

            Debugging is simpler when the logs tell one complete story instead of leaving you
            to piece together half the timeline from memory.
            """;

    public const string ZeroBugPolicy = """
            Discovered bugs get fixed before new work is added:

                // RIGHT
                Fix the defect first.
                Start the next piece of work only after the known bug is handled.

            Not:

                // WRONG
                Start something new and leave the known bug sitting there.
                Treat discovered defects as optional.

            Zero bugs does not mean perfection. It means the team does not knowingly stack
            new work on top of a known defect.
            """;

    public const string ANewPackageMustClearAHighBar = """
            The whole frontend dependency list, and why each survives the question:

                react, react-dom        the framework
                @tanstack/react-query   server-state caching is genuinely hard to get right
                @mui/material           a design system is a lot of code to not write
                @emotion/*              required by MUI

            Rejected on the same test: axios (fetch exists), moment (Intl exists), uuid
            (crypto.randomUUID exists), lodash (for one groupBy), classnames (template
            literals exist).

            The question is not "is this package good?" — they all are. It is "is this
            problem hard enough that I want to own an upgrade path for it?"
            """;

    public const string GreenBuildBeforeDone = """
            All four, every time, before saying the work is finished:

                // RIGHT
                cd backend && dotnet build && dotnet test
                cd frontend && npm run typecheck && npm run lint

            The same four commands run in CI on every pull request, so the standard is
            enforced mechanically rather than socially. "It works on my machine" and "I only
            changed a comment" are the two sentences that most often precede a red build.
            """;

    public const string RegenerationIsAHumanAction = """
            When a DTO or enum changes, the generated frontend model is stale until someone
            runs:

                // RIGHT
                # backend running on :5153
                cd frontend && npm run autogenerate-models

            An assistant must not run it, must not start the backend in order to run it, and
            must not hand-edit the output to compensate. It says so instead:

                "ArchitecturePrincipleDto gained Example — regenerate before using it."

            Generation rewrites files the whole frontend compiles against. That is a moment a
            person should choose, having seen what changed in the contract.
            """;
}
