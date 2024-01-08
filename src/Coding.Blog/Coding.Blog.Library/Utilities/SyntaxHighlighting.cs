using ColorCode.Common;
using ColorCode.Styling;

namespace Coding.Blog.Library.Utilities;

public static class SyntaxHighlighting
{
    public const string Blue = "#FF0000FF";
    public const string White = "#FFFFFFFF";
    public const string Black = "#FF000000";
    public const string DullRed = "#FFA31515";
    public const string Yellow = "#FFFFFF00";
    public const string Green = "#FF008000";
    public const string PowderBlue = "#FFB0E0E6";
    public const string Teal = "#FF008080";
    public const string Gray = "#FF808080";
    public const string Navy = "#FF000080";
    public const string OrangeRed = "#FFFF4500";
    public const string Purple = "#FF800080";
    public const string Red = "#FFFF0000";
    public const string MediumTurqoise = "FF48D1CC";
    public const string Magenta = "FFFF00FF";
    public const string OliveDrab = "#FF6B8E23";
    public const string DarkOliveGreen = "#FF556B2F";
    public const string DarkCyan = "#FF008B8B";
    public const string DarkOrange = "#FFFF8700";
    public const string BrightGreen = "#FF00d700";
    public const string BrightPurple = "#FFaf87ff";

    private const string VSDarkBackground = "#FF1E1E1E";
    private const string VSDarkPlainText = "#FFDADADA";
    private const string VSDarkXMLDelimeter = "#FF808080";
    private const string VSDarkXMLName = "#FF#E6E6E6";
    private const string VSDarkXMLAttribute = "#FF92CAF4";
    private const string VSDarkXAMLCData = "#FFC0D088";
    private const string VSDarkXMLComment = "#FF608B4E";
    private const string VSDarkComment = "#FF57A64A";
    private const string VSDarkKeyword = "#FF569CD6";
    private const string VSDarkControlKeyword = "#D8A0DF";
    private const string VSDarkGray = "#FF9B9B9B";
    private const string VSDarkNumber = "#FFB5CEA8";
    private const string VSDarkClass = "#FF4EC9B0";
    private const string VSDarkString = "#FFD69D85";

