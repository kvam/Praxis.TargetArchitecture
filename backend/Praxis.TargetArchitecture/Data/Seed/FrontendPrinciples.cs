using Praxis.TargetArchitecture.Entities;
using Praxis.TargetArchitecture.Data.Seed.Examples;

using static Praxis.TargetArchitecture.Data.Seed.ArchitecturePrincipleSeed;

namespace Praxis.TargetArchitecture.Data.Seed;

public static class FrontendPrinciples
{
    public static List<ArchitecturePrinciple> All =>
    [
        // Frontend: Server state
        Principle(
            "Server state lives in the query cache",
            "Data owned by the backend is read through query hooks rather than copied into component state, which removes a whole class of stale-copy bugs.",
            FrontendExamples.ServerStateLivesInTheQueryCache,
            ArchitectureLayer.Frontend,
            ArchitectureTag.React, ArchitectureTag.TypeScript, ArchitectureTag.Simplicity),
        Principle(
            "One hook per endpoint",
            "Each read or write gets its own hook in its feature folder, so finding every caller of an endpoint is a file search rather than an investigation.",
            HookExamples.OneHookPerEndpoint,
            ArchitectureLayer.Frontend,
            ArchitectureTag.React, ArchitectureTag.ApiDesign, ArchitectureTag.Simplicity),
        Principle(
            "Query keys and cache defaults are shared",
            "Queries key from the QueryKeys enum and spread defaultQueryConfig, so cache invalidation is a lookup rather than a guess about matching strings.",
            ArchitectureLayer.Frontend,
            ArchitectureTag.React, ArchitectureTag.TypeScript, ArchitectureTag.Simplicity),
        Principle(
            "Mutations invalidate what they changed",
            "A write hook invalidates the affected query keys on success, which keeps the UI truthful without manual refetch calls in components.",
            FrontendExamples.MutationsInvalidateWhatTheyChanged,
            ArchitectureLayer.Frontend,
            ArchitectureTag.React, ArchitectureTag.ApiDesign, ArchitectureTag.Simplicity),
        Principle(
            "Hooks return named values",
            "A mutation hook exposes createArchitecturePrinciple and isCreating rather than mutateAsync and isPending, so call sites read as intent instead of plumbing.",
            FrontendExamples.HooksReturnNamedValues,
            ArchitectureLayer.Frontend,
            ArchitectureTag.React, ArchitectureTag.Naming, ArchitectureTag.Readability),
        Principle(
            "API clients only speak HTTP",
            "Clients translate a call into a typed promise and hold no state, so swapping the transport never touches the layers above them.",
            ArchitectureLayer.Frontend,
            ArchitectureTag.React, ArchitectureTag.ApiDesign, ArchitectureTag.Simplicity),

        // Frontend: The backend decides
        Principle(
            "A failed mutation is read, not caught",
            "The mutation already tracks its own failure, so a component passes onSuccess and reads createError from the hook instead of wrapping the call in try/catch and keeping a second copy of state the mutation owns.",
            HookExamples.AFailedMutationIsReadNotCaught,
            ArchitectureLayer.Frontend,
            ArchitectureTag.ErrorHandling, ArchitectureTag.React, ArchitectureTag.Simplicity),
        Principle(
            "Error messages come from the backend",
            "The API returns a message and the frontend surfaces it, because a second copy of the same wording in the UI will eventually contradict the first.",
            HookExamples.ErrorMessagesComeFromTheBackend,
            ArchitectureLayer.Frontend,
            ArchitectureTag.ErrorHandling, ArchitectureTag.ApiDesign, ArchitectureTag.React),
        Principle(
            "Domain rules are not reimplemented in the UI",
            "If a value can be derived by the backend it is derived there and sent, so business rules cannot quietly fork between two languages.",
            HookExamples.DomainRulesAreNotReimplementedInTheUI,
            ArchitectureLayer.Frontend,
            ArchitectureTag.React, ArchitectureTag.ApiDesign, ArchitectureTag.Simplicity),

        // Frontend: Dependencies
        Principle(
            "The browser is the standard library",
            "fetch instead of axios, Intl instead of moment, crypto.randomUUID instead of uuid, URLSearchParams instead of a query-string package, template literals instead of classnames.",
            FrontendExamples.TheBrowserIsTheStandardLibrary,
            ArchitectureLayer.Frontend,
            ArchitectureTag.Dependencies, ArchitectureTag.React, ArchitectureTag.TypeScript),
        Principle(
            "No micro-dependencies",
            "A package that wraps a few lines of standard code costs a supply-chain risk, a version to upgrade, and a lookup for every reader, to save less than it costs.",
            ArchitectureLayer.Frontend,
            ArchitectureTag.Dependencies, ArchitectureTag.Security, ArchitectureTag.Simplicity),
        Principle(
            "Do not import a toolbelt for one function",
            "Pulling in lodash for groupBy or date-fns for one format call buys an entire library and its upgrade path. Write the helper next to where it is used.",
            HookExamples.DoNotImportAToolbeltForOneFunction,
            ArchitectureLayer.Frontend,
            ArchitectureTag.Dependencies, ArchitectureTag.Simplicity, ArchitectureTag.React),
    ];
}
