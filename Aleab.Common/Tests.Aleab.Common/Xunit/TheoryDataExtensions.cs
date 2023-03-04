using System;
using System.Collections.Generic;
using Xunit;

namespace Tests.Aleab.Common.Xunit
{
    public static class TheoryDataExtensions
    {
        #region Cast

        public static TheoryData<TOut> Cast<TOut, TIn>(this TheoryData<TIn> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                var outData = new TheoryData<TOut>();
                foreach (var item in data)
                {
                    outData.Add((TOut)Convert.ChangeType(item[0], typeof(TOut)));
                }

                return outData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static TheoryData<TOut1, TOut2> Cast<TOut1, TOut2, TIn1, TIn2>(this TheoryData<TIn1, TIn2> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                var outData = new TheoryData<TOut1, TOut2>();
                foreach (var item in data)
                {
                    outData.Add(
                        (TOut1)Convert.ChangeType(item[0], typeof(TOut1)),
                        (TOut2)Convert.ChangeType(item[1], typeof(TOut2)));
                }

                return outData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static TheoryData<TOut1, TOut2, TOut3> Cast<TOut1, TOut2, TOut3, TIn1, TIn2, TIn3>(this TheoryData<TIn1, TIn2, TIn3> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                var outData = new TheoryData<TOut1, TOut2, TOut3>();
                foreach (var item in data)
                {
                    outData.Add(
                        (TOut1)Convert.ChangeType(item[0], typeof(TOut1)),
                        (TOut2)Convert.ChangeType(item[1], typeof(TOut2)),
                        (TOut3)Convert.ChangeType(item[2], typeof(TOut3)));
                }

                return outData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static TheoryData<TOut1, TOut2, TOut3, TOut4> Cast<TOut1, TOut2, TOut3, TOut4, TIn1, TIn2, TIn3, TIn4>(this TheoryData<TIn1, TIn2, TIn3, TIn4> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                var outData = new TheoryData<TOut1, TOut2, TOut3, TOut4>();
                foreach (var item in data)
                {
                    outData.Add(
                        (TOut1)Convert.ChangeType(item[0], typeof(TOut1)),
                        (TOut2)Convert.ChangeType(item[1], typeof(TOut2)),
                        (TOut3)Convert.ChangeType(item[2], typeof(TOut3)),
                        (TOut4)Convert.ChangeType(item[3], typeof(TOut4)));
                }

                return outData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static TheoryData<TOut1, TOut2, TOut3, TOut4, TOut5> Cast<TOut1, TOut2, TOut3, TOut4, TOut5, TIn1, TIn2, TIn3, TIn4, TIn5>(this TheoryData<TIn1, TIn2, TIn3, TIn4, TIn5> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                var outData = new TheoryData<TOut1, TOut2, TOut3, TOut4, TOut5>();
                foreach (var item in data)
                {
                    outData.Add(
                        (TOut1)Convert.ChangeType(item[0], typeof(TOut1)),
                        (TOut2)Convert.ChangeType(item[1], typeof(TOut2)),
                        (TOut3)Convert.ChangeType(item[2], typeof(TOut3)),
                        (TOut4)Convert.ChangeType(item[3], typeof(TOut4)),
                        (TOut5)Convert.ChangeType(item[4], typeof(TOut5)));
                }

                return outData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static TheoryData<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> Cast<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TIn1, TIn2, TIn3, TIn4, TIn5, TIn6>(this TheoryData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                var outData = new TheoryData<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>();
                foreach (var item in data)
                {
                    outData.Add(
                        (TOut1)Convert.ChangeType(item[0], typeof(TOut1)),
                        (TOut2)Convert.ChangeType(item[1], typeof(TOut2)),
                        (TOut3)Convert.ChangeType(item[2], typeof(TOut3)),
                        (TOut4)Convert.ChangeType(item[3], typeof(TOut4)),
                        (TOut5)Convert.ChangeType(item[4], typeof(TOut5)),
                        (TOut6)Convert.ChangeType(item[5], typeof(TOut6)));
                }

                return outData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static TheoryData<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> Cast<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7>(this TheoryData<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            try
            {
                var outData = new TheoryData<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>();
                foreach (var item in data)
                {
                    outData.Add(
                        (TOut1)Convert.ChangeType(item[0], typeof(TOut1)),
                        (TOut2)Convert.ChangeType(item[1], typeof(TOut2)),
                        (TOut3)Convert.ChangeType(item[2], typeof(TOut3)),
                        (TOut4)Convert.ChangeType(item[3], typeof(TOut4)),
                        (TOut5)Convert.ChangeType(item[4], typeof(TOut5)),
                        (TOut6)Convert.ChangeType(item[5], typeof(TOut6)),
                        (TOut7)Convert.ChangeType(item[6], typeof(TOut7)));
                }

                return outData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Add (tuples)

        public static void Add<T>(this TheoryData<T> data, IEnumerable<T> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add(item);
            }
        }

        public static void Add<T1, T2>(this TheoryData<T1, T2> data, IEnumerable<Tuple<T1, T2>> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add(item.Item1, item.Item2);
            }
        }

        public static void Add<T1, T2, T3>(this TheoryData<T1, T2, T3> data, IEnumerable<Tuple<T1, T2, T3>> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add(item.Item1, item.Item2, item.Item3);
            }
        }

        public static void Add<T1, T2, T3, T4>(this TheoryData<T1, T2, T3, T4> data, IEnumerable<Tuple<T1, T2, T3, T4>> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add(item.Item1, item.Item2, item.Item3, item.Item4);
            }
        }

        public static void Add<T1, T2, T3, T4, T5>(this TheoryData<T1, T2, T3, T4, T5> data, IEnumerable<Tuple<T1, T2, T3, T4, T5>> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5);
            }
        }

        public static void Add<T1, T2, T3, T4, T5, T6>(this TheoryData<T1, T2, T3, T4, T5, T6> data, IEnumerable<Tuple<T1, T2, T3, T4, T5, T6>> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5, item.Item6);
            }
        }

