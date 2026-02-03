using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace CoreMathSharpUnity.Tests;

public sealed class Seiran
{
    private ulong State0, State1;

    public Seiran()
    {
        var state = (stackalloc ulong[2]);

        do
        {
            RandomNumberGenerator.Fill(MemoryMarshal.AsBytes(state));
        } while (state[0] == 0 && state[1] == 0);

        State0 = state[0];
        State1 = state[1];
    }

    public Seiran(ulong s0, ulong s1)
    {
        if (s0 == 0 && s1 == 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        State0 = s0;
        State1 = s1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ulong RotateLeft(ulong value, int offset)
    {
        return (value << offset) | (value >> (64 - offset));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ulong BigMul(ulong x, ulong y, out ulong lo)
    {
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
    }

    public ulong Next()
    {
        ulong s0 = State0, s1 = State1;

        ulong result = RotateLeft((s0 + s1) * 9, 29) + s0;

        State0 = s0 ^ RotateLeft(s1, 29);
        State1 = s0 ^ s1 << 9;

        return result;
    }

    public ulong NextULong(ulong max)
    {
        ulong hi = BigMul(Next(), max, out ulong lo);

        if (lo < max)
        {
            ulong mod = (0ul - max) % max;

            while (lo < mod)
            {
                hi = BigMul(Next(), max, out lo);
            }
        }

        return hi;
    }

    public double NextDouble()
    {
        return (Next() >> 11) * (1.0 / (1ul << 53));
    }

    public double NextSignedDouble()
    {
        return ((long)Next() >> 10) * (1.0 / (1ul << 53));
    }

    public double NextDouble(double min, double max)
    {
        double r = NextDouble();
        return (1.0 - r) * min + r * max;
    }

    public float NextFloat()
    {
        return (Next() >> 40) * (1.0f / (1u << 24));
    }

    public float NextSignedFloat()
    {
        return ((long)Next() >> 39) * (1.0f / (1u << 24));
    }

    public float NextFloat(float min, float max)
    {
        float r = NextFloat();
        return (1.0f - r) * min + r * max;
    }
}
