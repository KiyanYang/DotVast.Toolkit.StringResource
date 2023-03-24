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
    /// For example: "public static {0} => "{0}".GetLocalized();"
    /// <para>
    /// Generator: public static ReswKey => "ReswKey".GetLocalized();
    /// </para>
    /// </summary>
    public string PropertyFormat { get; }

    /// <summary>
    /// For example: "using App.Extensions;"
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

    public const string PublicStatic = "public static string {0} => \"{0}\";";
}
