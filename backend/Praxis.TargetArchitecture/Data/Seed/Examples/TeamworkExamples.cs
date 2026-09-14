namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class TeamworkExamples
{
    public const string EveryExtraPersonAddsCommunicationPaths = """
            The paths grow as n(n-1)/2, while the people grow as n:

                3 people   ->  3 paths
                5 people   ->  10 paths
                8 people   ->  28 paths
                12 people  ->  66 paths

            Going from five to eight adds three pairs of hands and eighteen new
            conversations that have to happen for everyone to stay aligned. That is why
            the standing meeting appears, then the status document, then the coordination
            role — each one a patch over a number nobody chose deliberately.

            A late project does not want more people. It wants fewer paths.
            """;

    public const string ContextSwitchingIsTheMostExpensiveThingYouDo = """
            A day that looks productive and is not:

                // WRONG
                09:00  feature A, finally have the model in your head
                10:30  "quick" review of feature B
                11:00  back to A, re-reading what you wrote at 09:00
                13:00  production question about feature C
                14:00  back to A, re-reading again

            Three tasks touched, one finished, and an hour spent rebuilding context you
            already had. The cost is not the interruption; it is the reload afterwards.

            Two things help: finish A before starting B, and batch the interruptible work
            so the deep work has an unbroken run.
            """;

    public const string WaitForTheSecondExample = """
            One use, and the abstraction is a guess:

                // WRONG
                // After writing a single list screen
                public abstract class FilterableListService<TEntity, TDto, TFilter> { ... }

            The second screen never fits. It needs paging, or a different sort, or two
            filters — so the base class grows flags, and every caller now pays for
            options it does not use.

            Write the second one by copying the first. With two concrete versions side by
            side, the parts that are genuinely the same are visible rather than imagined,
            and the abstraction you extract on the third is one you can actually defend.
            """;

    public const string BlockedWorkIsMadeVisibleNotStacked = """
            The instinct is to keep moving:

                // WRONG
                Feature A  -> blocked, waiting on an API key
                Feature B  -> started instead
                Feature B  -> blocked, waiting on a design answer
                Feature C  -> started instead

            Now three things are half-done, nothing can ship, and the two blockers are
            still exactly as unresolved as they were — but they are invisible, because the
            board shows three items in progress rather than two people stuck.

            Say it out loud the moment it happens. A blocker someone else can clear in
            five minutes will otherwise sit for a day, with a pile of unfinished work
            growing on top of it.
            """;

    public const string DeletingIsAContribution = """
            Things worth deleting today:

                - the feature flag whose rollout finished last quarter
                - the endpoint no client has called since the rewrite
                - the commented-out block someone kept "just in case"
                - the config option that has only ever had one value

            All of it still gets read, reviewed, compiled, and reasoned about. Every
            reader spends a moment deciding it does not matter.

            Delete it. Version control remembers; your colleagues should not have to. A
            pull request that only removes code is one of the most valuable kinds there
            is, and it is almost always the easiest to review.
            """;

    public const string SplitTheWorkNotTheFeature = """
            Split by layer, and nothing works until everything does:

                // WRONG
                Developer A: the backend slice     (done Tuesday, unverifiable)
                Developer B: the frontend screen   (done Thursday, against a guess)

            Two people, one integration day, and a contract that turns out not to match.

            Split by feature, and each part is deliverable on its own:

                // RIGHT
                Developer A: list the principles, end to end
                Developer B: create a principle, end to end

            Each can be built, tested, reviewed, and released without waiting on the
            other. Handoffs inside a feature are where the days disappear.
            """;

    public const string PairAcrossTheBoundary = """
            The most useful pairing session in this architecture is one person who knows
            the backend and one who knows the frontend, working the same slice:

                // RIGHT
                together:  agree the DTO
                driver:    write the endpoint and the service
                swap:      run autogenerate-models
                driver:    write the hook and the screen against the generated type

            By the end the contract has been argued about by both sides before it was
            frozen, and two people understand the whole vertical rather than one half
            each. That is how "everyone works across the stack" actually happens — not by
            assignment, but by sitting next to someone who already knows.
            """;
}
