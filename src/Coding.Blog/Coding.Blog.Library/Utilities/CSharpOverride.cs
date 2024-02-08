using System.Diagnostics.CodeAnalysis;
using ColorCode;
using ColorCode.Common;
using System.Globalization;

namespace Coding.Blog.Library.Utilities;

/// <summary>
///     Overrides the built-in C# language definition that ships with ColorCode-Universal. This augmented
///     version adds support for differentiated syntax highlighting for classes and control keywords.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class CSharpOverride : ILanguage
{
    public string Id => LanguageId.CSharp;

    public string Name => "C#";

    public string CssClassName => "csharp";

    public string? FirstLinePattern => null;

    public IList<LanguageRule> Rules =>
        new List<LanguageRule>
        {
            new(
                @"/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/",
                new Dictionary<int, string>
                {
                    { 0, ScopeName.Comment }
                }),
            new(
                @"(///)(?:\s*?(<[/a-zA-Z0-9\s""=]+>))*([^\r\n]*)",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.XmlDocTag },
                    { 2, ScopeName.XmlDocTag },
                    { 3, ScopeName.XmlDocComment }
                }),
            new(
                @"(//.*?)\r?$",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Comment }
                }),
            new(
                @"'[^\n]*?(?<!\\)'",
                new Dictionary<int, string>
                {
                    { 0, ScopeName.String }
                }),
            new(
                @"(?s)@""(?:""""|.)*?""(?!"")",
                new Dictionary<int, string>
                {
                    { 0, ScopeName.StringCSharpVerbatim }
                }),
            new(
                @"(?s)(""[^\n]*?(?<!\\)"")",
                new Dictionary<int, string>
                {
                    { 0, ScopeName.String }
                }),
            new(
                @"\[(assembly|module|type|return|param|method|field|property|event):[^\]""]*(""[^\n]*?(?<!\\)"")?[^\]]*\]",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.String }
                }),
            new(
                @"^\s*(\#define|\#elif|\#else|\#endif|\#endregion|\#error|\#if|\#line|\#pragma|\#region|\#undef|\#warning).*?$",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.PreprocessorKeyword }
                }),
            new(
                @"\b(public|protected|internal|private)?\s*(delegate)\s+(void|int|string|bool|double|float|decimal|char|long|byte|sbyte|short|ushort|uint|ulong|object|dynamic)\s+([A-Za-z_][A-Za-z0-9_]*)\b",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.Keyword },
                    { 3, ScopeName.Keyword },
                    { 4, ScopeName.ClassName }
                }),
            new(
                @"\b(public|protected|internal|private)?\s*(delegate)\s+([A-Za-z_][A-Za-z0-9_]*(?!\b(void|int|string|bool|double|float|decimal|char|long|byte|sbyte|short|ushort|uint|ulong|object|dynamic)\b))\s+([A-Za-z_][A-Za-z0-9_]*)",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.Keyword },
                    { 3, ScopeName.ClassName },
                    { 5, ScopeName.ClassName }
                }),
            new(
                @"\b(public|protected|internal|private|sealed)?\s*(abstract\s+)?(static\s+)?(class|record|struct)\s+([A-Za-z_][A-Za-z0-9_]*)",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.Keyword },
                    { 3, ScopeName.Keyword },
                    { 4, ScopeName.Keyword },
                    { 5, ScopeName.ClassName }
                }),
            new(
                @"\b(new)\s+([A-Za-z_][A-Za-z0-9_]*)(?:<(\b(?:int|string|bool|double|float|decimal|char|long|byte|sbyte|short|ushort|uint|ulong|object|dynamic)\b),\s*([A-Za-z_][A-Za-z0-9_]*),\s*([A-Za-z_][A-Za-z0-9_]*))?>",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.ClassName },
                    { 3, ScopeName.Keyword },
                    { 4, ScopeName.ClassName },
                    { 5, ScopeName.Keyword }
                }),
            new(
                @"\b(new)\s+([A-Za-z_][A-Za-z0-9_]*)(?:<([A-Za-z_][A-Za-z0-9_]*),\s*(\b(?:int|string|bool|double|float|decimal|char|long|byte|sbyte|short|ushort|uint|ulong|object|dynamic)\b),\s*([A-Za-z_][A-Za-z0-9_]*))?>",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.ClassName },
                    { 3, ScopeName.ClassName },
                    { 4, ScopeName.Keyword },
                    { 5, ScopeName.ClassName }
                }),

            new(
                @"\b(new)\s+([A-Za-z_][A-Za-z0-9_]*)(?:<([A-Za-z_][A-Za-z0-9_]*),\s*(\b(?:int|string|bool|double|float|decimal|char|long|byte|sbyte|short|ushort|uint|ulong|object|dynamic)\b))?>",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.ClassName },
                    { 3, ScopeName.ClassName },
                    { 4, ScopeName.Keyword }
                }),
            new(
                @"\b(new)\s+([A-Za-z_][A-Za-z0-9_]*)(?:<(\b(?:int|string|bool|double|float|decimal|char|long|byte|sbyte|short|ushort|uint|ulong|object|dynamic)\b),\s*([A-Za-z_][A-Za-z0-9_]*))?>",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.ClassName },
                    { 3, ScopeName.Keyword },
                    { 4, ScopeName.ClassName }
                }),
            new(
                @"\b(new)\s+([A-Za-z_][A-Za-z0-9_]*)(?=\s*<)(?:<\s*(void|int|string|bool|double|float|decimal|char|long|byte|sbyte|short|ushort|uint|ulong|object|dynamic)\b)",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.ClassName },
                    { 3, ScopeName.Keyword }
                }),

            new(
                @"\b(new)\s+([A-Za-z_][A-Za-z0-9_]*)(?=\s*<)(?:<\s*([A-Za-z_][A-Za-z0-9_]*),\s*([A-Za-z_][A-Za-z0-9_]*),\s*([A-Za-z_][A-Za-z0-9_]*),\s*([A-Za-z_][A-Za-z0-9_]*),\s*([A-Za-z_][A-Za-z0-9_]*))",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.ClassName },
                    { 3, ScopeName.ClassName },
                    { 4, ScopeName.ClassName },
                    { 5, ScopeName.ClassName },
                    { 6, ScopeName.ClassName },
                    { 7, ScopeName.ClassName }
                }),
            new(
                @"\b(new)\s+([A-Za-z_][A-Za-z0-9_]*)(?=\s*<)(?:<\s*([A-Za-z_][A-Za-z0-9_]*),\s*([A-Za-z_][A-Za-z0-9_]*),\s*([A-Za-z_][A-Za-z0-9_]*),\s*([A-Za-z_][A-Za-z0-9_]*))",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.ClassName },
                    { 3, ScopeName.ClassName },
                    { 4, ScopeName.ClassName },
                    { 5, ScopeName.ClassName },
                    { 6, ScopeName.ClassName }
                }),
            new(
                @"\b(new)\s+([A-Za-z_][A-Za-z0-9_]*)(?=\s*<)(?:<\s*([A-Za-z_][A-Za-z0-9_]*),\s*([A-Za-z_][A-Za-z0-9_]*),\s*([A-Za-z_][A-Za-z0-9_]*))",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.ClassName },
                    { 3, ScopeName.ClassName },
                    { 4, ScopeName.ClassName },
                    { 5, ScopeName.ClassName }
                }),
            new(
                @"\b(new)\s+([A-Za-z_][A-Za-z0-9_]*)(?=\s*<)(?:<\s*([A-Za-z_][A-Za-z0-9_]*),\s*([A-Za-z_][A-Za-z0-9_]*))",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.ClassName },
                    { 3, ScopeName.ClassName },
                    { 4, ScopeName.ClassName }
                }),
            new(
                @"\b(new)\s+([A-Za-z_][A-Za-z0-9_]*)(?=\s*<)(?:<\s*([A-Za-z_][A-Za-z0-9_]*))",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword },
                    { 2, ScopeName.ClassName },
                    { 3, ScopeName.ClassName }
                }),
            new(
                @"new\s+([A-Za-z_][A-Za-z0-9_]*)",
                new Dictionary<int, string>
                {
                    { 0, ScopeName.Keyword },
                    { 1, ScopeName.ClassName }
                }),
            new(
                @"\b(break|case|catch|continue|do|else|finally|for|foreach|goto|if|in|return|switch|throw|try|while|yield)\b",
                new Dictionary<int, string>
                {
                    { 0, ScopeName.ControlKeyword }
                }),
            new(
                @"\b(abstract|as|ascending|base|bool|by|byte|char|checked|class|const|decimal|default|delegate|descending|double|dynamic|enum|equals|event|explicit|extern|false|fixed|float|from|get|group|implicit|int|into|interface|internal|is|join|let|lock|long|namespace|new|null|object|on|operator|orderby|out|override|params|partial|private|protected|public|readonly|ref|sbyte|sealed|select|set|short|sizeof|stackalloc|static|string|struct|this|throw|true|typeof|uint|ulong|unchecked|unsafe|ushort|using|var|virtual|void|volatile|where|async|await|warning|disable)\b",
                new Dictionary<int, string>
                {
                    { 1, ScopeName.Keyword }
                }),
            new(
                @"\b[0-9]{1,}\b",
                new Dictionary<int, string>
                {
                    { 0, ScopeName.Number }
                })
        };

    public bool HasAlias(string lang) => lang.ToLower(CultureInfo.InvariantCulture) switch
    {
        "cs" => true,
        "c#" => true,
        "csharp" => true,
        "cake" => true,
        _ => false
    };

    public override string ToString() => Name;
}
