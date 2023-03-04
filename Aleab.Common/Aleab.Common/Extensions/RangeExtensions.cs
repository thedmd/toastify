using System;
using System.Collections.Generic;

namespace Aleab.Common.Extensions
{
    public static class RangeExtensions
    {
        #region Static members

        /// <summary>
        ///     Cast this range of type <see cref="TIn" /> to a new range of type <see cref="TOut" />.
        /// </summary>
        /// <typeparam name="TOut">The new type</typeparam>
        /// <typeparam name="TIn">The current type</typeparam>
        /// <param name="range">The range to cast</param>
        /// <param name="equalityComparer">The <see cref="IEqualityComparer{T}" /> to use for the new range's type</param>
        /// <returns>The new range</returns>
        public static Range<TOut> CastToType<TOut, TIn>(this Range<TIn> range, IEqualityComparer<TOut> equalityComparer = null)
            where TIn : IComparable
            where TOut : IComparable
        {
            if (range.Min is TOut min && range.Max is TOut max && range.EqualityComparer is IEqualityComparer<TOut> eq)
                return new Range<TOut>(min, max, range.InclusiveMin, range.InclusiveMax, eq);

            return new Range<TOut>(
                (TOut)Convert.ChangeType(range.Min, typeof(TOut)),
                (TOut)Convert.ChangeType(range.Max, typeof(TOut)),
                range.InclusiveMin,
                range.InclusiveMax,
                equalityComparer);
        }

        #endregion
    }
}