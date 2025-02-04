using System.Linq.Expressions;

namespace PaymentGateway.Domain.Helpers;
public static class ExpressionExtensions
{
    /// <summary>
    /// Returns name of the member of object of type <typeparamref name="TMember"/> described by the given expression.
    /// </summary>
    /// <typeparam name="TSource">The source type of the member.</typeparam>
    /// <typeparam name="TMember">The type of the member.</typeparam>
    /// <param name="expression">The expression that describes member.</param>
    /// <returns>The name of the member.</returns>
    /// <exception cref="ArgumentException">Invalid <paramref name="expression"/> expression.</exception>
    public static string GetMemberName<TSource, TMember>(this Expression<Func<TSource, TMember>> expression)
    {
        var memberExpression = expression.Body as MemberExpression;
        if (memberExpression == null)
        {
            throw new ArgumentException($"Only objects of type '{nameof(MemberExpression)}' allowed");
        }

        return memberExpression.Member.Name;
    }
}

