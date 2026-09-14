# Praxis.TargetArchitecture — Copilot Instructions

Baseline guidance applied to every AI session in this repository.

**Read alongside this file:**
- `/AGENTS.md` — full map of AI configuration, also auto-loaded.
- `docs/coding-conventions.md` — the authoritative rules for this standard.
- `.github/instructions/*.instructions.md` — domain-specific rules:
  - `.github/instructions/general.instructions.md` — always applies (`**`)
  - `.github/instructions/git-governance.instructions.md` — always applies (`**`)
  - `.github/instructions/security.instructions.md` — always applies (`**`)
  - `.github/instructions/dotnet.instructions.md` — when touching `backend/**/*.{cs,csproj}`
  - `.github/instructions/frontend.instructions.md` — when touching `frontend/**/*.{ts,tsx}`

**AI agents — read instruction files before editing.** Before making code changes, read the relevant instruction files and the docs they reference.

## Code simplicity is the top priority

**Anyone with a year of experience should be able to work in this system, frontend and
backend.** That is the high-level goal every other rule serves. Prefer boring, obvious
code. Comments explain *why*, not *what*.

Everyone here works across the stack. Specialize deeply if you like, but deliver features
end to end — a change to a DTO includes the frontend that consumes it.

The bar for adding a dependency is high on both sides of the stack: prefer the platform,
write the small thing yourself, never build a framework inside the framework, and raise
any new package with the user rather than slipping it into a change.

Avoid popular libraries that do very little — AutoMapper, MediatR, FluentValidation, a
repository over `DbContext`, axios, moment, uuid, lodash. See the table in
`docs/coding-conventions.md`.

## Architecture at a glance

| Area | Tech |
|---|---|
| Frontend | TypeScript, React Query, Fusion HTTP client pattern, generated OpenAPI models |
| Backend | .NET 10, ASP.NET Core OpenAPI, vertical slices, EF Core |
| Tests | xUnit + Shouldly against an in-memory `PraxisDbContext` |
| Contract sync | OpenAPI + `@hey-api/openapi-ts` |

## Skills

| Skill | Use it when |
|---|---|
| `add-vertical-slice` | Adding a new API endpoint or feature |
| `audit-own-changes` | Finishing an implementation, before reporting done |
| `audit-api-frontend-sync` | A DTO, route, enum, or constant changed |
| `pre-merge-review` | Reviewing a branch before merge |

## Non-negotiable rules

- Never push to a remote.
- Never modify the default branch directly.
- Never commit secrets or log them.
- Backend owns domain DTOs, enums, and shared constants.
- Frontend consumes generated models instead of maintaining contract copies by hand.
- Do not run model-generation commands automatically; hand them to the user.

## Build commands

```bash
cd backend
dotnet build
dotnet test

cd ../frontend
npm run lint
npm run typecheck
```
