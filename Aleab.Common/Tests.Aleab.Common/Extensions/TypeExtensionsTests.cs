using System;
using System.Collections.Generic;
using Aleab.Common.Extensions;
using Tests.Aleab.Common.Extensions.TestData;
using Xunit;

namespace Tests.Aleab.Common.Extensions
{
    public class TypeExtensionsTests
    {
        [Theory]
        [MemberData(nameof(TypeData.IsAData), MemberType = typeof(TypeData))]
        public void TestIsA<T>(Type type, T unused, bool expectedResult)
        {
            if (unused is Type typeToBe)
                Assert.Equal(expectedResult, type.IsA(typeToBe));
            else
            {
                Assert.Equal(expectedResult, type.IsA<T>());
                Assert.Equal(expectedResult, type.IsA(typeof(T)));
            }
        }

        [Theory]
        [MemberData(nameof(TypeData.IsNumericTypeData), MemberType = typeof(TypeData))]
        public void TestIsNumericType(IEnumerable<Type> types, bool expectedResult)
        {
            foreach (var type in types)
            {
                bool actualResult = type.IsNumericType();
                Assert.True(
                    actualResult == expectedResult,
                    $"{type.Name} is{(actualResult ? string.Empty : " not")} a numeric type! Expected result: {(expectedResult ? string.Empty : "not ")}numeric.");
            }
        }

        [Theory]
        [MemberData(nameof(TypeData.IsIntegerTypeData), MemberType = typeof(TypeData))]
        public void TestIsIntegerType(IEnumerable<Type> types, bool expectedResult)
        {
            foreach (var type in types)
            {
                bool actualResult = type.IsIntegerType();
                Assert.True(
                    actualResult == expectedResult,
                    $"{type.Name} is{(actualResult ? string.Empty : " not")} an integer type! Expected result: {(expectedResult ? string.Empty : "not ")}integer.");
            }
        }
    }
}