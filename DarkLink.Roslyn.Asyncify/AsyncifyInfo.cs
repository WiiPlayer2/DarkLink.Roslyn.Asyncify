using System;
using Microsoft.CodeAnalysis;

namespace DarkLink.Roslyn.Asyncify;

internal record AsyncifyInfo(AttributeConfig Config, INamedTypeSymbol ExtensionType, IMethodSymbol Method);
