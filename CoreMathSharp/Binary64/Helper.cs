using System;
using System.Runtime.CompilerServices;

#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
#endif

namespace CoreMathSharp;

public static partial class StrictMath
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static double BuiltinRound(double x)
    {
#if NETCOREAPP3_0_OR_GREATER
        if (AdvSimd.IsSupported)
        {
            return AdvSimd.RoundAwayFromZeroScalar(Vector64.CreateScalar(x)).ToScalar();
        }
#endif

        return Truncate(x + CopySign(0.49999999999999994, x));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static double Truncate(double x)
    {
        // TODO: math-dependent
        return Math.Truncate(x);
    }
}

