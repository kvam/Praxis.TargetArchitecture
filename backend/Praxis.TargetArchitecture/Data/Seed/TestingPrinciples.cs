using Praxis.TargetArchitecture.Entities;
using Praxis.TargetArchitecture.Data.Seed.Examples;

using static Praxis.TargetArchitecture.Data.Seed.ArchitecturePrincipleSeed;

namespace Praxis.TargetArchitecture.Data.Seed;

public static class TestingPrinciples
{
    public static List<ArchitecturePrinciple> All =>
    [
        // Testing: Testing
        Principle(
            "Every slice has a test",
            "A slice is finished when its happy path and each exception it throws are covered. Tests are part of the feature, not a follow-up task.",
            ArchitectureLayer.Testing,
            ArchitectureTag.VerticalSlices, ArchitectureTag.Delivery),
        Principle(
            "Tests mirror the slice path",
            "A test lives at the same path under the test project as the code it covers, so the test for a feature is found without searching.",
            TestExamples.TestsMirrorTheSlicePath,
            ArchitectureLayer.Testing,
            ArchitectureTag.VerticalSlices, ArchitectureTag.Naming),
        Principle(
            "Test against a real database, not a mock",
            "Services are exercised through an in-memory DbContext with a unique name per test, which verifies the query actually translates instead of asserting on a mock.",
            TestExamples.TestAgainstARealDatabaseNotAMock,
            ArchitectureLayer.Testing,
            ArchitectureTag.Database, ArchitectureTag.EntityFramework),
        Principle(
            "Tests describe behaviour, not implementation",
            "A test name states the expected outcome, so refactoring internals breaks a test only when observable behaviour changed.",
            TestExamples.TestsDescribeBehaviourNotImplementation,
            ArchitectureLayer.Testing,
            ArchitectureTag.Readability, ArchitectureTag.Naming),
    ];
}
