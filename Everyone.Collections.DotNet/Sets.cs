using System;
using System.Collections.Generic;

namespace Everyone
{
    public static class Sets
    {
        public static void AddRange<T>(this ISet<T> set, IEnumerable<T>? values)
        {
            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            if (values != null)
            {
                foreach (T value in values)
                {
                    set.Add(value);
                }
            }
        }
    }
}
