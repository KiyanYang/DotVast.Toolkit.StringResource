using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotVast.Toolkit.StringResource.SourceGenerators;

[Generator(LanguageNames.CSharp)]
public class StringResourceGenerator : IIncrementalGenerator
{
    private const string TargetAttributeName = "DotVast.Toolkit.StringResource.StringResourceAttribute";
    private const string TargetAttributeQualifiedName = "global::DotVast.Toolkit.StringResource.StringResourceAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<StringResourceInfo> strReswInfos = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                TargetAttributeName,
                static (node, _) => node is ClassDeclarationSyntax n && n.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)),
                static (context, c) =>
                {
                    if (context.TargetSymbol is not ITypeSymbol type)
                        return null;

                    var basePath = context.TargetNode.SyntaxTree.FilePath;

                    var args = GetAttributeCtorArgsValue(context.Attributes, TargetAttributeQualifiedName);

                    return args?.Count switch
                    {
                        // Path
                        1 => new StringResourceInfo(type, Path.Combine(basePath, args[0])),
                        // Path, ExtensionMethod, ExtensionMethodNamespace
                        3 => new StringResourceInfo(type, Path.Combine(basePath, args[0]))
                        {
                            ExMethed = args[1],
                            ExMethedNamespace = args[2]
                        },
                        _ => null,
                    };
                })
            .Where(static i => i != null)!;

        var infos = strReswInfos
            .Where(static i => i.ReswPath.EndsWith(".resw"))
            .Where(static i => File.Exists(i.ReswPath));

        context.RegisterSourceOutput(strReswInfos, GenerateCode);
    }

    private static IList<string>? GetAttributeCtorArgsValue(ImmutableArray<AttributeData> attributeData, string attributeQualifiedName) =>
        attributeData.FirstOrDefault(a => a.AttributeClass?.HasFullyQualifiedName(attributeQualifiedName) == true)
            ?.ConstructorArguments.Select(ca => ca.Value!.ToString()).ToList();

    private static void GenerateCode(SourceProductionContext context, StringResourceInfo info)
    {
        var ns = info.Namespace;
        var cls = info.Name;
        var path = info.ReswPath;
        var xEle = XElement.Load(path);
        var exMethed = info.ExMethed;
        var exMethedNs = info.ExMethedNamespace;
        var sb = new StringBuilder();

        sb.Append(@$"// <auto-generated/>
#pragma warning disable
#nullable enable
");

        if (exMethedNs != null)
            sb.Append(@$"
using {exMethedNs};
");

        sb.Append(@$"
namespace {ns}
{{
    partial class {cls}
    {{");

        foreach (var item in xEle.Elements("data"))
        {
            if (item.Attribute("name")?.Value is not string name
                || name.Contains("."))
                continue;

            if (item.Element("value")?.Value is string value)
                sb.Append($@"
        ///<value>
        /// <![CDATA[{value}]]>
        /// </value>");

            if (item.Element("comment")?.Value is string comment)
                sb.Append($@"
        ///<remarks>
        /// <![CDATA[{comment}]]>
        /// </remarks>");

            sb.Append($@"
        public static string {name} => ""{name}""{(exMethed != null ? $".{exMethed}()" : string.Empty)};
");
        }

        sb.Append(@"    }
}
");

        context.AddSource($"{ns}.{cls}.g.cs", sb.ToString());
    }
}
