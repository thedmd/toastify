using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Tests.Aleab.Common.Extensions.TestData
{
    public class EnumerableData
    {
        #region Static Fields and Properties

        public static TheoryData<IEnumerable, Func<object, object, bool>, Func<object, int>, IEnumerable> DistincData
        {
            get
            {
                return new TheoryData<IEnumerable, Func<object, object, bool>, Func<object, int>, IEnumerable>
                {
                    {
                        // Simple list of a primitive type using its default equality comparer
                        new List<int> { 3, 1, 2, 2, 1, 1, 8, 3, 2, 5, 4, 1 },
                        (o1, o2) => EqualityComparer<int>.Default.Equals((int)o1, (int)o2),
                        o => EqualityComparer<int>.Default.GetHashCode((int)o),
                        new List<int> { 1, 2, 3, 4, 5, 8 }
                    },
                    {
                        // List of a primitive type using a custom equality comparer
                        new List<int> { 3, 1, 2, 4, 2, 2, 1, 8, 6, 5, 3, 5, 3 },
                        (o1, o2) => (int)o1 % 2 == (int)o2 % 2,
                        o => (int)o % 2 == 0 ? 2 : 1,
                        new List<int> { 3, 2 }
                    },
                    {
                        // Null source
                        null,
                        (o1, o2) => EqualityComparer<int>.Default.Equals((int)o1, (int)o2),
                        o => EqualityComparer<int>.Default.GetHashCode((int)o),
                        null
                    },
                    {
                        // Null equals function
                        new List<int>(),
                        null,
                        o => EqualityComparer<int>.Default.GetHashCode((int)o),
                        new List<int>()
                    },
                    {
                        // Null hash code function
                        new List<int>(),
                        (o1, o2) => EqualityComparer<int>.Default.Equals((int)o1, (int)o2),
                        null,
                        new List<int>()
                    }
                };
            }
        }

        #endregion
    }
}