using System.Runtime.CompilerServices;

#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
#endif

namespace CoreMathSharp;

public static partial class StrictMath
{
    /// <summary>
    /// Computes the fused multiply-add of three values.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns>x * y + z, but the result is rounded only once</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double FusedMultiplyAdd(double x, double y, double z)
    {
#if NETCOREAPP3_0_OR_GREATER
        //return Math.FusedMultiplyAdd(x, y, z);

        if (Fma.IsSupported)
        {
            return Fma.MultiplyAdd(Vector128.CreateScalarUnsafe(x), Vector128.CreateScalarUnsafe(y), Vector128.CreateScalarUnsafe(z)).ToScalar();
        }
        if (AdvSimd.IsSupported)
        {
            return AdvSimd.FusedMultiplyAddScalar(Vector64.CreateScalarUnsafe(z), Vector64.CreateScalarUnsafe(x), Vector64.CreateScalarUnsafe(y)).ToScalar();
        }
#endif




        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (ulong mantissa, int exponent, int sign) normalize(double x)
        {
            ulong ix = Polyfill.DoubleToUInt64Bits(x);
            int e = (int)(ix >> 52) & 0x7ff;
            int sign = (int)(ix >> 63);

            if (e == 0)
            {
                ix = Polyfill.DoubleToUInt64Bits(x * 9223372036854775808.0);
                e = (int)(ix >> 52) & 0x7ff;
                e = e != 0 ? e - 63 : 0x800;
            }

            ix &= (1ul << 52) - 1;
            ix |= (1ul << 52);
            ix <<= 1;
            e -= 0x3ff + 52 + 1;
            return (ix, e, sign);
        }

        // https://git.musl-libc.org/cgit/musl/tree/src/math/fmaf.c
        static double Fallback(double x, double y, double z)
        {
            const int ZeroInfNaN = 0x7ff - 0x3ff - 52 - 1;

            var nx = normalize(x);
            var ny = normalize(y);
            var nz = normalize(z);

            if (nx.exponent >= ZeroInfNaN || ny.exponent >= ZeroInfNaN)
            {
                return x * y + z;
            }
            if (nz.exponent >= ZeroInfNaN)
            {
                if (nz.exponent > ZeroInfNaN)
                {
                    return x * y;
                }
                return z;
            }

            ulong rhi = Polyfill.BigMul(nx.mantissa, ny.mantissa, out ulong rlo);
            ulong zhi, zlo;

            int e = nx.exponent + ny.exponent;
            int d = nz.exponent - e;

            if (d > 0)
            {
                if (d < 64)
                {
                    zlo = nz.mantissa << d;
                    zhi = nz.mantissa >> -d;
                }
                else
                {
                    zlo = 0;
                    zhi = nz.mantissa;
                    e = nz.exponent - 64;
                    d -= 64;

                    if (d != 0)
                    {
                        if (d < 64)
                        {
                            rlo = rhi << -d | rlo >> d | (rlo << -d != 0 ? 1ul : 0ul);
                            rhi = rhi >> d;
                        }
                        else
                        {
                            rlo = 1;
                            rhi = 0;
                        }
                    }
                }
            }
            else
            {
                zhi = 0;
                d = -d;

                if (d == 0)
                {
                    zlo = nz.mantissa;
                }
                else if (d < 64)
                {
                    zlo = nz.mantissa >> d | (nz.mantissa << -d != 0 ? 1ul : 0ul);
                }
                else
                {
                    zlo = 1;
                }
            }

            int sign = nx.sign ^ ny.sign;
            bool sameSign = (sign ^ nz.sign) == 0;
            bool nonzero = true;

            if (sameSign)
            {
                rlo += zlo;
                rhi += zhi + (rlo < zlo ? 1ul : 0ul);
            }
            else
            {
                ulong t = rlo;
                rlo -= zlo;
                rhi = rhi - zhi - (t < rlo ? 1ul : 0ul);

                if (rhi >> 63 != 0)
                {
                    rlo = 0 - rlo;
                    rhi = 0 - rhi - (rlo != 0 ? 1ul : 0ul);
                    sign ^= 1;
                }

                nonzero = rhi != 0;
            }

            if (nonzero)
            {
                e += 64;
                d = Polyfill.LeadingZeroCount(rhi) - 1;
                rhi = rhi << d | rlo >> -d | (rlo << d != 0 ? 1ul : 0ul);
            }
            else if (rlo != 0)
            {
                d = Polyfill.LeadingZeroCount(rlo) - 1;
                if (d < 0)
                {
                    rhi = rlo >> 1 | (rlo & 1);
                }
                else
                {
                    rhi = rlo << d;
                }
            }
            else
            {
                return x * y + z;
            }
            e -= d;

            long i = (long)rhi;
            if (sign != 0)
            {
                i = -i;
            }

            double r = i;

            if (e < -1022 - 62)
            {
                if (e == -1022 - 63)
                {
                    double c = 9223372036854775808.0;

                    if (sign != 0)
                    {
                        c = -c;
                    }
                    if (r == c)
                    {
                        float fltmin = (float)(1.08420214017376175615e-19 * 1.17549435082228750797e-38 * r);
                        return 2.22507385850720138309e-308 / 1.17549435082228750797e-38 * fltmin;
                    }

                    if (rhi << 53 != 0)
                    {
                        i = (long)(rhi >> 1 | (rhi & 1) | 1ul << 62);
                        if (sign != 0)
                        {
                            i = -i;
                        }
                        r = i;
                        r = 2 * r - c;

                        // nop?
                    }
                }
                else
                {
                    d = 10;
                    i = (long)((rhi >> d | (rhi << -d != 0 ? 1ul : 0ul)) << d);
                    if (sign != 0)
                    {
                        i = -i;
                    }
                    r = i;
                }
            }
            double result = Ldexp(r, e);
            return result;
        }


        //*
        // https://hal.science/hal-04575249/document

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool isNot1Or3TimesPowerOf2(double x)
        {
            double delta = (2251799813685249.0 * x) - (2251799813685248.0 * x);
            return delta != x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double h, double l) twoSum(double a, double b)
        {
            double h = a + b;
            double aprime = h - b;
            double l = (a - aprime) + (b - (h - aprime));
            return (h, l);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double h, double l) split(double x)
        {
            const double k = 134217729.0;
            double gamma = k * x;
            double h = gamma + (x - gamma);
            double l = x - h;
            return (h, l);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double h, double l) dekkerProd(double a, double b)
        {
            (double ah, double al) = split(a);
            (double bh, double bl) = split(b);

            double h = a * b;
            double l = (((-h + ah * bh) + (ah * bl)) + al * bh) + al * bl;
            return (h, l);
        }


        static double FastEmulation(double x, double y, double z)
        {
            double xl, xh, sl, sh, vl, vh;
            (xh, xl) = dekkerProd(x, y);

            if (!double.IsNormal(xh))
            {
                return double.NaN;
            }

            (sh, sl) = twoSum(xh, z);
            (vh, vl) = twoSum(xl, sl);

            if (!double.IsNormal(vh))
            {
                return double.NaN;
            }

            if (!double.IsFinite(sh) || !double.IsFinite(xl))
            {
                if (double.IsFinite(x) && double.IsFinite(y) && !double.IsFinite(z))
                {
                    return z;
                }
                return sh;
            }

            if (isNot1Or3TimesPowerOf2(vh) || vl == 0.0)
            {
                return sh + vh;
            }
            if ((vl < 0.0) ^ (vh < 0.0))
            {
                return sh + (0.875 * vh);
            }
            return sh + (1.125 * vh);
        }
        //*/



        double fastPath = FastEmulation(x, y, z);
        if (!double.IsNaN(fastPath))
        {
            return fastPath;
        }

        return Fallback(x, y, z);
    }
}
