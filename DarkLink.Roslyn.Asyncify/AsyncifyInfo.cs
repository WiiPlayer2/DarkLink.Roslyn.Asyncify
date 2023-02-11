using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace DarkLink.Roslyn.Asyncify;

internal record TargetInfo(AttributeConfig Config, IReadOnlyList<IMethodSymbol> Methods);

internal record AsyncifyInfo(INamedTypeSymbol ExtensionType, IReadOnlyList<TargetInfo> Targets);
