# Coding conventions

These conventions are the standard. They are deliberately strict: consistency is worth
more than local cleverness.

## The goal

**Anyone with a year of experience should be able to work in this system, frontend or
backend.** Every rule below exists to protect that. When a rule and the goal appear to
conflict, the goal wins — and the rule needs revisiting.

## Universal

- No `else`. Use guard clauses and early returns.
- **Avoid `try/catch`.** Use it only when the failure can actually be handled here:
  adding context to an opaque third-party error, releasing a resource, or keeping a
  background job alive. Otherwise throw a domain exception and let the middleware
  translate it. A `catch` that logs and rethrows, swallows silently, or returns a
  default value hides the failure from the one place built to report it.
- **Persist unhandled exceptions for debugging.** Store enough structured exception data in
  the database that a production failure can be investigated after the request is gone.
  Debugging gets much simpler when the error survives longer than the log window.
- **Persist HTTP requests for debugging.** Store enough request data in the database to see
  what the user actually sent: route, method, payload shape, identity, and timing where
  useful. A request log makes production behaviour inspectable after the fact.
- **Return rich exception details in development.** In development environments the API
  should return enough error detail to see what failed quickly: message, stack trace, and
  useful inner-exception data. Optimize for fast diagnosis locally, not pretty failures.
- **Keep a debuggable story of the failure.** Debugging is simpler when the request log and
  exception log can be read together into one narrative of what the user did, what the
  system received, and where it failed.
- Never abbreviate names. `architecturePrincipleId`, not `apId`.
- **Prefix our own platform types with the solution name.** `PraxisIocConfiguration`, not
  `IocConfiguration`; `PraxisDbContext`, not `AppDbContext`. The prefix says *this one is
  ours* — the reader knows immediately it is not a framework type they should go looking
  up, and our names cannot collide with the ones the framework ships. Slice types are
  named after the feature (`GetArchitecturePrinciplesService`) and need no prefix.
- Prefer boring, explicit code over abstractions that save a few lines. Obvious beats
  short: code is written once and read for years.
- **A file is at most 200 lines.** That is roughly what one person can hold in their head
  at once. When a file passes it, it has usually become two ideas sharing a name — split
  it along that seam, not at the line count. This repository obeys it: the seed list is
  one file per layer under `Data/Seed/`, and the frontend register is split into
  `features/architecturePrinciples/`. Generated files and lock files are exempt.
- **Entity relationships are bi-directional.** A child entity keeps the foreign key and
  navigation pair (`FooId` and `Foo`), and the parent keeps `List<Bar>` on the other side.
  Do not add one-way navigation properties when the relationship is modeled in code.
- Match the surrounding code. Consistency beats personal preference.
- Comments explain *why*, never *what*. Delete comments that restate the code.

## Configuration and secrets

Configuration exists for two things: values that genuinely differ per environment, and
secrets. Everything else is code.

- **Secrets never live in the repository.** Locally, use user secrets:

  ```bash
  cd backend/Praxis.TargetArchitecture
  dotnet user-secrets init
  dotnet user-secrets set "ConnectionStrings:Praxis" "<connection string>"
  ```

  They are stored outside the working tree, so they cannot be committed by accident. In
  deployed environments the values come from the platform's secret store.
- **Static, non-secret values belong in code.** A value that is identical everywhere is a
  `const`, not a settings key. Putting it in configuration hides it from the compiler and
  from anyone reading the code that uses it.
- **Switches are computed in `AppInfrastructure/PraxisEnvironments.cs`.** Feature toggles
  and environment checks are C# properties, so they can be grepped, navigated to,
  type-checked, and tested:

  ```csharp
  public static bool ExposeOpenApiDocument => !IsProduction;
  ```

  Call sites read as a question with an answer, and the answer lives one click away.
- **Keep the reasoning next to the code.** Logic scattered across per-environment config
  files turns "why did it behave differently in test?" into archaeology. One file holding
  the conditions beats five files holding values.
