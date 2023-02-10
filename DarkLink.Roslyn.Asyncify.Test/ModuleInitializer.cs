using System.Runtime.CompilerServices;

namespace DarkLink.Roslyn.Asyncify.Test
{
    public static class ModuleInitializer
    {
        [ModuleInitializer]
        public static void Init() => VerifySourceGenerators.Enable();
    }
}