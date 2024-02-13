using System.Diagnostics.CodeAnalysis;

namespace Coding.Blog.Utilities;

[ExcludeFromCodeCoverage]
internal static class SyntaxHighlighting
{
    public const string CustomCss = """
                                    <style>
                                        .background {
                                          font-family: monaco, Consolas, LucidaConsole, monospace;
                                          background-color: #1e1e1e;
                                        }
                                        .numeric {
                                          color: #b5cea8;
                                        }
                                        .method {
                                          color: #dcdcaa;
                                        }
                                        .class {
                                          color: #4ec9b0;
                                        }
                                        .keyword {
                                          color: #569cd6;
                                        }
                                        .string {
                                          color: #ce9178;
                                        }
                                        .interface {
                                          color: #b8d7a3;
                                        }
                                        .enumName {
                                          color: #b8d7a3;
                                        }
                                        .numericLiteral {
                                          color: #b8d7a3;
                                        }
                                        .recordStruct {
                                          color: #b8d7a3;
                                        }
                                        .typeParam {
                                          color: #b8d7a3;
                                        }
                                        .extension {
                                          color: #b8d7a3;
                                        }
                                        .control {
                                          color: #c586c0;
                                        }
                                        .internalError {
                                          color: #ff0d0d;
                                        }
                                        .comment {
                                          color: #6a9955;
                                        }
                                        .preprocessor {
                                          color: #808080;
                                        }
                                        .preprocessorText {
                                          color: #a4a4a4;
                                        }
                                        .struct {
                                          color: #86c691;
                                        }
                                        .namespace {
                                          color: #dfdfdf;
                                        }
                                        .enumMember {
                                          color: #dfdfdf;
                                        }
                                        .identifier {
                                          color: #dfdfdf;
                                        }
                                        .punctuation {
                                          color: #dfdfdf;
                                        }
                                        .operator {
                                          color: #dfdfdf;
                                        }
                                        .propertyName {
                                          color: #dfdfdf;
                                        }
                                        .fieldName {
                                          color: #dfdfdf;
                                        }
                                        .labelName {
                                          color: #dfdfdf;
                                        }
                                        .operator_overloaded {
                                          color: #dfdfdf;
                                        }
                                        .constant {
                                          color: #dfdfdf;
                                        }
                                        .localName {
                                          color: #9cdcfe;
                                        }
                                        .parameter {
                                          color: #9cdcfe;
                                        }
                                        .delegate {
                                          color: #4ec9b0;
                                        }
                                        .eventName {
                                          color: #dfdfdf;
                                        }
                                        .excludedCode {
                                          color: #808080;
                                        }
                                    </style>
                                    """;
}