- **`appsettings.json` stays small** — logging, connection strings, and real
  per-environment values. A settings file with hundreds of keys has become an untyped,
  untested second program.
- **Required configuration is read at startup and fails loudly** if it is missing, rather
  than surfacing as a confusing null in the middle of a request.

## Collaboration

**Every developer works in both the backend and the frontend.**

- It is fine — expected, even — to be an expert in one of them. Depth is valuable and
  nobody is asked to be equally strong everywhere. What is not optional is learning
  enough of the other half to read, change, and review it.
- **A feature is delivered end to end.** Whoever changes a DTO changes the frontend that
  consumes it. Splitting work at the contract turns one task into two queues and a
  handoff, and the handoff is where the misunderstanding lives.
- **There is no frontend team and backend team.** Teams organized around layers optimize
  for their own layer, and the seams between them are where systems rot.
- **Review outside your specialty.** If you cannot follow code in your weaker half, that
  is evidence about the code, not about you — simplify it.
- **Experts teach rather than gatekeep.** A part of the system only one person can touch
  is a risk, not a strength. Explain it, pair on it, write it down.
- **Learning the other half is part of the job**, not overhead to do on your own time.
  It repays itself the first time a change does not have to wait for someone else.

This is also why the rest of the standard looks the way it does. Vertical slices,
generated models, a small dependency list, and boring code all exist to make crossing the
boundary cheap. Write the backend so a frontend specialist can follow it, and the frontend
so a backend specialist can — that audience *is* the one-year bar in practice.

### Small teams

**Smaller teams outperform larger ones.** A small team shares context without meetings,
trusts by default, and decides in a conversation rather than a process.

- **Trust is the multiplier.** It is what lets work be handed over without hedging,
  disagreement stay cheap, and nobody write defensively to protect themselves.
- **Every extra person adds communication paths.** Connections grow faster than headcount:
  what was a shared understanding at four becomes a document, a meeting, and a
  misunderstanding at ten.
- **Process is what replaces trust.** Most new gates and sign-offs are substitutes for a
  conversation that stopped scaling. Prefer restoring the trust, or shrinking the team,
  over adding the process.
- **Autonomy over approval.** Ship the change and let review catch what it catches.
- **Shared ownership.** Anyone may change any part of the system. Personal territory
  turns a person into a queue.
- **When a team must grow, split it around whole vertical features**, never around
  layers. Splitting by layer manufactures exactly the handoffs this architecture exists
  to avoid.

### Limit work in progress

**Keep work in progress to an absolute minimum.** Everything in flight is unfinished
inventory: it has cost time and delivered nothing.

- **Started is not delivered.** Three tasks at ninety percent are worth nothing.
- **Finish before you start.** When new work arrives, the first question is what gets
  completed or dropped to make room.
- **Context switching is the most expensive thing you do.** Two tasks at once take longer
  than the same two in sequence.
- **Small changes merge fast.** Size a change to be reviewed in one sitting.
- **Long-lived branches are work in progress.** They accumulate conflicts and hide risk
  until the worst possible moment.
- **Reviewing beats starting.** Work waiting for review is already paid for and not yet
  earning; clearing someone else's queue beats opening a new task of your own.
- **Blocked work is made visible, not stacked.** Starting a third task to stay busy turns
  one delay into three.

### Doing nothing beats doing the wrong thing
Not acting is always on the table. A wrong change costs the work, the revert, and
everything built on top of it in between — waiting costs only time.

- **Understand before you act.** Being busy is not progress. If the problem is not clear
  yet, understanding it *is* the work.
- **The best code is the code you never wrote.** Every line is reviewed, tested,
  maintained, and eventually migrated.
- **Wait for the second example.** Build for what exists. An abstraction invented for one
  case is a guess, usually wrong in an expensive way.
