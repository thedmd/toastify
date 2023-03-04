using System;
using System.Collections.Generic;
using Aleab.Common;
using Aleab.Common.Extensions;
using Tests.Aleab.Common.Extensions.TestData;
using Xunit;

namespace Tests.Aleab.Common.Extensions
{
    public class MathExtensionsTests
    {
        #region Static members

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void TestClamp<T>(T value, T min, T max, T expectedResult) where T : IComparable
        {
            // ReSharper disable once InvokeAsExtensionMethod
            T actualResult = MathExtensions.Clamp(value, min, max);
            Assert.Equal(expectedResult, actualResult);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void TestClampRange<T>(T value, Range<T>? range, T expectedResult) where T : IComparable
        {
            // ReSharper disable once InvokeAsExtensionMethod
            T actualResult = MathExtensions.Clamp(value, range);
            Assert.Equal(expectedResult, actualResult);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void TestClampRange<T>(T value, Range<int>? range, T expectedResult, IEqualityComparer<T> equalityComparer = null) where T : IComparable
        {
            // ReSharper disable once InvokeAsExtensionMethod
            T actualResult = MathExtensions.Clamp(value, range?.CastToType(equalityComparer));
            Assert.Equal(expectedResult, actualResult);
        }

        #endregion

        #region TEST – AddEpsilon()

        [Theory]
        [InlineData(float.NaN, false, float.NaN)]
        [InlineData(float.NegativeInfinity, false, float.NegativeInfinity)]
        [InlineData(float.PositiveInfinity, false, float.PositiveInfinity)]
        [InlineData(float.MinValue, true, float.NegativeInfinity)]
        [InlineData(float.MaxValue, false, float.PositiveInfinity)]
        [InlineData(-1.0f, false), InlineData(-1.0f, true)]
        [InlineData(0.0f, false), InlineData(0.0f, true)]
        [InlineData(1.0f, false), InlineData(1.0f, true)]
        public void TestAddEpsilon_Float(float value, bool negativeEpsilon, float? expectedResult = null)
        {
            // ReSharper disable once InvokeAsExtensionMethod
            float result = MathExtensions.AddEpsilon(value, negativeEpsilon);

            if (expectedResult.HasValue)
                Assert.Equal(expectedResult, result);
            else if (value.Equals(0.0f))
                Assert.Equal(negativeEpsilon ? -float.Epsilon : float.Epsilon, result);
            else
                Assert.True(negativeEpsilon ? result < value : result > value);
        }

        [Theory]
        [InlineData(double.NaN, false, double.NaN)]
        [InlineData(double.NegativeInfinity, false, double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity, false, double.PositiveInfinity)]
        [InlineData(double.MinValue, true, double.NegativeInfinity)]
        [InlineData(double.MaxValue, false, double.PositiveInfinity)]
        [InlineData(-1.0, false), InlineData(-1.0, true)]
        [InlineData(0.0, false), InlineData(0.0, true)]
        [InlineData(1.0, false), InlineData(1.0, true)]
        public void TestAddEpsilon_Double(double value, bool negativeEpsilon, double? expectedResult = null)
        {
            // ReSharper disable once InvokeAsExtensionMethod
            double result = MathExtensions.AddEpsilon(value, negativeEpsilon);

            if (expectedResult.HasValue)
                Assert.Equal(expectedResult, result);
            else if (value.Equals(0.0f))
                Assert.Equal(negativeEpsilon ? -double.Epsilon : double.Epsilon, result);
            else
                Assert.True(negativeEpsilon ? result < value : result > value);
        }

        #endregion

        #region TEST – Clamp(T, Range<T>?, T)

        [Theory]
        [MemberData(nameof(ClampTestData.IntRangeData), MemberType = typeof(ClampTestData))]
        public void TestClampRange_SByte(sbyte value, Range<int>? range, sbyte expectedResult) => TestClampRange(value, range, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.UIntRangeData), MemberType = typeof(ClampTestData))]
        public void TestClampRange_Byte(byte value, Range<int>? range, byte expectedResult) => TestClampRange(value, range, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.IntRangeData), MemberType = typeof(ClampTestData))]
        public void TestClampRange_Short(short value, Range<int>? range, short expectedResult) => TestClampRange(value, range, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.UIntRangeData), MemberType = typeof(ClampTestData))]
        public void TestClampRange_UShort(ushort value, Range<int>? range, ushort expectedResult) => TestClampRange(value, range, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.IntRangeData), MemberType = typeof(ClampTestData))]
        public void TestClampRange_Int(int value, Range<int>? range, int expectedResult) => TestClampRange(value, range, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.UIntRangeData), MemberType = typeof(ClampTestData))]
        public void TestClampRange_UInt(uint value, Range<int>? range, uint expectedResult) => TestClampRange(value, range, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.IntRangeData), MemberType = typeof(ClampTestData))]
        public void TestClampRange_Long(long value, Range<int>? range, long expectedResult) => TestClampRange(value, range, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.UIntRangeData), MemberType = typeof(ClampTestData))]
        public void TestClampRange_ULong(ulong value, Range<int>? range, ulong expectedResult) => TestClampRange(value, range, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.FloatRangeData), MemberType = typeof(ClampTestData))]
        public void TestClampRange_Float(float value, Range<float>? range, float expectedResult) => TestClampRange(value, range, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.DoubleRangeData), MemberType = typeof(ClampTestData))]
        public void TestClampRange_Double(double value, Range<double>? range, double expectedResult) => TestClampRange(value, range, expectedResult);

        #endregion

        #region TEST – Clamp(T, T, T)

        [Theory]
        [MemberData(nameof(ClampTestData.IntegerData), MemberType = typeof(ClampTestData))]
        public void TestClamp_SByte(sbyte value, sbyte min, sbyte max, sbyte expectedResult) => TestClamp(value, min, max, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.UnsignedIntegerData), MemberType = typeof(ClampTestData))]
        public void TestClamp_Byte(byte value, byte min, byte max, byte expectedResult) => TestClamp(value, min, max, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.IntegerData), MemberType = typeof(ClampTestData))]
        public void TestClamp_Short(short value, short min, short max, short expectedResult) => TestClamp(value, min, max, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.UnsignedIntegerData), MemberType = typeof(ClampTestData))]
        public void TestClamp_UShort(ushort value, ushort min, ushort max, ushort expectedResult) => TestClamp(value, min, max, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.IntegerData), MemberType = typeof(ClampTestData))]
        public void TestClamp_Int(int value, int min, int max, int expectedResult) => TestClamp(value, min, max, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.UnsignedIntegerData), MemberType = typeof(ClampTestData))]
        public void TestClamp_UInt(uint value, uint min, uint max, uint expectedResult) => TestClamp(value, min, max, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.IntegerData), MemberType = typeof(ClampTestData))]
        public void TestClamp_Long(long value, long min, long max, long expectedResult) => TestClamp(value, min, max, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.UnsignedIntegerData), MemberType = typeof(ClampTestData))]
        public void TestClamp_ULong(ulong value, ulong min, ulong max, ulong expectedResult) => TestClamp(value, min, max, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.IntegerData), MemberType = typeof(ClampTestData))]
        public void TestClamp_Float(float value, float min, float max, float expectedResult) => TestClamp(value, min, max, expectedResult);

        [Theory]
        [MemberData(nameof(ClampTestData.IntegerData), MemberType = typeof(ClampTestData))]
        public void TestClamp_Double(double value, double min, double max, double expectedResult) => TestClamp(value, min, max, expectedResult);

        #endregion
    }
}