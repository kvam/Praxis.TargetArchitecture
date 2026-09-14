---
description: Project-wide conventions, directory map, and cross-cutting rules.
name: General
applyTo: "**"
---

# General

This repository is the standard itself: the code is the specification. When changing it,
keep the code, `docs/`, and the AI configuration in agreement.

## The goal

**Anyone with a year of experience should be able to work in this system, frontend or
backend.** Every convention below serves that goal.

**Keep every code file at or under 200 lines.** Past that a file has usually become two
ideas sharing a name; split it by responsibility. Generated files and lock files are
exempt.

## Collaboration

Every developer works in both halves. Being an expert in one is fine; refusing to learn
the other is not. Deliver features end to end — whoever changes a DTO changes the
frontend that consumes it. Write each half so a specialist from the other half can follow
it.

Small, high-trust teams outperform large ones; prefer restoring trust over adding
process. Keep work in progress to an absolute minimum: finish before you start, size
changes to be reviewed in one sitting, and prefer reviewing over opening new work.

Doing nothing beats doing the wrong thing. If the problem is not clear yet, understanding
it is the work. Build for what exists, prefer reversible decisions, and treat deleting as
a contribution.

## Repository layout

```text
backend/
  Praxis.TargetArchitecture/
    AppInfrastructure/     shared platform (controllers, exceptions, middleware, IoC, OpenAPI)
    Data/                  PraxisDbContext
    Entities/
    Features/Api/          vertical slices
  Praxis.TargetArchitecture.Tests/
frontend/
  src/api/{clients,queries,mutations}/
  src/models/generated/    derived, never hand-edited
docs/
.github/                   instructions, skills, CI
```

## Conventions

- The core architecture is an opinionated vertical-slice backend with frontend models
  generated from backend models.
- Backend slices live under `Features/Api/<Feature>/<Action>/`.
- The backend is the source of truth for DTOs, enums, and shared constants.
- Generated frontend files must never be hand-edited.
- No `else`, no abbreviations in names.
- Avoid `try/catch` — use it only when the failure is genuinely handled where it is
  caught, never to log and rethrow, swallow, or return a default.
- Comments explain *why*, never *what*.

## Dependencies

The bar for adding a package is high, frontend and backend alike.

- Prefer the platform. .NET and the browser already do most of it.
- If the need is a few dozen lines you understand, write them.
- **Avoid popular libraries that do very little.** No AutoMapper (project with `.Select`),
  no MediatR (call the service), no FluentValidation (guard clauses), no repository over
  `DbContext`, no Newtonsoft. On the frontend: `fetch` not axios, `Intl` not moment,
  `crypto.randomUUID()` not uuid, `URLSearchParams` not query-string, and never a toolbelt
  like lodash for one function.
- Never build a house framework on top of ASP.NET, EF Core, or React.
- Never add a dependency without raising it with the user first.

## Authoritative documents

- `docs/coding-conventions.md` — the rules
- `docs/readme-backend.md` / `docs/readme-frontend.md` — walkthroughs
- `AGENTS.md` — map of instructions and skills

## Before finishing a task

1. `cd backend && dotnet build && dotnet test`
2. `cd frontend && npm run lint && npm run typecheck`
3. Update `docs/`, `README.md`, and `AGENTS.md` when architecture, workflow, or AI
   configuration changes.
4. If backend contracts changed, remind the user to run the frontend generation command.