- **Prefer the reversible decision.** Easy to undo: decide and move on. Hard to undo:
  slow down — those are the only choices that earn the delay.
- **Deleting is a contribution.** Subtraction makes every future change cheaper.

### Pairing

**Pair programming is a good thing.** Use it deliberately rather than constantly.

- **Pair on anything hard** — a gnarly bug, an unfamiliar area, a decision that is hard
  to undo. Two people are faster than one person stuck plus a review afterwards.
- **Pairing is review that happens immediately.** A problem caught while the code is
  being written costs a sentence; the same problem in review costs a round trip and a
  rebuilt mental model.
- **Pair across the boundary.** The fastest way to learn the other half of the stack is
  to build something in it beside someone who knows it — documentation cannot answer the
  follow-up question.
- **Pairing spreads ownership.** Knowledge held by one person is a bottleneck disguised
  as expertise.
- **Pairing is not supervision.** Both are peers; the less experienced one should drive
  more, because the person typing is the person learning.
- **Routine work is fine alone.** Pairing all day costs more attention than it returns.

### Raise questions immediately

**Do not wait for the next meeting.** Ask when the question appears.

- A blocker raised on Monday and answered in minutes is a different thing from the same
  blocker raised at Thursday's stand-up.
- **Meetings are a sync point, not the queue for questions.** If something needs an
  answer to move, it needed it hours ago.
- **The person who can unblock you would rather be asked.** Nobody prefers finding out a
  day later that someone sat stuck out of politeness.
- **Assume you are allowed to interrupt.** Fifteen minutes of attention beats a day of
  guessing.
- **Answer in the open**, not in a private message, so the answer is searchable and the
  next person does not have to ask it again.

## Definition of done

**Done means running in production with users.** Not written, not merged, not deployed to
a test environment.

- **Merged is not done.** Code waiting on the main branch for a release is inventory: paid
  for, aging, teaching nobody anything.
- **Ship small and often.** Frequent small releases make each one boring; batching
  concentrates risk and hides which change caused the problem.
- **Only users can tell you it works.** Passing tests prove the code does what you
  expected — not that what you expected was worth building.
- **You own it after it ships.** Deployment starts the feedback loop; watching it run is
  part of the same piece of work.
- **Releasing is routine, not an event.** If it needs a plan, a window, and an audience,
  it happens too rarely.

Before any of that, the mechanical bar still applies: `dotnet build`, `dotnet test`,
`npm run lint`, and `npm run typecheck` all pass.

## Dependencies

The threshold for adding a package is high, on both sides of the stack.

- A dependency is not free. You adopt its upgrade cadence, its breaking changes, its
  transitive tree, its security advisories, and its opinion about how code should look.
- **Prefer the platform.** .NET and the browser already do most of what is needed. Reach
  for a library when the problem is genuinely hard, not merely tedious.
- If the need is a few dozen lines you fully understand, write them. Understandable code
  you own beats a black box you do not.
- **No framework inside the framework.** Do not build a house abstraction over ASP.NET,
  EF Core, or React. It forces every newcomer to learn a private dialect first.
- Adding a package is a decision to discuss, not a detail to slip into a change.

### Avoid popular libraries that do very little

Popularity is not an argument. The worst dependencies are the well-liked ones that
replace obvious code with configuration — you still have to understand the underlying
behaviour, and now the library's model of it too. A wrapper you must still understand is
not an abstraction; it is a second thing to learn.

The test: *if this package vanished, would the replacement be a few readable lines?* If
so, write those lines.

