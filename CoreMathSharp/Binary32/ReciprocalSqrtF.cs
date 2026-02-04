using System;

namespace CoreMathSharp;

public static partial class StrictMathF
{
    /// <inheritdoc cref="StrictMath.ReciprocalSqrt(double)"/>
    public static float ReciprocalSqrt(float x)
    {
        double xd = x;
        uint ix = Polyfill.SingleToUInt32Bits(x);

        if (ix >= 0xff << 23 || ix == 0)
        {
            if (ix << 1 == 0)
            {
                return 1.0f / x;
            }
            if (ix >> 31 != 0)
            {
                ix &= ~0u >> 1;
                if (ix > 0xff << 23)
                {
                    return x + x;
                }

                return float.NaN;
            }
            if (ix << 9 == 0)
            {
                return 0.0f;
            }
            return x + x;
        }

        uint m = ix << 8;
        if (ix == 0x2f7e2au || m == 0xbdf8a800u || m == 0x55b7bd00u)
        {
            if (ix != 0x0055b7bdu)
            {
                uint e = ix >> 23;
                int k = 1;
                if (ix == 0x2f7e2au)
                {
                    e = ~0u;
                }
                if (m == 0x55b7bd00u)
                {
                    k = 0;
                }

                ReadOnlySpan<uint> tb = [0x000c1740u, 0x005222e0u];
                uint r = tb[k], dr;

                e = (512 - e) / 2 - 578;
                r |= e << 23;
                dr = (e - 25) << 23;

                return r - dr;
            }
        }

        return (float)((1.0 / xd) * StrictMath.Sqrt(xd));
    }
}
