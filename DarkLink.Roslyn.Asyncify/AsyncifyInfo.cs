using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace DarkLink.Roslyn.Asyncify;

internal record AsyncifyInfo(AttributeConfig Config, INamedTypeSymbol ExtensionType, IReadOnlyList<IMethodSymbol> Methods);
