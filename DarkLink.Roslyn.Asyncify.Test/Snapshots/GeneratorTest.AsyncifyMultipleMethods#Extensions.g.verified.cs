//HintName: Extensions.g.cs
// <auto-generated />
#nullable enable

using System;
using System.Threading.Tasks;

partial class Extensions
{
    public static async Task CallMe(this Task<Subject> ___this)
        => (await ___this).CallMe();

    public static async Task InvokeMe(this Task<Subject> ___this)
        => (await ___this).InvokeMe();
}
