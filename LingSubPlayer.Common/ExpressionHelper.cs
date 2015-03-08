using System;
using System.CodeDom;
using System.Linq.Expressions;
using System.Reflection;

namespace LingSubPlayer.Common
{
    public static class ExpressionHelper
    {
        public static string GetPropertyOrMethodName<T, TResult>(this Expression<Func<T, TResult>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            var methodCallExpression = expression.Body as MethodCallExpression;

            if (memberExpression == null && methodCallExpression == null)
            {
                throw new NotSupportedException("Only direct access to property or method is supported: c => c.Property or c => c.Method() [1]");
            }

            var property = memberExpression != null ? memberExpression.Member as PropertyInfo : null;
            var method = methodCallExpression != null ? methodCallExpression.Method : null;
            
            return property != null ? property.Name : (method != null ? method.Name : null);
        }
    }
}
