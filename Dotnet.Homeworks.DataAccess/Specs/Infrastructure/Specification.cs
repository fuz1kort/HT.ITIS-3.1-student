using System.Linq.Expressions;

namespace Dotnet.Homeworks.DataAccess.Specs.Infrastructure;

public class Specification<T> : IQueryableFilter<T> where T : class
{
    private readonly Expression<Func<T, bool>> _expression;

    public Specification(Expression<Func<T, bool>> expression)
    {
        _expression = expression;
    }

    public IQueryable<T> Apply(IQueryable<T> query)
    {
        return query.Where(_expression);
    }

    public static Specification<T> operator &(Specification<T> left, Specification<T> right)
    {
        var parameter = ParameterExpression(left, right, out var leftExpression, out var rightExpression);

        var binaryExpression = Expression.AndAlso(leftExpression, rightExpression);
        var expression = Expression.Lambda<Func<T, bool>>(binaryExpression, parameter);

        return new Specification<T>(expression);
    }
    
    public static Specification<T> operator |(Specification<T> left, Specification<T> right)
    {
        var parameter = ParameterExpression(left, right, out var leftExpression, out var rightExpression);

        var binaryExpression = Expression.OrElse(leftExpression, rightExpression);
        var expression = Expression.Lambda<Func<T, bool>>(binaryExpression, parameter);

        return new Specification<T>(expression);
    }

    public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
    {
        return specification._expression;
    }
    
    private static ParameterExpression ParameterExpression(Specification<T> left, Specification<T> right,
        out Expression leftExpression, out Expression rightExpression)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        
        var leftVisitor = new ReplaceParameterVisitor(left._expression.Parameters[0], parameter);
        leftExpression = leftVisitor.Visit(left._expression.Body);
        
        var rightVisitor = new ReplaceParameterVisitor(right._expression.Parameters[0], parameter);
        rightExpression = rightVisitor.Visit(right._expression.Body);
        return parameter;
    }
}