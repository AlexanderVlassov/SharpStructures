using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoFixture;
using Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace DataStructures.Tests
{
    public class AutoSortingEnumerableConcurrencyTests
    {
        private readonly Fixture fixture = new Fixture();
        private readonly Random random = new Random();

        [Test]
        public void AutoSortingEnumerator_ShouldBe_ThreadSafe()
        {
            const int itemsCount = 100;
            const int threadsCount = 100;
            const int syclesInThreadCount = 100;

            var sourceCollection = fixture.CreateMany<string>(itemsCount).ToArray();
            var testQueue = new FromLastIteratedEnumerable<string>(sourceCollection.ToArray());
            var threads = CreateTestThreads(testQueue, itemsCount, threadsCount, syclesInThreadCount).ToArray();
            threads.ForEach(thread => thread.Start());

            while (threads.Any(thread => thread.IsAlive))
            {
            }

            var result = testQueue.Iterate().ToArray();
            result.Should().BeEquivalentTo(sourceCollection);
        }

        private IEnumerable<Thread> CreateTestThreads(
            FromLastIteratedEnumerable<string> autosorter,
            int itemsCount,
            int threadsCount,
            int syclesInThreadCount)
        {
            for (var threadIndex = 0; threadIndex < threadsCount; threadIndex++)
                yield return new Thread(
                    () =>
                    {
                        for (var i = 0; i < syclesInThreadCount; i++)
                            autosorter.Iterate().Take(random.Next(1, itemsCount)).ToArray();
                    });
        }
    }
}