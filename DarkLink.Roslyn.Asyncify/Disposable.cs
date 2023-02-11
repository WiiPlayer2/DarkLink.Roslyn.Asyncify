using System;

namespace DarkLink.Roslyn.Asyncify;

internal static class Disposable
{
    public static IDisposable Create(Action action) => new ActionDisposable(action);

    private record ActionDisposable(Action Action) : IDisposable
    {
        public void Dispose() => Action();
    }
}
