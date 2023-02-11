using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace DarkLink.Roslyn.Asyncify;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(PostInitialize);

        var infos = AttributeConfig.Find(context.SyntaxProvider, CheckNode, CreateInfo);
        context.RegisterSourceOutput(infos, GenerateMethods);
    }

    private bool CheckNode(SyntaxNode node, CancellationToken cancellationToken)
        => node is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword));

    private AsyncifyInfo CreateInfo(GeneratorAttributeSyntaxContext syntaxContext, IReadOnlyList<AttributeConfig> configs, CancellationToken cancellationToken)
    {
        var config = configs.First();
        var methods = config.TargetType.GetMembers(config.Method).OfType<IMethodSymbol>().ToList();
        return new AsyncifyInfo(config, (INamedTypeSymbol) syntaxContext.TargetSymbol, methods);
    }

    private void GenerateMethods(SourceProductionContext context, AsyncifyInfo info)
    {
        var hintName = $"{info.ExtensionType.ToDisplayString()}.g.cs";
        using var writer = new CodeWriter(info);
        writer.Write();
        var source = writer.ToString();
        context.AddSource(hintName, SourceText.From(source, new UTF8Encoding(false)));
    }

    private void PostInitialize(IncrementalGeneratorPostInitializationContext context) => AttributeConfig.AddTo(context);
}
