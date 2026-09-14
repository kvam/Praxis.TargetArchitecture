using Praxis.TargetArchitecture.Entities;
using Praxis.TargetArchitecture.Data.Seed.Examples;

using static Praxis.TargetArchitecture.Data.Seed.ArchitecturePrincipleSeed;

namespace Praxis.TargetArchitecture.Data.Seed;

public static class ContractPrinciples
{
    public static List<ArchitecturePrinciple> All =>
    [
        // Contract: The contract is generated, not agreed
        Principle(
            "Backend owns the contract",
            "DTOs, enums, and shared constants originate in the backend so there is exactly one definition of the domain to disagree with.",
            ArchitectureLayer.Contract,
            ArchitectureTag.ApiDesign, ArchitectureTag.CodeGeneration, ArchitectureTag.DotNet),
        Principle(
            "Frontend models are generated, never written",
            "Models come from the backend OpenAPI document through openapi-ts. Hand-written copies drift silently; generated ones break the build instead.",
            ArchitectureLayer.Contract,
            ArchitectureTag.CodeGeneration, ArchitectureTag.TypeScript, ArchitectureTag.ApiDesign),
        Principle(
            "Generated artifacts are never hand-edited",
            "Anything under src/models/generated is derived output. Editing it moves the fix away from its cause and is erased by the next regeneration.",
            ArchitectureLayer.Contract,
            ArchitectureTag.CodeGeneration, ArchitectureTag.TypeScript, ArchitectureTag.Automation),
        Principle(
            "A missing type is a backend problem",
            "When the frontend needs a shape it does not have, the fix is the backend DTO followed by regeneration, never a hand-written interface that papers over the gap.",
            ContractExamples.AMissingTypeIsABackendProblem,
            ArchitectureLayer.Contract,
            ArchitectureTag.CodeGeneration, ArchitectureTag.ApiDesign, ArchitectureTag.TypeScript),
        Principle(
            "Enums cross the boundary as values",
            "By using an OpenAPI schema transformer, backend enums arrive in the frontend as runtime objects rather than bare numbers.",
            ContractExamples.EnumsCrossTheBoundaryAsValues,
            ArchitectureLayer.Contract,
            ArchitectureTag.CodeGeneration, ArchitectureTag.TypeScript, ArchitectureTag.ApiDesign),
        Principle(
            "Enums start at 1",
            "A zero enum value hides bugs because it matches the default value. Starting at 1 makes an uninitialized enum stand out instead of looking valid.",
            ContractExamples.EnumsStartAt1,
            ArchitectureLayer.Contract,
            ArchitectureTag.CodeGeneration, ArchitectureTag.Readability, ArchitectureTag.ApiDesign),
        Principle(
            "Entities never cross the wire",
            "Endpoints return DTOs, so persistence concerns and lazy-loaded relations cannot leak into the published contract.",
            ContractExamples.EntitiesNeverCrossTheWire,
            ArchitectureLayer.Contract,
            ArchitectureTag.ApiDesign, ArchitectureTag.Database, ArchitectureTag.DotNet),
        Principle(
            "DTO properties are required by default",
            "Optionality is expressed by the type rather than by omission, so the generated frontend model tells the truth about what will actually be present.",
            ContractExamples.DTOPropertiesAreRequiredByDefault,
            ArchitectureLayer.Contract,
            ArchitectureTag.ApiDesign, ArchitectureTag.TypeScript, ArchitectureTag.Readability),
        Principle(
            "Identifiers carry their entity name",
            "A DTO exposes ArchitecturePrincipleId, never a bare Id, so an identifier remains unambiguous once it is passed around the frontend.",
            ContractExamples.IdentifiersCarryTheirEntityName,
            ArchitectureLayer.Contract,
            ArchitectureTag.Naming, ArchitectureTag.ApiDesign, ArchitectureTag.Readability),
        Principle(
            "External API enums cross the wire as strings",
            "Enums on a contract published to another organisation are sent as strings, not numbers, so renumbering an internal enum cannot silently change what an external consumer reads. Their release schedule is not yours to coordinate.",
            ArchitectureLayer.Contract,
            ArchitectureTag.ApiDesign, ArchitectureTag.CodeGeneration, ArchitectureTag.DotNet),
    ];
}
