﻿//HintName: Extensions.g.cs
// <auto-generated />
#nullable enable

using System;
using System.Threading.Tasks;

partial class Extensions
{
    public static async Task CallMe(this Task<Subject> ___this, int param1, string param2 = "nani?!")
        => (await ___this).CallMe(param1, param2);
}
