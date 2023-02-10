namespace DarkLink.Roslyn.Asyncify.Test;

[TestClass]
public class GeneratorTest : VerifySourceGenerator
{
    [TestMethod]
    public async Task Empty()
    {
        var source = string.Empty;

        await Verify(source);
    }

    [TestMethod]
    public async Task AsyncifyParameterLessVoidMethod()
    {
        var source = @"
using System;
using DarkLink.Roslyn;

internal class Subject
{
    public void CallMe() => throw new NotImplementedException();
}

[Asyncify(typeof(Subject), nameof(Subject.CallMe))]
internal static partial class Extensions { }
";

        await Verify(source);
    }
}
