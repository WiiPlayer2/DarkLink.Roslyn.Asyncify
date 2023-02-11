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

    private void CheckForWarnings(SourceProductionContext context, AsyncifyInfo info)
    {
        foreach (var target in info.Targets.Where(t => t.InvalidMethods.Any()))
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Rules.UnableToAsyncifyMethod,
                    target.AttributeLocation,
                    target.Methods.Concat(target.InvalidMethods).First().Name,
                    FormatMethods(target.InvalidMethods)));

        string FormatMethods(IEnumerable<IMethodSymbol> methods) => string.Join(", ", methods.Select(FormatMethod));

        string FormatMethod(IMethodSymbol method) => $"{method.ReturnType.ToDisplayString()}({string.Join(", ", method.Parameters.Select(p => p.Type.ToDisplayString()))})";
    }

    private bool CheckNode(SyntaxNode node, CancellationToken cancellationToken)
        => node is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword));

    private AsyncifyInfo CreateInfo(GeneratorAttributeSyntaxContext syntaxContext, IReadOnlyList<(AttributeData Data, AttributeConfig Config)> configs, CancellationToken cancellationToken)
    {
        var targets = configs.Select(CreateTargetInfo).ToList();
        return new AsyncifyInfo((INamedTypeSymbol) syntaxContext.TargetSymbol, targets);

        TargetInfo CreateTargetInfo((AttributeData Data, AttributeConfig Config) pair)
        {
            var allMethods = pair.Config.TargetType.GetMembers(pair.Config.Method).OfType<IMethodSymbol>().ToList();
            var validMethods = allMethods.Where(IsMethodValid).ToList();
            var invalidMethods = allMethods.Where(m => !IsMethodValid(m)).ToList();
            return new TargetInfo(pair.Config, validMethods, invalidMethods, pair.Data.ApplicationSyntaxReference?.GetSyntax().GetLocation());
        }
    }

    private void GenerateMethods(SourceProductionContext context, AsyncifyInfo info)
    {
        CheckForWarnings(context, info);

        var hintName = $"{info.ExtensionType.ToDisplayString()}.g.cs";
        using var writer = new CodeWriter(info);
        writer.Write();
        var source = writer.ToString();
        context.AddSource(hintName, SourceText.From(source, new UTF8Encoding(false)));
    }

    private bool IsMethodValid(IMethodSymbol method) => method.Parameters.All(p => p.RefKind == RefKind.None);

    private void PostInitialize(IncrementalGeneratorPostInitializationContext context) => AttributeConfig.AddTo(context);
}
