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
    internal static double BuiltinFloor(double x)
    {
        // TODO: math-dependent
        return Math.Floor(x);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static double Truncate(double x)
    {
        // TODO: math-dependent
        return Math.Truncate(x);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static double Ldexp(double x, int exp)
    {
        const double Exp2_1023 = 8.98846567431158E+307;
        const double Exp2_M1022 = 2.2250738585072014E-308;
        const double Exp2_53 = 9007199254740992;

        if (exp > 0x3ff)
        {
            x *= Exp2_1023;
            exp -= 0x3ff;

            if (exp > 0x3ff)
            {
                x *= Exp2_1023;
                exp -= 0x3ff;

                if (exp > 0x3ff)
                {
                    exp = 0x3ff;
                }
            }
        }
        else if (exp <= -0x3ff)
        {
            x *= Exp2_M1022 * Exp2_53;
            exp += 0x3fe - 53;

            if (exp <= -0x3ff)
            {
                x *= Exp2_M1022 * Exp2_53;
                exp += 0x3fe - 53;

                if (exp <= 0x3ff)
                {
                    exp = -0x3fe;
                }
            }
        }

        double e = Polyfill.UInt64BitsToDouble((ulong)(exp + 0x3ff) << 52);
        return x * e;
    }
}

