namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class ContractExamples
{
    public const string AMissingTypeIsABackendProblem = """
            If the frontend needs a shape the backend already exposes, change the backend DTO:

                // RIGHT
                public class ArchitecturePrincipleDto
                {
                    public required Guid ArchitecturePrincipleId { get; set; }
                    public required string Title { get; set; }
                }

            Then regenerate and import the generated model:

                // RIGHT
                import type { ArchitecturePrincipleDto } from "@/models/generated"

            What it must never do is paper over the gap with a hand-written interface:

                // WRONG
                type Principle = { id: string; title: string }

            The hand-written version keeps compiling long after the contract changed. The
            generated one forces the mismatch to surface the same day.
            """;

    public const string EnumsCrossTheBoundaryAsValues = """
            By using an OpenAPI schema transformer, this backend enum:

                public enum ArchitectureLayer
                {
                    Backend = 1,
                    Frontend = 2
                }

            arrives in TypeScript as a runtime object, not a bare number:

                export const ArchitectureLayer = { Backend: 1, Frontend: 2 } as const

            which means the frontend writes what it means:

                // RIGHT
                if (principle.layer === ArchitectureLayer.Backend)   // reads as intent
                if (principle.layer === 1)                           // reads as a riddle
            """;

    public const string EnumsStartAt1 = """
            Zero looks valid when it is actually just the default value:

                // WRONG
                public enum ArchitectureLayer
                {
                    Backend = 0,
                    Frontend = 1
                }

            Starting at 1 makes an uninitialized enum stand out:

                // RIGHT
                public enum ArchitectureLayer
                {
                    Backend = 1,
                    Frontend = 2
                }

            A zero enum value hides bugs because default(T) produces 0. Starting at 1 makes
            that mistake easier to spot in tests and in production data.
            """;

    public const string IdentifiersCarryTheirEntityName = """
            In the DTO, the identifier says what it identifies:

                // RIGHT
                public required Guid ArchitecturePrincipleId { get; set; }

            A bare Id reads fine right here, and nowhere else:

                // WRONG
                public required Guid Id { get; set; }

            The reason shows up three files later in the frontend, where the value has
            travelled far from the type it came from:

                // WRONG
                onSelect(id)                        // id of what?

            The same call site, when the name carries its entity:

                // RIGHT
                onSelect(architecturePrincipleId)   // unambiguous anywhere it travels

            Entities keep a plain Id, because inside the entity there is no ambiguity. The
            rename happens at the boundary, where the value starts travelling.
            """;

    public const string DTOPropertiesAreRequiredByDefault = """
            Required unless genuinely optional, and optionality stated in the type:

                // RIGHT
                public required string Title { get; set; }      // always present
                public required string? Example { get; set; }   // present, may be null

            The generated model then tells the frontend the truth:

                // RIGHT
                title: string
                example: string | null

            Leaving Title non-required would generate title?: string, and every call site
            would carry a null check for a value that is always there.
            """;

    public const string EntitiesNeverCrossTheWire = """
            Returning the entity looks like less work:

                // WRONG
                return await dbContext.ArchitecturePrinciples.ToListAsync(cancellationToken);

            It also publishes your audit columns, your foreign keys, and your navigation
            properties as a public contract. Rename a column and you have broken a client;
            add a sensitive column and you have leaked it, silently, to everyone.

            Project to a DTO the slice owns:

                // RIGHT
                .Select(principle => new ArchitecturePrincipleDto
                {
                    ArchitecturePrincipleId = principle.ArchitecturePrincipleId,
                    Title = principle.Title,
                    Number = principle.Number
                })

            The storage model and the wire model are now free to change independently,
            which is the entire point of having two of them.
            """;
}
