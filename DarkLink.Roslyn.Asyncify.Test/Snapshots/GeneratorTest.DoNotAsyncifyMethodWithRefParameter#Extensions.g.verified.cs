﻿//HintName: Extensions.g.cs
// <auto-generated />
#nullable enable

using System;
using System.Threading.Tasks;

partial class Extensions
{
    public static async Task CallMe(this Task<Subject> ___this, string text)
        => (await ___this).CallMe(text);
}
