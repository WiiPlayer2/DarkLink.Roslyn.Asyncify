﻿//HintName: Extensions.g.cs
// <auto-generated />

using System;
using System.Threading.Tasks;

partial class Extensions
{
    public static async Task CallMe(this Task<Subject> ___this, int number = 42)
        => (await ___this).CallMe(number);
}
