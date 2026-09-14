using Praxis.TargetArchitecture.Entities;
using Praxis.TargetArchitecture.Data.Seed.Examples;

using static Praxis.TargetArchitecture.Data.Seed.ArchitecturePrincipleSeed;

namespace Praxis.TargetArchitecture.Data.Seed;

public static class BackendPrinciples
{
    public static List<ArchitecturePrinciple> All =>
    [
        // Backend: Vertical slices
        Principle(
            "Vertical over horizontal",
            "Code is grouped by feature, never by technical role. Layered folders force every change to be made in four places at once.",
            SliceExamples.VerticalOverHorizontal,
            ArchitectureLayer.Backend,
            ArchitectureTag.VerticalSlices, ArchitectureTag.Simplicity, ArchitectureTag.DotNet),
        Principle(
            "Every endpoint is a vertical slice",
            "A slice under Features/Api/<Feature>/<Action>/ owns its controller, service, and DTOs, so a feature is read, changed, and deleted in one place.",
            BackendExamples.EveryEndpointIsAVerticalSlice,
            ArchitectureLayer.Backend,
            ArchitectureTag.VerticalSlices, ArchitectureTag.DotNet, ArchitectureTag.ApiDesign),
        Principle(
            "A new endpoint is a new folder",
            "Endpoints are never added as extra methods on an existing controller. One folder, one route, one reason to open the file.",
            ArchitectureLayer.Backend,
            ArchitectureTag.VerticalSlices, ArchitectureTag.ApiDesign, ArchitectureTag.Simplicity),
        Principle(
            "A slice service has one public method",
            "The slice is named after what it does, so a second public entry point means a second slice rather than a growing service.",
            SliceExamples.ASliceServiceHasOnePublicMethod,
            ArchitectureLayer.Backend,
            ArchitectureTag.VerticalSlices, ArchitectureTag.Simplicity, ArchitectureTag.DotNet),
        Principle(
            "Controllers are thin",
            "A controller validates input, calls one service, and returns. Logic in the transport layer cannot be tested or reused.",
            BackendExamples.ControllersAreThin,
            ArchitectureLayer.Backend,
            ArchitectureTag.VerticalSlices, ArchitectureTag.DotNet, ArchitectureTag.ApiDesign),
        Principle(
            "Validation happens at the edge",
            "Shape validation runs in the controller before the service is called, so a service can assume its input is structurally sound and only enforce domain rules.",
            SliceExamples.ValidationHappensAtTheEdge,
            ArchitectureLayer.Backend,
            ArchitectureTag.ApiDesign, ArchitectureTag.ErrorHandling, ArchitectureTag.DotNet),

        // Backend: Slice isolation
        Principle(
            "Slices do not reference each other's types",
            "A DTO defined in one slice stays in that slice. Duplicating a small shape is cheaper than a dependency that outlives the reason for it.",
            BackendExamples.SlicesDoNotReferenceEachOthersTypes,
            ArchitectureLayer.Backend,
            ArchitectureTag.VerticalSlices, ArchitectureTag.Simplicity, ArchitectureTag.DotNet),
        Principle(
            "There is no shared Dtos, Models, or Helpers folder",
            "Contract types live in the slice that returns them and utilities live where they are used. Catch-all folders are where architecture goes to erode.",
            ArchitectureLayer.Backend,
            ArchitectureTag.VerticalSlices, ArchitectureTag.Simplicity, ArchitectureTag.Naming),
        Principle(
            "No manager, helper, or utility classes",
            "A class named for what it is rather than what it does attracts unrelated code. Behaviour belongs to the slice or to a named platform concern.",
            SliceExamples.NoManagerHelperOrUtilityClasses,
            ArchitectureLayer.Backend,
            ArchitectureTag.Naming, ArchitectureTag.Simplicity, ArchitectureTag.VerticalSlices),
        Principle(
            "Shared code must earn its place",
            "Code moves into AppInfrastructure only once more than one slice needs it. Premature sharing couples features that were meant to evolve apart.",
            ArchitectureLayer.Backend,
            ArchitectureTag.VerticalSlices, ArchitectureTag.Simplicity, ArchitectureTag.DotNet),
        Principle(
            "A slice can be deleted by deleting its folder",
            "Removing a feature should leave nothing behind but a service registration. If deletion is hard, the slice was not self-contained.",
            ArchitectureLayer.Backend,
            ArchitectureTag.VerticalSlices, ArchitectureTag.Simplicity, ArchitectureTag.DotNet),
        Principle(
            "The slice folder is the unit of review",
            "A feature change should touch one folder. A diff spread across many slices is a signal that shared state or shared logic has crept in.",
            ArchitectureLayer.Backend,
            ArchitectureTag.VerticalSlices, ArchitectureTag.Review),
        Principle(
            "Only one cross-slice call is sanctioned",
            "A write controller may call the matching Get service to build its response. This keeps the read shape defined once and makes every other coupling a deliberate refactor.",
            ArchitectureLayer.Backend,
            ArchitectureTag.VerticalSlices, ArchitectureTag.ApiDesign, ArchitectureTag.DotNet),

        // Backend: Failure handling
        Principle(
            "Exceptions are for errors, not flow",
            "Throw exceptions to surface a real error and let middleware turn it into a sensible API response. Do not use exceptions as ordinary program flow.",
            BackendExamples.ExceptionsAreForErrorsNotFlow,
            ArchitectureLayer.Backend,
            ArchitectureTag.ErrorHandling, ArchitectureTag.DotNet, ArchitectureTag.ApiDesign),
        Principle(
            "try/catch is a last resort",
            "Catch only when you can genuinely handle the failure: adding context to a third-party error, cleaning up a resource, or stopping a background job from dying. Everywhere else, let it bubble to the middleware.",
            BackendExamples.TryCatchIsALastResort,
            ArchitectureLayer.Backend,
            ArchitectureTag.ErrorHandling, ArchitectureTag.Simplicity, ArchitectureTag.DotNet),
        Principle(
            "Never catch what you cannot handle",
            "A catch that logs and rethrows, swallows silently, or returns a default hides the failure from the one place designed to report it and turns a clear error into a mystery later.",
            SliceExamples.NeverCatchWhatYouCannotHandle,
            ArchitectureLayer.Backend,
            ArchitectureTag.ErrorHandling, ArchitectureTag.Simplicity, ArchitectureTag.DotNet),
    ];
}
