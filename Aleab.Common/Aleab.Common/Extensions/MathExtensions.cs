using System;

namespace Aleab.Common.Extensions
{
    public static class MathExtensions
    {
        #region Static members

        /// <summary>
        ///     Add a tiny amount, positive or negative, to the specified single-precision floating-point value.
        /// </summary>
        /// <param name="value">The float value</param>
        /// <param name="negativeEpsilon">Whether to add a negative or positive amount</param>
        /// <returns>The incremented or decremented float</returns>
        public static float AddEpsilon(this float value, bool negativeEpsilon = false)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
                return value;

            int bits = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);

            if (value > 0.0f)
                value = BitConverter.ToSingle(BitConverter.GetBytes(bits + (negativeEpsilon ? -1 : 1)), 0);
            else if (value < 0.0f)
                value = BitConverter.ToSingle(BitConverter.GetBytes(bits - (negativeEpsilon ? -1 : 1)), 0);
            else
                value = negativeEpsilon ? -float.Epsilon : float.Epsilon;

            return value;
        }

        /// <summary>
        ///     Add a tiny amount, positive or negative, to the specified double-precision floating-point value.
        /// </summary>
        /// <param name="value">The double value</param>
        /// <param name="negativeEpsilon">Whether to add a negative or positive amount</param>
        /// <returns>The incremented or decremented double</returns>
        public static double AddEpsilon(this double value, bool negativeEpsilon = false)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                return value;

            long bits = BitConverter.DoubleToInt64Bits(value);

            if (value > 0.0)
                value = BitConverter.Int64BitsToDouble(bits + (negativeEpsilon ? -1 : 1));
            else if (value < 0.0)
                value = BitConverter.Int64BitsToDouble(bits - (negativeEpsilon ? -1 : 1));
            else
                value = negativeEpsilon ? -double.Epsilon : double.Epsilon;

            return value;
        }

        /// <summary>
        ///     Clamp the specified value inside the mininum and maximum values specified.
        /// </summary>
        /// <typeparam name="T">The type of value</typeparam>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>The clamped value</returns>
        public static T Clamp<T>(this T value, T min, T max) where T : IComparable
        {
            T ret = value;
            if (value.CompareTo(min) < 0)
                ret = min;
            else if (value.CompareTo(max) > 0)
                ret = max;
            return ret;
        }

        /// <summary>
        ///     Clamp the specified value inside the the specified range.
        /// </summary>
        /// <typeparam name="T">The type of value</typeparam>
        /// <param name="value">The value to clamp</param>
        /// <param name="range">The range</param>
        /// <returns>The clamped value</returns>
        public static T Clamp<T>(this T value, Range<T> range) where T : IComparable
        {
            T min = range.Min;
            T max = range.Max;

            if (typeof(T) == typeof(float))
            {
                if (!range.InclusiveMin && min is float fMin)
                    min = (T)Convert.ChangeType(fMin.AddEpsilon(), typeof(T));
                if (!range.InclusiveMax && max is float fMax)
                    max = (T)Convert.ChangeType(fMax.AddEpsilon(true), typeof(T));
            }
            else if (typeof(T) == typeof(double))
            {
                if (!range.InclusiveMin && min is double dMin)
                    min = (T)Convert.ChangeType(dMin.AddEpsilon(), typeof(T));
                if (!range.InclusiveMax && max is double dMax)
                    max = (T)Convert.ChangeType(dMax.AddEpsilon(true), typeof(T));
            }
            else if (typeof(T).IsIntegerType())
            {
                if (!range.InclusiveMin)
                    min = Sum(min, 1);
                if (!range.InclusiveMax)
                    max = Sum(max, -1);
            }

            return value.Clamp(min, max);
        }


        /// <summary>
        ///     Clamp the specified value inside the the specified range.
        /// </summary>
        /// <typeparam name="T">The type of value</typeparam>
        /// <param name="value">The value to clamp</param>
        /// <param name="range">The range</param>
        /// <returns>The clamped value, or the unmodified value itself if the range is null</returns>
        public static T Clamp<T>(this T value, Range<T>? range) where T : IComparable
        {
            return range.HasValue ? value.Clamp(range.Value) : value;
        }

        private static T Sum<T>(T value, int n) where T : IComparable
        {
            unchecked
            {
                if (value is byte b)
                    value = (T)Convert.ChangeType(b + n, typeof(T));
                else if (value is sbyte sb)
                    value = (T)Convert.ChangeType(sb + n, typeof(T));
                else if (value is ushort us)
                    value = (T)Convert.ChangeType(us + n, typeof(T));
                else if (value is short s)
                    value = (T)Convert.ChangeType(s + n, typeof(T));
                else if (value is int i)
                    value = (T)Convert.ChangeType(i + n, typeof(T));
                else if (value is uint ui)
                    value = (T)Convert.ChangeType(ui + n, typeof(T));
                else if (value is long l)
                    value = (T)Convert.ChangeType(l + n, typeof(T));
                else if (value is ulong ul)
                    value = (T)Convert.ChangeType(ul + (ulong)n, typeof(T));
            }

            return value;
        }

        #endregion
    }
}