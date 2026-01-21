using System;
using System.Numerics;
using System.Runtime.CompilerServices;

#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
#endif

namespace CoreMathSharp;

public static partial class StrictMath
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ulong DoubleToUInt64Bits(double x)
    {
#if NET6_0_OR_GREATER
        return BitConverter.DoubleToUInt64Bits(x);
#else
        return (ulong)BitConverter.DoubleToInt64Bits(x);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static double UInt64BitsToDouble(ulong x)
    {
#if NET6_0_OR_GREATER
        return BitConverter.UInt64BitsToDouble(x);
#else
        return BitConverter.Int64BitsToDouble((long)x);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static uint SingleToUInt32Bits(float x)
    {
#if NET6_0_OR_GREATER
        return BitConverter.SingleToUInt32Bits(x);
#else
        return (uint)BitConverter.SingleToInt32Bits(x);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static float UInt32BitsToSingle(uint x)
    {
#if NET6_0_OR_GREATER
        return BitConverter.UInt32BitsToSingle(x);
#else
        return BitConverter.SingleToInt32Bits((int)x);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int TrailingZeroCount(uint x)
    {
#if NETCOREAPP3_0_OR_GREATER
        return BitOperations.TrailingZeroCount(x);
#else
        ReadOnlySpan<byte> trailingZeroCountDeBruijn = [
            00, 01, 28, 02, 29, 14, 24, 03,
            30, 22, 20, 15, 25, 17, 04, 08,
            31, 27, 13, 23, 21, 19, 16, 07,
            26, 12, 18, 06, 11, 05, 10, 09];

        return trailingZeroCountDeBruijn[(int)(((x & (0 - x)) * 0x077CB531u) >> 27)];
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int TrailingZeroCount(ulong x)
    {
#if NETCOREAPP3_0_OR_GREATER
        return BitOperations.TrailingZeroCount(x);
#else
        uint lo = (uint)x;
        uint hi = (uint)(x >> 32);
        if (lo == 0)
        {
            return 32 + TrailingZeroCount(hi);
        }
        return TrailingZeroCount(lo);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int LeadingZeroCount(uint x)
    {
#if NETCOREAPP3_0_OR_GREATER
        return BitOperations.LeadingZeroCount(x);
#else
        ReadOnlySpan<byte> log2DeBruijn = [
            00, 09, 01, 10, 13, 21, 02, 29,
            11, 14, 16, 18, 22, 25, 03, 30,
            08, 12, 20, 28, 15, 17, 24, 07,
            19, 27, 23, 06, 26, 05, 04, 31];

        x |= x >> 1;
        x |= x >> 2;
        x |= x >> 4;
        x |= x >> 8;
        x |= x >> 16;

        return 31 ^ log2DeBruijn[(int)((x * 0x07C4ACDDu) >> 27)];
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int LeadingZeroCount(ulong x)
    {
#if NETCOREAPP3_0_OR_GREATER
        return BitOperations.LeadingZeroCount(x);
#else
        uint hi = (uint)(x >> 32);
        uint lo = (uint)x;
        if (hi == 0) 
        {
            return 32 + LeadingZeroCount(lo);
        }
        return LeadingZeroCount(hi);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ulong BigMul(ulong x, ulong y, out ulong lo)
    {
#if NET5_0_OR_GREATER
        return Math.BigMul(x, y, out lo);
#else
#if NETCOREAPP3_0_OR_GREATER
        if (Bmi2.X64.IsSupported)
        {
            ulong low;
            ulong high = Bmi2.X64.MultiplyNoFlags(a, b, &low);
            lo = low;
            return high;
        }
        if (ArmBase.Arm64.IsSupported)
        {
            lo = x * y;
            return ArmBase.Arm64.MultiplyHigh(x, y);
        }
#endif

        uint xlo = (uint)x;
        uint xhi = (uint)(x >> 32);
        uint ylo = (uint)y;
        uint yhi = (uint)(y >> 32);

        ulong lolo = (ulong)xlo * ylo;
        ulong mid1 = (ulong)xhi * ylo + (lolo >> 32);
        ulong mid2 = (ulong)xlo * yhi + (uint)mid1;
        ulong hihi = (ulong)xhi * yhi;

        lo = mid2 << 32 | (uint)lolo;
        return hihi + (mid1 >> 32) + (mid2 >> 32);
#endif
    }

}
