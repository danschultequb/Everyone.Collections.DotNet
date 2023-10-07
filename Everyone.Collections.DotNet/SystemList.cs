using System.Collections.Generic;
using System.Linq;

namespace Everyone
{
    public static class SystemList
    {
        public static SystemList<T> Create<T>(params T[] values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            return SystemList.Create<T>(values.ToList());
        }

        public static SystemList<T> Create<T>(IEnumerable<T> values)
        {
            return SystemList<T>.Create(values);
        }
    }

    public class SystemList<T> : ListBase<T,SystemList<T>>
    {
        private readonly System.Collections.Generic.List<T> list;

        private SystemList(IEnumerable<T> values)
        {
            Pre.Condition.AssertNotNull(values, nameof(values));

            this.list = new System.Collections.Generic.List<T>(values);
        }

        internal static SystemList<T> Create(IEnumerable<T> values)
        {
            return new SystemList<T>(values);
        }

        public override int Count => this.list.Count;

        public override T Get(int index)
        {
            Pre.Condition.AssertAccessIndex(index, this, nameof(index));

            return this.list[index];
        }

        public override SystemList<T> Set(int index, T value)
        {
            Pre.Condition.AssertAccessIndex(index, this, nameof(index));

            this.list[index] = value;

            return this;
        }

        public override SystemList<T> Insert(int index, T value)
        {
            Pre.Condition.AssertInsertIndex(index, this, nameof(index));

            this.list.Insert(index, value);

            return this;
        }

        public override IndexableIterator<T> Iterate()
        {
            return IndexableIterator.Create(this.list.Iterate());
        }

        public override Result<int> IndexOf(T item)
        {
            int itemIndex = this.list.IndexOf(item);
            return itemIndex == -1
                ? Result.Create<int>(new NotFoundException($"Could not find {item}."))
                : Result.Create(itemIndex);
        }

        public override void RemoveAt(int index)
        {
            this.list.RemoveAt(index);
        }

        public override void Clear()
        {
            this.list.Clear();
        }

        public override bool Contains(T item)
        {
            return this.list.Contains(item);
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        public override bool Remove(T item)
        {
            return this.list.Remove(item);
        }
    }
}
