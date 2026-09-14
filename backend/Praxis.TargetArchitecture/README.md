# Praxis.TargetArchitecture Backend

The backend standard is an opinionated vertical-slice ASP.NET Core API.

## Architecture at a glance

- API code lives in `Features/Api`.
- Each endpoint owns its own `Feature/Action` folder.
- The backend owns DTO contracts, enums, and shared constants used by the frontend.
- Frontend models are generated from the backend OpenAPI document.

## Example slice

`Features/Api/Architecture/GetOverview/` shows the baseline pattern:

- `GetArchitectureOverviewService.cs`
- DTOs colocated with the service while the slice is small
- OpenAPI enum support through `AppInfrastructure/OpenApi/PraxisEnumSchemaTransformer.cs`

## Local validation

```bash
cd backend
dotnet build
```
