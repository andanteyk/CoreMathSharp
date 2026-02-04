using System;
using System.Runtime.CompilerServices;

namespace CoreMathSharp;

internal readonly record struct Uint128(ulong lo, ulong hi)
{
    public static Uint128 Zero => new Uint128(0, 0);
    public static Uint128 One => new Uint128(1, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Cmpu128(Uint128 a, Uint128 b)
    {
        int gt = a.hi > b.hi ? 1 : a.hi < b.hi ? 0 : a.lo > b.lo ? 1 : 0;
        int lt = a.hi < b.hi ? 1 : a.hi > b.hi ? 0 : a.lo < b.lo ? 1 : 0;
        return gt - lt;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int carry, Uint128 r) AddU128(in Uint128 a, in Uint128 b)
    {
        Uint128 r = a + b;
        return (r < a ? 1 : 0, r);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int borrow, Uint128 r) SubU128(in Uint128 a, in Uint128 b)
    {
        Uint128 r = a - b;
        return (r > a ? 1 : 0, r);
    }

    public static Uint128 operator +(Uint128 a, Uint128 b)
    {
        ulong lo = a.lo + b.lo;
        ulong carry = lo < a.lo ? 1ul : 0ul;
        ulong hi = a.hi + b.hi + carry;
        return new Uint128(lo, hi);
    }
    public static Uint128 operator -(Uint128 a, Uint128 b)
    {
        ulong lo = a.lo - b.lo;
        ulong borrow = lo > a.lo ? 1ul : 0ul;
        ulong hi = a.hi - b.hi - borrow;
        return new Uint128(lo, hi);
    }
    public static Uint128 operator *(Uint128 a, ulong b)
    {
        ulong lohi = Polyfill.BigMul(a.lo, b, out ulong lolo);
        ulong hilo = a.hi * b;
        return new Uint128(lolo, lohi + hilo);
    }
    public static Uint128 operator *(ulong a, Uint128 b)
        => b * a;

    public static Uint128 operator &(Uint128 a, Uint128 b)
    {
        return new Uint128(a.lo & b.lo, a.hi & b.hi);
    }

    public static Uint128 operator |(Uint128 a, Uint128 b)
    {
        return new Uint128(a.lo | b.lo, a.hi | b.hi);
    }

    public static Uint128 operator ^(Uint128 a, Uint128 b)
    {
        return new Uint128(a.lo ^ b.lo, a.hi ^ b.hi);
    }

    public static Uint128 operator <<(Uint128 a, int k)
    {
        // assumes 0 <= k < 128
        if (k == 0)
        {
            return a;
        }
        else if (k < 64)
        {
            return new Uint128(a.lo << k, a.hi << k | a.lo >> -k);
        }
        else
        {
            return new Uint128(0, a.lo << k);
        }
    }

    public static Uint128 operator >>(Uint128 a, int k)
    {
        // assumes 0 <= k < 128
        if (k == 0)
        {
            return a;
        }
        else if (k < 64)
        {
            return new Uint128(a.lo >> k | a.hi << -k, a.hi >> k);
        }
        else
        {
            return new Uint128(a.hi >> k, 0);
        }
    }

    public static bool operator <(Uint128 a, Uint128 b)
    {
        return a.hi < b.hi || (a.hi <= b.hi && a.lo < b.lo);
    }
    public static bool operator >(Uint128 a, Uint128 b)
    {
        return a.hi > b.hi || (a.hi >= b.hi && a.lo > b.lo);
    }

    public static implicit operator Uint128(ulong a)
    {
        return new Uint128(a, 0);
    }
}


public static partial class StrictMath
{
    private readonly record struct Dint(ulong lo, ulong hi, long ex, ulong sgn)
    {
        public static Dint Zero => new Dint(0, 0, -1076, 0);
        public static Dint One => new Dint(0, 0x8000000000000000, 0, 0);
        public static Dint MinusOne => new Dint(0, 0x8000000000000000, 0, 1);

        public static Dint Magic => new Dint(0, 0x8000000000000000, -10, 0);
        public static Dint Log2 => new Dint(0xc9e3b39803f2f6af, 0xb17217f7d1cf79ab, -1, 0);
        public static Dint Log2Inv => new Dint(0xbe87fed0691d3e89, 0xb8aa3b295c17f0bb, 12, 0);
        public static Dint Log2InvPow => new Dint(0, 0xb8aa3b295c17f0bc, 12, 0);
        public static Dint Log10Inv => new Dint(0x355baaafad33dc32, 0xde5bd8a937287195, -2, 0);

        public static Dint OneOverLog10 => new Dint(0x355baaafad33dc32, 0xde5bd8a937287195, -2, 0);

        public bool IsZero => hi == 0;
        public Uint128 r => new Uint128(lo, hi);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (int overflow, Uint128 result) Addu128(in Uint128 a, in Uint128 b)
        {
            ulong rl = a.lo + b.lo;
            ulong rh = a.hi + b.hi + (rl < a.lo ? 1ul : 0ul);
            return ((rh == a.hi ? rl < a.lo : rh < a.hi) ? 1 : 0, new Uint128(rl, rh));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int Cmp(long a, long b) => (a > b ? 1 : 0) - (a < b ? 1 : 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (long e, ulong m) fastExtract(double x)
        {
            ulong _x = Polyfill.DoubleToUInt64Bits(x);

            long e = (long)(_x >> 52) & 0x7ff;
            ulong m = (_x & (~0ul >> 12)) + (e != 0 ? (1ul << 52) : 0);
            e = e - 0x3fe;

            return (e, m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (long e, ulong m) fastExtractLog(double x)
        {
            ulong _x = Polyfill.DoubleToUInt64Bits(x);

            long e = (long)(_x >> 52) & 0x7ff;
            ulong m = (_x & (~0ul >> 12)) + (e != 0 ? (1ul << 52) : 0);
            e = e - 0x3ff;

            return (e, m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CmpDintAbs(in Dint a, in Dint b)
        {
            if (a.IsZero)
            {
                return b.IsZero ? 0 : -1;
            }
            if (b.IsZero)
            {
                return 1;
            }

            int c1 = Cmp(a.ex, b.ex);
            if (c1 != 0)
            {
                return c1;
            }

            return Uint128.Cmpu128(a.r, b.r);
        }

        public static Dint Add(in Dint a, in Dint b)
        {
            if ((a.hi | a.lo) == 0)
            {
                return b;
            }

            int cmp = CmpDintAbs(a, b);
            Dint aa, bb;
            if (cmp == 0)
            {
                if ((a.sgn ^ b.sgn) != 0)
                {
                    return Zero;
                }

                return a with { ex = a.ex + 1 };
            }
            else if (cmp == -1)
            {
                aa = b;
                bb = a;
            }
            else
            {
                aa = a;
                bb = b;
            }


            Uint128 A = aa.r, B = bb.r;
            ulong k = (ulong)(aa.ex - bb.ex);

            if (k > 0)
            {
                B = k < 128 ? B >> (int)k : Uint128.Zero;
            }

            Uint128 C;
            ulong sgn = aa.sgn;

            long rex = aa.ex;
            if ((aa.sgn ^ bb.sgn) != 0)
            {
                C = A - B;
                ulong ch = C.hi;
                int ex = ch != 0 ? Polyfill.LeadingZeroCount(ch) : 64 + Polyfill.LeadingZeroCount(C.lo);

                if (ex > 0)
                {
                    if (k == 1)
                    {
                        C = (A << ex) - (bb.r << (ex - 1));
                    }
                    else
                    {
                        C = (A << ex) - (B << ex);
                    }

                    rex -= ex;
                    ex = Polyfill.LeadingZeroCount(C.hi);
                }

                C = C << ex;
                rex -= ex;

            }
            else
            {
                C = A + B;
                if (C < A)
                {
                    C = (Uint128.One << 127) | (C >> 1);
                    rex++;
                }
            }

            return new Dint(C.lo, C.hi, rex, sgn);
        }

        public static int CmpDint11(in Dint a, in Dint b)
        {
            int cmp = Cmp(a.ex, b.ex);
            return cmp != 0 ? cmp : Uint128.Cmpu128(a.r, b.r);
        }

        public static Dint Add11(in Dint a, in Dint b)
        {
            if (a.hi == 0)
            {
                return b;
            }

            if (b.hi == 0)
            {
                return a;
            }

            int cmp = CmpDint11(a, b);
            Dint aa, bb;
            if (cmp == 0)
            {
                if ((a.sgn ^ b.sgn) != 0)
                {
                    return Zero;
                }

                return a with { ex = a.ex + 1 };
            }
            else if (cmp == -1)
            {
                (aa, bb) = (b, a);
            }
            else
            {
                (aa, bb) = (a, b);
            }

            ulong A = aa.hi, B = bb.hi;
            if (aa.ex > bb.ex)
            {
                long k = aa.ex - bb.ex;
                B = (k < 64) ? B >> (int)k : 0;
            }

            Uint128 C;
            int sgn = (int)aa.sgn;

            long rex = aa.ex;

            if ((aa.sgn ^ bb.sgn) != 0)
            {
                C = A - B;
                int ex = Polyfill.LeadingZeroCount(C.lo);

                if (ex > 0)
                {
                    C = (A << ex) - (B << ex);
                    rex -= ex;
                    ex = Polyfill.LeadingZeroCount(C.lo);
                }

                C <<= ex;
                rex -= ex;
            }
            else
            {
                C = A + B;
                if (C < A)
                {
                    C = (1ul << 63) | (C >> 1);
                    rex++;
                }
            }

            return new Dint(0, C.lo, rex, (ulong)sgn);
        }

        public static Dint Mul(in Dint a, in Dint b)
        {
            Uint128 bh = (Uint128)b.hi, bl = (Uint128)b.lo;

            Uint128 m1 = a.hi * bl;
            Uint128 m2 = a.lo * bh;

            Uint128 rr = a.hi * bh;
            rr += (Uint128)m1.hi + m2.hi;

            long ex = (long)(rr.hi >> 63);
            rr = rr << (1 - (int)ex);

            long rex = a.ex + b.ex + ex - 1;
            ulong sgn = a.sgn ^ b.sgn;

            return new Dint(rr.lo, rr.hi, rex, sgn);
        }

        public static Dint MulPow(in Dint a, in Dint b)
        {
            Uint128 bh = (Uint128)b.hi, bl = (Uint128)b.lo;

            Uint128 m1 = a.hi * bl;
            Uint128 m2 = a.lo * bh;

            Uint128 rr = a.hi * bh;
            rr += (Uint128)m1.hi + m2.hi;

            long ex = (long)(rr.hi >> 63);
            rr = rr << (1 - (int)ex);

            long rex = a.ex + b.ex + ex;
            ulong sgn = a.sgn ^ b.sgn;

            return new Dint(rr.lo, rr.hi, rex, sgn);
        }

        public static Dint Mul21(in Dint a, in Dint b)
        {
            Uint128 bh = (Uint128)b.hi;
            Uint128 hi = a.hi * bh;
            Uint128 lo = a.lo * bh;

            var rr = hi;
            rr += lo.hi;

            long ex = (long)(rr.hi >> 63);
            rr = rr << (1 - (int)ex);

            long rex = a.ex + b.ex + ex;
            ulong sgn = a.sgn ^ b.sgn;

            return new Dint(rr.lo, rr.hi, rex, sgn);
        }

        public static Dint Mul21Tan(in Dint a, in Dint b)
        {
            Uint128 bh = (Uint128)b.hi;
            Uint128 hi = a.hi * bh;
            Uint128 lo = a.lo * bh;

            var rr = hi;
            rr += lo.hi;

            long ex = (long)(rr.hi >> 63);
            rr = rr << (1 - (int)ex);

            long rex = a.ex + b.ex + ex - 1;
            ulong sgn = a.sgn ^ b.sgn;

            return new Dint(rr.lo, rr.hi, rex, sgn);
        }

        public static Dint Mul2(long b, in Dint a)
        {
            if (b == 0)
            {
                return Zero;
            }

            ulong c = (ulong)(b < 0 ? -b : b);
            ulong rsgn = b < 0 ? a.sgn ^ 1 : a.sgn;

            Uint128 t = (Uint128)a.hi * c;

            int m = t.hi != 0 ? Polyfill.LeadingZeroCount(t.hi) : 64;
            t = t << m;

            Uint128 l = (Uint128)a.lo * c;
            l = (l << (m - 1)) >> 63;

            (int overflow, t) = Addu128(l, t);
            if (overflow != 0)
            {
                t += t.lo & 1;
                t = (Uint128.One << 127) | (t >> 1);
                m--;
            }

            return new Dint(t.lo, t.hi, a.ex + 64 - m, rsgn);
        }

        public static Dint MulLog(in Dint a, in Dint b)
        {
            Uint128 t = (Uint128)a.hi * b.hi;
            Uint128 m1 = (Uint128)a.hi * b.lo;
            Uint128 m2 = (Uint128)a.lo * b.hi;

            Uint128 m;

            (int overflow, m) = Addu128(m1, m2);
            if (overflow != 0)
            {
                t = t with { hi = t.hi + 1 };
            }
            t += m.hi;

            long ex = (t.hi >> 63 == 0 ? 1 : 0);
            if (ex != 0)
            {
                t <<= 1;
            }

            t += (m.lo >> 63);

            return new Dint(t.lo, t.hi, a.ex + b.ex - ex + 1, a.sgn ^ b.sgn);
        }

        public static Dint Mul11(in Dint a, in Dint b)
        {
            Uint128 rr = (Uint128)a.hi * b.hi;

            int ex = (int)(rr.hi >> 63);
            rr = rr << (1 - ex);

            return new Dint(rr.lo, rr.hi, a.ex + b.ex + ex, a.sgn ^ b.sgn);
        }

        public static Dint MulInt64(in Dint a, long b)
        {
            if (b == 0)
            {
                return Zero;
            }

            ulong c = (ulong)(b < 0 ? -b : b);
            ulong rsgn = b < 0 ? a.sgn ^ 1 : a.sgn;
            long rex = a.ex + 64;

            Uint128 rr = (Uint128)a.hi * c;

            int m = rr.hi != 0 ? Polyfill.LeadingZeroCount(rr.hi) : 64;
            rr <<= m;
            rex -= m;

            Uint128 l = (Uint128)a.lo * c;
            l = (l << (m - 1)) >> 63;

            rr += l;
            if (rr < l)
            {
                rr = (Uint128.One << 127) | (rr >> 1);
                rex++;
            }

            return new Dint(rr.lo, rr.hi, rex, rsgn);
        }

        public static Dint FromDouble(double b)
        {
            var (ex, hi) = fastExtract(b);

            int t = Polyfill.LeadingZeroCount(hi);

            ulong sgn = b < 0.0 ? 1ul : 0ul;
            hi = hi << t;
            ex = ex - (t > 11 ? t - 12 : 0);
            ulong lo = 0;

            return new Dint(lo, hi, ex, sgn);
        }

        public static Dint FromDoubleLog(double b)
        {
            var (ex, hi) = fastExtractLog(b);

            int t = Polyfill.LeadingZeroCount(hi);

            ulong sgn = b < 0.0 ? 1ul : 0ul;
            hi = hi << t;
            ex = ex - (t > 11 ? t - 12 : 0);
            ulong lo = 0;

            return new Dint(lo, hi, ex, sgn);
        }

        public Dint Subnormalize()
        {
            if (ex > -1023)
            {
                return this;
            }

            int rex = (int)-(1011 + ex);

            ulong rhi = hi >> rex;
            ulong rmd = (hi >> (rex - 1)) & 1;
            ulong rlo = hi & (~0ul >> rex);
            if (rlo == 0)
            {
                rlo = lo;
            }


            rhi += rlo != 0 ? rmd : rhi & rmd;


            rhi = rhi << rex;
            rlo = 0;

            if (rhi == 0)
            {
                return new Dint(rlo, 1ul << 63, ex + 1, sgn);
            }

            return new Dint(rlo, rhi, ex, sgn);
        }

        public double ToDouble()
        {
            Dint a = Subnormalize();

            ulong r = a.hi >> 11 | 0x3fful << 52;

            double rd = 0.0;
            if (((a.hi >> 10) & 0x1) != 0)
            {
                rd += 1.1102230246251565e-16;
            }

            if ((a.hi & 0x3ff) != 0 || a.lo != 0)
            {
                rd += 5.5511151231257827e-17;
            }

            if (a.sgn != 0)
            {
                rd = -rd;
            }

            r = r | (a.sgn << 63);
            r = Polyfill.DoubleToUInt64Bits(Polyfill.UInt64BitsToDouble(r) + rd);

            ulong e;

            if (a.ex > -1022)
            {
                if (a.ex > 1024)
                {
                    if (a.ex == 1025)
                    {
                        r = Polyfill.DoubleToUInt64Bits(Polyfill.UInt64BitsToDouble(r) * 2.0);
                        e = Polyfill.DoubleToUInt64Bits(8.9884656743115795e+307);
                    }
                    else
                    {
                        r = Polyfill.DoubleToUInt64Bits(1.7976931348623157e+308);
                        e = Polyfill.DoubleToUInt64Bits(1.7976931348623157e+308);
                    }
                }
                else
                {
                    e = (ulong)(((a.ex + 1022) & 0x7ff) << 52);
                }
            }
            else
            {
                if (a.ex < -1073)
                {
                    if (a.ex == -1074)
                    {
                        r = Polyfill.DoubleToUInt64Bits(Polyfill.UInt64BitsToDouble(r) * 0.5);
                        e = Polyfill.DoubleToUInt64Bits(4.9406564584124654e-324);
                    }
                    else
                    {
                        r = Polyfill.DoubleToUInt64Bits(4.9406564584124654e-324);
                        e = Polyfill.DoubleToUInt64Bits(4.9406564584124654e-324);
                    }
                }
                else
                {
                    e = 1ul << (int)(a.ex + 1073);
                }
            }

            return Polyfill.UInt64BitsToDouble(r) * Polyfill.UInt64BitsToDouble(e);
        }

        public double ToDoubleLog()
        {
            ulong r = (hi >> 11) | (0x3fful << 52);

            double rd = 0.0;
            if (((hi >> 10) & 1) != 0)
            {
                rd += 1.1102230246251565e-16;
            }
            if ((hi & 0x3ff) != 0 || lo != 0)
            {
                rd += 5.5511151231257827e-17;
            }

            r = r | (sgn << 63);
            r = Polyfill.DoubleToUInt64Bits(Polyfill.UInt64BitsToDouble(r) + (sgn == 0 ? rd : -rd));

            ulong e = ((ulong)(ex + 1023) & 0x7fful) << 52;
            return Polyfill.UInt64BitsToDouble(r) * Polyfill.UInt64BitsToDouble(e);
        }



        public Dint Normalize()
        {
            ulong xhi = hi, xlo = lo;
            long xex = ex;

            int cnt;
            if (hi != 0)
            {
                cnt = Polyfill.LeadingZeroCount(hi);
                if (cnt != 0)
                {
                    xhi = (hi << cnt) | (lo >> -cnt);
                    xlo = lo << cnt;
                }
                xex -= cnt;
            }
            else if (lo != 0)
            {
                cnt = Polyfill.LeadingZeroCount(lo);
                xhi = lo << cnt;
                xlo = 0;
                xex -= 64 + cnt;
            }

            return new Dint(xlo, xhi, xex, sgn);
        }

        public Dint Reduce(ReadOnlySpan<ulong> T)
        {
            ulong xlo = lo, xhi = hi;
            long e = ex;
            Uint128 u;

            if (e <= 1)
            {
                u = (Uint128)hi * T[1];
                ulong tiny = u.lo;
                xlo = u.hi;

                u = (Uint128)hi * T[0];
                xlo += u.lo;
                xhi = u.hi + (xlo < u.lo ? 1ul : 0ul);

                e = ex;
                var result = new Dint(xlo, xhi, ex, sgn).Normalize();
                e = e - result.ex;

                if (e != 0)
                {
                    result = result with { lo = result.lo | tiny >> -(int)e };
                }

                return result;
            }

            int i = (e < 127) ? 0 : ((int)e - 127 + 64 - 1) / 64;
            var c = (stackalloc ulong[5]);

            u = (Uint128)hi * T[i + 3];
            c[0] = u.lo;
            c[1] = u.hi;
            u = (Uint128)hi * T[i + 2];
            c[1] += u.lo;
            c[2] = u.hi + (c[1] < u.lo ? 1ul : 0ul);
            u = (Uint128)hi * T[i + 1];
            c[2] += u.lo;
            c[3] = u.hi + (c[2] < u.lo ? 1ul : 0ul);
            u = (Uint128)hi * T[i + 0];
            c[3] += u.lo;
            c[4] = u.hi + (c[3] < u.lo ? 1ul : 0ul);

            {
                int f = (int)e - 64 * i;
                ulong tiny;
                if (f < 64)
                {
                    xhi = (c[4] << f) | (c[3] >> -f);
                    xlo = (c[3] << f) | (c[2] >> -f);
                    tiny = (c[2] << f) | (c[1] >> -f);
                }
                else if (f == 64)
                {
                    xhi = c[3];
                    xlo = c[2];
                    tiny = c[1];
                }
                else
                {
                    int g = f - 64;
                    u = (Uint128)hi * T[i + 4];
                    u = u >> 64;
                    c[0] += u.lo;
                    c[1] += c[0] < u.lo ? 1ul : 0ul;
                    c[2] += c[0] < u.lo && c[1] == 0 ? 1ul : 0ul;
                    c[3] += c[0] < u.lo && c[1] == 0 && c[2] == 0 ? 1ul : 0ul;
                    c[4] += c[0] < u.lo && c[1] == 0 && c[2] == 0 && c[3] == 0 ? 1ul : 0ul;

                    xhi = c[3] << g | c[2] >> -g;
                    xlo = c[2] << g | c[1] >> -g;
                    tiny = c[1] << g | c[0] >> -g;
                }

                long xex = 0;
                var result = new Dint(xlo, xhi, xex, sgn).Normalize();
                if (result.ex < 0)
                {
                    result = result with { lo = result.lo | tiny >> (64 + (int)result.ex) };
                }
                return result;
            }
        }

        public (Dint result, int i) Reduce2()
        {
            if (ex <= -11)
            {
                return (this, 0);
            }

            int sh = 64 - 11 - (int)ex;
            int i = (int)(hi >> sh);
            ulong xhi = hi & ((1ul << sh) - 1);
            return (new Dint(lo, xhi, ex, sgn).Normalize(), i);
        }

        public static Dint Inv(double a)
        {
            Dint q, A;
            Dint r = FromDoubleLog(4.0 / a);
            r = r with { ex = r.ex - 2 };

            A = FromDoubleLog(-a);
            q = MulLog(A, r);
            q = Add(One, q);
            q = MulLog(r, q);
            r = Add(r, q);

            return r;
        }

        public static Dint Div(double b, double a)
        {
            Dint B;
            Dint r = Inv(a);
            B = FromDoubleLog(b);
            r = MulLog(r, B);
            return r;
        }

        public Dint Inv()
        {
            ReadOnlySpan<ulong> Tinv = [0xff00ff00ff00ff02, 0xfe03f80fe03f80ff, 0xfd08e5500fd08e56, 0xfc0fc0fc0fc0fc11, 0xfb18856506ddaba7, 0xfa232cf252138ac1, 0xf92fb2211855a867, 0xf83e0f83e0f83e11, 0xf74e3fc22c700f76, 0xf6603d980f6603db, 0xf57403d5d00f5742, 0xf4898d5f85bb3952, 0xf3a0d52cba872338, 0xf2b9d6480f2b9d66, 0xf1d48bcee0d399fc, 0xf0f0f0f0f0f0f0f2, 0xf00f00f00f00f010, 0xef2eb71fc4345239, 0xee500ee500ee5010, 0xed7303b5cc0ed731, 0xec979118f3fc4da3, 0xebbdb2a5c1619c8d, 0xeae56403ab959010, 0xea0ea0ea0ea0ea10, 0xe939651fe2d8d35d, 0xe865ac7b7603a198, 0xe79372e225fe30da, 0xe6c2b4481cd8568a, 0xe5f36cb00e5f36cc, 0xe525982af70c880f, 0xe45932d7dc52100f, 0xe38e38e38e38e38f, 0xe2c4a6886a4c2e11, 0xe1fc780e1fc780e3, 0xe135a9c97500e137, 0xe070381c0e070383, 0xdfac1f74346c5760, 0xdee95c4ca037ba58, 0xde27eb2c41f3d9d2, 0xdd67c8a60dd67c8b, 0xdca8f158c7f91ab9, 0xdbeb61eed19c5959, 0xdb2f171df770291a, 0xda740da740da740f, 0xd9ba4256c0366e92, 0xd901b2036406c80f, 0xd84a598ec9151f44, 0xd79435e50d79435f, 0xd6df43fca482f00e, 0xd62b80d62b80d62c, 0xd578e97c3f5fe552, 0xd4c77b03531dec0e, 0xd4173289870ac52e, 0xd3680d3680d3680e, 0xd2ba083b445250ac, 0xd20d20d20d20d20e, 0xd161543e28e50275, 0xd0b69fcbd2580d0c, 0xd00d00d00d00d00e, 0xcf6474a8819ec8ea, 0xcebcf8bb5b4169cc, 0xce168a7725080ce2, 0xcd712752a886d243, 0xccccccccccccccce, 0xcc29786c7607f99f, 0xcb8727c065c393e1, 0xcae5d85f1bbd6c96, 0xca4587e6b74f032a, 0xc9a633fcd967300d, 0xc907da4e871146ad, 0xc86a78900c86a78a, 0xc7ce0c7ce0c7ce0d, 0xc73293d789b9f839, 0xc6980c6980c6980d, 0xc5fe740317f9d00d, 0xc565c87b5f9d4d1c, 0xc4ce07b00c4ce07c, 0xc4372f855d824ca6, 0xc3a13de60495c774, 0xc30c30c30c30c30d, 0xc2780613c0309e02, 0xc1e4bbd595f6e948, 0xc152500c152500c2, 0xc0c0c0c0c0c0c0c1, 0xc0300c0300c0300d, 0xbfa02fe80bfa02ff, 0xbf112a8ad278e8de, 0xbe82fa0be82fa0bf, 0xbdf59c91700bdf5a, 0xbd69104707661aa3, 0xbcdd535db1cc5b7c, 0xbc52640bc52640bd, 0xbbc8408cd63069a1, 0xbb3ee721a54d880c, 0xbab656100bab6562, 0xba2e8ba2e8ba2e8c, 0xb9a7862a0ff46588, 0xb92143fa36f5e02f, 0xb89bc36ce3e0453b, 0xb81702e05c0b8171, 0xb79300b79300b794, 0xb70fbb5a19be3659, 0xb68d31340e4307d9, 0xb60b60b60b60b60c, 0xb58a485518d1e7e4, 0xb509e68a9b948220, 0xb48a39d44685fe97, 0xb40b40b40b40b40c, 0xb38cf9b00b38cf9b, 0xb30f63528917c80c, 0xb2927c29da5519d0, 0xb21642c8590b2165, 0xb19ab5c45606f00c, 0xb11fd3b80b11fd3c, 0xb0a59b418d749d54, 0xb02c0b02c0b02c0b, 0xafb321a1496fdf0f, 0xaf3addc680af3ade, 0xaec33e1f671529a5, 0xae4c415c9882b931, 0xadd5e6323fd48a87, 0xad602b580ad602b6, 0xaceb0f891e6551bc, 0xac7691840ac76919, 0xac02b00ac02b00ac, 0xab8f69e28359cd12, 0xab1cbdd3e2970f60, 0xaaaaaaaaaaaaaaab, 0xaa392f35dc17f00b, 0xa9c84a47a07f5638, 0xa957fab5402a55ff, 0xa8e83f5717c0a8e9, 0xa87917088e262b70, 0xa80a80a80a80a80b, 0xa79c7b16ea64d422, 0xa72f05397829cbc2, 0xa6c21df6e1625c80, 0xa655c4392d7b73a8, 0xa5e9f6ed347f0721, 0xa57eb50295fad40b, 0xa513fd6bb00a5140, 0xa4a9cf1d96833751, 0xa44029100a440291, 0xa3d70a3d70a3d70b, 0xa36e71a2cb033129, 0xa3065e3fae7cd0e0, 0xa29ecf163bb6500a, 0xa237c32b16cfd772, 0xa1d139855f7268ee, 0xa16b312ea8fc377d, 0xa105a932f2ca891f, 0xa0a0a0a0a0a0a0a1, 0xa03c1688732b3032, 0x9fd809fd809fd80a, 0x9f747a152d7836d0, 0x9f1165e7254813e2, 0x9eaecc8d53ae2ddf, 0x9e4cad23dd5f3a20, 0x9deb06c9194aa416, 0x9d89d89d89d89d8a, 0x9d2921c3d6411308, 0x9cc8e160c3fb19b9, 0x9c69169b30446dfa, 0x9c09c09c09c09c0a, 0x9baade8e4a2f6e10, 0x9b4c6f9ef03a3caa, 0x9aee72fcf957c10f, 0x9a90e7d95bc609a9, 0x9a33cd67009a33ce, 0x99d722dabde58f06, 0x997ae76b50efd00a, 0x991f1a515885fb37, 0x98c3bac74f5db00a, 0x9868c809868c8099, 0x980e4156201301c8, 0x97b425ed097b425f, 0x975a750ff68a58af, 0x97012e025c04b80a, 0x96a850096a850097, 0x964fda6c0964fda7, 0x95f7cc72d1b887e9, 0x95a02568095a0257, 0x9548e4979e0829fd, 0x94f2094f2094f209, 0x949b92ddc02526e5, 0x9445809445809446, 0x93efd1c50e726b7c, 0x939a85c40939a85c, 0x93459be6b009345a, 0x92f113840497889c, 0x929cebf48bbd90e5, 0x9249249249249249, 0x91f5bcb8bb02d9cd, 0x91a2b3c4d5e6f809, 0x9150091500915009, 0x90fdbc090fdbc091, 0x90abcc0242af3009, 0x905a38633e06c43b, 0x9009009009009009, 0x8fb823ee08fb823f, 0x8f67a1e3fdc26179, 0x8f1779d9fdc3a219, 0x8ec7ab397255e41d, 0x8e78356d1408e783, 0x8e2917e0e702c6ce, 0x8dda520237694809, 0x8d8be33f95d71590, 0x8d3dcb08d3dcb08d, 0x8cf008cf008cf009, 0x8ca29c046514e023, 0x8c55841c815ed5ca, 0x8c08c08c08c08c09, 0x8bbc50c8deb420c0, 0x8b70344a139bc75b, 0x8b246a87e19008b2, 0x8ad8f2fba9386823, 0x8a8dcd1feeae465c, 0x8a42f8705669db46, 0x89f87469a23920e0, 0x89ae4089ae4089ae, 0x89645c4f6e055dec, 0x891ac73ae9819b50, 0x88d180cd3a4133d7, 0x8888888888888889, 0x883fddf00883fddf, 0x87f78087f78087f8, 0x87af6fd5992d0d40, 0x8767ab5f34e47ef1, 0x872032ac13008720, 0x86d905447a34acc6, 0x869222b1acf1ce96, 0x864b8a7de6d1d608, 0x86053c345a0b8473, 0x85bf37612cee3c9b, 0x85797b917765ab89, 0x8534085340853408, 0x84eedd357c1b0085, 0x84a9f9c8084a9f9d, 0x84655d9bab2f1008, 0x8421084210842108, 0x83dcf94dc7570ce1, 0x839930523fbe3368, 0x8355ace3c897db10, 0x83126e978d4fdf3b, 0x82cf750393ac3319, 0x828cbfbeb9a020a3, 0x824a4e60b3262bc5, 0x8208208208208208, 0x81c635bc123fdf8e, 0x81848da8faf0d277, 0x814327e3b94f462f, 0x8102040810204081, 0x80c121b28bd1ba98, 0x8080808080808081, 0x8040201008040201, 0x8000000000000000];

            ulong h = hi;

            int i = (int)(h >> 55) & 0xff;
            ulong t = Tinv[i];

            Uint128 e = (Uint128.One << 127) - (Uint128)h * t;
            e = t * (e >> 55);
            t += (e >> 72).lo;

            e = (Uint128.One << 127) - (Uint128)h * t;
            e = t * (e >> 47);
            t += (e >> 80).lo;

            e = (Uint128.One << 127) - (Uint128)h * t;
            e = t * (e >> 31);
            t += (e >> 96).lo;

            Dint q, r;
            r = new Dint(0, t, 1 - ex, 1);
            q = Mul21Tan(this, r);
            r = r with { sgn = 0 };
            q = Add(One with { ex = 1 }, q);
            q = Mul(r, q);
            r = Add(r, q);

            return r;
        }

        public static Dint Div(Dint b, Dint a)
        {
            Dint r = a.Inv();
            r = Mul(r, b);
            return r;
        }
    }




    /// <summary>
    /// Computes the cosine of a value.
    /// </summary>
    /// <returns>[-1, 1]</returns>
    public static double Cos(double x)
    {
        ReadOnlySpan<ulong> T = [0x28be60db9391054a, 0x7f09d5f47d4d3770, 0x36d8a5664f10e410, 0x7f9458eaf7aef158, 0x6dc91b8e909374b8, 0x1924bba82746487, 0x3f877ac72c4a69cf, 0xba208d7d4baed121, 0x3a671c09ad17df90, 0x4e64758e60d4ce7d, 0x272117e2ef7e4a0e, 0xc7fe25fff7816603, 0xfbcbc462d6829b47, 0xdb4d9fb3c9f2c26d, 0xd3d18fd9a797fa8b, 0x5d49eeb1faf97c5e, 0xcf41ce7de294a4ba, 0x9afed7ec47e35742, 0x1580cc11bf1edaea, 0xfc33ef0826bd0d87,];



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double hi, double lo) aMul(double a, double b)
        {
            double hi = a * b;
            double lo = FusedMultiplyAdd(a, b, -hi);
            return (hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double hi, double lo) sMul(double a, double bh, double bl)
        {
            var (hi, lo) = aMul(a, bh);
            lo = FusedMultiplyAdd(a, bl, lo);
            return (hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double hi, double lo) dMul(double ah, double al, double bh, double bl)
        {
            double s, t;
            (double hi, s) = aMul(ah, bh);
            t = FusedMultiplyAdd(al, bh, s);
            double lo = FusedMultiplyAdd(ah, bl, t);
            return (hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double hi, double lo) fastTwoSum(double a, double b)
        {
            double e;
            double hi = a + b;
            e = hi - a;
            double lo = b - e;
            return (hi, lo);
        }

        static (double hi, double lo) evalPSfast(double xh, double xl, double uh, double ul)
        {
            ReadOnlySpan<double> PSfast = [6.2831853071795862, 2.4492937487389357e-16, -41.341702240399762, 81.605249298319492, -76.770831258583385];

            double t;
            double h = PSfast[4];
            h = FusedMultiplyAdd(h, uh, PSfast[3]);
            h = FusedMultiplyAdd(h, uh, PSfast[2]);
            (h, double l) = sMul(h, uh, ul);
            (h, t) = fastTwoSum(PSfast[0], h);
            l += PSfast[1] + t;
            (h, l) = dMul(h, l, xh, xl);
            return (h, l);
        }

        static (double hi, double lo) evalPCfast(double uh, double ul)
        {
            ReadOnlySpan<double> PCfast = [1, -1.0396311785461432e-23, -19.739208802178716, 64.939394007293487, -85.411886468174686];

            double t, h, l;
            h = PCfast[4];
            h = FusedMultiplyAdd(h, uh, PCfast[3]);
            h = FusedMultiplyAdd(h, uh, PCfast[2]);
            (h, l) = sMul(h, uh, ul);
            (h, t) = fastTwoSum(PCfast[0], h);
            l += PCfast[1] + t;
            return (h, l);
        }

        static Dint evalPS(in Dint X, in Dint X2)
        {
            ReadOnlySpan<ulong> PShi = [0xc90fdaa22168c234, 0xa55de7312df295f5, 0xa335e33bad570e92, 0x9969667315ec2d9d, 0xa83c1a43bf1c6485, 0xf16ab2898eae62f9,];
            ReadOnlySpan<ulong> PSlo = [0xc4c6628b80dc1cd1, 0x5dc72f712aa57db4, 0x3f33be0021aa54d2, 0xe59d6ab8509a2025, 0x7d5f8f76fa7d74ed, 0xa7f0339113b8b3c5,];
            ReadOnlySpan<int> PSex = [3, 6, 7, 7, 6, 4,];
            ReadOnlySpan<byte> PSsgn = [0, 1, 0, 1, 0, 1];

            Dint Y;

            Y = Dint.Mul21(X2, new Dint(PSlo[5], PShi[5], PSex[5], PSsgn[5]));

            Y = Dint.Add(Y, new Dint(PSlo[4], PShi[4], PSex[4], PSsgn[4]));
            Y = Dint.Mul(Y, X2);
            Y = Dint.Add(Y, new Dint(PSlo[3], PShi[3], PSex[3], PSsgn[3]));
            Y = Dint.Mul(Y, X2);
            Y = Dint.Add(Y, new Dint(PSlo[2], PShi[2], PSex[2], PSsgn[2]));
            Y = Dint.Mul(Y, X2);
            Y = Dint.Add(Y, new Dint(PSlo[1], PShi[1], PSex[1], PSsgn[1]));
            Y = Dint.Mul(Y, X2);
            Y = Dint.Add(Y, new Dint(PSlo[0], PShi[0], PSex[0], PSsgn[0]));
            Y = Dint.Mul(Y, X);

            return Y;
        }

        static Dint evalPC(in Dint X2)
        {
            ReadOnlySpan<ulong> PChi = [0x8000000000000000, 0x9de9e64df22ef2d2, 0x81e0f840dad61d9a, 0xaae9e3f1e5ffcfe2, 0xf0fa83448dd1e094, 0xd368f6f4207cfe49,];
            ReadOnlySpan<ulong> PClo = [0x0, 0x56e26cd9808c1949, 0x9980f00630cb655e, 0xa508509534006249, 0xe0603ce7044eeba, 0xec63157807ebffa,];
            ReadOnlySpan<int> PCex = [1, 5, 7, 7, 6, 5,];
            ReadOnlySpan<byte> PCsgn = [0, 1, 0, 1, 0, 1];

            Dint Y;

            Y = Dint.Mul21(X2, new Dint(PClo[5], PChi[5], PCex[5], PCsgn[5]));

            Y = Dint.Add(Y, new Dint(PClo[4], PChi[4], PCex[4], PCsgn[4]));
            Y = Dint.Mul(Y, X2);
            Y = Dint.Add(Y, new Dint(PClo[3], PChi[3], PCex[3], PCsgn[3]));
            Y = Dint.Mul(Y, X2);
            Y = Dint.Add(Y, new Dint(PClo[2], PChi[2], PCex[2], PCsgn[2]));
            Y = Dint.Mul(Y, X2);
            Y = Dint.Add(Y, new Dint(PClo[1], PChi[1], PCex[1], PCsgn[1]));
            Y = Dint.Mul(Y, X2);
            Y = Dint.Add(Y, new Dint(PClo[0], PChi[0], PCex[0], PCsgn[0]));

            return Y;
        }

        static (double hi, double lo) setdd(ulong c1, ulong c0)
        {
            int e, f, g;
            ulong t;
            double h, l;

            if (c1 != 0)
            {
                e = Polyfill.LeadingZeroCount(c1);
                if (e != 0)
                {
                    c1 = c1 << e | c0 >> -e;
                    c0 = c0 << e;
                }

                f = 0x3fe - e;
                t = (ulong)f << 52 | ((c1 << 1) >> 12);
                h = Polyfill.UInt64BitsToDouble(t);
                c0 = (c1 << 53) | (c0 >> 11);
                if (c0 != 0)
                {
                    g = Polyfill.LeadingZeroCount(c0);
                    if (g != 0)
                    {
                        c0 = c0 << g;
                    }
                    t = (ulong)(f - 53 - g) << 52 | ((c0 << 1) >> 12);
                    l = Polyfill.UInt64BitsToDouble(t);
                }
                else
                {
                    l = 0.0;
                }
            }
            else if (c0 != 0)
            {
                e = Polyfill.LeadingZeroCount(c0);
                f = 0x3fe - 64 - e;
                c0 = c0 << (e + 1);
                t = (ulong)f << 52 | c0 >> 12;
                h = Polyfill.UInt64BitsToDouble(t);

                c0 = c0 << 52;
                if (c0 != 0)
                {
                    g = Polyfill.LeadingZeroCount(c0);
                    c0 = c0 << (g + 1);
                    t = (ulong)(f - 64 - g) << 52 | c0 >> 12;
                    l = Polyfill.UInt64BitsToDouble(t);
                }
                else
                {
                    l = 0.0;
                }
            }
            else
            {
                h = l = 0.0;
            }

            return (h, l);
        }


        static (int something, double hi, double lo, double err1) reduceFast(double x, ReadOnlySpan<ulong> T)
        {
            double h, l, err1;

            if (x <= 6.2831853071795853)
            {
                const double Ch = 0.15915494309189535;
                const double Cl = -9.8393383375912429e-18;

                (h, l) = aMul(Ch, x);
                l = FusedMultiplyAdd(Cl, x, l);
                err1 = 4.5548243184758128e-32 * h;
            }
            else
            {
                ulong t = Polyfill.DoubleToUInt64Bits(x);
                int e = (int)(t >> 52) & 0x7ff;

                ulong m = (1ul << 52) | (t & 0xffffffffffffful);
                var c = (stackalloc ulong[3]);
                Uint128 u;

                if (e <= 1074)
                {
                    u = (Uint128)m * T[1];
                    c[0] = u.lo;
                    c[1] = u.hi;
                    u = (Uint128)m * T[0];
                    c[1] += u.lo;
                    c[2] = u.hi + (c[1] < u.lo ? 1ul : 0ul);

                    e = 1075 - e;
                }
                else
                {
                    int i = (e - 1138 + 63) / 64;

                    u = (Uint128)m * T[i + 2];
                    c[0] = u.lo;
                    c[1] = u.hi;
                    u = (Uint128)m * T[i + 1];
                    c[1] += u.lo;
                    c[2] = u.hi + (c[1] < u.lo ? 1ul : 0ul);
                    u = (Uint128)m * T[i + 0];
                    c[2] += u.lo;

                    e = 1139 + (i << 6) - e;
                }

                if (e == 64)
                {
                    c[0] = c[1];
                    c[1] = c[2];
                }
                else
                {
                    c[0] = c[1] << -e | c[0] >> e;
                    c[1] = c[2] << -e | c[1] >> e;
                }

                (h, l) = setdd(c[1], c[0]);
                err1 = 1.3286588589133007e-23;
            }

            {
                double i = BuiltinFloor(h * 2048.0);
                h = FusedMultiplyAdd(i, -0.00048828125, h);
                return ((int)i, h, l, err1);
            }
        }


        static (double err, double h, double l) cosFast(double x, ReadOnlySpan<ulong> T)
        {
            ReadOnlySpan<double> SC = [0, 0, 1, -5.1041282311814484e-11, 0.0030679564422656505, 0.99999529381056007, -4.689280539392493e-11, 0.0061358843545238359, 0.999981175284409, -1.0579724162712578e-10, 0.0092037541173443007, 0.999957644558082, -2.1630513114163918e-10, 0.012271536926737041, 0.99992470185582261, 3.8445976870832288e-10, 0.015339208700335866, 0.99988234741715865, 1.9853647068371338e-10, 0.018406731153034917, 0.9998305817728621, -3.5646790640803117e-11, 0.021474080051545764, 0.99976940535602499, -8.9114606752899173e-10, 0.024541222925362795, 0.99969881883361633, -2.809495518174554e-10, 0.027608144014380522, 0.9996188225439141, -3.7162287483405443e-10, 0.030674800842760037, 0.99952941757271807, 9.4828642507871486e-10, 0.033741177806244312, 0.9994306043544231, -1.9473419404780756e-11, 0.036807222819086639, 0.99932238459285305, -5.1304943800289937e-11, 0.039872927265637695, 0.99920475863121727, 3.7698673523156589e-10, 0.042938259301433773, 0.9990777276509385, 7.2548048297982337e-11, 0.046003182586264867, 0.99894129316588709, 2.038292815781384e-10, 0.049067675606572506, 0.99879545614233156, 3.864018827126614e-11, 0.052131704922736652, 0.99864021816760851, 1.9884507998502166e-09, 0.055195256824448986, 0.99847557988369606, 7.8205839720846271e-10, 0.058258269405907691, 0.99830154464762233, -1.6957436062975395e-09, 0.061320725667588151, 0.99811811355350144, -1.219928548804039e-09, 0.064382623280723084, 0.99792528669209124, -1.0440826883617582e-09, 0.067443913018436108, 0.99772306708663483, 3.3476149109701314e-09, 0.070504594370955423, 0.99751145465733226, 1.2465360861579899e-10, 0.073564564380766972, 0.99729045662107285, -9.4316891102730338e-11, 0.076623860801163221, 0.99706007038489108, -4.7462769478534561e-10, 0.079682434998738783, 0.99682029952879259, -5.0955725056089918e-10, 0.082740261358711029, 0.9965711460554596, 8.4023417043499471e-10, 0.085797317604319884, 0.99631261172982422, -8.9844500762714663e-10, 0.088853546959756169, 0.99604470140283885, -5.4322995407529717e-10, 0.091908953098364984, 0.99576741478136477, 4.2598937051441244e-10, 0.094963497994113077, 0.99548075523775048, 3.6146684707916155e-09, 0.098017162931829688, 0.99518472444606743, -1.2521791936670468e-09, 0.10106985492744167, 0.99487933158999031, -1.1005134165076313e-09, 0.10412162699490937, 0.99456457145422839, -4.010193975501064e-09, 0.10717239990513915, 0.99424045215358892, 1.3425500676456092e-09, 0.11022221567797622, 0.99390696907257758, -4.1047207993694101e-10, 0.11327094961509077, 0.99356413581272929, 8.8278017396081765e-09, 0.1163186860021079, 0.99321194278298075, -1.9581834417858879e-10, 0.11936521358942501, 0.99285041460672763, 1.8354532962949754e-09, 0.12241068664497966, 0.99247953318700965, -3.1466675896874019e-09, 0.12545496379665588, 0.99209931562257403, 1.6729396340453562e-09, 0.12849812121804088, 0.99170975231840575, 4.5368848747284751e-09, 0.13154005696127805, 0.99131085609642333, -3.820524292763583e-09, 0.13458068472044685, 0.99090263865839801, -2.9005606909382475e-09, 0.13762010353513277, 0.9904850867645506, 5.1582451018883368e-09, 0.1406582714208435, 0.99005820570353353, -1.9312542953731082e-09, 0.14369502114179672, 0.98962201920685788, 2.9914760599214496e-09, 0.14673049304792185, 0.98917650720683503, 6.5792087056149029e-11, 0.14976453508604312, 0.98872169189841352, -1.4728632237448469e-09, 0.15279717611283852, 0.98825756914477625, 2.7296200542925586e-09, 0.1558284145954632, 0.98778413897200457, -1.9550494799458473e-09, 0.15885813120591186, 0.98730142010926192, -7.2839912920730221e-10, 0.1618863892638141, 0.98680940255508554, 2.5789892964256378e-09, 0.1649131364723703, 0.98630809457230217, 7.3469792576108528e-09, 0.1679383404815416, 0.98579750141512623, -6.8152940582710819e-10, 0.1709618845411694, 0.98527764312103006, -9.6792194398398479e-10, 0.1739838673985849, 0.98474850286001026, 6.6657900830990879e-09, 0.17700426163322378, 0.98421008497356766, -1.8840811306930494e-09, 0.18002288976107342, 0.98366242134284687, 3.1672526268344825e-09, 0.18303990751936794, 0.98310548378864271, 2.5295304739714197e-10, 0.18605515322504634, 0.98253930199173434, -8.2328841660705976e-10, 0.18906865907023115, 0.98196387008758357, 2.1089624560599596e-09, 0.19208041005415, 0.98137919076849678, 1.5584749948893695e-09, 0.19509033162016132, 0.98078527849286945, 8.6128333839918803e-09, 0.19809846376151738, 0.98018212524781678, 1.566280827258737e-09, 0.20110464448226589, 0.97956976370632298, 3.5745637422257559e-09, 0.20410898807964667, 0.97894817073484675, -3.2583361495031049e-09, 0.2071113561633913, 0.97831737495976268, 2.7707582339608905e-09, 0.21011185390103795, 0.97767735416663348, -4.2522157947533223e-09, 0.21311029381238117, 0.97702814835152041, 9.407016693385728e-10, 0.21610680284715328, 0.97636973005269967, -2.7626462367758009e-09, 0.21910122322041925, 0.97570213384173554, 2.1512653602950849e-09, 0.22209363415242503, 0.97502534206499925, -8.7943365090215053e-09, 0.22508385752126106, 0.97433939522291135, -1.3352382949305408e-09, 0.22807207500244897, 0.97364425156423395, 2.376363966716255e-09, 0.23105812280776902, 0.97293994875560019, 3.0497118025385461e-10, 0.23404196044651454, 0.97222649663046734, 2.7422488307893289e-09, 0.23702362273343514, 0.97150388690232126, 2.9988943966996473e-10, 0.24000302427792949, 0.97077214027672198, -5.2347291085164116e-09, 0.24298014799818599, 0.97003126118634941, 3.2653907702795415e-09, 0.24595507022259128, 0.96928123031027491, -1.0894593630017546e-09, 0.24892759911592002, 0.96852209597839523, -2.1866383009827572e-10, 0.25189781682461476, 0.96775383743955923, -3.585845592679604e-09, 0.25486563781801985, 0.96697647678711085, 3.5735430517491729e-09, 0.2578311238562484, 0.96618999765627045, 1.2024867906967884e-09, 0.26079412520926237, 0.96539443972727312, 1.1183973400163971e-09, 0.26375468575309813, 0.96458979143638279, -5.6993235131308495e-10, 0.26671275402362538, 0.96377606675053573, 9.2434763315774049e-10, 0.26966833116560079, 0.96295326530749137, 3.0890653854309669e-09, 0.27262137412392706, 0.96212139897768711, -1.1543851909101566e-08, 0.27557174958720676, 0.9612805057991376, 7.4874516586831419e-09, 0.278519734568551, 0.96043050631259308, -6.2277686896372586e-09, 0.28146490037750882, 0.95957152409577007, 5.0829592410095437e-09, 0.28440756782955218, 0.95870346581269783, -3.0390984728789761e-09, 0.28734744125482448, 0.95782641851449535, -2.7341927333335292e-09, 0.29028466081476362, 0.95694034071913681, 1.4600410255805052e-09, 0.29321917146473891, 0.95604524866008933, -6.2240616410802474e-09, 0.29615085089098225, 0.95514117988732283, -6.4163921606463781e-09, 0.29907978783797107, 0.95422810716662199, -4.1447148282913737e-09, 0.30200592449322128, 0.95330604821903586, -4.3845703456835139e-10, 0.30492922711169801, 0.95237501355981746, -5.3239376796021531e-10, 0.3078496368588623, 0.95143502199880503, -7.7504824086416235e-10, 0.31076714812096129, 0.9504860754628468, -1.6505960834245492e-09, 0.31368173055133369, 0.94952818384623028, 2.6548929027425672e-10, 0.31659337713847852, 0.94856134938761505, -2.1763488372572581e-09, 0.31950201785834836, 0.94758559538674059, -8.6341035709924796e-10, 0.32240767366579087, 0.94660091483233466, -4.455577025386237e-09, 0.32531026568978139, 0.94560733448765288, 2.5935820713307578e-09, 0.32820985897233207, 0.94460483191298672, 3.9913393631851868e-09, 0.33110632942361956, 0.94359344985836857, -1.0599716125658798e-08, 0.33399958866665197, 0.9425732198458151, 3.1088838101100968e-09, 0.33688987178405277, 0.9415440586023176, -2.6682363749030991e-09, 0.33977686863922035, 0.94050607628963567, -4.2323378135478151e-10, 0.34266071481373156, 0.93945922451341257, -9.5538056632538115e-11, 0.34554132440068108, 0.9384035342705308, -1.1780352249246562e-09, 0.3484186733114259, 0.93733901449150503, -1.1539197791732292e-08, 0.35129268820357329, 0.93626569264002568, 1.033394557425904e-09, 0.35416353149264579, 0.93518350763936042, 6.936202723295537e-10, 0.35703096530434053, 0.93409254884826631, 5.854304464814053e-09, 0.35989507085389627, 0.93299278559647447, -1.3559395758977466e-09, 0.36275571642809773, 0.93188426867220886, -1.3724797409686573e-09, 0.36561298977826351, 0.93076696423186367, 2.7493280152501853e-09, 0.36846684601248869, 0.9296408894780871, 1.3802285495079225e-09, 0.37131720200405743, 0.92850607725306677, -4.3643446426266941e-09, 0.37416403754133554, 0.92736253591072249, 1.3505429136140279e-09, 0.37700741807597105, 0.92621023893913523, -1.4282816963862022e-08, 0.37984712590866393, 0.92504927487076472, 1.2475021557434296e-08, 0.38268350478141988, 0.92387950251545736, -1.4590044172813066e-09, 0.38551604538533757, 0.92270113186797942, -5.5976218105158182e-10, 0.38834504345777893, 0.92151404070788623, -1.0532098054794048e-08, 0.39117032340007718, 0.92031830259485714, 1.3503587151242336e-08, 0.39399211804375811, 0.91911381826158689, -2.2710503644329805e-08, 0.39680985643749506, 0.91790083224390562, 1.168928065453656e-09, 0.39962420657828018, 0.91667905698596608, -1.2957533199187132e-08, 0.40243457632854235, 0.91544874885231375, 2.7018556669755611e-08, 0.40524146920360754, 0.91420968690869908, -9.9073076131883653e-10, 0.40804415718183928, 0.9129621929684506, -7.4980600200946768e-09, 0.41084312810588192, 0.91170605136094929, 2.3056466458282188e-09, 0.41363832542782009, 0.91044128626576948, -4.1900935499050718e-10, 0.41642955770405843, 0.90916798418686207, 2.5444446549727218e-09, 0.4192169028777965, 0.90788610978555462, 2.2334532973067311e-09, 0.42200028352223934, 0.90659569859290068, 2.6982485343274298e-08, 0.42477983468945379, 0.90529668730267665, 1.4302902751062163e-09, 0.42755510155423393, 0.90398928928110023, -4.626385510797526e-09, 0.43032645510077955, 0.90267333074617684, -4.2888544660169003e-09, 0.4330937945639029, 0.90134885871688986, 2.1229143859491373e-09, 0.43585709192726546, 0.9000158862024088, -3.6358097643685028e-09, 0.43861621800878881, 0.89867447571390757, -3.994878303159588e-09, 0.44137124620836649, 0.8973245917840843, 2.5907548190096108e-09, 0.44412215915514036, 0.89596624252667922, 7.8246656098901468e-09, 0.44686888414430531, 0.89459946366160059, -4.9671867868861241e-09, 0.44961130177729475, 0.89322431522777435, 4.9675868141196844e-09, 0.45234961507014232, 0.89184069527348553, -4.8611474984250336e-09, 0.45508355992893146, 0.89044873714459871, 2.3605262050585551e-09, 0.45781331678490772, 0.88904834906454988, 1.9807998019594208e-09, 0.460538722005565, 0.88763961467111241, 5.0070432244808671e-09, 0.46325981143258066, 0.88622251557464382, -2.5475836856747591e-09, 0.46597648160507171, 0.88479710588979565, -3.0439082226996206e-09, 0.46868880514111583, 0.88336334762961111, -6.0965166248649894e-10, 0.47139673344775029, 0.88192126615406585, -2.7190485973882517e-11, 0.47410021450012785, 0.88047088913315741, 3.498621975461802e-09, 0.47679924938619966, 0.87901221594739887, -3.9967942011553959e-09, 0.47949373562271025, 0.87754530224859528, 1.1935213789504573e-08, 0.48218383777663393, 0.87607005803588378, 2.6250401158334213e-09, 0.48486926242588524, 0.87458664428093003, 2.4212431504189169e-09, 0.48755017343093404, 0.8730949710011312, -1.4811098791867039e-08, 0.49022640217688473, 0.87159513227685437, -3.1396157895713372e-09, 0.4928981750657625, 0.87008700083200929, 1.6654035095431396e-10, 0.49556526273464829, 0.86857070545277948, 4.4379445396369732e-09, 0.4982276911498702, 0.86704623162289884, 6.4470301319730083e-10, 0.50088538611725342, 0.86551362206158833, 1.268844751489695e-09, 0.50353839061364325, 0.863972852107184, -3.2156996510046554e-10, 0.50618664360264176, 0.86242395713378239, -6.604389535258548e-10, 0.50883013897080165, 0.86086694074923953, -1.9934919964770259e-09, 0.51146883967480294, 0.859301824763401, -7.8810553638675174e-09, 0.51410270172010319, 0.85772863545767819, -4.4756114578325956e-08, 0.51673155825961603, 0.85614747368580668, -1.2043419955753976e-08, 0.51935592550029697, 0.85455802766560562, -1.9129692274688637e-09, 0.52197528268496007, 0.85296061120426658, -1.1301724417434045e-09, 0.52458967663292511, 0.85135519683041994, 1.2944660343627845e-09, 0.52719914169316551, 0.84974176371294685, 2.9051107366950824e-08, 0.52980377949665314, 0.84812024809637798, -3.6264315439460404e-09, 0.53240310858944351, 0.84649095090514537, 8.3096960562523137e-09, 0.53499766399805027, 0.8448535373167525, -7.7095593936649109e-09, 0.53758703545013997, 0.84320826568287977, 1.3435870499511893e-08, 0.54017154377401611, 0.84155493183558505, -1.8160276060719127e-09, 0.54275077528095983, 0.83989380038902361, 4.0090852443386282e-09, 0.54532500953678031, 0.83822469181819648, 2.114215454096513e-08, 0.54789417030015797, 0.83654765444121681, -7.2350125057685233e-09, 0.55045793498463602, 0.8348629000096065, -4.3224814005871082e-09, 0.55301668295199913, 0.83317017972126683, -5.4763648671407594e-08, 0.55556994691906347, 0.83146980346874233, 8.8801536679561366e-09, 0.5581185775176235, 0.82976120265393494, -5.852610479584186e-10, 0.56066157315236298, 0.82804504731947859, -2.6666227281868515e-08, 0.5631992055646845, 0.82632115720905286, -5.8360515170496896e-09, 0.56573178054675333, 0.8245893235298406, 3.4922258737579703e-08, 0.56825913322230437, 0.82284965668670929, -1.0218628901381699e-08, 0.57078069316763658, 0.82110255163838841, -1.7283059983164151e-09, 0.57329715780052881, 0.81934752630238383, -1.3793890474289583e-09, 0.57580818433187286, 0.8175848181420885, -3.1713452142145826e-09, 0.57831378015561541, 0.81581442233030088, 3.5002601095346364e-09, 0.58081397599868867, 0.81403631693223288, -4.9245421906984888e-09, 0.58330862780519377, 0.8122506046338297, -3.8630498938596602e-09, 0.5857978377848122, 0.81045721247123148, 6.2591607313811082e-09, 0.58828158002504394, 0.80865615845255134, -9.0502462607444656e-09, 0.59075965597799196, 0.80684758713697879, -4.2490328339139793e-09, 0.59323227354750729, 0.80503134698075918, 2.0205713491261434e-10, 0.59569930551215544, 0.80320753072436746, -6.7478212878446797e-09, 0.59816067301974585, 0.80137619708384444, 1.9877954324232228e-08, 0.60061657924356704, 0.79953719409278012, 5.1287326052751325e-09, 0.60306662424575763, 0.79769082150970383, -1.6732839835942315e-09, 0.60551103303725173, 0.79583691097495612, 1.165264645885955e-08, 0.6079498430992718, 0.79397543304284346, -1.3810321719542884e-08, 0.61038273754299321, 0.79210663026484096, 2.9315473659696778e-09, 0.61281009698501987, 0.79023021014968198, -3.2593405335790138e-09, 0.61523157443604826, 0.78834644022595879, 1.8887585939708629e-11, 0.61764730803113588, 0.78645521352578696, -7.0486546738246503e-09, 0.62005717701684326, 0.78455662461667042, 9.8479215060454095e-10, 0.6224612842169035, 0.78265059231501466, -2.566023407646334e-09, 0.62485947555471566, 0.78073723864657929, 1.0516099741009199e-08, 0.62725186695513491, 0.77881647093605932, 3.0372553500024324e-09, 0.6296382537407853, 0.77688845365744397, -8.6071013372102101e-09, 0.63201869403033428, 0.77495314077445399, -5.1348892360358889e-09, 0.63439325922365286, 0.77301047383045929, -2.3897712572718e-08, 0.63676174545864284, 0.77106061987399055, 8.9193566976675243e-09, 0.63912448796584165, 0.76910330182778486, -1.6070920316746395e-08, 0.64148093534552353, 0.76713897671036912, -7.4333861010966729e-09, 0.64383150715239168, 0.76516729569283071, 1.135280203179434e-08, 0.64617606742288691, 0.76318837117050775, 1.2903970947331089e-10, 0.64851440163928042, 0.76120238495845904, -7.2939876083477628e-09, 0.6508466502022211, 0.75920921880634962, -1.0988975285286529e-09, 0.65317283772557011, 0.75720885101636659, -3.6841576595003644e-09, 0.6554928355180285, 0.75520139207004566, 2.0158295865879694e-10, 0.65780669425105232, 0.75318679821044565, 6.0693567949243743e-09, 0.66011437071302226, 0.75116510673629577, 1.2428513856965751e-08, 0.66241583609072197, 0.74913634279497465, -6.6884390248489467e-09, 0.66471094680666409, 0.74710063391446013, -1.9633781545014273e-09, 0.66699991311240436, 0.74505779366975622, -7.1254454697688985e-09, 0.66928255508180212, 0.74300798209923324, 2.1126507904201119e-09, 0.67155896468253429, 0.74095111644056699, -1.1260275378788265e-08, 0.67382894810218297, 0.73888737213428246, 1.9653870822478048e-08, 0.67609279456398752, 0.73681648538741162, 1.6040188954846712e-08, 0.67835011717939875, 0.73473880972948202, 1.1975920671813967e-09, 0.68060100330845141, 0.73265426655109933, 1.0653107146429974e-09, 0.68284555127530255, 0.7305627646571704, -1.5156629229462482e-08, 0.68508359839964195, 0.72846445569004814, -2.3228977075184787e-09, 0.68731533029040426, 0.72635916511584853, -1.5350985899109659e-09, 0.68954053775148005, 0.72424708960229844, 4.1033660913569747e-09, 0.69175927698221795, 0.72212817609413293, -4.9887353553712543e-09, 0.693971438321068, 0.7200025297140199, -4.5996229325329097e-09, 0.69617711074481503, 0.71787006517544771, -3.6251987384217088e-09, 0.69837623310620234, 0.71573084119128982, 1.0415403817187752e-09, 0.70056879861308419, 0.71358486419613743, -9.1236852933773704e-09, 0.70275470367380055, 0.71143223603119699, -4.1936464023617503e-09, 0.7049340616869505, 0.70927284501349597];


            int neg = 0, isCos = 1;

            (int i, double h, double l, double err1) = reduceFast(x, T);

            neg = neg ^ (i >> 10);
            i = i & 0x3ff;

            isCos = isCos ^ (i >> 9);
            neg = neg ^ (i >> 9);
            i = i & 0x1ff;

            if ((i & 0x100) != 0)
            {
                isCos = (isCos != 0 ? 0 : 1);
                i = 0x1ff - i;

                h = 0.00048828125 - h;
                l = -l;
            }

            double sh, sl, ch, cl;

            h -= SC[i * 3 + 0];

            double uh, ul;
            (uh, ul) = aMul(h, h);
            ul = FusedMultiplyAdd(h + h, l, ul);

            (sh, sl) = evalPSfast(h, l, uh, ul);
            (ch, cl) = evalPCfast(uh, ul);

            double err;

            if (isCos == 0)
            {
                (sh, sl) = sMul(SC[i * 3 + 2], sh, sl);
                (ch, cl) = sMul(SC[i * 3 + 1], ch, cl);
                (h, l) = fastTwoSum(ch, sh);
                l += sl + cl;

                err = 2.2565487110446595e-21;
            }
            else
            {
                (ch, cl) = sMul(SC[i * 3 + 2], ch, cl);
                (sh, sl) = sMul(SC[i * 3 + 1], sh, sl);
                (h, l) = fastTwoSum(ch, -sh);
                l += cl - sl;

                err = 2.5477162866633252e-21;
            }

            ReadOnlySpan<double> sgn = [1.0, -1.0];
            h *= sgn[neg];
            l *= sgn[neg];
            return (err + err1, h, l);
        }

        static double cosAccurate(double x, ReadOnlySpan<ulong> T)
        {
            ReadOnlySpan<ulong> Shi = [0x0000000000000000, 0xc90fc5f66525d257, 0xc90f87f3380388d5, 0x96cb587284b81770, 0xc90e8fe6f63c2330, 0xfb514b55ccbe541a, 0x96c9b5df1877e9b5, 0xafea690fd5912ef3, 0xc90aafbd1b33efc9, 0xe22a7a6729d8e453, 0xfb49b98e8e7807f6, 0x8a342eda160bf5ae, 0x96c32baca2ae68b4, 0xa351cb7fc30bc889, 0xafe00694866a1b44, 0xbc6dd52c3a342eb5, 0xc8fb2f886ec09f37, 0xd5880deafc18b534, 0xe214689606bf1676, 0xeea037cc04764844, 0xfb2b73cfc106ff68, 0x83db0a7231831d8f, 0x8a2009a6b84d9402, 0x9064b3a76a22640c, 0x96a9049670cfae65, 0x9cecf8962d14c822, 0xa3308bc93904ad69, 0xa973ba526a6850d9, 0xafb68054d520c60b, 0xb5f8d9f3cd8945d6, 0xbc3ac352ead90abe, 0xc27c389609850433, 0xc8bd35e14da15f0e, 0xcefdb7592542e1e9, 0xd53db9224ae01bca, 0xdb7d3761c7b263b6, 0xe1bc2e3cf616a7ac, 0xe7fa99d983ee098f, 0xee38765d74fe4897, 0xf475bfef2551f5b9, 0xfab272b54b9871a2, 0x8077456b7dc2d967, 0x8395023dd418e919, 0x86b26de5933c2e8e, 0x89cf8676d7abb55b, 0x8cec4a05f12739e8, 0x9008b6a763de75b7, 0x9324ca6fe9a04b4e, 0x964083747309d113, 0x995bdfca28b53a54, 0x9c76dd866c689dcc, 0x9f917abeda4498df, 0xa2abb58949f2ced7, 0xa5c58bfbcfd4436a, 0xa8defc2cbe2f8fcc, 0xabf80432a65ef190, 0xaf10a22459fe32a6, 0xb228d418ec1869ad, 0xb5409827b25591f0, 0xb857ec684627fa4c, 0xbb6ecef285f98a3a, 0xbe853dde9658dc60, 0xc19b3744e3262dcd, 0xc4b0b93e20c0213f, 0xc7c5c1e34d3055b2, 0xcada4f4db157cf77, 0xcdee5f96e21b332c, 0xd101f0d8c18ed1c1, 0xd415012d802284f0, 0xd7278eaf9dcd5b55, 0xda399779eb391377, 0xdd4b19a78aed6515, 0xe05c1353f27b17e5, 0xe36c829aeba6e720, 0xe67c659895943123, 0xe98bba6965ef725f, 0xec9a7f2a2a188aeb, 0xefa8b1f8084ccdfc, 0xf2b650f080d0da8d, 0xf5c35a316f1a3c80, 0xf8cfcbd90af8d57a, 0xfbdba405e9c00cca, 0xfee6e0d6ff6fc5a4, 0x80f8c035cfee8d76, 0x827dc071bfed6ffa, 0x8402702f5b30f2a9, 0x8586ce7ededc809d, 0x870ada70ba4e6d49, 0x888e93158fb3bb04, 0x8a11f77e349bc245, 0x8b9506bbb28bb922, 0x8d17bfdf47921ac8, 0x8e9a21fa66d9ee8d, 0x901c2c1eb93dee39, 0x919ddd5e1ddb8b33, 0x931f34caaaa5d23a, 0x94a03176acf82d45, 0x9620d274aa290339, 0x97a116d7601c3515, 0x9920fdb1c5d5783d, 0x9aa086170c0a8d86, 0x9c1faf1a9db554af, 0x9d9e77d020a5bbe6, 0x9f1cdf4b76138b02, 0xa09ae4a0bb300a19, 0xa21886e449b78316, 0xa395c52ab8829dfc, 0xa5129e88dc17976a, 0xa68f1213c73b5124, 0xa80b1ee0cb823c27, 0xa986c40579e11c0a, 0xab020097a33da341, 0xac7cd3ad58fee7f0, 0xadf73c5ced9db0f3, 0xaf7139bcf5349ac6, 0xb0eacae4461013ed, 0xb263eee9f93e3088, 0xb3dca4e56b1e54bb, 0xb554ebee3bf0b58e, 0xb6ccc31c5065afee, 0xb8442987d22cf576, 0xb9bb1e4930848ead, 0xbb31a07920c7b256, 0xbca7af309efd7182, 0xbe1d4988ee67380c, 0xbf926e9b9a0f2127, 0xc1071d8275561f9b, 0xc27b55579c81f96d, 0xc3ef1535754b168d, 0xc5625c36af6a222f, 0xc6d5297645257e8d, 0xc8477c0f7bde8a98, 0xc9b9531de49eb968, 0xcb2aadbd5ca47af5, 0xcc9b8b0a0deff5d4, 0xce0bea206fcf9192, 0xcf7bca1d476c516d, 0xd0eb2a1da855fefd, 0xd25a093ef50f2482, 0xd3c8669edf98d680, 0xd536415b69fe4c54, 0xd6a39892e6e04764, 0xd8106b63fa0048a0, 0xd97cb8ed98cb93f5, 0xdae8804f0ae6015b, 0xdc53c0a7eab49b35, 0xddbe791825e8099e, 0xdf28a8bffe06ca56, 0xe0924ec008f734fd, 0xe1fb6a3931894b38, 0xe363fa4cb8005482, 0xe4cbfe1c329c453a, 0xe63374c98e22f0b4, 0xe79a5d770e6905dc, 0xe900b7474edad637, 0xea66815d4304e6c8, 0xebcbbadc371c4aaa, 0xed3062e7d086c6f0, 0xee9478a40e62bf86, 0xeff7fb354a0eecb1, 0xf15ae9c037b1d8f0, 0xf2bd4369e6c126d3, 0xf41f0757c2889e84, 0xf58034af92b102a7, 0xf6e0ca977bc6ac45, 0xf840c835ffbfed66, 0xf9a02cb1fe833a0d, 0xfafef732b66d1742, 0xfc5d26dfc4d5cfda, 0xfdbabae12696eea4, 0xff17b25f38907dad, 0x803a06415c170525, 0x80e7e43a61f5b6cb, 0x819572af6decac84, 0x8242b1357110d372, 0x82ef9f618dc5b70e, 0x839c3cc917ff6cb4, 0x8448890195846099, 0x84f483a0be2f0403, 0x85a02c3c7c2f5ca5, 0x864b826aec4c74e5, 0x86f685c25e25acf5, 0x87a135d95473ec89, 0x884b9246854ab50b, 0x88f59aa0da591421, 0x899f4e7f712a765e, 0x8a48ad799b6759f3, 0x8af1b726df15e13c, 0x8b9a6b1ef6da4502, 0x8c42c8f9d2372644, 0x8cead04f95cdbf66, 0x8d9280b89b9df49b, 0x8e39d9cd73464364, 0x8ee0db26e24390f8, 0x8f87845de430d777, 0x902dd50bab06b1b7, 0x90d3ccc99f5ac58b, 0x91796b31609f0c54, 0x921eafdcc560f9c5, 0x92c39a65db88809d, 0x93682a66e896f544, 0x940c5f7a69e5ce1c, 0x94b0393b14e54156, 0x9553b743d75ac03f, 0x95f6d92fd79f4fba, 0x96999e9a74ddbde3, 0x973c071f4750b49c, 0x97de125a2080a8ed, 0x987fbfe70b81a708, 0x99210f624d30facb, 0x99c200686472b4a8, 0x9a6292960a6f0ab0, 0x9b02c58832cf95c0, 0x9ba298dc0bfc6a88, 0x9c420c2eff590e5f, 0x9ce11f1eb18147b1, 0x9d7fd1490285c9e3, 0x9e1e224c0e28bc94, 0x9ebc11c62c1a1dfb, 0x9f599f55f0340061, 0x9ff6ca9a2ab6a26d, 0xa0939331e8846237, 0xa12ff8bc735d8af6, 0xa1cbfad9521bfd1b, 0xa267992848eeb0c0, 0xa302d34959951243, 0xa39da8dcc39a38e5, 0xa4381983048ff747, 0xa4d224dcd849c5b0, 0xa56bca8b391785db, 0xa6050a2f60002049, 0xa69de36ac4fbfadc, 0xa73655df1f2f489e, 0xa7ce612e65243291, 0xa86604facd04d969, 0xa8fd40e6ccd52ffd, 0xa99414951aacae5e, 0xaa2a7fa8acefdd63, 0xaac081c4ba89ba8a, 0xab561a8cbb24f410, 0xabeb49a46764fd15, 0xac800eafb91ef9a9, 0xad146952eb9282af, 0xada859327ba24151, 0xae3bddf3280c620d, 0xaecef739f1a2df10, 0xaf61a4ac1b83a1de, 0xaff3e5ef2b507c06, 0xb085baa8e966f6da, 0xb117227f6117f9f9, 0xb1a81d18e0df4889, 0xb238aa1bfa9ad507, 0xb2c8c92f83c1eb87, 0xb35879fa959c323c, 0xb3e7bc248d78802e, 0xb4768f550ce389fd,];
            ReadOnlySpan<ulong> Slo = [0x0000000000000000, 0x480f7956b6470765, 0xcb3ff35bd4d81baa, 0xb767005691b9d9d1, 0xf1d7d06db39ea9fc, 0xd784e031f9af76d6, 0xf91ee371d6467dca, 0xf56e3c87ae3c56df, 0xc539edcbfda0cf2c, 0x850021e392744a4f, 0xb21ccebc9caac3, 0xde5b1068d174be9c, 0x37b2dd49d5fca3c0, 0xb56007d16d4ad5a3, 0xcd34d2751c2e1da7, 0xf10bfca3d6464012, 0x6a17954b2b7c5171, 0x73d1472472f4a390, 0x438b4a73aecd2541, 0xc4e92d01a2f42935, 0xf0a0e36a000c7350, 0x60e782313f6161af, 0x77724a2b2a669bc4, 0x56e0a8b0d177b55d, 0xf77574094d3c35c4, 0x50ffe4f5caa7f1fa, 0xdec1b7f2768bdafa, 0x76f8c63986598c79, 0xfdd2fc0936594c2d, 0x924bef13600f9852, 0xeb13e106732687f1, 0xb228a03916371f6f, 0xc7396c894bbf7389, 0x6b47b8c44e5b037e, 0x7337412cf70716cb, 0xbb286d23e11c8337, 0x31883b30137c6e62, 0xeeb8f9c33340a2f2, 0xed16b994af6c18ae, 0x14e1a5488eaeab96, 0x704729ae56d78a37, 0x3eac8308f1113e5e, 0xdb1f70118c9c2198, 0xc5a9decdfaad4db5, 0x97965c9860c34e44, 0xdcdca90cc73b116a, 0xa6e3df5975cca9da, 0x899c4de737feec22, 0xa89a11e07c1fe, 0x49c4863de522b217, 0xe7bc08111d0bfca4, 0xf3ff913a4aadb85e, 0xa5dbee6084ee1260, 0x69fcb11e19f58619, 0xcd12a1f6ab6b095, 0x8c95c4c91179176b, 0x3feef3bb58b1f10d, 0x16031a34d4fc855d, 0xcd73fb5d8d45d302, 0x187e26d290714d70, 0xbddd8a0365d6b1d3, 0xdfe1b074e22fc666, 0xad5a41de48f6b26f, 0xdab4e426409b23a0, 0x5cc8c00e4fccd850, 0xfa6171200ab2efc3, 0x65a3132adfb7dfd5, 0xaadb580a1eba209f, 0xdf4005ef6a64aa02, 0x1779df36d1cc8912, 0xcbabaeb97af8e8aa, 0xece7f445cecf1e28, 0xebc61ade6ca83cd, 0x26a0eecdb4f16266, 0x82b0aecadf808123, 0xb91caf23416e7e80, 0x7244ee20f591983b, 0x1050cdf22f34182f, 0x587f3fa044e2d27d, 0x643720de93ba81bd, 0x4221dc4ba772598d, 0xd24d3023da491920, 0x8b74fe2508ab8fc2, 0xfd958d68e8b49e6b, 0xfb4c92369f0cf008, 0xcb07b25a7b0372a7, 0x9d3dc689006896f4, 0x9d52755ece3f70, 0x984156f553344306, 0xa66d1d936c38c329, 0x575f33366be0afef, 0xcb590d74f64e77c9, 0xf2be3ecae62789d4, 0x632b9cff5cfee724, 0x609c464b3dd676ec, 0x6a1ff8bfe6396e28, 0xae4ba773da6bf754, 0xe06a955a5b8e301d, 0xfc8b7184b21f2d50, 0x9dd1eedf18a2e4df, 0x9ffa0d23f3c26c62, 0xdab6b478577e7be5, 0xdb895384528d0d60, 0x98dbd3555ebcdefe, 0x2f895f44a303cc0b, 0xd29d23a624acd00c, 0x2be036401ba87cc2, 0x82d9495ead5be348, 0x17218792857f4c5a, 0x3269f4702b88324a, 0x8e3bdf8085321556, 0xc1654b64a0081b46, 0x811f953984eff83e, 0x9a5318ac6fe94e4d, 0x9fe5f4ea48965e2c, 0x63c66682bae74898, 0x695a5332090bb09b, 0x992d96e5021e3c37, 0x971f4da709ad4378, 0x35ebacd79f209137, 0x9cc3ef36746de3b8, 0xcdb0531c4e58484b, 0x55b92083658bb897, 0xa4b0d21fc5036a5, 0xd1f90f79f46c7e01, 0x91a1b5eb79658c67, 0x721853f8e528a934, 0xcdc2bd470675104d, 0x3122c2a59efddc37, 0xf4ff2895ab6ebe89, 0x14d24739de27e2e9, 0x4ce0246ad4fa74, 0x4319e5ad5b0dcb84, 0xfaa3dfe675a65ee2, 0x2e663b3c7555a6c3, 0x3c540a9eec47af38, 0xa81290bdbaad62e4, 0xb9302788604e88f1, 0x721fc87ba1d42456, 0x87967926fdcecec4, 0x1df22346611c6b4b, 0x3090d44db12c418c, 0xa573f2aa90434ba5, 0x2e349483e3fb2a6a, 0x362cb974182e3030, 0x3ccca3982328ed8b, 0x1a5bd9269d408d7e, 0xcce2634be2bf54df, 0x8aa895d5bf3e84ea, 0xf7a1f9bd9ba13b6b, 0x7b32c72e31824e51, 0xd40e9e6b989f89e5, 0x2872ce1bfc7ad1cd, 0xf1b65cc5fd780262, 0x431626c10485bdda, 0xcc39cfcc29960b1, 0x1d90f780ae951140, 0xc71debc372b6f9d4, 0x2a24164daec85ccb, 0x527233b40d3432bb, 0x6c48e9e3420b0f1e, 0x7f232aee178c6323, 0x3c7f10db458c337c, 0x93fa6107c4327527, 0xe1079824233fef46, 0xa9a56012067c570c, 0x8da894471de1a18, 0x343fbf4a7d42af3, 0x27c07c911290b8d1, 0x2377c3799c052fa, 0xa9c6ba50490539f, 0x6f53873e2f1477ff, 0x5ca183dc973abc22, 0x9fba97fdf0c4d24c, 0x6fb2123fedfa6e22, 0x91a965931f1a200a, 0xbfd79717f2880abf, 0x246efcff30cb064a, 0x51917cac857fd5f5, 0x327888fe4b62687b, 0x85043222c9bdd18d, 0x7e0b9b07548471a2, 0x4e091160e2430712, 0x4f14c8afe4560291, 0xb892ca8361d8c84c, 0xc88302a31afce54a, 0x660558a02136130a, 0x545f7d79ead8fa19, 0x21a6675f51580bc4, 0x101a5adbcb9ffb43, 0x4d49cbaf15aecd80, 0xde2d43c6b67a7cbe, 0xbba4cfecbff54867, 0xaf0e2345f3bd24b4, 0x9311a82459aa0f72, 0xb144016c7a30b39a, 0x9d1072e09b72292, 0x6714fe6925b78cc4, 0x33d0a284a8c954ad, 0x1f8481e704e4a767, 0xb17821911e71c16e, 0x1489a97671a42, 0xd6c7af02d5c16fd9, 0xac0106650f4ef023, 0xd9f8e1a446e973b9, 0xa7a7556c3b33abc1, 0xc0a03934f0cce19b, 0xd243aa0843a2c144, 0x19cec845ac87a5c6, 0xc4b992a37fb9b9bd, 0x1ab42d43235757b6, 0x7e92c655656e6b85, 0x698b94f50326a043, 0x9a5614e8ffbeac6f, 0xc7fd954194e6d8aa, 0x3e93627de8fd5779, 0xe25e39549638ae68, 0x2cad377d5c9c35d8, 0xcc141e10c6460c8b, 0xa88d5f46834bbf8d, 0x22cc118a0c118aa0, 0x7cec6df5bea167cf, 0x71acea2819360c35, 0x166c36e7bb3c402f, 0x3b5167ee359a234e, 0x9443372e20d4377c, 0xca9a8a720d4c69c, 0xbf623cf5301a2dde, 0x23d251cc8d7975cc, 0x189d39ffe11aaa2b, 0x8c33ebf3aa8501fb, 0x9b3ad6e4022183d9, 0x149f6e75993468a3, 0x6b2a39f856a69781, 0x3463a2c2e6e9cc55, 0x6cc14c4f53e2e82d, 0xd147625fda929af8, 0xb714ee81b53b4b9d, 0xe1b3dfc4dbda9bfd, 0xf17cee69b0d2ecde, 0x1becda8089c1a94c, 0xf86ba0dde982fb59, 0x44bf16268608db96, 0x9d30d4cfeb04f1fb, 0x3d53817865422565, 0xf74d099042e8f326, 0xa89a9b8f726b95bf, 0x8c679e67fc462d51, 0xe4cad00d5c94bcd2, 0x8d8be132d576e614, 0x24784f32c3e3e5bd, 0x8cc7d4bd05ffd5ae, 0xac9f7ebbc469ef59, 0x5d6635109164f740, 0xa156468ef6c18c60, 0x4a85350f69018c55,];
            ReadOnlySpan<int> Sex = [128, -8, -7, -6, -6, -6, -5, -5, -5, -5, -5, -4, -4, -4, -4, -4, -4, -4, -4, -4, -4, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,];
            ReadOnlySpan<byte> Ssgn = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

            ReadOnlySpan<ulong> Chi = [0x8000000000000000, 0xffffb10b10e80e95, 0xfffec42c7454926b, 0xfffd3964bc6275ba, 0xfffb10b4dc96dabb, 0xfff84a1e29de8571, 0xfff4e5a25a8d095b, 0xfff0e343865bbb13, 0xffec4304266865d9, 0xffe704e71533c508, 0xffe128ef8e9fc17a, 0xffdaaf212fed72db, 0xffd3977ff7bae4e9, 0xffcbe2104600a0a9, 0xffc38ed6dc0ef98b, 0xffba9dd8dc8b1e83, 0xffb10f1bcb6bef1d, 0xffa6e2a58df6947d, 0xff9c187c6abade6a, 0xff90b0a7098f6443, 0xff84ab2c738d6a03, 0xff780814130c893c, 0xff6ac765b39e1e19, 0xff5ce92982087867, 0xff4e6d680c41d0a9, 0xff3f542a416b0134, 0xff2f9d7971ca0364, 0xff1f495f4ec430d7, 0xff0e57e5ead848d1, 0xfefcc917b99839a5, 0xfeea9cff8fa2ae54, 0xfed7d3a8a29c603b, 0xfec46d1e89292cf0, 0xfeb0696d3ae4f04d, 0xfe9bc8a1105c22a5, 0xfe868ac6c3043b2e, 0xfe70afeb6d33d6a2, 0xfe5a381c8a1aa224, 0xfe432367f5b90a62, 0xfe2b71dbecd7aefc, 0xfe1323870cfe9a3d, 0xfdfa3878546c3d28, 0xfde0b0bf220c2fd4, 0xfdc68c6b356db62f, 0xfdabcb8caeba091b, 0xfd906e340eaa6401, 0xfd747472367dd6c5, 0xfd57de5867eedc39, 0xfd3aabf84528b50b, 0xfd1cdd63d0bc8735, 0xfcfe72ad6d9641f2, 0xfcdf6be7def1464c, 0xfcbfc926484cd43a, 0xfc9f8a7c2d603c60, 0xfc7eaffd720ed673, 0xfc5d39be5a5bbc4b, 0xfc3b27d38a5d49ab, 0xfc187a52063060c2, 0xfbf5314f31eb7375, 0xfbd14ce0d191516e, 0xfbaccd1d0903bb09, 0xfb87b21a5bf5b917, 0xfb61fbefadddb985, 0xfb3baab441e770f7, 0xfb14be7fbae58156, 0xfaed376a1b42e559, 0xfac5158bc4f4211f, 0xfa9c58fd796837d4, 0xfa7301d859796671, 0xfa491035e55da3a3, 0xfa1e842ffc96e4e0, 0xf9f35de0dde328ab, 0xf9c79d63272c4628, 0xf99b42d1d57781eb, 0xf96e4e4844d4e82a, 0xf940bfe2304e6c45, 0xf91297bbb1d6cdbe, 0xf8e3d5f1423842a0, 0xf8b47a9fb902e76c, 0xf88485e44c7af48a, 0xf853f7dc9186b952, 0xf822d0a67b9c5cb5, 0xf7f110605caf6390, 0xf7beb728e51dfcb8, 0xf78bc51f239e12c6, 0xf7583a62852a23b2, 0xf7241712d4edde49, 0xf6ef5b503c328589, 0xf6ba073b424b19e8, 0xf6841af4cc8048a4, 0xf64d969e1dfc2119, 0xf6167a58d7b59026, 0xf5dec646f85ba1c6, 0xf5a67a8adc4088ca, 0xf56d97473d446cda, 0xf5341c9f32bffeb9, 0xf4fa0ab6316ed2ec, 0xf4bf61b00b5982b7, 0xf48421b0efbf939b, 0xf4484add6b01254b, 0xf40bdd5a6688662f, 0xf3ced94d28b2ce8a, 0xf3913edb54ba2242, 0xf3530e2aea9d3966, 0xf314476247088f74, 0xf2d4eaa8233e997d, 0xf294f82394ffe320, 0xf2546ffc0e72f286, 0xf21352595e0bf350, 0xf1d19f63ae7428a2, 0xf18f574386712643, 0xf14c7a21c8cbd0f4, 0xf1090827b43725fd, 0xf0c5017ee336ca0f, 0xf08066514c055f7e, 0xf03b36c9407aa3e8, 0xeff573116df1555d, 0xefaf1b54dd2cdf0f, 0xef682fbef23ecda6, 0xef20b07b6c6c0b37, 0xeed89db66611e307, 0xee8ff79c548acd0f, 0xee46be5a0813016b, 0xedfcf21cabacd3b1, 0xedb29311c504d652, 0xed67a1673455c601, 0xed1c1d4b344c3d4f, 0xecd006ec59ea306f, 0xec835e79946a3145, 0xec3624222d227bd1, 0xebe85815c767cb00, 0xeb99fa84606ff5ff, 0xeb4b0b9e4f345617, 0xeafb8b944453f52f, 0xeaab7a9749f584fe, 0xea5ad8d8c3a91f05, 0xea09a68a6e49cd62, 0xe9b7e3de5fdedc8b, 0xe9659107077cf60f, 0xe912ae372d27045d, 0xe8bf3ba1f1aedfbb, 0xe86b397ace95c46f, 0xe816a7f595ec9232, 0xe7c187467233d508, 0xe76bd7a1e63b9786, 0xe715993ccd02fe9c, 0xe6becc4c5997af06, 0xe667710616f4fc59, 0xe60f879fe7e2e1e5, 0xe5b7105006d4c560, 0xe55e0b4d05c80388, 0xe50478cdce2246bc, 0xe4aa5909a08fa7b4, 0xe44fac3814e09856, 0xe3f4729119e798d9, 0xe398ac4cf556b732, 0xe33c59a4439cd8ec, 0xe2df7acff7c2cf83, 0xe28210095b483751, 0xe224198a0e002123, 0xe1c5978c05ed8691, 0xe1668a498f1f892c, 0xe106f1fd4b8d7c96, 0xe0a6cee232f2bb9c, 0xe046213392aa486c, 0xdfe4e92d0d8a37f5, 0xdf83270a9bbee890, 0xdf20db088aa60404, 0xdebe05637ca94cfb, 0xde5aa65869193805, 0xddf6be249c075037, 0xdd924d05b620678a, 0xdd2d5339ac8692fd, 0xdcc7d0fec8aaf2aa, 0xdc61c693a82745d5, 0xdbfb34373c974b0e, 0xdb941a28cb71ec87, 0xdb2c78a7ede238a9, 0xdac44ff490a02710, 0xda5ba04ef3c929f4, 0xd9f269f7aab88c29, 0xd988ad2f9bdf9bbb, 0xd91e6a38009da15a, 0xd8b3a1526517a48b, 0xd84852c0a80ffcdb, 0xd7dc7ec4fabdb011, 0xd77025a1e0a39d8b, 0xd703479a2f6776cc, 0xd695e4f10ea88570, 0xd627fde9f7d63e7e, 0xd5b992c8b606a351, 0xd54aa3d165cc7018, 0xd4db3148750d1819, 0xd46b3b72a2d68fc9, 0xd3fac294ff34e4d0, 0xd389c6f4eb07a41c, 0xd31848d817d70e16, 0xd2a6488487a91918, 0xd233c6408cd64236, 0xd1c0c252c9de2c86, 0xd14d3d02313c0eed, 0xd0d93696053af098, 0xd064af55d7c9b43e, 0xcfefa7898a4ef23c, 0xcf7a1f794d7ca1b1, 0xcf04176da12390ac, 0xce8d8faf5406ab8b, 0xce16888783ae13b3, 0xcd9f023f9c3a059e, 0xcd26fd2158358e7d, 0xccae7976c0691177, 0xcc35778a2bac9ca1, 0xcbbbf7a63eba0dd5, 0xcb41fa15ebff0777, 0xcac77f24736eb553, 0xca4c871d625361a9, 0xc9d1124c931fda7a, 0xc95520fe2d40a74b, 0xc8d8b37ea4ed0f62, 0xc85bca1abaf7f0a7, 0xc7de651f7ca06749, 0xc76084da43624634, 0xc6e22998b4c6608e, 0xc66353a8c232a43c, 0xc5e40358a8ba05a7, 0xc56438f6f0ec3cca, 0xc4e3f4d26ea553b6, 0xc463373a40dd06a3, 0xc3e2007dd175f5a4, 0xc36050ecd50ca830, 0xc2de28d74ac6628b, 0xc25b888d7c1fcd38, 0xc1d8705ffcbb6e90, 0xc154e09faa2ff69a, 0xc0d0d99dabd65d44, 0xc04c5bab7297d322, 0xbfc7671ab8bb84c6, 0xbf41fc3d81b430db, 0xbebc1b6619ed9116, 0xbe35c4e716999630, 0xbdaef913557d76f0, 0xbd27b83dfcbe9279, 0xbca002ba7aaf25ea, 0xbc17d8dc859ad583, 0xbb8f3af81b93095c, 0xbb062961823b1ddc, 0xba7ca46d46946802, 0xb9f2ac703cca0db3, 0xb96841bf7ffcb21a, 0xb8dd64b0720df647, 0xb8521598bb6bce26, 0xb7c654ce4adba9f2, 0xb73a22a755457448, 0xb6ad7f7a557e64f2, 0xb6206b9e0c13a892, 0xb592e7697f14dd4a,];
            ReadOnlySpan<ulong> Clo = [0x0000000000000000, 0x3031437d7eccb9df, 0x38e310779edfec68, 0x69fff9ae0dedb047, 0xb47903f7a19f8ee2, 0x8cc193c5d508e13f, 0x43366df666fd54ff, 0x5428ed0647c9e5d1, 0x5657552366961732, 0x53aa9423bb0adc21, 0x7d209f32d42d864e, 0x4fd8f038449ec436, 0x664649b4d541b9c5, 0x5595ca3f421ae09c, 0x1c676208aa3be545, 0xccfed60a91097c48, 0x421e8edaaf59453e, 0xd2c665c2da3e7844, 0x1e1862cca089938b, 0x2dabd3195a05710f, 0x519c314973ccae6b, 0x3ea4f30adda3016f, 0x1b9d5851979f28fb, 0x50a7bb6a6ee3b0f1, 0xf668633f1ab858a, 0xb085c1828f69296a, 0x27e31939e2eec09c, 0xf5971326a3540ea9, 0x1f1901544271c3f8, 0xe0abd3a9b64df725, 0xec34413e87ef2740, 0x2f88b949a72ff96c, 0x41390efdc726e9ef, 0xb7b6cc53c3abc817, 0xd3af6ee4f2101c20, 0xb4f70c910505e10, 0x2907cf2b3f6feac2, 0xd54faa364b7da8f6, 0x87b8875373a818a4, 0x8598c2c429caf7, 0x90cd1d959db674ef, 0x9bfe5c51e91cbdcd, 0xe276d247626a23fd, 0x499ddb331d19539d, 0xfac7397cc07a6470, 0xd6e270740a186977, 0x61beb8cd2696fc78, 0x6c696582f346fd91, 0xeae6bd951c1dabbe, 0x863b87258f11ad7e, 0xa06fab9f9d106709, 0xa4e064308f4999f4, 0xa3e22b4d38917e73, 0x5d582cac7cb4391c, 0x2880268f2e62955, 0x1c0d254b6c8da4bd, 0x256778ffcb5c1769, 0x9433b49289417ea2, 0x25aafd7fdba12c5f, 0x7190c94899dff1b8, 0xe63ae8632b84473c, 0x75df66f0ec3dd459, 0x61ce9d5ef5a81487, 0xb4b54683879c9c17, 0x2172a361fd2a722f, 0x2079880c450348ac, 0x4a188aa367f90ab1, 0x10655ecd5cc771d8, 0x1fe196a53fb5b237, 0xd24377c77a591e24, 0x431c393c7f62da65, 0xba5dbf4510eddc8f, 0x4504ae08d19b2980, 0x78685d850f80ecdc, 0x80e8c17bf80e8f02, 0xc0e2a1352ed7f292, 0x68fc6e4d6a920bd2, 0x9701914c7f8fbcd7, 0xac9f07f54ff5bc14, 0xb36a9dfaadafc1e1, 0xc7adc6b4988891bb, 0xa776175bd284fe05, 0xa76f7efc19aed41c, 0x730785813f78aa1e, 0x214cffcee9dd33ca, 0x4becad887680c197, 0xf99107e50d631330, 0x50ca117eb18beed7, 0x2c791f59cc1ffc23, 0xce8c455197cdf8a7, 0x119d358de0493956, 0x9dc7e5954c5a8f24, 0xc8c615e72768d6b5, 0xed0dd4bf62edd13f, 0x275a2bbb2bab6c8a, 0x8da64484aaa0febc, 0x163c5c7f03b718c5, 0x890ac4aafa6a37bf, 0xf8f9d3b87d11fd52, 0x667e06866c07c369, 0x5019794a1f5896e5, 0x18ef535a7ffa7a3d, 0x50f29b4b49f31c37, 0xd981acdcf6bc3e4, 0xa5486bdc455d56a2, 0x431be53f92ece9e6, 0xebadcdbf915e8f6c, 0xaf0eed81e8c51e55, 0xe7112e89103cc0c7, 0x844e6a35ddc2b713, 0x8f6bac72988088b0, 0x2730081c758fb42b, 0x67127db35b287316, 0xc4e557b119ef3185, 0x973ea9903ed5125f, 0x992d39ec5c561d28, 0x62aef7b55319d1d4, 0xf03a18a5e16ab641, 0x767c0e8ad33bc085, 0xe2398bf0eeb28cde, 0x86f8c20fb664b01b, 0xa1d2c3d018a9279f, 0x7872773830d368be, 0xfee6a1eebfa13b4a, 0x11815196b9fbf5df, 0x7289102076a125e5, 0xddffe98c4f8aa031, 0xa8392eb238578ab0, 0x7e610231ac1d6181, 0x278047ae3dd0889, 0x1e99ccb9adc62ca6, 0xdae311e656e0661, 0x39e39c6c2ab3655d, 0x3383bbb5156bf1d7, 0x24db98ad3a0647a1, 0x4a0ca5ea449b1c83, 0x15ad45b4a1b5e823, 0xcd24d4bd1056c826, 0x89a92b199adfbafa, 0xacb1c26a06e5ae02, 0xf8972affb3d98e1f, 0x9fec1e78c4376186, 0xbfe8378abfb87b6f, 0xdbfb0fe56c6f80fe, 0x125129529d48a92f, 0xe2ba81b9ce96e02e, 0x82fcedb4c6434d76, 0xdd2a3e32c3859960, 0x7613b68f6ab03130, 0x9b695cd67c93bd79, 0x5a7c210a3a15e7ea, 0xe1f5a58c80292554, 0x122785ae67f5515d, 0x20d63b5b9e3cd6ac, 0x56992551ae074e99, 0xd1197dc12c63176, 0x36563e2ffad8351a, 0xd6fe4dd22e60a4a2, 0xfd39138aa2d508ed, 0xe0521df01a1be6f5, 0xf4e8a8372f8c5810, 0xe2f9d4600f4d0325, 0x6ba8a9d9ba877899, 0x6d6c98fe79817946, 0x55ff6038a5197367, 0x720588ff6547d884, 0xab01350f013d78dd, 0x64a58b2f103485dd, 0x4b19aa71fec3ae6d, 0x4248f15548f69ca, 0xd597b10a01676659, 0x739c45b982193b5e, 0x49c6e0ea76cbcaac, 0xb2069fd0b482b4e8, 0xaca8017e375b64e5, 0xccb7fd40d543f4a1, 0x2c19b63253da43fc, 0x5a98479cbef2ecbc, 0x5b267c1bcff0ab62, 0xe257bde73d83dc1a, 0x28e81dcb6dab91ac, 0xc4e4dc69fc2fff6f, 0x1bb35ad6d2e74b67, 0x1ed1a8ff78f1b632, 0x24b9fe00663574a4, 0xced12d2899b803db, 0xcb78e80e67ba1b8, 0x6cb3bfd65b38562b, 0x83f082b570611d7, 0x7afbefc05e9f7d99, 0x7190b755535d4f18, 0x7d00ae97abaa4096, 0xf630e8b6dac83e69, 0xdc4663a3168698d2, 0xb77d4f6bd0ee8591, 0xa8faac741a6394dc, 0xeeeaddb72f00e0dd, 0x4300fd1c1ce507e5, 0x981ba7e42537275f, 0xda7485a5aeffeb4c, 0x744fea20e8abef92, 0x77a18eb13d2ecde5, 0x6b8a685f6cb61c21, 0xdaf200dd81212d10, 0xdfcb60445c1bf973, 0x4d27090f10c454e, 0xf5babff66def7892, 0x93e391861a034684, 0x23af31db7179a4aa, 0x649474e36b8db9d3, 0x83e907fbd7aaf0b0, 0xf839ce18e08bfb50, 0x70cbb7f3343451be, 0x2293661be51140ab, 0xd9944be1631846d8, 0x5328edeb3e6784de, 0x8335241be1693225, 0x83b0e96e1249c2b0, 0xb562c00b34ee771, 0x65862939b83382e0, 0x2b31bc86877fd2c, 0xd5c149509e9059f1, 0xcfe6c1b1a6b4e2a4, 0xe993503baf5afb41, 0x43da25d99267326b, 0xab4906075507e74, 0xdd40950cf1ed92fa, 0x9dd768f30ca8e85c, 0xa87e78136665cdb2, 0x8ac9e1386e4cbabb, 0x74c8f010d986a9e0, 0xb7041e9bc8c18b0d, 0xbdf0715cb8b20bd7, 0x17858573216e0a22, 0x2bda5328933c854a, 0x6dd06968e0ed1957, 0xe4e62d86dd136e78, 0xd46655d6b012455, 0x2715ef03f8543355, 0x29d7f7b67d43b177, 0xac85320f528d6d5d, 0x2ea36923d5d8e213, 0x4a48496734be336d, 0x727c405ffc73af56, 0xfce8d84068e825b6, 0x5120e35e1c1a250c, 0x33201477347447d8, 0x39db32d014440024, 0x9de1e3b22b8bf4db, 0xa726f4f0828585c9, 0x1c041d1ea5fb3fdb, 0x2e7a35723f3ed035, 0x7f86f63bb23f496a, 0xeb2d28ef943dc88c, 0xea7c015f12b987f7, 0x737dd2824b608d13,];
            ReadOnlySpan<int> Cex = [1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,];
            ReadOnlySpan<byte> Csgn = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];



            Dint X = Dint.FromDouble(x);

            X = X.Reduce(T);

            int neg = 0, isCos = 1;

            (X, int i) = X.Reduce2();

            if ((i & 0x400) != 0)
            {
                neg = 1;
                i = i & 0x3ff;
            }

            if ((i & 0x200) != 0)
            {
                neg = neg != 0 ? 0 : 1;
                isCos = 0;
                i = i & 0x1ff;
            }

            if ((i & 0x100) != 0)
            {
                isCos = isCos != 0 ? 0 : 1;
                X = X with { sgn = 1 };
                X = Dint.Add(Dint.Magic, X);
                i = 0x1ff - i;
            }

            Dint X2 = Dint.Mul(X, X);
            Dint U = evalPC(X2);
            Dint V = evalPS(X, X2);

            if (isCos == 0)
            {
                U = Dint.Mul(new Dint(Slo[i], Shi[i], Sex[i], Ssgn[i]), U);
                V = Dint.Mul(new Dint(Clo[i], Chi[i], Cex[i], Csgn[i]), V);
            }
            else
            {
                U = Dint.Mul(new Dint(Clo[i], Chi[i], Cex[i], Csgn[i]), U);
                V = Dint.Mul(new Dint(Slo[i], Shi[i], Sex[i], Ssgn[i]), V);
                V = V with { sgn = 1 - V.sgn };
            }

            U = Dint.Add(U, V);

            ulong err = 41;
            ulong hi0, hi1, lo0, lo1;

            lo0 = U.lo - err;
            hi0 = U.hi - (lo0 > U.lo ? 1ul : 0ul);
            lo1 = U.lo + err;
            hi1 = U.hi + (lo1 < U.lo ? 1ul : 0ul);

            if ((hi0 >> 10) != (hi1 >> 10))
            {
                ReadOnlySpan<double> exceptions = [1.7881393432617211e-07, 0.99999999999998401, 1.5323198707391886e-43, 3.5762786865234566e-07, 0.99999999999993605, 9.8068471727308299e-42, 7.1525573730470275e-07, 0.9999999999997442, 6.2763821905477882e-40, 1.0728836059570827e-06, 0.99999999999942446, 7.1491915889209496e-39, 1.430511474609497e-06, 0.99999999999897682, 4.0168846019507311e-38];

                for (int k = 0; k < 5; k++)
                {
                    if (Abs(x) == exceptions[k * 3 + 0])
                    {
                        return exceptions[k * 3 + 1] + exceptions[k * 3 + 2];
                    }
                }
            }

            if (neg != 0)
            {
                U = U with { sgn = 1 - U.sgn };
            }

            double y = U.ToDouble();
            return y;
        }




        ulong t = Polyfill.DoubleToUInt64Bits(x);
        int e = (int)(t >> 52) & 0x7ff;

        if (e == 0x7ff)
        {
            if (t << 1 == 0x7fful << 53)
            {
                return 0.0 / 0.0;
            }

            return x + x;
        }

        t &= 0x7fffffffffffffff;
        if (t <= 0x3e46a09e667f3bcc)
        {
            return FusedMultiplyAdd(Polyfill.UInt64BitsToDouble(t), -3.7252902984619141e-09, 1.0);
        }

        double h, l, err;
        (err, h, l) = cosFast(Polyfill.UInt64BitsToDouble(t), T);

        double left = h + (l - err), right = h + (l + err);
        if (left == right)
        {
            return left;
        }

        return cosAccurate(Polyfill.UInt64BitsToDouble(t), T);
    }
}
