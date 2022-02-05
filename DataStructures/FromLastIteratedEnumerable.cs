using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures;

/// <summary>
/// Tries to set last iterated item to the beginning, so next iterating will start from it.
/// </summary>
/// <typeparam name="T"></typeparam>
public class FromLastIteratedEnumerable<T>
{
    private readonly object lockObject;
    private T[] collection;
    private Guid version;

    public FromLastIteratedEnumerable(IEnumerable<T> values)
    {
        collection = values.ToArray();
        version = Guid.NewGuid();
        lockObject = new object();
    }

    public IEnumerable<T> Iterate()
    {
        var (takenCollection, takenVersion) = GetCollection();

        if (takenCollection == null)
            yield break;

        using var iterator = new AutoSortingIterator(
            takenCollection,
            newCollection => TrySetCollection(newCollection, takenVersion));
        foreach (var item in iterator.Iterate())
            yield return item;
    }

    private (T[] collection, Guid version) GetCollection()
    {
        lock (lockObject)
        {
            return (collection, version);
        }
    }

    private void TrySetCollection(T[] newValue, Guid fromVersion)
    {
        if (fromVersion != version)
            return;

        lock (lockObject)
        {
            if (fromVersion != version)
                return;

            version = Guid.NewGuid();
            collection = newValue;
        }
    }

    private class AutoSortingIterator : IDisposable
    {
        private readonly T[] collection;
        private readonly Action<T[]> updateCollection;
        private int lastIteratedIndex;

        public AutoSortingIterator(
            T[] collection,
            Action<T[]> updateCollection)
        {
            this.collection = collection;
            this.updateCollection = updateCollection;
            lastIteratedIndex = 0;
        }

        public void Dispose()
        {
            if (lastIteratedIndex == 0)
                return;

            var newCollection = EnumerateWithFirstItem(lastIteratedIndex).ToArray();
            updateCollection(newCollection);
        }

        public IEnumerable<T> Iterate()
        {
            for (var i = 0; i < collection.Length; i++)
            {
                lastIteratedIndex = i;
                yield return collection[i];
            }

            lastIteratedIndex = 0;
        }

        private IEnumerable<T> EnumerateWithFirstItem(int index)
        {
            yield return collection[index];

            for (var i = 0; i < collection.Length; i++)
            {
                if (i == index)
                    continue;

                yield return collection[i];
            }
        }
    }
}