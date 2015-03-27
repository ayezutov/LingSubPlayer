using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LingSubPlayer.Common
{
    public static class ReflectionHelpers
    {
        public static string GetPropertyOrMethodName<T, TResult>(this Expression<Func<T, TResult>> expression)
        {
            var body = expression.Body;

            if (body.NodeType == ExpressionType.Convert)
            {
                var unary = body as UnaryExpression;
                if (unary != null)
                {
                    body = unary.Operand;
                }
            }

            var memberExpression = body as MemberExpression;
            var methodCallExpression = body as MethodCallExpression;

            if (memberExpression == null && methodCallExpression == null)
            {
                throw new NotSupportedException("Only direct access to property or method is supported: c => c.Property or c => c.Method() [1]");
            }

            var property = memberExpression != null ? memberExpression.Member as PropertyInfo : null;
            var method = methodCallExpression != null ? methodCallExpression.Method : null;
            
            return property != null ? property.Name : (method != null ? method.Name : null);
        }

        public static bool IsAssignableTo(this Type initial, Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            if (initial == type)
            {
                return true;
            }

            if (!type.ContainsGenericParameters)
            {
                return type.IsAssignableFrom(initial);
            }

            if (!type.IsInterface)
            {
                var currentType = initial;
                while (true)
                {
                    currentType = currentType.BaseType;

                    if (currentType == null)
                    {
                        return false;
                    }

                    if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == type)
                    {
                        return true;
                    }
                }
            }

            var interfaces = initial.GetInterfaces();

            return interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type);
        }
    }
}
