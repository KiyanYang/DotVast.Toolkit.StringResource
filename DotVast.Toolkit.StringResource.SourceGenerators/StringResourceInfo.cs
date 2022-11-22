using System;

namespace DotVast.Toolkit.StringResource.SourceGenerators;

public record struct StringResourceInfo(
    string? Namespace,
    string Name,
    string ReswPath,
    DateTime ReswLastWriteTime,
    string? ExtensionMethod,
    string? ExtensionMethedNamespace);
