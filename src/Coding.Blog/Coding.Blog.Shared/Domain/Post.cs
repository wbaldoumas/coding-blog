﻿namespace Coding.Blog.Shared.Domain;

public sealed record Post(
    string Id,
    string Slug,
    string Title,
    string Content,
    TimeSpan ReadingTime,
    DateTime DatePublished,
    IEnumerable<string> Tags,
    Hero Hero,
    Post? Next = null,
    Post? Previous = null
);