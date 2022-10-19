using System.IO;

using Microsoft.CodeAnalysis;

namespace DotVast.Toolkit.StringResource.SourceGenerators;

public sealed class StringResourceInfo
{
    public string Namespace
    {
        get;
    }

    public string Name
    {
        get;
    }

    public string ReswPath
    {
        get;
    }

    public string? ExMethed
    {
        get;
    }

    public string? ExMethedNamespace
    {
        get;
    }

    public StringResourceInfo(ITypeSymbol type, string path) : this(type, path, null, null)
    {
    }

    public StringResourceInfo(ITypeSymbol type, string path, string? exMethed = null, string? exMethedNamespace = null)
    {
        Namespace = type.ContainingNamespace.IsGlobalNamespace
            ? string.Empty
            : type.ContainingNamespace.ToDisplayString();
        Name = type.Name;
        ReswPath = Path.GetFullPath(path);
        ExMethed = exMethed;
        ExMethedNamespace = exMethedNamespace;
    }
}
