using System;
using System.Collections.Generic;

namespace Aleab.Common
{
    public struct Range<T> : IEquatable<Range<T>> where T : IComparable
    {
        private readonly IEqualityComparer<T> _equalityComparer;

        public T Min { get; }
        public T Max { get; }

        public bool InclusiveMin { get; }
        public bool InclusiveMax { get; }

        public IEqualityComparer<T> EqualityComparer
        {
            get { return this._equalityComparer ?? EqualityComparer<T>.Default; }
        }

        public Range(T min, T max, bool inclusiveMin = true, bool inclusiveMax = true, IEqualityComparer<T> equalityComparer = null)
        {
            if (min.CompareTo(max) > 0)
                throw new ArgumentException($"Illegal arguments: {nameof(min)} is greater than {nameof(max)}!");

            this._equalityComparer = equalityComparer;

            this.Min = min;
            this.Max = max;
            this.InclusiveMin = inclusiveMin;
            this.InclusiveMax = inclusiveMax;

            if (this.EqualityComparer.Equals(min, max) && (!inclusiveMin || !inclusiveMax))
            {
                throw new ArgumentException($"Illegal arguments: {nameof(min)} equals {nameof(max)} and the value is not included in the range! " +
                                            $"Set both {nameof(inclusiveMin)} and {nameof(inclusiveMax)} to true (default) if you want to have a one-value range.");
            }
        }

        public bool Contains(T value)
        {
            bool min = this.InclusiveMin ? value.CompareTo(this.Min) >= 0 : value.CompareTo(this.Min) > 0;
            bool max = this.InclusiveMax ? value.CompareTo(this.Max) <= 0 : value.CompareTo(this.Max) < 0;
            return min && max;
        }

        public bool Contains(Range<T> range)
        {
            if (this.EqualityComparer.Equals(this.Min, range.Min) && !this.InclusiveMin && range.InclusiveMin)
                return false;

            if (this.EqualityComparer.Equals(this.Max, range.Max) && !this.InclusiveMax && range.InclusiveMax)
                return false;

            return this.Contains(range.Min) && this.Contains(range.Max);
        }

        public bool Overlaps(Range<T> range)
        {
            if (this.EqualityComparer.Equals(this.Max, range.Min) && this.InclusiveMax && range.InclusiveMin)
                return true;

            if (this.EqualityComparer.Equals(this.Min, range.Max) && this.InclusiveMin && range.InclusiveMax)
                return true;

            return this.Contains(range.Min) || this.Contains(range.Max);
        }

        public override string ToString()
        {
            string l = this.InclusiveMin ? "[" : "(";
            string u = this.InclusiveMax ? "]" : ")";
            return $"{l}{this.Min}, {this.Max}{u}";
        }

        #region Equals / GetHashCode

        /// <inheritdoc />
        public bool Equals(Range<T> other)
        {
            return this.EqualityComparer.Equals(this.Min, other.Min) &&
                   this.EqualityComparer.Equals(this.Max, other.Max) &&
                   this.InclusiveMin == other.InclusiveMin &&
                   this.InclusiveMax == other.InclusiveMax;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            return obj is Range<T> range && this.Equals(range);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.EqualityComparer.GetHashCode(this.Min);
                hashCode = (hashCode * 397) ^ this.EqualityComparer.GetHashCode(this.Max);
                hashCode = (hashCode * 397) ^ this.InclusiveMin.GetHashCode();
                hashCode = (hashCode * 397) ^ this.InclusiveMax.GetHashCode();
                return hashCode;
            }
        }

        #endregion Equals / GetHashCode
    }
}