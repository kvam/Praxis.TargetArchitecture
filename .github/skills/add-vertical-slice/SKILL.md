---
name: add-vertical-slice
description: Add a new backend vertical slice and its frontend consumption following the repository standard. Use when asked to add a new API endpoint or feature.
---

# Add a vertical slice

The canonical reference is `Features/Api/ArchitecturePrinciples/{Get,Create}`. Read those
files before writing new ones.

## Backend

1. **Decide the slice path**: `Features/Api/<Feature>/<Action>/`. `<Action>` is the verb
   (`Get`, `Create`, `Update`, `Delete`), one action per folder. Never add the endpoint
   as a second action on an existing controller.

2. **Entity**, only if new storage is needed. Put it in `Entities/`, implement
   `IDateTrackedEntity`, add audit properties in an `#region Audit` block, add a `DbSet`
   to `Data/PraxisDbContext.cs`.

3. **Service** in the slice folder, with its DTO in the same file:
   - concrete class, primary constructor, no interface
   - exactly one public method — a second entry point means a second slice
   - no `Async` suffix
   - read services return DTOs; write services return the new id
   - query with `AsNoTracking()` and project straight into the DTO
   - throw `NotFoundException` / `ConflictException` / `ValidationException` rather than
     returning error results
   - DTO properties are all `required`; the identifier is `<Entity>Id`
   - never import a type from another slice — declare your own

4. **Controller** in the slice folder:
   - inherits `PraxisControllerBase`
   - single public action, route on the action
   - no logic beyond validation, service call, return
   - for a write action: validate, call the write service, then call the matching Get
     service to build the response

5. **Register** the services in `AppInfrastructure/PraxisIocConfiguration.cs`.

6. **Test** at the mirrored path under `backend/Praxis.TargetArchitecture.Tests/`, using a
   uniquely named in-memory `PraxisDbContext`. Cover the happy path plus every exception
   the service can throw.

7. **Verify**: `cd backend && dotnet build && dotnet test`.

## Frontend

8. Add the call to the feature's client in `src/api/clients/`, typed with the generated
   DTOs from `@/models/generated`.

9. Add a `QueryKeys` entry, then a query hook in `src/api/queries/<feature>/` or a
   mutation hook in `src/api/mutations/<feature>/`. Mutations invalidate the keys they
   affect.

10. **Verify**: `cd frontend && npm run typecheck && npm run lint`.

## Stop and report

If the new slice introduces or changes a DTO or enum, the generated models are stale.
Tell the user to run `npm run autogenerate-models` with the backend running. Do not run
it yourself and do not hand-write the missing types.
