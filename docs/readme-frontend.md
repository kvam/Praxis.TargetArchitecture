# Frontend

React + TypeScript. The frontend owns presentation; it never owns a contract.

## Layout

```text
frontend/src/
  api/
    clients/      HTTP translation only
    queries/      read hooks + queryConfig.ts
    mutations/    write hooks
  features/       components for one feature, kept next to each other
  models/
    generated/      never edited
      types.gen.ts  produced by openapi-ts
      constants.ts  produced by the backend on startup
  styles/         one stylesheet per area, imported by styles.css
```

`App.tsx` composes; it holds the filter state and nothing else. Everything that renders
the register lives in `features/architecturePrinciples/`, one component per file, so no
file passes the 200-line bar.

Optionality comes from the generated types, not from a convention. `example` arrives as
`null | string`, so `PrincipleCard` renders the "See an example" button only where the
backend actually has one, and `PrincipleExampleDialog` is the one place that knows how to
show it. Nothing on the frontend guesses which principles are documented.

## Commands

```bash
npm install
npm run typecheck
npm run lint
npm run autogenerate-models   # humans only, backend must be running
```

## The generation contract

`openapi-ts.config.ts` reads `http://localhost:5153/openapi/v1.json` and writes
`src/models/generated/`. Enums are emitted as `javascript` with `preserve` casing so
backend enum names survive into runtime objects.

Rules:

- Regeneration is a human action. AI agents must never run it, never start the backend
  to enable it, and never edit anything under `src/models/generated/`.
- If a type you need is missing, the fix is in the backend DTO, followed by a
  regeneration request — not a hand-written interface.

## Layers

**Client** — typed HTTP only, no caching or state:

```ts
export const useArchitectureApi = () => {
    const client = usePraxisTargetArchitectureApiClient()

    return {
        getArchitecturePrinciples: (): Promise<ArchitecturePrincipleDto[]> =>
            client.get<ArchitecturePrincipleDto[]>("api/architecture-principles"),
    }
}
```

**Query** — one hook per read, keyed from `QueryKeys`, spreading `defaultQueryConfig`:

```ts
export const useGetArchitecturePrinciples = () => {
    const client = useArchitectureApi()

    return useQuery({
        queryKey: [QueryKeys.ArchitecturePrinciples],
        queryFn: () => client.getArchitecturePrinciples(),
        ...defaultQueryConfig<ArchitecturePrincipleDto[]>(),
    })
}
```

**Mutation** — invalidates what it changed, returns named values:

```ts
return { createArchitecturePrinciple: mutate, isCreating: isPending, createError: error?.message }
```

The call site hands over the success path and reads the failure — no `try`/`catch`:

```tsx
createArchitecturePrinciple(form, { onSuccess: () => setForm(initialForm) })
```

## Errors

The API client throws `ApiError` carrying the HTTP status and the backend's own
`{ "message": "..." }` for domain failures. Mutations expose that message as
`createError`, so components surface it rather than inventing their own copy — and
without a `catch` block to keep it in.

## Fusion note

The base client uses `useHttpClient` from the Fusion framework because that is the host
platform in the reference implementations. In a non-Fusion project, replace the body of
`praxisTargetArchitectureApiClient.ts` with `fetch`. Nothing above the client changes —
that is the point of keeping the client layer dumb.
