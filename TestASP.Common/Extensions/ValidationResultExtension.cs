using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;


namespace TestASP.Common.Extensions;
public static class ValidationResultExtension
{
    /// <summary>
    /// default errorMessage = {0} is required
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProp"></typeparam>
    /// <param name="data"></param>
    /// <param name="getProp"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public static ValidationResult RequiredFor<T,TProp>(this T data, Expression<Func<T,TProp>> getProp, string? errorMessage = null)
    {
        if (getProp.Compile().Invoke(data) == null)
        {
            return new ValidationResult(errorMessage ?? $"{getProp.GetProperty()} is required", new []{ getProp.GetProperty()});
        }
        return null;
    }

    public static bool TryRequiredFor<T,TProp>(this T data, Expression<Func<T,TProp>> getProp, out ValidationResult result, string? errorMessage = null)
    {
        if (getProp.Compile().Invoke(data) == null)
        {
            result =new ValidationResult(errorMessage ?? $"{getProp.GetProperty()} is required", new []{ getProp.GetProperty()});
        }
        result = null;
        return false;
    }
}