| Instead of | Use | Why |
|---|---|---|
| AutoMapper | An explicit `.Select(...)` projection | Assignments are verifiable and fail at compile time. A mapper turns a rename into a silent null. |
| MediatR | Call the slice service directly | The slice is already the handler. Go-to-definition should reach it. |
| FluentValidation | A static validator with guard clauses | An `if` that throws reads top to bottom and debugs like normal code. |
| A repository over `DbContext` | `DbContext` | It is already a repository and a unit of work. |
| Newtonsoft.Json | `System.Text.Json` | In the box, faster, already patched. |
| axios | `fetch` | Built into the browser. |
| moment / date-fns for one call | `Intl.DateTimeFormat` | The formatting is standard. |
| uuid | `crypto.randomUUID()` | One line, no package. |
| query-string | `URLSearchParams` | Same. |
| classnames | Template literals | Same. |
| lodash for `groupBy` | A local helper | Do not import a toolbelt for one function. |

This is a list of examples, not an exhaustive ban list. Apply the reasoning, not the
table. A library that solves something genuinely hard — a database provider, a UI
component set, a test runner — is a good trade; one that saves ten lines is not.

## Backend

### Slice layout

Every endpoint lives in `Features/Api/<Feature>/<Action>/` and owns its controller,
service, and DTOs:

```text
Features/Api/ArchitecturePrinciples/
  Get/
    GetArchitecturePrinciplesController.cs
    GetArchitecturePrinciplesService.cs
  Create/
    CreateArchitecturePrincipleController.cs
    CreateArchitecturePrincipleService.cs
```

### Slice isolation

These rules are the point of the architecture. Breaking them quietly rebuilds a layered
application inside a folder structure that claims not to be one.

- **A new endpoint is a new folder.** Never add a second action to an existing
  controller.
- **One public method per slice service.** A second entry point means a second slice.
- **A slice never references another slice's types.** If two slices need the same DTO,
  each declares its own. Duplicating a small shape is cheaper than a dependency that
  outlives its reason.
- **No shared `Dtos/`, `Models/`, `Services/`, or `Helpers/` folder.** Contract types
  live in the slice that returns them. Genuinely cross-cutting platform concerns — and
  only those — go in `AppInfrastructure/` under a name that says what they do.
- **No `Manager`, `Helper`, or `Utility` classes.** A class named for what it is rather
  than what it does becomes a dumping ground.
- **Deleting the folder deletes the feature.** The only residue allowed is the service
  registration in `PraxisIocConfiguration`. If removal is hard, the slice was not
  self-contained.
- **A feature change touches one folder.** A diff spread across slices means shared
  state or shared logic has crept in — treat it as a design defect, not a merge problem.

The single exception is entities: they live in `Entities/` because the schema really is
one shared thing. That exception does not generalize.

- **Backend owns DTOs that cross the contract.** If the frontend needs a shape the backend
  already exposes, change the backend DTO and regenerate the frontend model. Do not paper
  over that gap with a hand-written frontend interface.

### Controllers

- Inherit `PraxisControllerBase`; never `ControllerBase` directly.
- One controller per slice, one public action per controller.
- Controllers contain no business logic. They validate input, call the service, return.
- Routes are declared on the action (`[HttpGet("api/architecture-principles")]`).

### Services

- Concrete classes. Do not introduce an interface for a single implementation.
- Registered in `AppInfrastructure/PraxisIocConfiguration.cs`.
- Use primary constructors for dependencies.
- No `Async` suffix on method names.
- Read services return DTOs. Write services return the new id, or nothing.

### The one sanctioned cross-slice call

A write controller may call the matching **Get** service to build its response body.
This keeps the read shape defined in exactly one place. No other cross-slice calls are
allowed — if two slices need the same logic, move it into a shared service.

```csharp
[HttpPost("api/architecture-principles")]
public async Task<ArchitecturePrincipleDto> CreateArchitecturePrinciple(CreateArchitecturePrincipleDto dto)
{
    CreateArchitecturePrincipleDtoValidator.Validate(dto);

    var architecturePrincipleId = await createService.Create(dto);

    return await getService.GetById(architecturePrincipleId);
}
```

### DTOs

