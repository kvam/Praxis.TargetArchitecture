# AI Configuration Map

Registry of AI-related configuration in this repository. Keep this table current — when
a skill or instruction file is added, removed, or renamed, update it in the same change.

## Instructions

Applied automatically by matching path.

| File | Applies to | Purpose |
|---|---|---|
| `.github/instructions/general.instructions.md` | `**` | Repository layout and architecture rules |
| `.github/instructions/git-governance.instructions.md` | `**` | Git restrictions |
| `.github/instructions/security.instructions.md` | `**` | Secrets and safe output |
| `.github/instructions/dotnet.instructions.md` | `backend/**/*.{cs,csproj}` | Backend slice conventions |
| `.github/instructions/frontend.instructions.md` | `frontend/**/*.{ts,tsx}` | Generated-model frontend conventions |

## Skills

Invoked on demand.

| Skill | Use it when |
|---|---|
| `.github/skills/add-vertical-slice/SKILL.md` | Adding a new API endpoint or feature |
| `.github/skills/audit-own-changes/SKILL.md` | Finishing an implementation, before reporting done |
| `.github/skills/audit-api-frontend-sync/SKILL.md` | A DTO, route, enum, or constant changed |
| `.github/skills/pre-merge-review/SKILL.md` | Reviewing a branch before merge |

## Documentation the AI should read

| File | Purpose |
|---|---|
| `docs/coding-conventions.md` | The authoritative rules |
| `docs/readme-backend.md` | Backend slice walkthrough |
| `docs/readme-frontend.md` | Frontend generated-model walkthrough |

## Repository conventions

- Entity relationships are modeled bi-directionally: the dependent entity keeps both
  `FooId` and `Foo`, and the principal side keeps `List<Bar>`.
- Persist unhandled exceptions with enough structured detail to debug production failures
  after the request is gone.
- Persist HTTP requests with enough structured detail to see what the user actually sent.
- Development APIs return rich exception details to optimize local debugging speed.
- Keep request and exception logs connected so production failures can be reconstructed as
  one story.

## Non-negotiables

- Never run `npm run autogenerate-models`, and never start the backend in order to
  regenerate models. That is the user's action.
- Never edit `frontend/src/models/generated/**` or `frontend/src/models/constants.ts`.
- Never commit to or push the default branch.
- Never commit secrets, connection strings, or tokens. Locally they belong in
  `dotnet user-secrets`, never in `appsettings.json`.
