# Praxis.TargetArchitecture

An opinionated standard for a target architecture built around **vertical slices in the
backend** and **frontend models generated from backend models**.

It distills the parts of **dcd** and **streamline** worth standardizing, as working
scaffolding rather than prose: a runnable backend, a type-checked frontend, the AI
configuration that keeps both aligned, and documentation describing the rules.

## Standard principles

**The goal: anyone with a year of experience should be able to work in this system,
frontend or backend.** Everything below serves that.

1. **Backend-first contracts** — DTOs, enums, and shared constants originate in the backend.
2. **Vertical slices** — every endpoint owns its `Features/Api/<Feature>/<Action>/` folder.
3. **Generated frontend models** — the frontend imports generated types, never duplicates them.
4. **Thin transport layer** — controllers delegate; business rules live in services.
5. **Few dependencies** — prefer the platform; a new package must clear a high bar.
6. **Full-stack developers** — everyone works in both halves; specialize deeply, contribute broadly.
7. **Obvious beats short** — readability and consistency over clever abstractions.

## Repository layout

```text
backend/
  Praxis.TargetArchitecture/
    AppInfrastructure/        shared platform: controllers, exceptions, middleware, IoC, OpenAPI
    Data/                     PraxisDbContext with audit stamping
    Entities/                 persistence models
    Features/Api/             vertical slices
  Praxis.TargetArchitecture.Tests/
frontend/
  src/api/clients/            typed HTTP
  src/api/queries/            react-query read hooks + queryConfig
  src/api/mutations/          react-query write hooks
  src/models/generated/       produced by openapi-ts, never hand-edited
docs/                         the conventions and walkthroughs
.github/                      instructions, skills, CI
```

## What is included

**Backend platform** — `PraxisControllerBase`, domain exceptions with
`PraxisExceptionHandlingMiddleware` translating them to status codes, `PraxisIocConfiguration`
for registration, `PraxisDbContext` stamping `IDateTrackedEntity`/`IChangeTrackable`, and
`ActionType` authorization attributes.

**Reference slices** — `Features/Api/ArchitecturePrinciples/{Get,Create}` demonstrate a
read slice, a write slice, and the single sanctioned cross-slice call (a write controller
using the Get service to build its response). `Features/Api/Architecture/GetOverview`
shows a slice with no persistence. The database is seeded at startup with the principles
of this very architecture, so `GET /api/architecture-principles` describes the standard
it is written in. Each principle has one layer and two or three cross-cutting tags
(`.NET`, `dependencies`, `observability`, …) so the register can be read by theme.

**Contract generation** — `PraxisEnumSchemaTransformer` makes backend enums generate as
runtime TypeScript objects; `PraxisTypescriptGenerator` mirrors backend constants, enums, and
their display labels into `frontend/src/models/generated/constants.ts`.

**Frontend conventions** — an API client that only does HTTP, `queryConfig.ts` with a
`QueryKeys` enum and shared defaults, and one query and one mutation hook showing the
pattern including cache invalidation.

**Tests** — xUnit + Shouldly against an in-memory `PraxisDbContext`, mirroring slice paths.

**AI configuration** — five instruction files, four skills, and `AGENTS.md` as the registry.

## Local workflow

```bash
cd backend
dotnet build
dotnet test
dotnet run --project Praxis.TargetArchitecture   # http://localhost:5153

cd ../frontend
npm install
npm start                    # http://localhost:5173 (or the next free port)
npm run typecheck
npm run lint
```

## Model generation workflow

With the backend running, a human regenerates the frontend models:

```bash
cd frontend
npm run autogenerate-models
```

Output lands in `frontend/src/models/generated/`.
These are derived artifacts: never hand-edit them, and AI agents must never run the
generation themselves.

## Further reading

- [`docs/coding-conventions.md`](docs/coding-conventions.md) — the rules
- [`docs/readme-backend.md`](docs/readme-backend.md) — adding a slice
- [`docs/readme-frontend.md`](docs/readme-frontend.md) — consuming the contract
- [`AGENTS.md`](AGENTS.md) — AI configuration map
