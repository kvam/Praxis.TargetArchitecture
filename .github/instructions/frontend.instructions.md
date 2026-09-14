---
description: Frontend conventions for generated backend contracts.
name: Frontend
applyTo: "frontend/**/*.{ts,tsx}"
---

# Frontend

Full rules: `docs/coding-conventions.md`. Walkthrough: `docs/readme-frontend.md`.

## Generated models are sacred

- Import API types from `@/models/generated`. Never hand-write a type the backend owns.
- Never edit `src/models/generated/**`, which includes the generated `constants.ts`.
- Never run `npm run autogenerate-models` and never start the backend to enable it. The
  user does that. If a type is missing, fix the backend DTO and ask for a regeneration.
- Never recompute a backend-owned domain value in the frontend.

## Layers

- `src/api/clients/` — HTTP only, typed with generated DTOs. No caching, no state.
  Use the `usePraxisTargetArchitectureApiClient` helpers (`get`, `post`, `patch`, `remove`).
- `src/api/queries/<feature>/` — one hook per read. Key from the `QueryKeys` enum in
  `src/api/queries/queryConfig.ts`; never inline a string key. Always spread
  `defaultQueryConfig<T>()`.
- `src/api/mutations/<feature>/` — one hook per write. Invalidate the affected query keys
  in `onSuccess`. Return named values (`createThing`, `isCreating`, `createError`), not
  raw `mutateAsync`/`isPending`/`error`. Call sites use `mutate(input, { onSuccess })` and
  render the hook's error — never `try`/`catch` around a mutation.

## Style

- No `else`; use early returns.
- Errors surface the backend `message` from `ApiError`; do not invent parallel copy.
- The browser is the standard library: `fetch`, `Intl`, `crypto.randomUUID()`,
  `URLSearchParams`, template literals. No axios, moment, uuid, query-string, classnames,
  and no lodash for a single function.

## Before finishing

`cd frontend && npm run lint && npm run typecheck`.
