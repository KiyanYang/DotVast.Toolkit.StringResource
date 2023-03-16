using System;

namespace DotVast.Toolkit.StringResource.SourceGenerators;

public record struct StringResourceInfo(
    string ReswPath,
    DateTime ReswLastWriteTime,
    string ClassName,
    string? NamespaceName,
    string? ExtensionMethodName,
    string? ExtensionNamespaceName);
