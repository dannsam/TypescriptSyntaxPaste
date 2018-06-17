using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace RoslynTypeScript.Translation
{
    public static class TrivialToJsDoc
    {
        public static string ConvertTriviaToJsDoc(this SyntaxNode node)
        {
            var result = node.GetLeadingTrivia().ToString();

            var trivia = node.GetLeadingTrivia().FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
            if (!trivia.IsKind(SyntaxKind.None)) {
                try {
                    return Convert(result);
                }
                catch (Exception ex) {
                    return result;
                }
            }

            return result;
        }

        static string Convert(string source)
        {
            var regex = new Regex(@"\/\/\/\s*");
            var xml = "<comment>" + Environment.NewLine;
            var lines = new List<string> { "/**" };

            // Convert original comment to simple XML string
            foreach (var line in source.Split(Environment.NewLine)) {
                if (string.IsNullOrWhiteSpace(line)) {
                    continue;
                }

                // Remove leading triple-slash
                xml += regex.Replace(line.Trim(), "") + Environment.NewLine;
            }

            xml += "</comment>";

            var doc = XDocument.Parse(xml);

            // See https://msdn.microsoft.com/en-us/library/b2s063f7.aspx

            // Get description(s)
            lines.AddRange(doc.Descendants("summary").SelectMany(x => x.Value.Split("\n", StringSplitOptions.RemoveEmptyEntries)).Select(x => $" * {x.Trim()}"));

            // Get remark(s)
            lines.AddRange(doc.Descendants("remark").SelectMany(x => x.Value.Split("\n", StringSplitOptions.RemoveEmptyEntries)).Select(x => $" * {x.Trim()}"));

            // Collect parameters
            lines.AddRange(doc.Descendants("param").Select(x => {
                var name = x.Attribute("name");
                if (string.IsNullOrEmpty(name.Value)) {
                    return x.ToString();
                }

                return $" * @param {name.Value} {x.Value.Replace("\n", "")}";
            }));

            // Collect return object
            foreach (var x in doc.Descendants("returns")) {
                var value = x.Value.Replace("\n", "");
                if (!string.IsNullOrWhiteSpace(value)) {
                    lines.Add($" * @return {value}");
                }
            }

            lines.Add(" */");

            return string.Concat(lines.Select(x=> x + Environment.NewLine));
        }
    }
}
