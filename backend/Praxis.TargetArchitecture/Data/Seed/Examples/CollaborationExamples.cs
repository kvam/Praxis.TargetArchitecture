namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class CollaborationExamples
{
    public const string LimitWorkInProgressToAnAbsoluteMinimum = """
            Two developers, four tasks, one week.

            In parallel: both start two tasks. On Friday all four are "almost done" —
            reviewed by nobody, running nowhere, each carrying a mental model that has to be
            rebuilt on Monday. Delivered value for the week: zero.

            In sequence: they take one task each and finish it, including review and
            release, before opening the next. On Friday two are live and two have not been
            started — and the two not started may have changed in the meantime, which is a
            gift rather than a loss.

            The second week produces the same amount of code and four times the value. The
            difference is not effort, it is how much was in flight at once.
            """;

    public const string DoingNothingBeatsDoingTheWrongThing = """
            A request arrives: "we need to support importing principles from a spreadsheet."

            The tempting response is to start building, because building feels like
            progress. The cheaper response is twenty minutes of questions: who is asking,
            how many principles, how often, what happens today.

            Often the answer is "one person, once, forty rows" — which is an afternoon with
            a script and no feature at all. Sometimes the answer is that the real problem is
            somewhere else entirely.

            The cost of waiting was twenty minutes. The cost of guessing wrong would have
            been the feature, the tests, the revert, and everything built on top of it in
            between.
            """;

    public const string PairOnAnythingHard = """
            Worth pairing on:

            - a bug nobody understands yet, where two sets of assumptions beat one
            - the first slice in an area you have never touched
            - a decision that is expensive to reverse, like a schema or contract change
            - anything you would otherwise have to explain carefully in review afterwards

            Not worth pairing on:

            - a rename, a copy of an existing slice, a dependency bump
            - anything where one person already knows exactly what to type

            The test is not seniority, it is uncertainty. Two people on a well-understood
            task is one person doing the work and one person watching.
            """;

    public const string DoNotWaitForTheNextMeetingToRaiseAQuestion = """
            Monday, 09:20. You are unsure whether a new endpoint should return the whole
            principle or just its number.

            Asking now: a message in the team channel, an answer over coffee, twenty minutes
            lost, and the answer is searchable for the next person.

            Waiting for Thursday's refinement: three days of building on a guess. If the
            guess was wrong, that is three days of work to unwind, plus the meeting you were
            waiting for anyway.

            The meeting is a sync point for things that need everyone. A blocker needs one
            person, right now.
            """;
}
