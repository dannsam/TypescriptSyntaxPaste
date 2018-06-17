using System;
using Xunit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using TypescriptSyntaxPaste;

namespace TypescriptConverter.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //var allCFiles = DirSearch(@"D:\Projects\stateless\");
            var allCFiles = Directory.GetFiles(@"D:\Projects\stateless\", "*.*", SearchOption.AllDirectories)
                .Where(s => !s.EndsWith(".Designer.cs") && Path.GetExtension(s) == ".cs");

            foreach (var f in allCFiles) {
                var cScript = File.ReadAllText(f);
                CSharpToTypescriptConverter csharpToTypescriptConverter = new CSharpToTypescriptConverter();
                var typescript = csharpToTypescriptConverter.ConvertToTypescript(cScript, new TestSettingsStore());
                var path = f.Replace(@"D:\Projects\stateless\", @"D:\Projects\stateless-js\generated\");
                path = Path.ChangeExtension(path, ".ts");
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                File.WriteAllText(path, typescript);
            }
        }

        static IEnumerable<string> DirSearch(string sDir)
        {
            foreach (string d in Directory.GetDirectories(sDir)) {
                foreach (string f in Directory.GetFiles(d, "*.cs")) {
                    yield return f;
                }
                DirSearch(d);
            }
        }
    }

    public class TestSettingsStore : ISettingStore
    {
        public bool AddIPrefixInterfaceDeclaration => false;
        public bool IsConvertListToArray => true;
        public bool IsConvertMemberToCamelCase => false;
        public bool IsConvertToInterface => false;
        public bool IsInterfaceOptionalProperties => false;
    }
}
