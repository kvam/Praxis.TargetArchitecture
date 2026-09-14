---
description: .NET and backend slice conventions.
name: Dotnet
applyTo: "backend/**/*.{cs,csproj}"
---

# Backend

Full rules: `docs/coding-conventions.md`. Walkthrough: `docs/readme-backend.md`.
Reference implementation: `Features/Api/ArchitecturePrinciples/{Get,Create}`.

## Structure

- Vertical slices under `Features/Api/<Feature>/<Action>/`, one action per folder,
  containing the controller, the service, and the DTOs it owns.
- Shared platform code goes in `AppInfrastructure/`. Entities go in `Entities/`.
- Types we own that sit alongside framework types are prefixed with the solution name:
  `PraxisControllerBase`, `PraxisDbContext`, `PraxisIocConfiguration`, `PraxisEnvironments`. The
  prefix marks them as ours rather than something ASP.NET or EF provides. Slice types are
  named after the action instead and take no prefix.
- If something is used by exactly one slice, it belongs in that slice.

## Slice isolation (strict)

- A new endpoint is a new folder — never a second action on an existing controller.
- One public method per slice service. A second entry point means a second slice.
- A slice never references another slice's types. Duplicate the DTO instead.
- No shared `Dtos/`, `Models/`, `Services/`, or `Helpers/` folder. No `Manager`,
  `Helper`, or `Utility` classes.
- Deleting the slice folder must delete the feature, leaving only its IoC registration.
- A feature change should touch one folder. If your diff spans slices, stop and rethink.
- Entities in `Entities/` are the single sanctioned shared type. That exception does not
  generalize.

## Controllers

- Inherit `PraxisControllerBase`, never `ControllerBase`.
- One public action per controller; route declared on the action.
- No business logic: validate, call the service, return.
- A write controller may call the matching Get service to build its response. This is
  the only permitted cross-slice call.

## Services

- Concrete classes with primary constructors; no interface for a single implementation.
- Register in `AppInfrastructure/PraxisIocConfiguration.cs`.
- No `Async` suffix. Read services return DTOs; write services return the new id.

## DTOs

- Every property `required`; optionality expressed by nullable types.
- Identifiers named `<Entity>Id`, never bare `Id`.
- Never expose entities over the wire.

## Errors

Throw `ValidationException` (400), `ForbiddenException` (403), `NotFoundException` (404),
or `ConflictException` (409) from `AppInfrastructure/Exceptions`.
`PraxisExceptionHandlingMiddleware` translates them.

Avoid `try/catch`. Catch only when the failure is genuinely handled here — adding context
to an opaque third-party error, releasing a resource, keeping a background job alive.
Never catch to log and rethrow, to swallow, or to return a default.

## Configuration

- Secrets are set with `dotnet user-secrets` locally and come from the platform's secret
  store when deployed. Never in `appsettings.json`, never in the repository.
- Non-secret values that are the same everywhere are constants in code, not settings keys.
- Feature switches are computed properties on `AppInfrastructure/PraxisEnvironments.cs`, so
  the condition is greppable and type-checked. Do not add a config key to turn something
  on or off.
- Keep `appsettings.json` small: logging, connection strings, genuine per-environment
  values.

## Style

- File-scoped namespaces, always braces, no `else` — use guard clauses.
- `List<T>` over `IEnumerable<T>` on public surfaces.
- Warnings are errors. Never suppress a warning to make the build pass.
- Map entities to DTOs by hand inside the query. No AutoMapper, no MediatR, no
  FluentValidation, no repository wrapper over `DbContext`, no Newtonsoft.

## Entities and persistence

- Implement `IDateTrackedEntity` for `CreatedUtc`/`UpdatedUtc`, `IChangeTrackable` to
  also record the user. `PraxisDbContext.SaveChanges` stamps them; do not set them by hand.
- Audit properties live in an `#region Audit` block.

## Entity Framework

- The `DbContext` is the repository and the unit of work. Never wrap it.
- Queries live in the slice that needs them; there is no shared query layer.
- Project into the DTO with `.Select(...)` inside the query; never materialize entities
  and map afterwards.
- `AsNoTracking()` on every read path.
- Loading is explicit: no lazy-loading proxies, use `Include` or a projection.
- No queries inside loops. No filtering or sorting in memory after `ToList()`.
- One `SaveChanges` per unit of work.
- The `DbContext` is scoped and not thread-safe — never capture it in a singleton,
  static, or parallel task.
- Never renumber a persisted enum; append new members.
- Migrations are append-only and reviewed by hand before commit.

## Tests

Mirror the slice path under `backend/Praxis.TargetArchitecture.Tests/`. xUnit + Shouldly,
a uniquely named in-memory `PraxisDbContext` per test, covering the happy path and each
thrown exception.

## Before finishing

`cd backend && dotnet build && dotnet test`. If a DTO or enum changed, tell the user the
frontend models need regenerating — do not regenerate them yourself.
