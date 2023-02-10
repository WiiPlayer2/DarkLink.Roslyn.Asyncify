using System;
using DarkLink.RoslynHelpers;
using Microsoft.CodeAnalysis;

namespace DarkLink.Roslyn.Asyncify;

[GenerateAttribute(AttributeTargets.Class, AllowMultiple = false, Inherited = false, Name = "AsyncifyAttribute", Namespace = "DarkLink.Roslyn")]
internal partial record AttributeConfig(INamedTypeSymbol TargetType, string Method);
