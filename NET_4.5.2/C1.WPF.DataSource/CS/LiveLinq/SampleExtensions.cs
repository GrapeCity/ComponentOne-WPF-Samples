using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using C1.LiveLinq.Indexing.Search;

namespace C1.LiveLinq
{
    public static class IndexScannerExtensions
    {
        public static IEnumerable<TKey> Keys<T, TKey>(this IIndexScanner<T, TKey> scanner)
        {
            return scanner.Keys(Order.Unordered);
        }

        public static IndexQuery<T, DateTime> FindYear<T>(this IIndexScanner<T, DateTime> scanner, int year,
                                                           Order order)
        {
            return scanner.FindBetween(
                new DateTime(year, 1, 1), true,
                new DateTime(year + 1, 1, 1), false,
                null,
                order);
        }

        #region Search overloads with default parameters

        #region All
        public static IndexQuery<T> All<T>(this IIndexScanner<T> scanner)
        {
            return scanner.All(Order.Unordered);
        }
        public static IndexQuery<T, TKey> All<T, TKey>(this IIndexScanner<T, TKey> scanner)
        {
            return scanner.All(Order.Unordered);
        }
        #endregion

        #region FindKeys

        public static IndexQuery<T, TKey> FindKeys<T, TKey>(this IIndexScanner<T, TKey> scanner, IEnumerable<TKey> keys)
        {
            return scanner.FindKeys(keys, Order.Unordered);
        }
        public static IndexQuery<T, TKey> FindKeys<T, TKey>(this IIndexScanner<T, TKey> scanner, params TKey[] keys)
        {
            return scanner.FindKeys(keys, Order.Unordered);
        }

        public static IndexQuery<T> FindKeys<T>(this IIndexScanner<T> scanner, IEnumerable keys)
        {
            return scanner.FindKeys(keys, Order.Unordered);
        }
        public static IndexQuery<T> FindKeys<T>(this IIndexScanner<T> scanner, params object[] keys)
        {
            return scanner.FindKeys(keys, Order.Unordered);
        }

        #endregion

        #region FindGreater

        public static IndexQuery<T, TKey> FindGreater<T, TKey>(this IIndexScanner<T, TKey> indexScanner, TKey key)
        {
            return FindGreater(indexScanner, key, true);
        }
        public static IndexQuery<T, TKey> FindGreater<T, TKey>(this IIndexScanner<T, TKey> indexScanner, TKey key, bool inclusive)
        {
            return FindGreater(indexScanner, key, inclusive, null);
        }
        public static IndexQuery<T, TKey> FindGreater<T, TKey>(this IIndexScanner<T, TKey> indexScanner, TKey key, bool inclusive, Func<TKey, bool> keySelector)
        {
            return indexScanner.FindGreater(key, inclusive, keySelector, Order.Unordered);
        }

        #endregion

        #region FindLess

        public static IndexQuery<T, TKey> FindLess<T, TKey>(this IIndexScanner<T, TKey> indexScanner, TKey key)
        {
            return FindLess(indexScanner, key, true);
        }
        public static IndexQuery<T, TKey> FindLess<T, TKey>(this IIndexScanner<T, TKey> indexScanner, TKey key, bool inclusive)
        {
            return FindLess(indexScanner, key, inclusive, null);
        }
        public static IndexQuery<T, TKey> FindLess<T, TKey>(this IIndexScanner<T, TKey> indexScanner, TKey key, bool inclusive, Func<TKey, bool> keyPredicate)
        {
            return indexScanner.FindLess(key, inclusive, keyPredicate, Order.Unordered);
        }

        #endregion

        #region FindBetween

        public static IndexQuery<T, TKey> FindBetween<T, TKey>(this IIndexScanner<T, TKey> indexScanner, TKey min, TKey max)
        {
            return FindBetween(indexScanner, min, true, max, true);
        }
        public static IndexQuery<T, TKey> FindBetween<T, TKey>(this IIndexScanner<T, TKey> indexScanner, TKey min, bool minInclusive, TKey max, bool maxInclusive)
        {
            return FindBetween(indexScanner, min, minInclusive, max, maxInclusive, null);
        }
        public static IndexQuery<T, TKey> FindBetween<T, TKey>(this IIndexScanner<T, TKey> indexScanner, TKey min, bool minInclusive, TKey max, bool maxInclusive, Func<TKey, bool> keyPredicate)
        {
            return indexScanner.FindBetween(min, minInclusive, max, maxInclusive, keyPredicate, Order.Unordered);
        }

        public static IndexQuery<T, string> FindStartingWith<T>(this IIndexScanner<T> indexScanner, string value)
        {
            return FindStartingWith(indexScanner, value, Order.Unordered);
        }
        public static IndexQuery<T, string> FindStartingWith<T>(this IIndexScanner<T> indexScanner, string value, Order order)
        {
            return indexScanner.FindStartingWith(value, null, order);
        }
        #endregion

        #endregion

        #region GroupBy

        public static GroupingQuery<TKey, T> GroupByKey<TKey, T>(this IIndexScanner<T, TKey> scanner)
        {
            return GroupByKey(scanner, Order.Unordered);
        }
        public static GroupingQuery<TKey, T> GroupByKey<TKey, T>(this IIndexScanner<T, TKey> scanner, Order order)
        {
            return scanner.All(order).GroupByKey();
        }

        public static GroupingQuery<T> GroupByUntypedKey<T>(this IIndexScanner<T> scanner)
        {
            return GroupByUntypedKey(scanner, Order.Unordered);
        }
        public static GroupingQuery<T> GroupByUntypedKey<T>(this IIndexScanner<T> scanner, Order order)
        {
            return scanner.All(order).GroupByUntypedKey();
        }

        #endregion

        #region Join
        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IIndexScanner<TOuter, TKey> outer,
                                                                        IIndexScanner<TInner, TKey> inner,
                                                                        Func<TOuter, TInner, TResult> resultSelector)
        {
            return outer.Join(inner, resultSelector, JoinOperator.Equal);
        }

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IIndexScanner<TOuter, TKey> outer,
                                                                IEnumerable<TInner> inner,
                                                                Func<TInner, TKey> innerKeySelector,
                                                                Func<TOuter, TInner, TResult> resultSelector)
        {
            return outer.Join(inner, innerKeySelector, resultSelector, JoinOperator.Equal);
        }
        #endregion
    }
}