using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DarkLink.Roslyn.Asyncify;

internal record AsyncifyInfo(IReadOnlyList<AttributeConfig> Configs);

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

    private AsyncifyInfo CreateInfo(GeneratorAttributeSyntaxContext syntaxContext, IReadOnlyList<AttributeConfig> configs, CancellationToken cancellationToken) => default!;

    private void GenerateMethods(SourceProductionContext context, AsyncifyInfo info) { }

    private void PostInitialize(IncrementalGeneratorPostInitializationContext context) => AttributeConfig.AddTo(context);
}
