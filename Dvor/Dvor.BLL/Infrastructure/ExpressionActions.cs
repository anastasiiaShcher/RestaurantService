using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Dvor.BLL.Infrastructure
{
    public static class ExpressionActions
    {
        public static Expression<Func<T, bool>> CombinePredicates<T>(
            IList<Expression<Func<T, bool>>> predicateExpressions,
            Func<Expression, Expression, BinaryExpression> logicalFunction)
        {
            Expression<Func<T, bool>> filter = null;

            if (predicateExpressions.Count <= 0)
            {
                return filter;
            }

            var firstPredicate = predicateExpressions[0];
            var body = firstPredicate.Body;

            for (var i = 1; i < predicateExpressions.Count; i++)
            {
                body = logicalFunction(body, Expression.Invoke(predicateExpressions[i], firstPredicate.Parameters));
            }

            filter = Expression.Lambda<Func<T, bool>>(body, firstPredicate.Parameters);

            return filter;
        }
    }
}