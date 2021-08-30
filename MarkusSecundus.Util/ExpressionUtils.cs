using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    /// <summary>
    /// Static class with utility functions that should have been in standard library.
    /// </summary>
    public static class ExpressionUtils
    {

        /// <summary>
        /// Creates a block that asserts certain expression is null and if it is calls provided action.
        /// </summary>
        /// <param name="body">The subexpression to be checked for null.</param>
        /// <param name="onNull">Action to be called when value is null. Should always throw an exception.</param>
        /// <returns>Expression with added null-check.</returns>
        public static Expression AssertNotNull(Expression body, Action onNull)
        {
            Func<object> subst = () => { onNull(); return null; };
            return Expression.Condition(
                        Expression.Equal(body, Expression.Constant(null)),
                        Expression.Convert(Expression.Invoke(Expression.Constant(subst)), body.Type),
                        body
                    );
        }
    }
}
