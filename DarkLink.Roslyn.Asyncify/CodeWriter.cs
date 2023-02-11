﻿using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DarkLink.Roslyn.Asyncify;

internal class CodeWriter : IDisposable
{
    private readonly AsyncifyInfo info;

    private readonly StringWriter stringWriter;

    private readonly IndentedTextWriter writer;

    public CodeWriter(AsyncifyInfo info)
    {
        this.info = info;
        stringWriter = new StringWriter();
        writer = new IndentedTextWriter(stringWriter);
    }

    public void Dispose()
    {
        writer.Dispose();
        stringWriter.Dispose();
    }

    private IDisposable Reset()
    {
        var originalIndent = writer.Indent;
        writer.Indent = 0;
        return Disposable.Create(() => writer.Indent = originalIndent);
    }

    private static string SanitizeIdentifier(string identifier)
        => SyntaxFacts.GetKeywordKind(identifier) != SyntaxKind.None || SyntaxFacts.GetContextualKeywordKind(identifier) != SyntaxKind.None
            ? $"@{identifier}"
            : identifier;

    private IDisposable Scope()
    {
        writer.Indent++;
        return Disposable.Create(() => writer.Indent--);
    }

    private static string ToDefaultLiteral(IParameterSymbol parameter)
        => parameter.Type.TypeKind == TypeKind.Enum
            ? $"(({parameter.Type.ToDisplayString()}){ToLiteral(parameter.ExplicitDefaultValue)})"
            : ToLiteral(parameter.ExplicitDefaultValue);

    private static string ToLiteral(object? value)
    {
        return Map().ToString();

        LiteralExpressionSyntax Map() => value switch
        {
            null => LiteralExpression(SyntaxKind.NullLiteralExpression),
            string stringValue => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(stringValue)),
            int intValue => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(intValue)),
            bool boolValue => LiteralExpression(boolValue ? SyntaxKind.TrueLiteralExpression : SyntaxKind.FalseLiteralExpression),
            byte byteValue => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(byteValue)),
            char charValue => LiteralExpression(SyntaxKind.CharacterLiteralExpression, Literal(charValue)),
            double doubleValue => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(doubleValue)),
            float floatValue => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(floatValue)),
            long longValue => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(longValue)),
            sbyte sbyteValue => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(sbyteValue)),
            short shortValue => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(shortValue)),
            uint uintValue => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(uintValue)),
            ulong ulongValue => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(ulongValue)),
            ushort ushortValue => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(ushortValue)),
            _ => throw new NotImplementedException($"Not implemented for type {value.GetType().AssemblyQualifiedName}"),
        };
    }

    public override string ToString() => stringWriter.ToString();

    public void Write()
    {
        writer.WriteLine("// <auto-generated />");
        writer.WriteLine();
        writer.WriteLine("using System;");
        writer.WriteLine("using System.Threading.Tasks;");
        writer.WriteLine();
        writer.WriteLine($"partial class {info.ExtensionType.Name}");
        writer.WriteLine("{");

        using (Scope())
        {
            WriteMethods();
        }

        writer.WriteLine("}");
    }

    private void WriteEmptyLine() => writer.WriteLineNoTabs(string.Empty);

    private void WriteMethod(IMethodSymbol method)
    {
        writer.WriteLine($"public static async {FormatReturnType()} {method.Name}(this Task<{method.ContainingType.ToDisplayString()}> ___this{FormatParameters()})");

        using (Scope())
        {
            writer.WriteLine($"=> (await ___this).{method.Name}({FormatArguments()});");
        }

        string FormatReturnType()
            => method.ReturnsVoid
                ? "Task"
                : $"Task<{method.ReturnType.ToDisplayString()}>";

        string FormatParameters() => string.Concat(method.Parameters.Select(p => $", {FormatParameter(p)}"));

        string FormatArguments() => string.Join(", ", method.Parameters.Select(FormatArgument));

        string FormatArgument(IParameterSymbol parameter) => SanitizeIdentifier(parameter.Name);

        string FormatParameter(IParameterSymbol parameter)
        {
            var parameterString = $"{parameter.Type.ToDisplayString()} {SanitizeIdentifier(parameter.Name)}";
            if (parameter.HasExplicitDefaultValue)
                parameterString += $" = {ToDefaultLiteral(parameter)}";
            return parameterString;
        }
    }

    private void WriteMethods()
    {
        var first = true;
        foreach (var method in info.Methods)
        {
            if (!first)
                WriteEmptyLine();
            else
                first = false;

            WriteMethod(method);
        }
    }
}
