using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class ThrowExpressionTranslation : ExpressionTranslation
    {
        public new ThrowExpressionSyntax Syntax
        {
            get { return (ThrowExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public ThrowExpressionTranslation(ThrowExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Expression = syntax.Expression.Get<ExpressionTranslation>(this);
        }

        public ExpressionTranslation Expression { get; set; }

        protected override string InnerTranslate()
        {
            var err = "err";
            // try to find exception variable from catch clause
            if (Expression == null) {
                var tokenText = this.Syntax.Ancestors().OfType<CatchClauseSyntax>().FirstOrDefault()?.Declaration?.Identifier.ValueText;

                err = tokenText ?? err;

            } else {
                err = Expression.Translate();
            }

            return $"throw {err};";
        }
    }
}
