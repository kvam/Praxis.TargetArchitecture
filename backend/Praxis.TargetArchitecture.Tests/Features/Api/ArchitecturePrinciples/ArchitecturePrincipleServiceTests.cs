using Praxis.TargetArchitecture.AppInfrastructure.Exceptions;
using Praxis.TargetArchitecture.Data;
using Praxis.TargetArchitecture.Entities;
using Praxis.TargetArchitecture.Features.Api.ArchitecturePrinciples.Create;
using Praxis.TargetArchitecture.Features.Api.ArchitecturePrinciples.Get;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Praxis.TargetArchitecture.Tests.Features.Api.ArchitecturePrinciples;

public class ArchitecturePrincipleServiceTests
{
    private static PraxisDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<PraxisDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static CreateArchitecturePrincipleDto BuildDto(string title = "Backend owns contracts") =>
        new()
        {
            Title = title,
            Rationale = "The backend is the single source of truth for domain contracts.",
            Layer = ArchitectureLayer.Contract,
            Tags = [ArchitectureTag.ApiDesign, ArchitectureTag.CodeGeneration],
            IsHighlighted = false,
            Example = null
        };

    [Fact]
    public async Task Should_create_principle_and_stamp_audit_fields()
    {
        var context = CreateContext();
        var service = new CreateArchitecturePrincipleService(context);

        var architecturePrincipleId = await service.Create(BuildDto());

        var stored = await context.ArchitecturePrinciples.SingleAsync();
        stored.Id.ShouldBe(architecturePrincipleId);
        stored.CreatedUtc.ShouldNotBe(default);
        stored.UpdatedUtc.ShouldNotBe(default);
    }

    [Fact]
    public async Task Should_number_created_principles_after_the_highest_existing_number()
    {
        var context = CreateContext();
        var service = new CreateArchitecturePrincipleService(context);

        await service.Create(BuildDto("First principle"));
        await service.Create(BuildDto("Second principle"));

        var numbers = context.ArchitecturePrinciples
            .OrderBy(principle => principle.Number)
            .Select(principle => principle.Number)
            .ToList();

        numbers.ShouldBe([1, 2]);
    }

    [Fact]
    public async Task Should_reject_duplicate_principle_titles()
    {
        var context = CreateContext();
        var service = new CreateArchitecturePrincipleService(context);

        await service.Create(BuildDto());

        await Should.ThrowAsync<ConflictException>(() => service.Create(BuildDto()));
    }

    [Fact]
    public async Task Should_throw_not_found_for_unknown_principle()
    {
        var service = new GetArchitecturePrinciplesService(CreateContext());

        await Should.ThrowAsync<NotFoundException>(() => service.GetById(Guid.NewGuid()));
    }

    [Fact]
    public void Should_reject_blank_title_during_validation()
    {
        var dto = BuildDto(title: "  ");

        Should.Throw<ValidationException>(() => CreateArchitecturePrincipleDtoValidator.Validate(dto));
    }

    [Fact]
    public void Should_reject_a_principle_with_too_few_tags()
    {
        var dto = BuildDto();
        dto.Tags = [ArchitectureTag.ApiDesign];

        Should.Throw<ValidationException>(() => CreateArchitecturePrincipleDtoValidator.Validate(dto));
    }

    [Fact]
    public void Should_reject_repeated_tags()
    {
        var dto = BuildDto();
        dto.Tags = [ArchitectureTag.ApiDesign, ArchitectureTag.ApiDesign];

        Should.Throw<ValidationException>(() => CreateArchitecturePrincipleDtoValidator.Validate(dto));
    }

    [Fact]
    public async Task Should_return_tags_on_the_created_principle()
    {
        var context = CreateContext();
        var createService = new CreateArchitecturePrincipleService(context);
        var getService = new GetArchitecturePrinciplesService(context);

        var architecturePrincipleId = await createService.Create(BuildDto());

        var principle = await getService.GetById(architecturePrincipleId);
        principle.Tags.ShouldBe([ArchitectureTag.ApiDesign, ArchitectureTag.CodeGeneration]);
    }
}
