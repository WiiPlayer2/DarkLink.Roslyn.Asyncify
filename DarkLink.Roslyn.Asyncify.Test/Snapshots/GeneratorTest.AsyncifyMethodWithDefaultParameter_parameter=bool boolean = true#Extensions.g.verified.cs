//HintName: Extensions.g.cs
// <auto-generated />
#nullable enable

using System;
using System.Threading.Tasks;

partial class Extensions
{
    public static async Task CallMe(this Task<Subject> ___this, bool boolean = true)
        => (await ___this).CallMe(boolean);
}
