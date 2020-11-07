using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace h73.Elastic.TypeMapping
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, T2>> PropertyName<T, T2>(this List<T> obj, Expression<Func<T, T2>> expression)
        {
            return expression;
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <typeparam name="T2">The type of the T2.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>PropertyName</returns>
        public static string GetPropertyName<T, T2>(Expression<Func<T, T2>> expression)
        {
            return GetMemberName(expression.Body);
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>PropertyName</returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression.Body);
        }

        private static string GetMemberName(Expression expression)
        {
            switch (expression)
            {
                case null:
                    throw new ArgumentException("Expression can not be null");
                case MemberExpression memberExpression:
                    var memberExpressionList = memberExpression.ToString().Split('.').ToList();
                    return string.Join(".", memberExpressionList.GetRange(1, memberExpressionList.Count - 1));
                case MethodCallExpression methodCallExpression:
                    if (methodCallExpression.Method.Name == "PropertyName")
                    {
                        return string.Join(".", methodCallExpression.Arguments.Select(GetMemberName));
                    }

                    return methodCallExpression.Method.Name;
                case UnaryExpression unaryExpression:
                    return GetMemberName(unaryExpression);
                case ConditionalExpression conditionalExpression:
                    return GetMemberName(conditionalExpression.IfTrue);
            }

            throw new ArgumentException("Invalid expression");
        }

        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression methodExpression)
            {
                return methodExpression.Method.Name;
            }

            var operandList = (unaryExpression.Operand).ToString().Split('.').ToList();
            return string.Join(".", operandList.GetRange(1, operandList.Count - 1));
        }
    }
}