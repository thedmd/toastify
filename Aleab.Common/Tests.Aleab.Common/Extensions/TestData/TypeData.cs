using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Xunit;

namespace Tests.Aleab.Common.Extensions.TestData
{
    public class TypeData
    {
        #region Static Fields and Properties

        public static TheoryData<Type, object, bool> IsAData
        {
            get
            {
                return new TheoryData<Type, object, bool>
                {
                    { typeof(int), default(int), true },                               // Same type
                    { typeof(int), default(long), false },                             // Different type
                    { typeof(int), typeof(IComparable), true },                        // Implemented interface
                    { typeof(int), typeof(IEnumerable), false },                       // Not implemented interface
                    { typeof(int), typeof(IComparable<>), true },                      // Implemented unbound generic interface
                    { typeof(int), typeof(IEnumerable<>), false },                     // Not implemented unbound generic interface
                    { typeof(int), typeof(IComparable<int>), true },                   // Implemented generic interface w/ correct type argument
                    { typeof(int), typeof(IComparable<object>), false },               // Implemented generic interface w/ wrong type argument
                    { typeof(TheoryData<>), typeof(TheoryData), true },                // Extended abstract type
                    { typeof(TheoryData<>), typeof(IEnumerable), true },               // Interface implemented by a parent type
                    { typeof(FakeExtendedConcreteType), new FakeConcreteType(), true } // Extended concrete type
                };
            }
        }

        public static TheoryData<IEnumerable<Type>, bool> IsNumericTypeData
        {
            get
            {
                return new TheoryData<IEnumerable<Type>, bool>
                {
                    { NumericTypes, true },
                    { NonNumericTypes, false }
                };
            }
        }

        public static TheoryData<IEnumerable<Type>, bool> IsIntegerTypeData
        {
            get
            {
                return new TheoryData<IEnumerable<Type>, bool>
                {
                    { NumericIntegerTypes, true },
                    { NumericNonIntegerTypes, false },
                    { NonNumericTypes, false }
                };
            }
        }

        private static IEnumerable<Type> NumericIntegerTypes
        {
            get
            {
                return new List<Type>
                {
                    typeof(sbyte),
                    typeof(byte),
                    typeof(short),
                    typeof(ushort),
                    typeof(int),
                    typeof(uint),
                    typeof(long),
                    typeof(ulong),
                    typeof(BigInteger)
                };
            }
        }

        private static IEnumerable<Type> NumericFloatingPointTypes
        {
            get
            {
                return new List<Type>
                {
                    typeof(float),
                    typeof(double)
                };
            }
        }

        private static IEnumerable<Type> NumericNonIntegerTypes
        {
            get
            {
                var list = new List<Type>
                {
                    typeof(Complex)
                };
                list.AddRange(NumericFloatingPointTypes);
                return list.Distinct();
            }
        }

        private static IEnumerable<Type> NumericTypes
        {
            get
            {
                var list = new List<Type>();
                list.AddRange(NumericIntegerTypes);
                list.AddRange(NumericNonIntegerTypes);
                return list.Distinct();
            }
        }

        private static IEnumerable<Type> NonNumericTypes
        {
            get
            {
                return new List<Type>
                {
                    typeof(char),
                    typeof(string),
                    typeof(TimeSpan),
                    typeof(Array),
                    typeof(dynamic[])
                };
            }
        }

        #endregion

        internal class FakeConcreteType
        {
        }

        internal class FakeExtendedConcreteType : FakeConcreteType
        {
        }
    }
}