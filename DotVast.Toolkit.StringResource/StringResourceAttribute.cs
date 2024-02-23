using System;

namespace DotVast.Toolkit.StringResource;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class StringResourceAttribute : Attribute
{
    /// <summary>
    /// For example: <code>../Strings/Resources.resw</code>
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// For example: <code>public static {0} => "{0}".GetLocalized();</code>
    /// <para>
    /// Generator: <code>public static ReswKey => "ReswKey".GetLocalized();</code>
    /// </para>
    /// </summary>
    public string PropertyFormat { get; }

    /// <summary>
    /// For example: <code>using App.Extensions;</code>
    /// </summary>
    public string? UsingsFormat { get; }

    public StringResourceAttribute(string path, string propertyFormat)
    {
        Path = path;
        PropertyFormat = propertyFormat;
    }

    public StringResourceAttribute(string path, string propertyFormat, string usingsFormat)
    {
        Path = path;
        PropertyFormat = propertyFormat;
        UsingsFormat = usingsFormat;
    }

    public const string PublicStatic = """public static string {0} => "{0}";""";
}
