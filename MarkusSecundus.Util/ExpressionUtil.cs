using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    public static class ExpressionUtil
    {
        public static Expression SubstituteThrow<TToCatch>(Expression body, Action<TToCatch> substituteSupplier)
        {
            var exceptionParam = Expression.Parameter(typeof(TToCatch), "#exception");
            Func<TToCatch, object> subst = e => { substituteSupplier(e); return null; };
            return Expression.TryCatch
                (
                    body,
                    Expression.MakeCatchBlock(
                        typeof(TToCatch),
                        exceptionParam,
                        Expression.Convert(Expression.Invoke(Expression.Constant(subst), exceptionParam), body.Type),
                        null
                    )
                );
        }
    }
}