    public static StyleDictionary Dark =>
    [
        new Style(ScopeName.PlainText)
        {
            Foreground = VSDarkPlainText,
            Background = VSDarkBackground,
            ReferenceName = "plainText"
        },

        new Style(ScopeName.HtmlServerSideScript)
        {
            Background = Yellow,
            ReferenceName = "htmlServerSideScript"
        },

        new Style(ScopeName.HtmlComment)
        {
            Foreground = VSDarkComment,
            ReferenceName = "htmlComment"
        },

        new Style(ScopeName.HtmlTagDelimiter)
        {
            Foreground = VSDarkKeyword,
            ReferenceName = "htmlTagDelimiter"
        },

        new Style(ScopeName.HtmlElementName)
        {
            Foreground = DullRed,
            ReferenceName = "htmlElementName"
        },

        new Style(ScopeName.HtmlAttributeName)
        {
            Foreground = Red,
            ReferenceName = "htmlAttributeName"
        },

        new Style(ScopeName.HtmlAttributeValue)
        {
            Foreground = VSDarkKeyword,
            ReferenceName = "htmlAttributeValue"
        },

        new Style(ScopeName.HtmlOperator)
        {
            Foreground = VSDarkKeyword,
            ReferenceName = "htmlOperator"
        },

        new Style(ScopeName.Comment)
        {
            Foreground = VSDarkComment,
            ReferenceName = "comment"
        },

        new Style(ScopeName.XmlDocTag)
        {
            Foreground = VSDarkXMLComment,
            ReferenceName = "xmlDocTag"
        },

        new Style(ScopeName.XmlDocComment)
        {
            Foreground = VSDarkXMLComment,
            ReferenceName = "xmlDocComment"
        },

        new Style(ScopeName.String)
        {
            Foreground = VSDarkString,
            ReferenceName = "string"
        },

        new Style(ScopeName.StringCSharpVerbatim)
        {
            Foreground = VSDarkString,
            ReferenceName = "stringCSharpVerbatim"
        },

        new Style(ScopeName.Keyword)
        {
            Foreground = VSDarkKeyword,
            ReferenceName = "keyword"
        },

        new Style(ScopeName.PreprocessorKeyword)
        {
            Foreground = VSDarkKeyword,
            ReferenceName = "preprocessorKeyword"
        },

        new Style(ScopeName.HtmlEntity)
        {
            Foreground = Red,
            ReferenceName = "htmlEntity"
        },

        new Style(ScopeName.JsonKey)
        {
            Foreground = DarkOrange,
            ReferenceName = "jsonKey"
        },

        new Style(ScopeName.JsonString)
        {
            Foreground = DarkCyan,
            ReferenceName = "jsonString"
        },

        new Style(ScopeName.JsonNumber)
        {
            Foreground = BrightGreen,
            ReferenceName = "jsonNumber"
        },

        new Style(ScopeName.JsonConst)
        {
            Foreground = BrightPurple,
            ReferenceName = "jsonConst"
        },

        new Style(ScopeName.XmlAttribute)
        {
            Foreground = VSDarkXMLAttribute,
            ReferenceName = "xmlAttribute"
        },

        new Style(ScopeName.XmlAttributeQuotes)
        {
            Foreground = VSDarkKeyword,
            ReferenceName = "xmlAttributeQuotes"
        },

        new Style(ScopeName.XmlAttributeValue)
        {
            Foreground = VSDarkKeyword,
            ReferenceName = "xmlAttributeValue"
        },

        new Style(ScopeName.XmlCDataSection)
        {
            Foreground = VSDarkXAMLCData,
            ReferenceName = "xmlCDataSection"
        },

        new Style(ScopeName.XmlComment)
        {
            Foreground = VSDarkComment,
            ReferenceName = "xmlComment"
        },

        new Style(ScopeName.XmlDelimiter)
        {
            Foreground = VSDarkXMLDelimeter,
            ReferenceName = "xmlDelimiter"
        },

        new Style(ScopeName.XmlName)
        {
            Foreground = VSDarkXMLName,
            ReferenceName = "xmlName"
        },

        new Style(ScopeName.ClassName)
        {
            Foreground = VSDarkClass,
            ReferenceName = "className"
        },

        new Style(ScopeName.CssSelector)
        {
            Foreground = DullRed,
            ReferenceName = "cssSelector"
        },

        new Style(ScopeName.CssPropertyName)
        {
            Foreground = Red,
            ReferenceName = "cssPropertyName"
        },

        new Style(ScopeName.CssPropertyValue)
        {
            Foreground = VSDarkKeyword,
            ReferenceName = "cssPropertyValue"
        },

        new Style(ScopeName.SqlSystemFunction)
        {
            Foreground = Magenta,
            ReferenceName = "sqlSystemFunction"
        },

        new Style(ScopeName.PowerShellAttribute)
        {
            Foreground = PowderBlue,
            ReferenceName = "powershellAttribute"
        },

        new Style(ScopeName.PowerShellOperator)
        {
            Foreground = VSDarkGray,
            ReferenceName = "powershellOperator"
        },

        new Style(ScopeName.PowerShellType)
        {
            Foreground = Teal,
            ReferenceName = "powershellType"
        },

        new Style(ScopeName.PowerShellVariable)
        {
            Foreground = OrangeRed,
            ReferenceName = "powershellVariable"
        },

        new Style(ScopeName.PowerShellCommand)
        {
            Foreground = Yellow,
            ReferenceName = "powershellCommand"
        },

        new Style(ScopeName.PowerShellParameter)
        {
            Foreground = VSDarkGray,
            ReferenceName = "powershellParameter"
        },


        new Style(ScopeName.Type)
        {
            Foreground = Teal,
            ReferenceName = "type"
        },

        new Style(ScopeName.TypeVariable)
        {
            Foreground = Teal,
            Italic = true,
            ReferenceName = "typeVariable"
        },

        new Style(ScopeName.NameSpace)
        {
            Foreground = Navy,
            ReferenceName = "namespace"
        },

        new Style(ScopeName.Constructor)
        {
            Foreground = Purple,
            ReferenceName = "constructor"
        },

        new Style(ScopeName.Predefined)
        {
            Foreground = Navy,
            ReferenceName = "predefined"
        },

        new Style(ScopeName.PseudoKeyword)
        {
            Foreground = Navy,
            ReferenceName = "pseudoKeyword"
        },

        new Style(ScopeName.StringEscape)
        {
            Foreground = VSDarkGray,
            ReferenceName = "stringEscape"
        },

        new Style(ScopeName.ControlKeyword)
        {
            Foreground = VSDarkControlKeyword,
            ReferenceName = "controlKeyword"
        },

        new Style(ScopeName.Number)
        {
            ReferenceName = "number",
            Foreground = VSDarkNumber
        },

        new Style(ScopeName.Operator)
        {
            ReferenceName = "operator"
        },

        new Style(ScopeName.Delimiter)
        {
            ReferenceName = "delimiter"
        },


        new Style(ScopeName.MarkdownHeader)
        {
            Foreground = VSDarkKeyword,
            Bold = true,
            ReferenceName = "markdownHeader"
        },

        new Style(ScopeName.MarkdownCode)
        {
            Foreground = VSDarkString,
            ReferenceName = "markdownCode"
        },

        new Style(ScopeName.MarkdownListItem)
        {
            Bold = true,
            ReferenceName = "markdownListItem"
        },

        new Style(ScopeName.MarkdownEmph)
        {
            Italic = true,
            ReferenceName = "italic"
        },

        new Style(ScopeName.MarkdownBold)
        {
            Bold = true,
            ReferenceName = "bold"
        },

        new Style(ScopeName.BuiltinFunction)
        {
            Foreground = OliveDrab,
            Bold = true,
            ReferenceName = "builtinFunction"
        },

        new Style(ScopeName.BuiltinValue)
        {
            Foreground = DarkOliveGreen,
            Bold = true,
            ReferenceName = "builtinValue"
        },

        new Style(ScopeName.Attribute)
        {
            Foreground = DarkCyan,
            Italic = true,
            ReferenceName = "attribute"
        },

        new Style(ScopeName.SpecialCharacter)
        {
            ReferenceName = "specialChar"
        }
    ];
}
