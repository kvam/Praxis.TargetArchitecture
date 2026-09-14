namespace Praxis.TargetArchitecture.Data.Seed.Examples;

public static class TestExamples
{
    public const string TestsMirrorTheSlicePath = """
            The production slice:

                Praxis.TargetArchitecture/Features/Api/ArchitecturePrinciples/Get/

            The test:

                Praxis.TargetArchitecture.Tests/Features/Api/ArchitecturePrinciples/Get/

            No searching, no naming convention to remember, no orphaned tests left behind
            when a slice is deleted — the folder goes and its tests go with it.

            A flat `Tests/` folder with two hundred files in it answers the question "does
            this have a test?" only by reading all two hundred names.
            """;

    public const string TestAgainstARealDatabaseNotAMock = """
            A mocked context tests your mock:

                // WRONG
                var dbSet = new Mock<DbSet<ArchitecturePrinciple>>();
                mockContext.Setup(context => context.ArchitecturePrinciples)
                    .Returns(dbSet.Object);

            It passes with a query the real provider cannot even translate, and it says
            nothing about ordering, filtering, or projection — the parts most likely to be
            wrong.

            Give the test a real context instead:

                // RIGHT
                var options = new DbContextOptionsBuilder<PraxisDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                await using var dbContext = new PraxisDbContext(options);

            A fresh database per test, no shared state, and a query that either works or
            does not. The provider differences that remain are worth knowing about.
            """;

    public const string TestsDescribeBehaviourNotImplementation = """
            A test bound to how the code works today:

                // WRONG
                [Fact]
                public void Should_call_add_range_once() { ... }

            Refactor the seeder to add in batches and the test fails even though nothing a
            user could observe has changed. Tests like this make refactoring expensive,
            which is exactly backwards.

            A test bound to what must be true:

                // RIGHT
                [Fact]
                public async Task Should_number_principles_sequentially_from_one()
                {
                    var numbers = await dbContext.ArchitecturePrinciples
                        .Select(principle => principle.Number)
                        .ToListAsync();

                    numbers.ShouldBe(Enumerable.Range(1, numbers.Count));
                }

            The name states the guarantee, and the implementation is free to change
            underneath it.
            """;
}
