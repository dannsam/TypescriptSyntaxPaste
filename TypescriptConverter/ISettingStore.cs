// using TypescriptSyntaxPaste.VSIX;

namespace TypescriptSyntaxPaste
{
    public interface ISettingStore
    {
        bool AddIPrefixInterfaceDeclaration { get; }
        bool IsConvertListToArray { get; }
        bool IsConvertMemberToCamelCase { get; }
        bool IsConvertToInterface { get; }
        bool IsInterfaceOptionalProperties { get; }
        // TypeNameReplacementData[] ReplacedTypeNameArray { get; set; }
    }
}