using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.SaldoDevedor.WinApp.Classes
{
    public static class Extensions
    {
        public static IOrderedEnumerable<TSource> OrderByWithDirection<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool descending)
        {
            return descending ? source.OrderByDescending(keySelector)
                              : source.OrderBy(keySelector);
        }

        public static IOrderedQueryable<TSource> OrderByWithDirection<TSource, TKey>(this IQueryable<TSource> source, System.Linq.Expressions.Expression<Func<TSource, TKey>> keySelector, bool descending)
        {
            return descending ? source.OrderByDescending(keySelector)
                              : source.OrderBy(keySelector);
        }

    }
}
