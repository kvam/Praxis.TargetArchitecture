# Backend

ASP.NET Core (net10.0), vertical slices, EF Core. The reference slices are
`Features/Api/ArchitecturePrinciples/{Get,Create}`.

## Layout

```text
backend/Praxis.TargetArchitecture/
  AppInfrastructure/
    Authorization/            CurrentUser
    ControllerAttributes/     PraxisControllerBase, ActionType
    Exceptions/               domain exceptions
    Middleware/               exception -> HTTP translation
    OpenApi/                  enum schema transformer
    PraxisIocConfiguration.cs   service registration
    PraxisTypescriptGenerator.cs
  Data/PraxisDbContext.cs
  Entities/
  Features/Api/<Feature>/<Action>/
```

`AppInfrastructure` is the only shared layer. If something is used by one slice, it
belongs in that slice.

## Run it

```bash
cd backend
dotnet build
dotnet test
dotnet run --project Praxis.TargetArchitecture
```

The API listens on `http://localhost:5153`; the OpenAPI document is at
`/openapi/v1.json`. `backend/Praxis.TargetArchitecture.http` has ready-made requests.

The starter uses the EF Core in-memory provider so it runs with no database. Swap
`UseInMemoryDatabase` in `Program.cs` for a real provider in a concrete project.

## Seed data

`Data/PraxisDbSeeder.cs` runs at startup and fills `ArchitecturePrinciples` with the
principles this standard is built on, so the API returns something meaningful on a fresh
clone. It is idempotent — it inserts only principles whose titles are missing — so it is
safe against a persistent database.

Each principle carries a stable `Number`, assigned from its position in the seed list and
returned in the DTO. The number belongs to the principle, not to its position in a
result, so filtering or searching in the frontend never renumbers anything. The seed list
is grouped by layer and then by theme, with a comment heading per group — add a new
principle to the group it belongs to. Principles created through the API take the next
number after the highest existing one.

The list itself lives in `Data/Seed/`, one file per layer (`ContractPrinciples`,
`BackendPrinciples`, …, with `Collaboration` split into `TeamPrinciples` and
`FlowPrinciples`), because a single list of this size would blow past the 200-line bar.
`PraxisDbSeeder` concatenates them in order, and that order is what assigns the numbers.

Because the sample runs on an in-memory database, the store is empty on every start and
the whole list is reseeded, so numbers always match the declaration order. Against a
persistent database the idempotent insert would keep existing rows at their old numbers,
so reordering the seed list there means renumbering the existing rows too.

Each principle also carries two or three `ArchitectureTag` values. A principle belongs to
exactly one layer but usually touches several themes, so tags are the cross-cutting view:
`.NET`, `flow`, `dependencies`, and so on. Tags are an enum rather than free
text, so the vocabulary is owned by the backend and reaches the frontend as runtime values
through the same transformer that handles `ArchitectureLayer`.

Some principles carry a worked `Example`. Examples are prose and code rather than one-line
rationales, so they live apart from the principle lists in `Data/Seed/Examples/`, keyed by
the exact principle title, and `PraxisDbSeeder` attaches them while it numbers the list.
Those files are split by theme rather than by layer — `SliceExamples`, `QueryExamples`,
`CodeStyleExamples`, and so on — because an example runs to twenty lines and a per-layer
file would pass the 200-line bar within a handful of entries. That
keeps the principle files short and makes an unmatched key a test failure rather than a
silent no-op. `Example` is `required string?`: always present on the wire, null when a
principle has none, so the frontend decides from the type alone whether to offer the
dialog.

## Adding a slice

### 1. Entity (only if you need new storage)

`Entities/ArchitecturePrinciple.cs`, implementing `IDateTrackedEntity`, with audit
properties in an `#region Audit` block. Add a `DbSet` to `PraxisDbContext`.

### 2. Read slice

`Features/Api/ArchitecturePrinciples/Get/GetArchitecturePrinciplesService.cs` holds the
service **and** the DTO it returns:

```csharp
public class GetArchitecturePrinciplesService(PraxisDbContext context)
{
    public async Task<List<ArchitecturePrincipleDto>> Get()
    {
        return await context.ArchitecturePrinciples
            .AsNoTracking()
            .OrderBy(principle => principle.Title)
            .Select(principle => new ArchitecturePrincipleDto { /* ... */ })
            .ToListAsync();
    }

    public async Task<ArchitecturePrincipleDto> GetById(Guid architecturePrincipleId)
    {
        var principle = await context.ArchitecturePrinciples
            .AsNoTracking()
            .SingleOrDefaultAsync(candidate => candidate.Id == architecturePrincipleId);

        if (principle == null)
        {
            throw new NotFoundException($"Architecture principle {architecturePrincipleId} was not found");
        }

        return new ArchitecturePrincipleDto { /* ... */ };
    }
}
```

The controller next to it is three lines and does nothing but delegate.

### 3. Write slice

The create service validates domain invariants (throwing `ConflictException`), saves,
and returns the new id. Its controller runs the static DTO validator, calls the create
service, then calls the **Get** service to produce the response. That is the only
cross-slice call the standard permits.

### 4. Register the services

Add them to `AppInfrastructure/PraxisIocConfiguration.cs`.

### 5. Test

Mirror the slice path under `Praxis.TargetArchitecture.Tests/`, using a uniquely named
in-memory `PraxisDbContext` per test.

### 6. Hand the contract to the frontend

Run the backend, then ask a human to run `npm run autogenerate-models` in `frontend/`.

## Enums reach the frontend as values

`PraxisEnumSchemaTransformer` adds `enum` plus `x-enum-varnames` to the OpenAPI schema of
every backend enum. Without it, `@hey-api/openapi-ts` can only emit a numeric type
alias. With it, the frontend gets a usable runtime object:

```ts
export const ArchitectureLayer = { Backend: 1, Frontend: 2, Contract: 3 } as const
```

## Shared constants and enum labels

`PraxisTypescriptGenerator` writes `frontend/src/models/generated/constants.ts` at startup
in every environment except production. It emits two things: non-enum constants both sides
need, and a label map per enum. The enums themselves already reach the frontend through
OpenAPI, so the generated file imports them rather than declaring a second copy.

Running the backend is enough to refresh it — there is no flag to remember, so the file
cannot drift because someone forgot to opt in.

Display labels live in `Entities/ArchitectureLabels.cs` as a plain switch expression, so
the name a user reads is owned by the backend alongside the value:

```csharp
public static string For(ArchitectureTag tag) => tag switch
{
    ArchitectureTag.ErrorHandling => "error handling",
    ...
};
```

That removes the hand-written label map the frontend used to keep. A member added without
a label throws, which fails `PraxisTypescriptGeneratorTests` rather than shipping a blank
chip. The backend stays the source of truth; the TypeScript file is a derived artifact.

## Configuration and switches

`AppInfrastructure/PraxisEnvironments.cs` is the single place that decides what is on:

```csharp
public static bool ExposeOpenApiDocument => !IsProduction;
public static bool SeedArchitecturePrinciples => !IsProduction;
public static bool WriteTypescriptFiles => !IsProduction;
```

`Program.cs` reads those properties rather than poking at configuration. A switch
expressed in C# can be found with a search, followed with go-to-definition, and checked
by the compiler — none of which is true of a key in a settings file.

`appsettings.json` deliberately holds almost nothing. Secrets are never added to it; set
them locally with user secrets instead:

```bash
cd backend/Praxis.TargetArchitecture
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Praxis" "<connection string>"
```
