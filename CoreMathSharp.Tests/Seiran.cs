using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace CoreMathSharp.Tests;

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

    public ulong Next()
    {
        ulong s0 = State0, s1 = State1;

        ulong result = BitOperations.RotateLeft((s0 + s1) * 9, 29) + s0;

        State0 = s0 ^ BitOperations.RotateLeft(s1, 29);
        State1 = s0 ^ s1 << 9;

        return result;
    }

    public ulong NextULong(ulong max)
    {
        ulong hi = StrictMath.BigMul(Next(), max, out ulong lo);

        if (lo < max)
        {
            ulong mod = (0ul - max) % max;

            while (lo < mod)
            {
                hi = StrictMath.BigMul(Next(), max, out lo);
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

    public float NextFloat()
    {
        return (Next() >> 40) * (1.0f / (1u << 24));
    }

    public float NextSignedFloat()
    {
        return ((long)Next() >> 39) * (1.0f / (1u << 24));
    }
}
