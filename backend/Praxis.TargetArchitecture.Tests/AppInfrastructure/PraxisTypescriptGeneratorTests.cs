using Praxis.TargetArchitecture.AppInfrastructure;
using Praxis.TargetArchitecture.Entities;
using Shouldly;

namespace Praxis.TargetArchitecture.Tests.AppInfrastructure;

public class PraxisTypescriptGeneratorTests
{
    [Fact]
    public void Should_give_every_enum_member_a_distinct_non_empty_label()
    {
        var labels = Enum.GetValues<ArchitectureLayer>().Select(ArchitectureLabels.For)
            .Concat(Enum.GetValues<ArchitectureTag>().Select(ArchitectureLabels.For))
            .ToList();

        labels.ShouldAllBe(label => label.Length > 0);
        labels.Distinct().Count().ShouldBe(labels.Count);
    }

    [Fact]
    public void Should_import_the_enums_rather_than_redeclaring_them()
    {
        var typescript = PraxisTypescriptGenerator.BuildTypescript();

        typescript.ShouldContain("import { ArchitectureLayer, ArchitectureTag } from \"./types.gen\"");
        typescript.ShouldNotContain("as const");
    }

    [Fact]
    public void Should_emit_a_label_entry_for_every_enum_member()
    {
        var typescript = PraxisTypescriptGenerator.BuildTypescript();

        foreach (var name in Enum.GetNames<ArchitectureLayer>())
        {
            typescript.ShouldContain($"[ArchitectureLayer.{name}]:");
        }

        foreach (var name in Enum.GetNames<ArchitectureTag>())
        {
            typescript.ShouldContain($"[ArchitectureTag.{name}]:");
        }
    }

    [Fact]
    public void Should_emit_shared_constants()
    {
        PraxisTypescriptGenerator.BuildTypescript().ShouldContain("export const MaxArchitecturePrinciples = 3");
    }
}
