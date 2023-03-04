using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Aleab.Common.Extensions
{
    public static class TypeExtensions
    {
        #region Static Fields and Properties

        private static HashSet<Type> NumericTypes { get; } = new HashSet<Type>
        {
            typeof(decimal),
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(BigInteger),
            typeof(Complex)
        };

        private static HashSet<Type> IntegerTypes { get; } = new HashSet<Type>
        {
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(BigInteger)
        };

        #endregion

        #region Static members

        /// <summary>
        ///     Check whether a type is exactly or is derived from the specified type (<see cref="typeToBe" />).
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <param name="typeToBe">The type to check against</param>
        /// <returns>true if <see cref="type" /> is or is derived from <see cref="typeToBe" />; false otherwise</returns>
        public static bool IsA(this Type type, Type typeToBe)
        {
            if (type == typeToBe)
                return true;

            if (!typeToBe.IsGenericTypeDefinition)
                return typeToBe.IsAssignableFrom(type);

            var toCheckTypes = new List<Type> { type };
            if (typeToBe.IsInterface)
                toCheckTypes.AddRange(type.GetInterfaces());

            Type basedOn = type;
            while (basedOn.BaseType != null)
            {
                toCheckTypes.Add(basedOn.BaseType);
                basedOn = basedOn.BaseType;
            }

            return toCheckTypes.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeToBe);
        }

        /// <summary>
        ///     Check whether a type is exactly or is derived from the specified type parameter.
        /// </summary>
        /// <typeparam name="T">The type to check against</typeparam>
        /// <param name="type">The type to check</param>
        /// <returns>true if <see cref="type" /> is or is derived from <see cref="T" />; false otherwise</returns>
        public static bool IsA<T>(this Type type)
        {
            return type.IsA(typeof(T));
        }

        /// <summary>
        ///     Check whether the specified type is a numeric type.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>true if the type is a numeric type; false otherwise</returns>
        public static bool IsNumericType(this Type type)
        {
            return NumericTypes.Contains(type);
        }

        /// <summary>
        ///     Check whether the specified type is an integer numeric type.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>true if the type is an integer numeric type; false otherwise</returns>
        public static bool IsIntegerType(this Type type)
        {
            return IntegerTypes.Contains(type);
        }

        public static object GetDefault(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        #endregion
    }
}