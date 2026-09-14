using Praxis.TargetArchitecture.AppInfrastructure;
using Praxis.TargetArchitecture.AppInfrastructure.Middleware;
using Praxis.TargetArchitecture.AppInfrastructure.OpenApi;
using Praxis.TargetArchitecture.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(options => options.AddSchemaTransformer<PraxisEnumSchemaTransformer>());
builder.Services.AddDbContext<PraxisDbContext>(options => options.UseInMemoryDatabase("praxis-target-architecture"));
builder.Services.AddPraxisServices();

var app = builder.Build();

app.UseMiddleware<PraxisExceptionHandlingMiddleware>();

if (PraxisEnvironments.ExposeOpenApiDocument)
{
    app.MapOpenApi();
}

app.MapControllers();

await PraxisTypescriptGenerator.GenerateFiles();

if (PraxisEnvironments.SeedArchitecturePrinciples)
{
    await PraxisDbSeeder.Seed(app.Services);
}

app.Run();
