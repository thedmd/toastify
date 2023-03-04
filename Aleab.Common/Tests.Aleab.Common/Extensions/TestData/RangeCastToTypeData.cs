using System;
using Aleab.Common;
using Xunit;

namespace Tests.Aleab.Common.Extensions.TestData
{
    public class RangeCastToTypeData
    {
        #region Static Fields and Properties

        public static TheoryData<Range<IComparable>, IComparable> IncompatibleTypesData
        {
            get
            {
                return new TheoryData<Range<IComparable>, IComparable>
                {
                    { new Range<IComparable>(TimeSpan.FromSeconds(0.0), TimeSpan.FromSeconds(1.0)), 1 }, // TimeSpan -> int
                    { new Range<IComparable>(0, 1), TimeSpan.FromSeconds(1.0) },                         // int -> TimeSpan
                    { new Range<IComparable>(FakeComparable.Fake1, FakeComparable.Fake2), 1 },           // custom IComparable -> int
                    { new Range<IComparable>(0, 1), FakeComparable.Fake1 }                               // int -> custom IComparable
                };
            }
        }

        #endregion

        internal class FakeComparable : IComparable, IComparable<FakeComparable>
        {
            #region Static Fields and Properties

            internal static readonly FakeComparable Fake1 = new FakeComparable(nameof(Fake1));
            internal static readonly FakeComparable Fake2 = new FakeComparable(nameof(Fake2));

            #endregion

            public string String { get; }

            public FakeComparable(string @string)
            {
                this.String = @string;
            }

            public int CompareTo(object obj)
            {
                if (obj == null)
                    return 1;
                if (ReferenceEquals(this, obj))
                    return 0;
                return obj is FakeComparable other
                    ? this.CompareTo(other)
                    : throw new ArgumentException($"Object must be of type {nameof(FakeComparable)}");
            }

            public int CompareTo(FakeComparable other)
            {
                if (ReferenceEquals(this, other))
                    return 0;
                return other == null ? 1 : string.Compare(this.String, other.String, StringComparison.InvariantCulture);
            }
        }
    }
}