using Praxis.TargetArchitecture.Data;
using Praxis.TargetArchitecture.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Praxis.TargetArchitecture.Tests.Data;

public class PraxisDbSeederTests
{
    private static ServiceProvider BuildServices()
    {
        var databaseName = Guid.NewGuid().ToString();

        return new ServiceCollection()
            .AddDbContext<PraxisDbContext>(options => options.UseInMemoryDatabase(databaseName))
            .BuildServiceProvider();
    }

    [Fact]
    public async Task Should_seed_architecture_principles()
    {
        var services = BuildServices();

        await PraxisDbSeeder.Seed(services);

        var context = services.GetRequiredService<PraxisDbContext>();
        context.ArchitecturePrinciples.Count().ShouldBeGreaterThan(0);
        context.ArchitecturePrinciples.ShouldAllBe(principle => principle.CreatedUtc != default);
    }

    [Fact]
    public async Task Should_number_principles_sequentially_from_one()
    {
        var services = BuildServices();

        await PraxisDbSeeder.Seed(services);

        var numbers = services.GetRequiredService<PraxisDbContext>()
            .ArchitecturePrinciples
            .OrderBy(principle => principle.Number)
            .Select(principle => principle.Number)
            .ToList();

        numbers.ShouldBe(Enumerable.Range(1, numbers.Count).ToList());
    }

    [Fact]
    public async Task Should_not_duplicate_principles_when_run_twice()
    {
        var services = BuildServices();

        await PraxisDbSeeder.Seed(services);
        var afterFirstRun = services.GetRequiredService<PraxisDbContext>().ArchitecturePrinciples.Count();

        await PraxisDbSeeder.Seed(services);
        var afterSecondRun = services.GetRequiredService<PraxisDbContext>().ArchitecturePrinciples.Count();

        afterSecondRun.ShouldBe(afterFirstRun);
    }

    [Fact]
    public async Task Should_give_every_principle_a_distinct_title()
    {
        var services = BuildServices();

        await PraxisDbSeeder.Seed(services);

        var titles = services.GetRequiredService<PraxisDbContext>()
            .ArchitecturePrinciples
            .Select(principle => principle.Title)
            .ToList();

        titles.Distinct().Count().ShouldBe(titles.Count);
    }

    [Fact]
    public async Task Should_give_every_principle_two_or_three_distinct_tags()
    {
        var services = BuildServices();

        await PraxisDbSeeder.Seed(services);

        var principles = services.GetRequiredService<PraxisDbContext>()
            .ArchitecturePrinciples
            .ToList();

        principles.ShouldAllBe(principle => principle.Tags.Count >= 2 && principle.Tags.Count <= 3);
        principles.ShouldAllBe(principle => principle.Tags.Distinct().Count() == principle.Tags.Count);
    }

    [Fact]
    public async Task Should_attach_examples_only_to_the_principles_that_have_one()
    {
        var services = BuildServices();

        await PraxisDbSeeder.Seed(services);

        var principles = services.GetRequiredService<PraxisDbContext>()
            .ArchitecturePrinciples
            .ToList();

        var withExample = principles.Where(principle => principle.Example != null).ToList();
        withExample.ShouldNotBeEmpty();
        withExample.Count.ShouldBeLessThan(principles.Count);
        withExample.ShouldAllBe(principle => principle.Example!.Length > 0);
    }

    [Fact]
    public async Task Should_keep_each_layer_in_one_contiguous_block()
    {
        var services = BuildServices();

        await PraxisDbSeeder.Seed(services);

        var layers = services.GetRequiredService<PraxisDbContext>()
            .ArchitecturePrinciples
            .OrderBy(principle => principle.Number)
            .Select(principle => principle.Layer)
            .ToList();

        var blocks = layers.Where((layer, index) => index == 0 || layers[index - 1] != layer).ToList();

        blocks.Distinct().Count().ShouldBe(blocks.Count);
    }

    /// <summary>
    /// The key set is meant to be a readable introduction, not a second copy of the register.
    /// A wide band still catches the set quietly growing until the star means nothing.
    /// </summary>
    [Fact]
    public async Task Should_highlight_roughly_one_principle_in_five()
    {
        var services = BuildServices();

        await PraxisDbSeeder.Seed(services);

        var principles = services.GetRequiredService<PraxisDbContext>()
            .ArchitecturePrinciples
            .ToList();

        var highlightedShare = (double)principles.Count(principle => principle.IsHighlighted) / principles.Count;

        highlightedShare.ShouldBeInRange(0.15, 0.25);
    }

    [Fact]
    public async Task Should_highlight_at_least_one_principle_in_every_layer()
    {
        var services = BuildServices();

        await PraxisDbSeeder.Seed(services);

        var highlightedLayers = services.GetRequiredService<PraxisDbContext>()
            .ArchitecturePrinciples
            .Where(principle => principle.IsHighlighted)
            .Select(principle => principle.Layer)
            .Distinct()
            .ToList();

        highlightedLayers.ShouldBe(Enum.GetValues<ArchitectureLayer>(), ignoreOrder: true);
    }
}
