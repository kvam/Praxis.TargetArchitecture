---
name: audit-own-changes
description: Review your own uncommitted changes against the repository standard before presenting them as done. Use after finishing an implementation task and before reporting completion.
---

# Audit own changes

Self-review pass. Run it before telling the user a change is finished.

## Steps

1. Establish the change set:

   ```bash
   git --no-pager status --short
   git --no-pager diff
   git --no-pager diff --staged
   ```

2. Read every changed file in full, not just the diff hunks. Context outside the hunk is
   where most standard violations hide.

3. Check each change against `docs/coding-conventions.md`. Pay particular attention to:
   - slice placement: is new API code under `Features/Api/<Feature>/<Action>/`?
   - slice isolation: a new endpoint added as a second action on an existing controller,
     a slice importing another slice's DTO, a new shared `Dtos`/`Models`/`Helpers`
     folder, or a new `Manager`/`Helper`/`Utility` class — all are violations
   - controller thinness: any logic that belongs in a service?
   - cross-slice calls: the only allowed one is a write controller calling the matching
     Get service
   - DTO rules: all props `required`, ids named `<Entity>Id`
   - EF: `AsNoTracking()` on reads, projection into the DTO inside the query, no query
     inside a loop, no in-memory filtering after `ToList()`, one `SaveChanges`, no
     hand-set audit fields, no renumbered persisted enum
   - no `else`; `try/catch` only where the failure is genuinely handled — never to log
     and rethrow, swallow, or return a default
   - frontend: no hand-written model that duplicates a generated one; no inline query
     keys; mutations invalidate what they change
   - no edits to `frontend/src/models/generated/**`, which includes `constants.ts`

4. Look for incompleteness the diff makes obvious:
   - a new service that was never registered in `PraxisIocConfiguration`
   - a new entity with no `DbSet`
   - a new slice with no test
   - a changed DTO with no note that regeneration is needed

5. Verify:

   ```bash
   cd backend && dotnet build && dotnet test
   cd ../frontend && npm run typecheck && npm run lint
   ```

6. Fix what you find, then re-run step 5. Report remaining issues you chose not to fix
   and why.

## Rules

- Do not run `npm run autogenerate-models` and do not start the backend to regenerate.
  If a contract changed, say so and let the user regenerate.
- Do not commit, amend, or push as part of this audit.
