using System;
using DarkLink.RoslynHelpers;

namespace DarkLink.Roslyn.Asyncify;

[GenerateAttribute(AttributeTargets.Class, AllowMultiple = false, Inherited = false, Name = "AsyncifyAttribute", Namespace = "DarkLink.Roslyn")]
internal partial record AttributeConfig(Type TargetType, string Method);
