using Aleab.Common;
using Aleab.Common.Extensions;
using Tests.Aleab.Common.Xunit;
using Xunit;

namespace Tests.Aleab.Common.Extensions.TestData
{
    public class ClampTestData
    {
        #region Clamp(T, Range<T>?, T)

        public static TheoryData<int, Range<int>?, int> IntRangeData
        {
            get
            {
                return new TheoryData<int, Range<int>?, int>
                {
                    UIntRangeData,
                    NegativeIntegerRangeData
                };
            }
        }

        public static TheoryData<int, Range<int>?, int> UIntRangeData
        {
            get
            {
                return new TheoryData<int, Range<int>?, int>
                {
                    { 1, null, 1 }, // Null range

                    { 1, new Range<int>(0, 2), 1 }, // Within range
                    { 1, new Range<int>(1, 2), 1 }, // Equal to min
                    { 0, new Range<int>(1, 2), 1 }, // Less than min
                    { 2, new Range<int>(0, 2), 2 }, // Equal to max
                    { 3, new Range<int>(1, 2), 2 }, // More than max

                    { 1, new Range<int>(1, 2, inclusiveMin: false), 2 }, // Equal to not included min
                    { 2, new Range<int>(0, 2, inclusiveMax: false), 1 }  // Equal to not included max
                };
            }
        }

        public static TheoryData<float, Range<float>?, float> FloatRangeData
        {
            get
            {
                return new TheoryData<float, Range<float>?, float>
                {
                    { 1.0f, null, 1.0f },   // Null range
                    { -1.0f, null, -1.0f }, // Null range

                    { 1.0f, new Range<float>(0.0f, 2.0f), 1.0f }, // Within range
                    { 1.0f, new Range<float>(1.0f, 2.0f), 1.0f }, // Equal to min
                    { 0.0f, new Range<float>(1.0f, 2.0f), 1.0f }, // Less than min
                    { 2.0f, new Range<float>(0.0f, 2.0f), 2.0f }, // Equal to max
                    { 3.0f, new Range<float>(1.0f, 2.0f), 2.0f }, // More than max

                    { 1.0f, new Range<float>(1.0f, 2.0f, inclusiveMin: false), 1.0f.AddEpsilon() },     // Equal to not included min
                    { 2.0f, new Range<float>(0.0f, 2.0f, inclusiveMax: false), 2.0f.AddEpsilon(true) }, // Equal to not included max

                    { -3.0f, new Range<float>(-4.0f, -2.0f), -3.0f }, // Within range
                    { -4.0f, new Range<float>(-4.0f, -2.0f), -4.0f }, // Equal to min
                    { -5.0f, new Range<float>(-4.0f, -2.0f), -4.0f }, // Less than min
                    { -2.0f, new Range<float>(-4.0f, -2.0f), -2.0f }, // Equal to max
                    { -1.0f, new Range<float>(-4.0f, -2.0f), -2.0f }, // More than max

                    { -4.0f, new Range<float>(-4.0f, -2.0f, inclusiveMin: false), (-4.0f).AddEpsilon() },    // Equal to not included min
                    { -2.0f, new Range<float>(-4.0f, -2.0f, inclusiveMax: false), (-2.0f).AddEpsilon(true) } // Equal to not included max
                };
            }
        }

        public static TheoryData<double, Range<double>?, double> DoubleRangeData
        {
            get
            {
                return new TheoryData<double, Range<double>?, double>
                {
                    { 1.0, null, 1.0 },   // Null range
                    { -1.0, null, -1.0 }, // Null range

                    { 1.0, new Range<double>(0.0, 2.0), 1.0 }, // Within range
                    { 1.0, new Range<double>(1.0, 2.0), 1.0 }, // Equal to min
                    { 0.0, new Range<double>(1.0, 2.0), 1.0 }, // Less than min
                    { 2.0, new Range<double>(0.0, 2.0), 2.0 }, // Equal to max
                    { 3.0, new Range<double>(1.0, 2.0), 2.0 }, // More than max

                    { 1.0, new Range<double>(1.0, 2.0, inclusiveMin: false), 1.0.AddEpsilon() },     // Equal to not included min
                    { 2.0, new Range<double>(0.0, 2.0, inclusiveMax: false), 2.0.AddEpsilon(true) }, // Equal to not included max

                    { -3.0, new Range<double>(-4.0, -2.0), -3.0 }, // Within range
                    { -4.0, new Range<double>(-4.0, -2.0), -4.0 }, // Equal to min
                    { -5.0, new Range<double>(-4.0, -2.0), -4.0 }, // Less than min
                    { -2.0, new Range<double>(-4.0, -2.0), -2.0 }, // Equal to max
                    { -1.0, new Range<double>(-4.0, -2.0), -2.0 }, // More than max

                    { -4.0, new Range<double>(-4.0, -2.0, inclusiveMin: false), (-4.0).AddEpsilon() },    // Equal to not included min
                    { -2.0, new Range<double>(-4.0, -2.0, inclusiveMax: false), (-2.0).AddEpsilon(true) } // Equal to not included max
                };
            }
        }

        private static TheoryData<int, Range<int>?, int> NegativeIntegerRangeData
        {
            get
            {
                return new TheoryData<int, Range<int>?, int>
                {
                    { -3, null, -3 }, // Null range

                    { -3, new Range<int>(-4, -2), -3 }, // Within range
                    { -4, new Range<int>(-4, -2), -4 }, // Equal to min
                    { -5, new Range<int>(-4, -2), -4 }, // Less than min
                    { -2, new Range<int>(-4, -2), -2 }, // Equal to max
                    { -1, new Range<int>(-4, -2), -2 }, // More than max

                    { -4, new Range<int>(-4, -2, inclusiveMin: false), -3 }, // Equal to not included min
                    { -2, new Range<int>(-4, -2, inclusiveMax: false), -3 }  // Equal to not included max
                };
            }
        }

        #endregion

        #region Clamp(T, T, T)

        public static TheoryData<int, int, int, int> IntegerData
        {
            get
            {
                return new TheoryData<int, int, int, int>
                {
                    UnsignedIntegerData,
                    NegativeIntegerData
                };
            }
        }

        public static TheoryData<int, int, int, int> UnsignedIntegerData
        {
            get
            {
                return new TheoryData<int, int, int, int>
                {
                    { 1, 0, 2, 1 }, // Within range
                    { 1, 1, 2, 1 }, // Equal to min
                    { 0, 1, 2, 1 }, // Less than min
                    { 2, 0, 2, 2 }, // Equal to max
                    { 3, 1, 2, 2 }  // More than max
                };
            }
        }

        private static TheoryData<int, int, int, int> NegativeIntegerData
        {
            get
            {
                return new TheoryData<int, int, int, int>
                {
                    { -3, -4, -2, -3 }, // Within range
                    { -4, -4, -2, -4 }, // Equal to min
                    { -5, -4, -2, -4 }, // Less than min
                    { -2, -4, -2, -2 }, // Equal to max
                    { -1, -4, -2, -2 }  // More than max
                };
            }
        }

        #endregion
    }
}