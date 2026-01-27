using System;
using System.Numerics;
using System.Runtime.CompilerServices;

#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
#endif

namespace CoreMathSharp;

public static partial class Polyfill
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
        return BitConverter.Int32BitsToSingle((int)x);
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
    internal static int PopCount(uint x)
    {
#if NETCOREAPP3_0_OR_GREATER
        return BitOperations.PopCount(x);
#else
        const uint c1 = 0x55555555;
        const uint c2 = 0x33333333;
        const uint c3 = 0x0f0f0f0f;
        const uint c4 = 0x01010101;

        x -= (x >> 1) & c1;
        x = (x & c2) + ((x >> 2) & c2);
        x = (((x + (x >> 4)) & c3) * c4) >> 24;
        return (int)x;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int PopCount(ulong x)
    {
#if NETCOREAPP3_0_OR_GREATER
        return BitOperations.PopCount(x);
#else
        const ulong c1 = 0x5555555555555555;
        const ulong c2 = 0x3333333333333333;
        const ulong c3 = 0x0f0f0f0f0f0f0f0f;
        const ulong c4 = 0x0101010101010101;

        x -= (x >> 1) & c1;
        x = (x & c2) + ((x >> 2) & c2);
        x = (((x + (x >> 4)) & c3) * c4) >> 56;
        return (int)x;
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static long BigMul(long x, long y, out long lo)
    {
#if NET5_0_OR_GREATER
        return Math.BigMul(x, y, out lo);
#else
#if NETCOREAPP3_0_OR_GREATER
        if (ArmBase.Arm64.IsSupported)
        {
            lo = x * y;
            return ArmBase.Arm64.MultiplyHigh(x, y);
        }
#endif

        ulong hi = BigMul((ulong)x, (ulong)y, out ulong ulo);
        lo = (long)ulo;
        return (long)hi - ((x >> 63) & y) - ((y >> 63) & x);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static float BitIncrement(float x)
    {
#if NETCOREAPP3_0_OR_GREATER
        return MathF.BitIncrement(x);
#else
        uint bits = SingleToUInt32Bits(x);

        if (((bits >> 23) & 0xff) == 0xff)
        {
            return bits == 0xff800000u ? float.MinValue : x;
        }

        if (bits == 1u << 31)
        {
            return float.Epsilon;
        }

        if (bits >= 1u << 31)
        {
            bits--;
        }
        else
        {
            bits++;
        }

        return UInt32BitsToSingle(bits);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static float BitDecrement(float x)
    {
#if NETCOREAPP3_0_OR_GREATER
        return MathF.BitDecrement(x);
#else
        uint bits = SingleToUInt32Bits(x);

        if (((bits >> 23) & 0xff) == 0xff)
        {
            return bits == 0x7f800000u ? float.MaxValue : x;
        }

        if (bits == 0)
        {
            return -float.Epsilon;
        }

        if (bits >= 1u << 31)
        {
            bits++;
        }
        else
        {
            bits--;
        }

        return UInt32BitsToSingle(bits);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static double BitIncrement(double x)
    {
#if NETCOREAPP3_0_OR_GREATER
        return Math.BitIncrement(x);
#else
        ulong bits = DoubleToUInt64Bits(x);

        if (((bits >> 52) & 0x7ff) == 0x7ff)
        {
            return bits == 0xfff00000_00000000ul ? double.MinValue : x;
        }

        if (bits == 1ul << 63)
        {
            return double.Epsilon;
        }

        if (bits >= 1ul << 63)
        {
            bits--;
        }
        else
        {
            bits++;
        }

        return UInt64BitsToDouble(bits);
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static double BitDecrement(double x)
    {
#if NETCOREAPP3_0_OR_GREATER
        return Math.BitDecrement(x);
#else
        ulong bits = DoubleToUInt64Bits(x);

        if (((bits >> 52) & 0x7ff) == 0x7ff)
        {
            return bits == 0x7ff00000_00000000ul ? double.MaxValue : x;
        }

        if (bits == 0)
        {
            return -double.Epsilon;
        }

        if (bits >= 1ul << 63)
        {
            bits++;
        }
        else
        {
            bits--;
        }

        return UInt64BitsToDouble(bits);
#endif
    }
}
