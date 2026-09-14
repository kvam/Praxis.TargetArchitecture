namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class ProcessExamples
{
    public const string AFileIsAtMost200Lines = """
            One file, one responsibility, one review:

                // RIGHT
                Features/Api/ArchitecturePrinciples/Get/
                    GetArchitecturePrinciplesController.cs
                    GetArchitecturePrinciplesService.cs
                    ArchitecturePrincipleDto.cs

            When the file starts carrying a second reason to change, the split follows that
            seam. The number is only a signal that the file may have become more than one
            idea wearing one name.
            """;

    public const string CustomerProjectsAreNotAPlayground = """
            The customer asked for this:

                // RIGHT
                Implement the feature the project needs, using the stack the project already
                has.

            Not:

                // WRONG
                Swap in a new library just to try it out.
                Rewrite a stable slice because you want to practice a pattern.
                Use production work as a proving ground for ideas the customer did not ask for.

            A customer project exists to solve the customer's problem. If you want to try a
            new tool or technique, do it on your own time and your own code.
            """;

    public const string FocusOnValue = """
            The next thing should move the customer toward something useful:

                // RIGHT
                Fix the defect that blocks users.
                Ship the feature that unblocks the next decision.

            Not:

                // WRONG
                Add ceremony because it feels thorough.
                Spend time polishing a low-value edge before the main path works.
                Build something just because it would be interesting to build.

            Value beats busywork. If a task does not move the customer toward a useful
            outcome, it is probably not the next thing to do.
            """;

    public const string UsabilityIsNotOptional = """
            UX belongs to a specialist, but everyone still has a role:

                // RIGHT
                Make the form readable, the labels clear, and the action obvious.
                Ask for design help when the flow is genuinely tricky.

            Not:

                // WRONG
                Assume UX is somebody else's problem.
                Ship a confusing screen because you are "just backend".

            Backend developers are not expected to be UX experts, but they are expected to
            make a serious effort on usability. The work is better when the basics are
            obvious before a designer ever looks at it.
            """;

    public const string ARealDatabaseIsUsuallyEnough = """
            Most projects need a normal relational database:

                // RIGHT
                Use PostgreSQL or SQL Server for the data the application owns.

            Not:

                // WRONG
                Add a specialized datastore just because it is interesting.
                Reach for extra database tech before the relational one has a real limit.

            PostgreSQL or SQL Server is usually enough. Bring in something fancier only when
            the problem genuinely falls outside what a normal relational database does well.
            """;

    public const string InternalAndExternalAPIsLiveApart = """
            Two API surfaces, two places to change:

                // RIGHT
                Keep internal endpoints in one area of the application and external endpoints
                in another.

            Not:

                // WRONG
                Mix the same controller set between internal and external callers.
                Assume the same contract will fit both audiences forever.

            Internal and external APIs evolve differently. Keeping them apart makes it easier
            to change one without dragging the other along.
            """;

    public const string IntegrationsLiveApartFromApplicationAPIs = """
            An integration is its own area:

                // RIGHT
                Keep partner-facing integration code in a separate part of the application
                from the internal and external APIs.

            Not:

                // WRONG
                Mix third-party integration endpoints into the same area as your own
                application APIs.

            Integrations depend on somebody else's contract and somebody else's timing. That
            makes them a different kind of change from your own APIs.
            """;

    public const string IntegrationsAreContainedAndMinimized = """
            Keep as little of the system as possible aware of the integration:

                // RIGHT
                Translate the partner contract at the edge and keep the rest of the
                application on its own types.

            Not:

                // WRONG
                Let third-party request and response shapes spread through the application.

            A partner contract changes on the partner's schedule. The smaller the integration
            surface inside your own code, the cheaper those changes are to absorb.
            """;

    public const string IntegrationFailuresAreASeparateConcern = """
            Third-party failure handling belongs with the integration:

                // RIGHT
                Keep retries, polling, timeouts, and partial-failure handling inside the
                integration area.

            Not:

                // WRONG
                Scatter third-party failure behavior through ordinary application flows.

            Integrations fail in different ways from your own APIs. Keeping that logic
            separate stops those concerns from leaking everywhere else.
            """;

    public const string UseInboxAndOutboxForMessageBasedIntegrations = """
            Queue-based integrations need their own delivery boundary:

                // RIGHT
                Persist outgoing messages in an outbox and process incoming ones through an
                inbox.

            Not:

                // WRONG
                Publish directly from application flow and hope the message send and database
                save succeed together.

            Inbox and outbox patterns keep asynchronous integration delivery reliable without
            spreading queue concerns through the rest of the application.
            """;
}
