using Praxis.TargetArchitecture.Entities;
using Praxis.TargetArchitecture.Data.Seed.Examples;

using static Praxis.TargetArchitecture.Data.Seed.ArchitecturePrincipleSeed;

namespace Praxis.TargetArchitecture.Data.Seed;

public static class TeamPrinciples
{
    public static List<ArchitecturePrinciple> All =>
    [
        // Collaboration: Everyone works across the stack
        Principle(
            "Everyone works across the stack",
            "Every developer works in both the backend and the frontend. Being an expert in one is expected and welcome; refusing to learn the other is not, and you are expected to read, change, and review the whole feature you are working on.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Learning, ArchitectureTag.VerticalSlices),
        Principle(
            "A feature is delivered end to end",
            "The person who changes a DTO changes the frontend that consumes it. Splitting a feature at the contract turns one task into two queues and a handoff.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Delivery, ArchitectureTag.VerticalSlices),
        Principle(
            "No frontend team and backend team",
            "There are no walls to throw work over. Teams organized around layers optimize for their layer, and the seams between them are where systems rot.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Learning, ArchitectureTag.Delivery),
        Principle(
            "The stack is chosen to be learnable",
            "Vertical slices, generated models, and a small dependency list exist so that crossing the boundary is cheap. Every rule here lowers the cost of working in the other half.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Learning, ArchitectureTag.Simplicity),
        Principle(
            "Learning the other half is part of the job",
            "Time spent getting comfortable in the unfamiliar half is work, not overhead. It is repaid the first time a change does not have to wait for someone else.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Ownership, ArchitectureTag.Learning),
        Principle(
            "Review outside your specialty",
            "Read code in the half you are weaker in. If you cannot follow it, that is evidence about the code, not about you, and the code should be simplified.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Review, ArchitectureTag.Learning),
        Principle(
            "Leave the code readable for whoever is weakest there",
            "Write the backend so a frontend specialist can follow it, and the frontend so a backend specialist can. That audience is the one-year bar in practice.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Readability, ArchitectureTag.Learning),

        // Collaboration: Small teams and trust
        Principle(
            "Small teams outperform large ones",
            "A small team shares context without meetings, trusts by default, and decides in a conversation. Adding people to a team buys throughput with coordination and usually loses the trade.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Ownership, ArchitectureTag.Delivery),
        Principle(
            "Trust is the multiplier",
            "High trust is what lets a small team move: work is handed over without hedging, disagreement is cheap, and nobody writes defensively to protect themselves.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Ownership, ArchitectureTag.Review),
        Principle(
            "Every extra person adds communication paths",
            "Connections between people grow faster than the people do. What felt like a shared understanding at four becomes a document, a meeting, and a misunderstanding at ten.",
            TeamworkExamples.EveryExtraPersonAddsCommunicationPaths,
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Communication, ArchitectureTag.Delivery),
        Principle(
            "Process is what replaces trust",
            "Each new rule, gate, or sign-off is usually a substitute for a conversation that no longer scales. Prefer fixing the trust or shrinking the team over adding the process.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Ownership, ArchitectureTag.Simplicity),
        Principle(
            "Autonomy over approval",
            "A trusted developer ships a change and lets review catch what it catches. Requiring permission before starting costs more than the mistakes it prevents.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Ownership, ArchitectureTag.Review, ArchitectureTag.Delivery),
        Principle(
            "Split the work, not the feature",
            "When a team must grow, split it around whole vertical features with their own slices. Splitting by layer manufactures the handoffs the architecture is designed to avoid.",
            TeamworkExamples.SplitTheWorkNotTheFeature,
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Flow, ArchitectureTag.VerticalSlices, ArchitectureTag.Delivery),
        Principle(
            "Shared ownership, no personal territory",
            "Anyone may change any part of the system. Code owned by one person becomes a queue, and a queue is a small team pretending to be a smaller one.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Ownership, ArchitectureTag.Review, ArchitectureTag.Learning),
        Principle(
            "Experts teach rather than gatekeep",
            "Knowing an area well means explaining it and pairing on it. A part of the system only one person can touch is a risk, not a strength.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Ownership, ArchitectureTag.Learning, ArchitectureTag.Review),
        Principle(
            "Review your own work with fresh eyes",
            "Come back to your own branch in a separate sitting before asking anyone else to read it. Straight after writing the code you are still defending every trade-off you just talked yourself into.",
            ArchitectureLayer.Collaboration,
            ArchitectureTag.Review, ArchitectureTag.Learning),
    ];
}
