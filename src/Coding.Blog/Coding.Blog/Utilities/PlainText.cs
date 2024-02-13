using ColorCode;
using ColorCode.Common;

namespace Coding.Blog.Utilities;

internal sealed class PlainText : ILanguage
{
    public static readonly string LanguageId = "plaintext";

    public string Id => LanguageId;

    public string Name => "TEXT";

    public string CssClassName => "text";

    public string? FirstLinePattern => null;

    public IList<LanguageRule> Rules => new List<LanguageRule>
    {
        new(".*", new Dictionary<int, string> { { 0, ScopeName.PlainText }, })
    };

    public bool HasAlias(string lang)
    {
        return false;
    }

    public override string ToString()
    {
        return Name;
    }
}
