using System;

namespace DotVast.Toolkit.StringResource;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class StringResourceAttribute : Attribute
{
    /// <summary>
    /// For example: "../Strings/Resources.resw"
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// For example: "GetLocalized()"
    /// <para>
    /// Generator : public static ReswKey => "ReswKey".GetLocalized();
    /// </para>
    /// </summary>
    public string? ExtensionMethod { get; }

    /// <summary>
    /// For example: "App.Extensions"
    /// <para>
    /// Generator : using App.Extensions;
    /// </para>
    /// </summary>
    public string? ExtensionMethodNamespace { get; }


    public StringResourceAttribute(string path)
    {
        Path = path;
    }

    public StringResourceAttribute(string path, string extensionMethod)
    {
        Path = path;
        ExtensionMethod = extensionMethod;
    }

    public StringResourceAttribute(string path, string extensionMethod, string extensionMethodNamespace)
    {
        Path = path;
        ExtensionMethod = extensionMethod;
        ExtensionMethodNamespace = extensionMethodNamespace;
    }
}
