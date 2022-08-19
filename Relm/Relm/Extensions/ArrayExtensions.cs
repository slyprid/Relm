using System;
using System.Runtime.CompilerServices;

namespace Relm.Extensions
{
    public static class ArrayExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains<T>(this T[] source, T value)
        {
            return Array.IndexOf(source, value) >= 0;
        }
    }
}