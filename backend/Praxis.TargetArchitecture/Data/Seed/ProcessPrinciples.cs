using Praxis.TargetArchitecture.Entities;
using Praxis.TargetArchitecture.Data.Seed.Examples;

using static Praxis.TargetArchitecture.Data.Seed.ArchitecturePrincipleSeed;

namespace Praxis.TargetArchitecture.Data.Seed;

public static class ProcessPrinciples
{
    public static List<ArchitecturePrinciple> All =>
    [
        // Process: The goal
        Principle(
            "One year of experience is the bar",
            "Anyone with a year of experience should be able to work in this system, frontend or backend. Every other rule exists to protect that, and any code that needs a tour guide has failed it.",
            ArchitectureLayer.Process,
            ArchitectureTag.Learning, ArchitectureTag.Simplicity, ArchitectureTag.Readability),
        Principle(
            "Obvious beats short",
            "Clever one-liners are written once and read for years. Boring, consistent code wins; optimize for the reader who is debugging at the end of a long day.",
            CodeStyleExamples.ObviousBeatsShort,
            ArchitectureLayer.Process,
            ArchitectureTag.Readability, ArchitectureTag.Simplicity, ArchitectureTag.Learning),
        Principle(
            "A file is at most 200 lines",
            "Two hundred lines is about as much as one person can hold in their head at once. A file past it has usually become two ideas sharing a name, so split it along that seam rather than at the line count.",
            ProcessExamples.AFileIsAtMost200Lines,
            ArchitectureLayer.Process,
            ArchitectureTag.Readability, ArchitectureTag.Simplicity),
        Principle(
            "Consistency beats personal preference",
            "A codebase that does one thing one way is easier to read than one full of locally optimal choices. Match the surrounding code.",
            ArchitectureLayer.Process,
            ArchitectureTag.Readability, ArchitectureTag.Simplicity),
        Principle(
            "Customer projects are not a playground",
            "Use customer work to solve the customer problem. Do not turn it into a private lab for trying out tools, patterns, or ideas that the project did not ask for.",
            ProcessExamples.CustomerProjectsAreNotAPlayground,
            ArchitectureLayer.Process,
            ArchitectureTag.Simplicity, ArchitectureTag.Delivery),
        Principle(
            "Focus on value",
            "If something does not move the customer toward a useful outcome, it is not the next thing to do. Value beats busywork, ceremony, and work done for its own sake.",
            ProcessExamples.FocusOnValue,
            ArchitectureLayer.Process,
            ArchitectureTag.Delivery, ArchitectureTag.Simplicity),
        Principle(
            "Usability is not optional",
            "Backend developers are not expected to be UX specialists, but they are expected to make a real effort on usability instead of ignoring it. Good enough UX still matters, even when someone else owns the design.",
            ProcessExamples.UsabilityIsNotOptional,
            ArchitectureLayer.Process,
            ArchitectureTag.Readability, ArchitectureTag.Delivery),
        Principle(
            "A real database is usually enough",
            "PostgreSQL or SQL Server is normally all the persistence technology a project needs. Reach for something fancier only when the problem is genuinely outside what a normal relational database does well.",
            ProcessExamples.ARealDatabaseIsUsuallyEnough,
            ArchitectureLayer.Process,
            ArchitectureTag.Database, ArchitectureTag.Simplicity, ArchitectureTag.Dependencies),
        Principle(
            "Internal and external APIs live apart",
            "Internal and external APIs evolve at different speeds and for different audiences. Keep them in separate areas so one contract can change without dragging the other with it.",
            ProcessExamples.InternalAndExternalAPIsLiveApart,
            ArchitectureLayer.Process,
            ArchitectureTag.ApiDesign, ArchitectureTag.Simplicity),
        Principle(
            "Integrations live apart from application APIs",
            "Integrations are not just another internal or external API. They depend on third-party contracts, schedules, and failure modes, so they belong in their own area of the application.",
            ProcessExamples.IntegrationsLiveApartFromApplicationAPIs,
            ArchitectureLayer.Process,
            ArchitectureTag.ApiDesign, ArchitectureTag.Simplicity),
        Principle(
            "Integrations are contained and minimized",
            "Keep the integration boundary as small as possible and stop third-party shapes from spreading through the application. The less of the system that knows about the integration, the cheaper it is to change.",
            ProcessExamples.IntegrationsAreContainedAndMinimized,
            ArchitectureLayer.Process,
            ArchitectureTag.ApiDesign, ArchitectureTag.Delivery, ArchitectureTag.Readability),
        Principle(
            "Integration failures are a separate concern",
            "Retries, timeouts, polling, and partial failures belong with the integration itself. Keeping that behavior separate stops third-party concerns from leaking into normal application flows.",
            ProcessExamples.IntegrationFailuresAreASeparateConcern,
            ArchitectureLayer.Process,
            ArchitectureTag.ErrorHandling, ArchitectureTag.Readability),
        Principle(
            "Use inbox and outbox for message-based integrations",
            "When queues or asynchronous messages are involved, handle them through inbox and outbox patterns. Process them once, mark them complete, and move them to a separate archive table so the active queue stays small and fast.",
            ProcessExamples.UseInboxAndOutboxForMessageBasedIntegrations,
            ArchitectureLayer.Process,
            ArchitectureTag.ErrorHandling, ArchitectureTag.Delivery, ArchitectureTag.Readability),
        Principle(
            "Unhandled exceptions are persisted for debugging",
            "Store unhandled exceptions in the database with enough structured detail to investigate failures after the request is gone. A production error is easier to fix when its data survives longer than a log tail.",
            ProcessDeliveryExamples.UnhandledExceptionsArePersistedForDebugging,
            ArchitectureLayer.Process,
            ArchitectureTag.Observability, ArchitectureTag.ErrorHandling, ArchitectureTag.Database),
        Principle(
            "HTTP requests are persisted for debugging",
            "Store HTTP requests in the database with enough detail to reconstruct what the user actually sent. A request log makes production behaviour inspectable after the request has finished.",
            ProcessDeliveryExamples.HTTPRequestsArePersistedForDebugging,
            ArchitectureLayer.Process,
            ArchitectureTag.Observability, ArchitectureTag.ApiDesign, ArchitectureTag.Database),
        Principle(
            "Development APIs return rich exception details",
            "In development environments the API returns enough exception data to diagnose failures quickly: message, stack trace, and useful inner-exception details. Local debugging speed matters more than polished error responses.",
            ProcessDeliveryExamples.DevelopmentAPIsReturnRichExceptionDetails,
            ArchitectureLayer.Process,
            ArchitectureTag.Observability, ArchitectureTag.ErrorHandling, ArchitectureTag.DotNet),
        Principle(
            "Request and exception logs tell one story",
            "Debugging gets much simpler when request logs and exception logs can be read together into one narrative of what the user did, what the system received, and where it failed.",
            ProcessDeliveryExamples.RequestAndExceptionLogsTellOneStory,
            ArchitectureLayer.Process,
            ArchitectureTag.Observability, ArchitectureTag.ErrorHandling, ArchitectureTag.Readability),
    ];
}
