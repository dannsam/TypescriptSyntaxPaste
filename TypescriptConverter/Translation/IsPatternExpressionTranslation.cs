using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class IsPatternExpressionTranslation : ExpressionTranslation
    {
        public new IsPatternExpressionSyntax Syntax
        {
            get { return (IsPatternExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public IsPatternExpressionTranslation() { }
        public IsPatternExpressionTranslation(IsPatternExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
        }

        public CSharpSyntaxTranslation Body { get; set; }
        public ParameterTranslation Parameter { get; set; }

        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
