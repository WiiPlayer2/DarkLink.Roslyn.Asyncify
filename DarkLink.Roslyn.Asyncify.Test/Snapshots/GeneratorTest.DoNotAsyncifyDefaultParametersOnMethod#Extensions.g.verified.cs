﻿//HintName: Extensions.g.cs
// <auto-generated />

using System;
using System.Threading.Tasks;

partial class Extensions
{
    public static async Task CallMe(this Task<Subject> ___this, int number, bool boolean = true)
        => (await ___this).CallMe(number, boolean);
    public static async Task CallMe(this Task<Subject> ___this, Task<int> number, bool boolean = true)
        => (await ___this).CallMe((await number), boolean);
}