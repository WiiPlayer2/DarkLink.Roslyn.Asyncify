//HintName: DarkLink.Roslyn.AsyncifyAttribute.g.cs
using System;

namespace DarkLink.Roslyn
{
    [AttributeUsage((AttributeTargets)4, AllowMultiple = true, Inherited = false)]
    public class AsyncifyAttribute : Attribute
    {
        public AsyncifyAttribute(System.Type targetType, string method) { }
        public bool TransformParameters { get; set; }
    }
}
