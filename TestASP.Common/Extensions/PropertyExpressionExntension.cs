using System.Linq.Expressions;

namespace TestASP.Common.Extensions;
public static class PropertyExpressionExntension
{
    public static string GetProperty(this Expression expression)
    {
        switch (expression)
        {
            case MemberExpression memberExpression:
                return memberExpression.Member.Name;
            case LambdaExpression lambdaExpression:
                return GetProperty(lambdaExpression.Body);
        }
        return null;
    }
}