        public static void Add<T1, T2, T3, T4, T5, T6, T7>(this TheoryData<T1, T2, T3, T4, T5, T6, T7> data, IEnumerable<Tuple<T1, T2, T3, T4, T5, T6, T7>> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5, item.Item6, item.Item7);
            }
        }

        #endregion

        #region Add (TheoryData<>)

        public static void Add<T>(this TheoryData<T> data, TheoryData<T> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add((T)item[0]);
            }
        }

        public static void Add<T1, T2>(this TheoryData<T1, T2> data, TheoryData<T1, T2> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add((T1)item[0], (T2)item[1]);
            }
        }

        public static void Add<T1, T2, T3>(this TheoryData<T1, T2, T3> data, TheoryData<T1, T2, T3> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add((T1)item[0], (T2)item[1], (T3)item[2]);
            }
        }

        public static void Add<T1, T2, T3, T4>(this TheoryData<T1, T2, T3, T4> data, TheoryData<T1, T2, T3, T4> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add((T1)item[0], (T2)item[1], (T3)item[2], (T4)item[3]);
            }
        }

        public static void Add<T1, T2, T3, T4, T5>(this TheoryData<T1, T2, T3, T4, T5> data, TheoryData<T1, T2, T3, T4, T5> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add((T1)item[0], (T2)item[1], (T3)item[2], (T4)item[3], (T5)item[4]);
            }
        }

        public static void Add<T1, T2, T3, T4, T5, T6>(this TheoryData<T1, T2, T3, T4, T5, T6> data, TheoryData<T1, T2, T3, T4, T5, T6> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add((T1)item[0], (T2)item[1], (T3)item[2], (T4)item[3], (T5)item[4], (T6)item[5]);
            }
        }

        public static void Add<T1, T2, T3, T4, T5, T6, T7>(this TheoryData<T1, T2, T3, T4, T5, T6, T7> data, TheoryData<T1, T2, T3, T4, T5, T6, T7> dataToAdd)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (dataToAdd == null)
                return;

            foreach (var item in dataToAdd)
            {
                data.Add((T1)item[0], (T2)item[1], (T3)item[2], (T4)item[3], (T5)item[4], (T6)item[5], (T7)item[6]);
            }
        }

        #endregion
    }
}