- Live in the slice that owns them, in the service file.
- Every property is `required`. Optionality is expressed by the type (`string?`), never
  by omission. `ArchitecturePrincipleDto.Example` is the live demonstration: `required
  string?` generates `example: null | string`, so the frontend reads optionality off the
  type instead of checking whether a field was sent.
- Identifier properties are named `<Entity>Id`. Never bare `Id` on a DTO.
- DTOs are flat. Do not expose entities over the wire.

### Errors

Throw from `AppInfrastructure/Exceptions`; `PraxisExceptionHandlingMiddleware` maps them:

| Exception | Status |
|---|---|
| `ValidationException` | 400 |
| `ForbiddenException` | 403 |
| `NotFoundException` | 404 |
| `ConflictException` | 409 |
| anything else | 500 |

### Entities

- Live in `Entities/`, never in a slice.
- Implement `IDateTrackedEntity` to get `CreatedUtc`/`UpdatedUtc` stamped by
  `PraxisDbContext.SaveChanges`. Implement `IChangeTrackable` to also record the user.
- Audit properties go in an `#region Audit` block at the bottom of the class.

### Entity Framework

- **The `DbContext` is the repository.** EF already implements repository and
  unit-of-work. Do not wrap it.
- **Queries live in the slice that needs them.** There is no shared query layer and no
  shared `IQueryable` extension bag.
- **Project into the DTO inside the query** (`.Select(...)`), so the database returns
  only the columns that get sent. Do not materialize entities and map afterwards.
- **`AsNoTracking()` on every read path.**
- **Loading is explicit.** Lazy-loading proxies stay off; use `Include` or a projection.
- **No queries inside loops.** Fetch what you need in one round trip — an N+1 only hurts
  once the table is big, which is exactly when you cannot afford it.
- **Everything must translate to SQL.** Filter, sort, and page in the database. Calling
  `ToList()` and then filtering in C# is a table scan wearing a disguise.
- **One `SaveChanges` per unit of work**, so a request applies atomically.
- **The `DbContext` is scoped.** Never capture it in a singleton, a static, or a
  parallel task; it is not thread-safe.
- **Never set audit fields by hand.** `SaveChanges` owns them.
- **Never renumber a persisted enum.** Values are stored as integers; reordering rewrites
  the meaning of existing rows. Append new members instead.
- **Migrations are append-only.** Once applied anywhere, a migration is never edited —
  fix forward with a new one. Read generated migrations before committing them; the tool
  guesses at renames and is confidently wrong.

### Style

- File-scoped namespaces.
- Always brace control flow, even single statements.
- `List<T>` over `IEnumerable<T>` on public surfaces — callers should not wonder whether
  enumeration is deferred.
- Warnings are errors in the app project.

## Frontend

- **Never hand-write a model that the backend owns.** Import from `@/models/generated`.- Regeneration (`npm run autogenerate-models`) is run by a human with the backend
  running. AI agents must not run it and must not edit generated files.
- API clients live in `src/api/clients/` and only translate HTTP into typed promises.
- Queries live in `src/api/queries/<feature>/`, mutations in
  `src/api/mutations/<feature>/`.
- Query keys come from the `QueryKeys` enum in `src/api/queries/queryConfig.ts`. Never
  inline a string key.
- Every query spreads `defaultQueryConfig<T>()`.
- Every mutation invalidates the query keys it affects in `onSuccess`.
- Mutation hooks return meaningfully named values (`createArchitecturePrinciple`,
  `isCreating`, `createError`), not raw `mutateAsync`/`isPending`/`error`.
- Components never wrap a mutation in `try`/`catch`. Pass `{ onSuccess }` for the happy
  path and render the hook's error value — the mutation already tracks the failure.

## Testing

- Tests mirror the slice path under `backend/Praxis.TargetArchitecture.Tests/`.
- Use xUnit with Shouldly assertions.
- Test services against an in-memory `PraxisDbContext`. Do not mock the DbContext.
- Cover the happy path and each thrown domain exception.
