using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public class Heap<T> : IEnumerable<T>
    {
        private readonly Func<T,T,int> comparer;
        private readonly List<T> list;

        public Heap(Func<T,T,int> comparer, int capacity = -1)
        {
            this.comparer = comparer;
            list = new List<T>(capacity + 1) {default};
        }

        private int Last => list.Count - 1;
        public int Count => list.Count - 1;
        public T Max => list[1];

        public IEnumerator<T> GetEnumerator() => list.Skip(1).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T value) {
            list.Add(value);
            Swim(Last);
        }

        public T DeleteMax() {
            var max = Max;
            Swap(1, Last);
            list.RemoveAt(Last);
            Sink(1);
            return max;
        }

        private void Swim(int node) {
            while (node > 1 && Less(Parent(node), node)) {
                Swap(Parent(node), node);
                node = Parent(node);
            }
        }

        private void Sink(int root) {
            while (Left(root) <= Last) {
                var child = Left(root);
                if (child < Last && Less(child, child + 1)) child++;
                if (!Less(root, child)) break;

                Swap(root, child);
                root = child;
            }
        }


        private static int Parent(int index) => index >> 1;
        private int Left(int index) => index << 1;
        private bool Less(int one, int two) => comparer(list[one], list[two]) < 0;

        private void Swap(int one, int two) {
            var acc = list[one];
            list[one] = list[two];
            list[two] = acc;
        }
    }
}