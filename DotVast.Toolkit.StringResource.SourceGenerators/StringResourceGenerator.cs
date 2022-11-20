using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotVast.Toolkit.StringResource.SourceGenerators;

[Generator(LanguageNames.CSharp)]
public sealed class StringResourceGenerator : IIncrementalGenerator
{
    private static readonly AssemblyName assemblyName = typeof(StringResourceGenerator).Assembly.GetName();
    private static readonly string GeneratedCodeAttribute = $"""
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("{assemblyName.Name}", "{assemblyName.Version}")]
        """;
    private const string ExcludeFromCodeCoverageAttribute = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";

    private const string TargetAttributeName = "DotVast.Toolkit.StringResource.StringResourceAttribute";
    private const string TargetAttributeQualifiedName = "global::DotVast.Toolkit.StringResource.StringResourceAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<StringResourceInfo> infos = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                TargetAttributeName,
                static (node, _) => node is ClassDeclarationSyntax n,
                static (context, _) =>
                {
                    if (context.TargetSymbol is not ITypeSymbol type
                        || GetAttributeCtorArgsValue(context.Attributes, TargetAttributeQualifiedName) is not string?[] args)
                    {
                        return default;
                    }

                    // args = [Path, ExtensionMethod, ExtensionMethodNamespace]
                    var basePath = context.TargetNode.SyntaxTree.FilePath;
                    var fullPath = Path.GetFullPath(Path.Combine(basePath, args[0]));
                    if (!fullPath.EndsWith(".resw") || !File.Exists(fullPath))
                    {
                        return default;
                    }

                    var classNamespace = type.ContainingNamespace.IsGlobalNamespace
                        ? string.Empty
                        : type.ContainingNamespace.ToDisplayString();
                    var className = type.Name;

                    return new StringResourceInfo(classNamespace, className, File.ReadAllText(fullPath), args[1], args[2]);
                })
            .Where(static i => i != default);

        context.RegisterSourceOutput(infos, GenerateCode);
    }

    private static string?[]? GetAttributeCtorArgsValue(ImmutableArray<AttributeData> attributeData, string attributeQualifiedName) =>
        attributeData.FirstOrDefault(a => a.AttributeClass?.HasFullyQualifiedName(attributeQualifiedName) == true)
            ?.ConstructorArguments.Select(ca => ca.Value?.ToString()).ToArray();

    private static void GenerateCode(SourceProductionContext context, StringResourceInfo info)
    {
        var extension = info.ExtensionMethod != null ? $".{info.ExtensionMethod}" : string.Empty;
        var sb = new StringBuilder();

        sb.AppendLine("""
            // <auto-generated/>
            #pragma warning disable
            #nullable enable
            
            """);

        if (info.ExtensionMethedNamespace != null)
            sb.AppendLine($"""
            using {info.ExtensionMethedNamespace};

            """);

        sb.Append($$"""
            namespace {{info.Namespace}}
            {
                partial class {{info.Name}}
                {
            """);

        foreach (var item in XElement.Parse(info.Resw).Elements("data"))
        {
            if (item.Attribute("name")?.Value is not string name || name.Contains("."))
                continue;

            if (item.Element("value")?.Value is string value)
                sb.AppendLine($"""

                    /// <value>
                    /// <![CDATA[{value}]]>
                    /// </value>
            """);

            if (item.Element("comment")?.Value is string comment)
                sb.AppendLine($"""
                    /// <remarks>
                    /// <![CDATA[{comment}]]>
                    /// </remarks>
            """);

            sb.AppendLine($"""
                    {GeneratedCodeAttribute}
                    {ExcludeFromCodeCoverageAttribute}
                    public static string {name} => "{name}"{extension};
            """);
        }

        sb.AppendLine("""
                }
            }
            """);

        context.AddSource($"{info.Namespace}.{info.Name}.g.cs", sb.ToString());
    }
}
