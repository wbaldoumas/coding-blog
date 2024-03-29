﻿using System.Text.Json.Serialization;

namespace Coding.Blog.DataTransfer;

internal sealed record CosmicPost(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("slug")] string Slug,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("created_at")] DateTime DatePublished,
    [property: JsonPropertyName("metadata")] CosmicPostMetadata Metadata
);
