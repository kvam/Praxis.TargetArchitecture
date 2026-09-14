using Praxis.TargetArchitecture.Entities;
using Praxis.TargetArchitecture.Data.Seed.Examples;

using static Praxis.TargetArchitecture.Data.Seed.ArchitecturePrincipleSeed;

namespace Praxis.TargetArchitecture.Data.Seed;

public static class FlowPrinciples
{
    public static List<ArchitecturePrinciple> All =>
    [
        // Collaboration: Limit work in progress
        Principle(
            "Limit work in progress to an absolute minimum",
            "Start as little as possible and finish it. Everything in flight is unfinished inventory that has cost time and delivered nothing yet.",
            CollaborationExamples.LimitWorkInProgressToAnAbsoluteMinimum,
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Flow, ArchitectureTag.Delivery),
        Principle(
            "Finish before you start",
            "Stop starting and start finishing. When something new arrives, the first question is what gets completed or dropped to make room for it.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Flow, ArchitectureTag.Delivery),
        Principle(
            "Started is not delivered",
            "A change has value when it is released and running, not when it is written. Three tasks at ninety percent are worth exactly nothing.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Flow, ArchitectureTag.Delivery),
        Principle(
            "Context switching is the most expensive thing you do",
            "Every parallel task keeps a mental model loaded and rebuilds it on every return. Two tasks at once take longer than the same two done in sequence.",
            TeamworkExamples.ContextSwitchingIsTheMostExpensiveThingYouDo,
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Flow, ArchitectureTag.Delivery),
        Principle(
            "Small changes merge fast",
            "A change sized to be reviewed in one sitting spends less time in flight, collides with less other work, and is easier to reason about when it breaks.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Flow, ArchitectureTag.Delivery, ArchitectureTag.Review),
        Principle(
            "Long-lived branches are work in progress",
            "A branch that lives for weeks accumulates conflicts and hides risk until the worst possible moment. Integrate early and often.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Flow, ArchitectureTag.Delivery, ArchitectureTag.Review),
        Principle(
            "Reviewing beats starting",
            "Work waiting for review is work already paid for and not yet earning. Clearing someone else's queue is more valuable than opening a new task of your own.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Flow, ArchitectureTag.Review, ArchitectureTag.Delivery),
        Principle(
            "Blocked work is made visible, not stacked",
            "When something is genuinely stuck, say so and resolve the block. Quietly starting a third task to stay busy converts one delay into three.",
            TeamworkExamples.BlockedWorkIsMadeVisibleNotStacked,
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Flow, ArchitectureTag.Delivery),

        // Collaboration: Restraint
        Principle(
            "Doing nothing beats doing the wrong thing",
            "Not acting is always an option on the table. A wrong change costs the work, the revert, and everything built on it in between; waiting costs only time.",
            CollaborationExamples.DoingNothingBeatsDoingTheWrongThing,
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Flow, ArchitectureTag.Simplicity),
        Principle(
            "Understand before you act",
            "Being busy is not progress. If the problem is not yet clear, the highest-value work is understanding it, not producing code that proves you were working.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Learning, ArchitectureTag.Simplicity),
        Principle(
            "The best code is the code you never wrote",
            "Every line is reviewed, tested, maintained, and eventually migrated. The cheapest feature is the one talked out of existence before it was built.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Simplicity, ArchitectureTag.Delivery),
        Principle(
            "Wait for the second example",
            "Build for what exists, not what might. An abstraction invented for one case is a guess, and the guess is usually wrong in a way that is expensive to undo.",
            TeamworkExamples.WaitForTheSecondExample,
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Simplicity, ArchitectureTag.Readability),
        Principle(
            "Prefer the reversible decision",
            "When a choice is easy to undo, make it and move on. When it is hard to undo, slow down — those are the only decisions that deserve the delay.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Delivery, ArchitectureTag.Simplicity),
        Principle(
            "Deleting is a contribution",
            "Removing a feature, a dependency, or dead code makes the system smaller and every future change cheaper. Subtraction counts as work.",
            TeamworkExamples.DeletingIsAContribution,
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Simplicity, ArchitectureTag.Readability),

        // Collaboration: Pairing
        Principle(
            "Pair on anything hard",
            "Two people on a gnarly bug, an unfamiliar area, or a decision that is hard to undo is faster than one person stuck and a review afterwards.",
            CollaborationExamples.PairOnAnythingHard,
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Review, ArchitectureTag.Learning),
        Principle(
            "Pairing is review that happens immediately",
            "A problem caught while the code is being written costs a sentence. The same problem caught in review costs a round trip, a rebase, and a rebuilt mental model.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Review, ArchitectureTag.Delivery),
        Principle(
            "Pair across the boundary",
            "The fastest way to learn the other half of the stack is to build something in it next to someone who knows it. Documentation cannot answer the follow-up question.",
            TeamworkExamples.PairAcrossTheBoundary,
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Learning, ArchitectureTag.Review),
        Principle(
            "Pairing spreads ownership",
            "Two people who have written a piece of code can both change it later. Knowledge held by one person is a bottleneck disguised as expertise.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Ownership, ArchitectureTag.Learning, ArchitectureTag.Review),
        Principle(
            "Pairing is not supervision",
            "Both people are peers and both take the keyboard. The less experienced one should drive more, because the person typing is the person learning.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Ownership, ArchitectureTag.Learning),
        Principle(
            "Pair deliberately, not permanently",
            "Pairing is a tool, not a rule. Routine, well-understood work is fine alone, and long stretches of it together cost more attention than they return.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Flow, ArchitectureTag.Simplicity),

        // Collaboration: Raise questions immediately
        Principle(
            "Do not wait for the next meeting to raise a question",
            "Ask when the question appears. A blocker raised on Monday and answered in minutes is a different thing from the same blocker raised at Thursday's meeting.",
            CollaborationExamples.DoNotWaitForTheNextMeetingToRaiseAQuestion,
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Communication, ArchitectureTag.Delivery),
        Principle(
            "Meetings are not the queue for questions",
            "A stand-up is a sync point, not the place where problems wait to be reported. If something needs an answer to move, it needed it hours ago.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Communication, ArchitectureTag.Delivery),
        Principle(
            "The person who can unblock you would rather be asked",
            "Nobody prefers discovering a day later that someone sat stuck out of politeness. Asking respects their time more than protecting it does.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Communication, ArchitectureTag.Learning),
        Principle(
            "Assume you are allowed to interrupt",
            "Fifteen minutes of someone's attention is cheaper than a day of guessing. Protecting focus matters, but not at the cost of work going the wrong direction.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Communication, ArchitectureTag.Learning),
        Principle(
            "Answer in a place others can find",
            "Ask in the open channel rather than a private message, so the answer is searchable and the next person does not have to ask it again. Silence is more expensive than looking inexperienced.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Communication, ArchitectureTag.Learning, ArchitectureTag.Review),
    ];
}
