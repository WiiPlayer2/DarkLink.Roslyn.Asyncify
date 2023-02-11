namespace DarkLink.Roslyn.Asyncify.Test;

[TestClass]
public class GeneratorTest : VerifySourceGenerator
{
    [TestMethod]
    public async Task AsyncifyMethodWithReturnValue()
    {
        var source = @"
using System;
using System.Threading.Tasks;
using DarkLink.Roslyn;

internal class Subject
{
    public int CallMe() => throw new NotImplementedException();
}

[Asyncify(typeof(Subject), nameof(Subject.CallMe))]
internal static partial class Extensions { }

internal static class Program
{
    public static Task<int> Main() => Task.FromResult(new Subject()).CallMe();
}
";

        await Verify(source);
    }

    [TestMethod]
    public async Task AsyncifyParameterLessVoidMethod()
    {
        var source = @"
using System;
using System.Threading.Tasks;
using DarkLink.Roslyn;

internal class Subject
{
    public void CallMe() => throw new NotImplementedException();
}

[Asyncify(typeof(Subject), nameof(Subject.CallMe))]
internal static partial class Extensions { }

internal static class Program
{
    public static Task Main() => Task.FromResult(new Subject()).CallMe();
}
";

        await Verify(source);
    }

    [TestMethod]
    public async Task AsyncifyVoidMethod()
    {
        var source = @"
using System;
using System.Threading.Tasks;
using DarkLink.Roslyn;

internal class Subject
{
    public void CallMe(int param1, string param2 = ""nani?!"") => throw new NotImplementedException();
}

[Asyncify(typeof(Subject), nameof(Subject.CallMe))]
internal static partial class Extensions { }

internal static class Program
{
    public static Task Main() => Task.FromResult(new Subject()).CallMe(420);
}
";

        await Verify(source);
    }

    [DataRow("namespace")]
    [DataRow("class")]
    [DataTestMethod]
    public async Task AsyncifyVoidMethodWithKeywordParameter(string keyword)
    {
        var source = @$"
using System;
using System.Threading.Tasks;
using DarkLink.Roslyn;

internal class Subject
{{
    public void CallMe(int @{keyword}) => throw new NotImplementedException();
}}

[Asyncify(typeof(Subject), nameof(Subject.CallMe))]
internal static partial class Extensions {{ }}

internal static class Program
{{
    public static Task Main() => Task.FromResult(new Subject()).CallMe(420);
}}
";

        await Verify(source, task => task.UseParameters(keyword));
    }

    [TestMethod]
    public async Task Empty()
    {
        var source = string.Empty;

        await Verify(source);
    }
}
