using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Common.Extensions;

[UsedImplicitly]
public static class EnumerableExtensions
{
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
            yield return item;
        }
    }
}