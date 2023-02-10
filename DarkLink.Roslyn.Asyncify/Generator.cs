using Microsoft.CodeAnalysis;
using System;

namespace DarkLink.Roslyn.Asyncify
{
    [Generator]
    public class Generator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(PostInitialize);

            // Initialize
        }

        private void PostInitialize(IncrementalGeneratorPostInitializationContext context)
        {
            // Generate immutable code
        }
    }
}