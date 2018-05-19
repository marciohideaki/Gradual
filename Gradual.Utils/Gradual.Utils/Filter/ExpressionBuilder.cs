using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Utils.Filter
{
    public static class ExpressionBuilder
    {
        private static System.Reflection.MethodInfo containsMethod = typeof(string).GetMethod("Contains");
        private static System.Reflection.MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static System.Reflection.MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });


        public static System.Linq.Expressions.Expression<Func<T, bool>> GetExpression<T>(IList<Filter> filters)
        {
            if (filters.Count == 0)
                return null;

            System.Linq.Expressions.ParameterExpression param = System.Linq.Expressions.Expression.Parameter(typeof(T), "t");
            System.Linq.Expressions.Expression exp = null;

            foreach(Filter lFilter in filters)
            {
                if (exp == null)
                {
                    exp = GetExpression<T>(param, lFilter);
                }
                else
                {
                    exp = System.Linq.Expressions.Expression.AndAlso(exp, GetExpression<T>(param, lFilter));
                    //exp = System.Linq.Expressions.Expression.Or(exp, GetExpression<T>(param, lFilter));
                }

                if (filters.Count == 1)
                {
                    exp = System.Linq.Expressions.Expression.AndAlso(exp, GetExpression<T>(param, lFilter));
                    //exp = System.Linq.Expressions.Expression.Or(exp, GetExpression<T>(param, lFilter));
                }
            }
            //}

            return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private static System.Linq.Expressions.Expression GetExpression<T>(System.Linq.Expressions.ParameterExpression param, Filter filter)
        {
            System.Linq.Expressions.MemberExpression member = System.Linq.Expressions.Expression.Property(param, filter.PropertyName);
            System.Linq.Expressions.ConstantExpression constant = System.Linq.Expressions.Expression.Constant(filter.Value);

            switch (filter.Operation)
            {
                case Operation.AndEquals:
                    return System.Linq.Expressions.Expression.Equal(member, constant);
                
                case Operation.OrEquals:
                    return System.Linq.Expressions.Expression.Equal(member, constant);

                case Operation.GreaterThan:
                    return System.Linq.Expressions.Expression.GreaterThan(member, constant);

                case Operation.GreaterThanOrEqual:
                    return System.Linq.Expressions.Expression.GreaterThanOrEqual(member, constant);

                case Operation.LessThan:
                    return System.Linq.Expressions.Expression.LessThan(member, constant);

                case Operation.LessThanOrEqual:
                    return System.Linq.Expressions.Expression.LessThanOrEqual(member, constant);

                case Operation.Contains:
                    return System.Linq.Expressions.Expression.Call(member, containsMethod, constant);

                case Operation.StartsWith:
                    return System.Linq.Expressions.Expression.Call(member, startsWithMethod, constant);

                case Operation.EndsWith:
                    return System.Linq.Expressions.Expression.Call(member, endsWithMethod, constant);
            }

            return null;
        }

        private static System.Linq.Expressions.BinaryExpression GetExpression<T> (System.Linq.Expressions.ParameterExpression param, Filter filter1, Filter filter2)
        {
            System.Linq.Expressions.Expression bin1 = GetExpression<T>(param, filter1);
            System.Linq.Expressions.Expression bin2 = GetExpression<T>(param, filter2);
            return System.Linq.Expressions.Expression.Or(bin1, bin2);
            return System.Linq.Expressions.Expression.AndAlso(bin1, bin2);
        }
    }
}
