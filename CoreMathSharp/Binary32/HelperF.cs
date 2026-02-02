using System;
using System.Runtime.CompilerServices;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static float BuiltinRound(float x)
    {
        return Truncate(x + CopySign(0.49999997f, x));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static float Truncate(float x)
    {
        // TODO: math-dependent
        return MathF.Truncate(x);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static float BuiltinFloor(float x)
    {
        // TODO: math-dependent
        return MathF.Floor(x);
    }
}
