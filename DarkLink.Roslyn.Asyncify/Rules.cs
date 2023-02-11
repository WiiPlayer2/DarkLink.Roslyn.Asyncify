using System;
using Microsoft.CodeAnalysis;

namespace DarkLink.Roslyn.Asyncify;

internal static class Rules
{
    public static DiagnosticDescriptor UnableToAsyncifyMethod { get; } = new(
        "ASYNCIFY01",
        "Unable to asyncify method",
        "Unable to asyncify overloads of method {0}: {1}",
        "Asyncify",
        DiagnosticSeverity.Warning,
        true);
}
