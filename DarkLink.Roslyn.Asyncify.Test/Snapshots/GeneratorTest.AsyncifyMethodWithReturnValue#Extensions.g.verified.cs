//HintName: Extensions.g.cs
// <auto-generated />
#nullable enable

using System;
using System.Threading.Tasks;

partial class Extensions
{
    public static async Task<int> CallMe(this Task<Subject> ___this)
        => (await ___this).CallMe();
}
