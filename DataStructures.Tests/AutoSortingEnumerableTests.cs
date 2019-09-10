using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace DataStructures.Tests
{
    public class AutoSortingEnumerableTests
    {
        [TestCase(1, "one", "two", "three", "four", "five")]
        [TestCase(2, "one", "two", "three", "four", "five")]
        [TestCase(3, "one", "two", "three", "four", "five")]
        [TestCase(4, "one", "two", "three", "four", "five")]
        [TestCase(5, "one", "two", "three", "four", "five")]
        public void AutoSortingEnumerator_Should_SetLastIteratedItemToTheBeginning(int takeCount,
            params string[] values)
        {
            var testQueue = new FromLastIteratedEnumerable<string>(values);

            testQueue.Iterate().Take(takeCount).ToArray();

            var result = testQueue.Iterate().ToArray();
            result[0].Should().BeEquivalentTo(values[takeCount - 1]);
        }

        [TestCase("A")]
        [TestCase("A", "B")]
        [TestCase("A", "B", "C")]
        public void AutoSortingEnumerator_Should_NotChange_IfItIsContinueIteratingToTheEnd(params string[] values)
        {
            var testQueue = new FromLastIteratedEnumerable<string>(values);

            testQueue.Iterate().ToArray();

            var result = testQueue.Iterate().ToArray();
            result.Should().BeInAscendingOrder();
            result.Should().BeEquivalentTo(values, config => config.WithStrictOrdering());
        }
    }
}