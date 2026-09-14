---
name: audit-api-frontend-sync
description: Verify that frontend API usage still matches the backend contract and generated models. Use after changing a DTO, controller route, enum, or shared constant.
---

# Audit API/frontend sync

The contract flows one way: backend DTO -> OpenAPI -> `frontend/src/models/generated`.
This skill checks nothing has drifted.

## Steps

1. List the backend contract surface that changed:

   ```bash
   git --no-pager diff --name-only -- 'backend/**/*.cs'
   ```

   For each changed slice, note its route, request DTO, and response DTO.

2. Compare against the generated models:

   ```bash
   git --no-pager diff --name-only -- 'frontend/src/models/generated'
   ```

   If a backend DTO or enum changed and the generated folder did not, the frontend is
   stale. Report it — do not regenerate yourself.

3. Grep the frontend for usages of each affected type and route:

   ```bash
   rg "ArchitecturePrincipleDto|api/architecture-principles" frontend/src
   ```

   Check that:
   - routes in `src/api/clients/` match the `[HttpGet]`/`[HttpPost]` route on the
     controller, including the `api/` prefix
   - the HTTP verb matches
   - request bodies contain every `required` property of the backend DTO
   - no frontend interface duplicates a generated type

4. Check enums specifically. A backend enum only reaches the frontend as a runtime value
   because `PraxisEnumSchemaTransformer` adds `x-enum-varnames`. If a new enum appears in a
   DTO, confirm the generated file contains a `const` object for it, not a bare numeric
   type alias.

5. Check shared constants: anything in `frontend/src/models/generated/constants.ts` must still
   match its backend source in the slice that declares it.

6. Report findings as a list of concrete mismatches, each with the backend location, the
   frontend location, and the fix.

## Rules

- Never run `npm run autogenerate-models`. Never start the backend to generate.
- Never edit generated files to make them agree. Fix the backend or the calling code.
