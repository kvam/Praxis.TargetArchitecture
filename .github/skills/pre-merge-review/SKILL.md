---
name: pre-merge-review
description: Review a branch against the default branch before it is merged. Use when asked to review a PR or check whether a branch is ready to merge.
---

# Pre-merge review

Reviews a whole branch, not just the working tree. Read-only.

## Steps

1. Scope the branch:

   ```bash
   git --no-pager log --oneline master..HEAD
   git --no-pager diff master...HEAD --stat
   git --no-pager diff master...HEAD
   ```

2. Read each changed file in full. Review against `docs/coding-conventions.md`.

3. Report findings in three buckets, highest confidence first:
   - **Blocking** — bugs, broken contracts, security issues, standard violations
   - **Should fix** — correctness risks, missing tests, incomplete registration
   - **Consider** — genuine improvements, clearly marked as optional

4. For each finding give the file, the line, why it matters, and the concrete fix. Skip
   style nits the linters already enforce.

5. Confirm the branch builds:

   ```bash
   cd backend && dotnet build && dotnet test
   cd ../frontend && npm run typecheck && npm run lint
   ```

6. Specifically confirm:
   - every new slice has a test
   - every new service is registered
   - contract changes are matched by regenerated models, or called out as needing
     regeneration
   - no secrets, connection strings, or tokens were added
   - no generated file was hand-edited

## Rules

- Do not modify files, commit, rebase, or push during a review.
- Say clearly whether you consider the branch mergeable.
