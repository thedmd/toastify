using System;
using System.Collections;
using System.Linq;
using Aleab.Common.Extensions;
using Tests.Aleab.Common.Extensions.TestData;
using Xunit;

namespace Tests.Aleab.Common.Extensions
{
    public class LinqExtensionsTests
    {
        [Theory]
        [MemberData(nameof(EnumerableData.DistincData), MemberType = typeof(EnumerableData))]
        public void TestDistinct(IEnumerable enumerable, Func<object, object, bool> equals, Func<object, int> getHashCode, IEnumerable expectedResult)
        {
            if (enumerable == null || equals == null || getHashCode == null)
                Assert.Throws<ArgumentNullException>(() => enumerable.Distinct(equals, getHashCode));
            else
            {
                var expected = expectedResult.Cast<object>().ToList();
                var actual = enumerable.Distinct(equals, getHashCode).ToList();
                Assert.True(expected.All(o => actual.SingleOrDefault(oo => equals.Invoke(o, oo)) != null));
                Assert.True(actual.All(o => expected.SingleOrDefault(oo => equals.Invoke(o, oo)) != null));
            }
        }
    }
}