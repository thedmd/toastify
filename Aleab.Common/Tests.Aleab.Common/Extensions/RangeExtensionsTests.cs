using System;
using Aleab.Common;
using Aleab.Common.Extensions;
using Tests.Aleab.Common.Extensions.TestData;
using Xunit;

namespace Tests.Aleab.Common.Extensions
{
    public class RangeExtensionsTests
    {
        #region TEST – CastToType()

#pragma warning disable xUnit1026 // Theory methods should use all of their parameters

        [Theory]
        [MemberData(nameof(RangeCastToTypeData.IncompatibleTypesData), MemberType = typeof(RangeCastToTypeData))]
        public void TestCastToType_IncompatibleCast<TOut, TIn>(Range<TIn> range, TOut unused = default(TOut))
            where TIn : IComparable
            where TOut : IComparable
        {
            // ReSharper disable once InvokeAsExtensionMethod
            Assert.Throws<InvalidCastException>(() => RangeExtensions.CastToType<TOut, TIn>(range));
        }

        [Theory]
        [InlineData((sbyte)0, (sbyte)1), InlineData((byte)0, (byte)2)]
        [InlineData((short)0, (short)3), InlineData((ushort)0, (ushort)4)]
        [InlineData(0, 5), InlineData(0u, 6u)]
        [InlineData(0L, 7L), InlineData(0uL, 8uL)]
        [InlineData(0.0f, 9.0f), InlineData(0.0, 10.0)]
        public void TestCastToType_SameTypeCast<T>(T min, T max) where T : IComparable
        {
            var range = new Range<T>(min, max);

            // ReSharper disable once InvokeAsExtensionMethod
            var castRange = RangeExtensions.CastToType<T, T>(range);

            Assert.Equal(range.Min, castRange.Min);
            Assert.Equal(range.Max, castRange.Max);
            Assert.Equal(range.InclusiveMin, castRange.InclusiveMin);
            Assert.Equal(range.InclusiveMax, castRange.InclusiveMax);
            Assert.Same(range.EqualityComparer, castRange.EqualityComparer);
        }

        [Theory]
        [InlineData((sbyte)0, (sbyte)1, (byte)0), InlineData((sbyte)0, (sbyte)1, (short)1), InlineData((sbyte)0, (sbyte)1, (ushort)2), InlineData((sbyte)0, (sbyte)1, 3u), InlineData((sbyte)0, (sbyte)1, 4), InlineData((sbyte)0, (sbyte)1, 5L), InlineData((sbyte)0, (sbyte)1, 6uL), InlineData((sbyte)0, (sbyte)1, 7.0f), InlineData((sbyte)0, (sbyte)1, 8.0)]
        [InlineData((byte)0, (byte)2, (sbyte)0), InlineData((byte)0, (byte)2, (short)1), InlineData((byte)0, (byte)2, (ushort)2), InlineData((byte)0, (byte)2, 3u), InlineData((byte)0, (byte)2, 4), InlineData((byte)0, (byte)2, 5L), InlineData((byte)0, (byte)2, 6uL), InlineData((byte)0, (byte)2, 7.0f), InlineData((byte)0, (byte)2, 8.0)]
        [InlineData((short)0, (short)3, (sbyte)0), InlineData((short)0, (short)3, (byte)1), InlineData((short)0, (short)3, (ushort)2), InlineData((short)0, (short)3, 3u), InlineData((short)0, (short)3, 4), InlineData((short)0, (short)3, 5L), InlineData((short)0, (short)3, 6uL), InlineData((short)0, (short)3, 7.0f), InlineData((short)0, (short)3, 8.0)]
        [InlineData((ushort)0, (ushort)4, (sbyte)0), InlineData((ushort)0, (ushort)4, (byte)1), InlineData((ushort)0, (ushort)4, (short)2), InlineData((ushort)0, (ushort)4, 3u), InlineData((ushort)0, (ushort)4, 4), InlineData((ushort)0, (ushort)4, 5L), InlineData((ushort)0, (ushort)4, 6uL), InlineData((ushort)0, (ushort)4, 7.0f), InlineData((ushort)0, (ushort)4, 8.0)]
        [InlineData(0, 5, (sbyte)0), InlineData(0, 5, (byte)1), InlineData(0, 5, (short)2), InlineData(0, 5, (ushort)3), InlineData(0, 5, 4u), InlineData(0, 5, 5L), InlineData(0, 5, 6uL), InlineData(0, 5, 7.0f), InlineData(0, 5, 8.0)]
        [InlineData(0u, 6u, (sbyte)0), InlineData(0u, 6u, (byte)1), InlineData(0u, 6u, (short)2), InlineData(0u, 6u, (ushort)3), InlineData(0u, 6u, 4), InlineData(0u, 6u, 5L), InlineData(0u, 6u, 6uL), InlineData(0, 6u, 7.0f), InlineData(0, 6u, 8.0)]
        [InlineData(0L, 7L, (sbyte)0), InlineData(0L, 7L, (byte)1), InlineData(0L, 7L, (short)2), InlineData(0L, 7L, (ushort)3), InlineData(0L, 7L, 4u), InlineData(0L, 7L, 5), InlineData(0L, 7L, 6uL), InlineData(0, 7L, 7.0f), InlineData(0, 7L, 8.0)]
        [InlineData(0uL, 8uL, (sbyte)0), InlineData(0uL, 8uL, (byte)1), InlineData(0uL, 8uL, (short)2), InlineData(0uL, 8uL, (ushort)3), InlineData(0uL, 8uL, 4u), InlineData(0uL, 8uL, 5), InlineData(0uL, 8uL, 6L), InlineData(0, 8uL, 7.0f), InlineData(0, 8uL, 8.0)]
        [InlineData(0.0f, 0.1f, (sbyte)0), InlineData(0.0f, 0.1f, (byte)1), InlineData(0.0f, 0.1f, (short)2), InlineData(0.0f, 0.1f, (ushort)3), InlineData(0.0f, 0.1f, 4), InlineData(0.0f, 0.1f, 5u), InlineData(0.0f, 0.1f, 6L), InlineData(0.0f, 0.1f, 7uL), InlineData(0.0f, 0.1f, 8.0)]
        [InlineData(0.0, 0.2, (sbyte)0), InlineData(0.0, 0.2, (byte)1), InlineData(0.0, 0.2, (short)2), InlineData(0.0, 0.2, (ushort)3), InlineData(0.0, 0.2, 4), InlineData(0.0, 0.2, 5u), InlineData(0.0, 0.2, 6L), InlineData(0.0, 0.2, 7uL), InlineData(0.0, 0.2, 8.0f)]
        public void TestCastToType<TOut, TIn>(TIn min, TIn max, TOut unused)
            where TIn : IComparable
            where TOut : IComparable
        {
            var range = new Range<TIn>(min, max);

            // ReSharper disable once InvokeAsExtensionMethod
            var castRange = RangeExtensions.CastToType<TOut, TIn>(range);

            TOut expectedMin = (TOut)Convert.ChangeType(range.Min, typeof(TOut));
            TOut expectedMax = (TOut)Convert.ChangeType(range.Max, typeof(TOut));

            Assert.Equal(expectedMin, castRange.Min, castRange.EqualityComparer);
            Assert.Equal(expectedMax, castRange.Max, castRange.EqualityComparer);
            Assert.Equal(range.InclusiveMin, castRange.InclusiveMin);
            Assert.Equal(range.InclusiveMax, castRange.InclusiveMax);
        }

#pragma warning restore xUnit1026 // Theory methods should use all of their parameters

        #endregion
    }
}