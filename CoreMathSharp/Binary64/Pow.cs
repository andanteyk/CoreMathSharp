using System;
using System.Runtime.CompilerServices;

#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
#endif

namespace CoreMathSharp;

public static partial class StrictMath
{
    private readonly record struct Qint(ulong ll, ulong lh, ulong hl, ulong hh, long ex, ulong sgn)
    {
        public readonly Uint128 rl => new Uint128(ll, lh);
        public readonly Uint128 rh => new Uint128(hl, hh);

        public static Qint One => new Qint(0, 0, 0, 0x8000000000000000, 0, 0);
        public static Qint MinusOne => new Qint(0, 0, 0, 0x8000000000000000, 0, 1);
        public static Qint Log2 => new Qint(0x8a0d175b8baafa2b, 0x40f343267298b62d, 0xc9e3b39803f2f6af, 0xb17217f7d1cf79ab, -1, 0);
        public static Qint Log2Inv => new Qint(0, 0, 0, 0xb8aa3b295c17f0bc, 12, 0);
        public static Qint Zero => new Qint(0, 0, 0, 0, 0, 0);


        private static int Cmp(long a, long b) => (a > b ? 1 : 0) - (a < b ? 1 : 0);
        private static int Cmp(ulong a, ulong b) => (a > b ? 1 : 0) - (a < b ? 1 : 0);

        public static int Cmp(in Qint a, in Qint b)
        {
            return Cmp(a.ex, b.ex) != 0 ? Cmp(a.ex, b.ex) :
                Uint128.Cmpu128(a.rh, b.rh) != 0 ? Uint128.Cmpu128(a.rh, b.rh) :
                Uint128.Cmpu128(a.rl, b.rl);
        }

        public static int Cmp22(in Qint a, in Qint b)
        {
            return Cmp(a.ex, b.ex) != 0 ? Cmp(a.ex, b.ex) :
                Uint128.Cmpu128(a.rh, b.rh);
        }

        public static Qint Add(in Qint a, in Qint b)
        {
            if (a.rh == Uint128.Zero && a.rl == Uint128.Zero)
            {
                return b;
            }

            if (b.rh == Uint128.Zero && b.rl == Uint128.Zero)
            {
                return a;
            }

            int cmp = Cmp(a, b);
            Qint aa, bb;
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


            Uint128 ah = aa.rh, al = aa.rl, bh = bb.rh, bl = bb.rl;

            long mEx = aa.ex;
            long k = aa.ex - bb.ex;

            if (k > 0)
            {
                if (k >= 128)
                {
                    bl = (k < 256) ? bh >> ((int)k - 128) : 0;
                    bh = 0;
                }
                else
                {
                    bl = (bl >> (int)k) | (bh << (128 - (int)k));
                    bh = bh >> (int)k;
                }
            }

            ulong sgn = aa.sgn;
            ulong ex;
            Uint128 ch, cl;

            long rex = mEx;

            if ((aa.sgn ^ bb.sgn) != 0)
            {
                ch = ah - bh;

                (int borrow, cl) = Uint128.SubU128(al, bl);
                if (borrow != 0)
                {
                    ch -= 1;
                }

                ulong chh = ch.hi, clh = cl.hi;
                ex = (ulong)(chh != 0 ? Polyfill.LeadingZeroCount(chh) :
                    64 + (ch != 0 ? Polyfill.LeadingZeroCount(ch.lo) :
                        64 + (clh != 0 ? Polyfill.LeadingZeroCount(clh) :
                            64 + Polyfill.LeadingZeroCount(cl.lo))));

                if (ex > 0)
                {
                    if (ex >= 128)
                    {
                        ah = al << ((int)ex - 128);
                        al = 0;
                    }
                    else
                    {
                        ah = (ah << (int)ex) | (al >> (128 - (int)ex));
                        al = al << (int)ex;
                    }

                    int sh = ((int)ex - (int)k);
                    bh = bb.rh;
                    bl = bb.rl;
                    if (sh >= 0)
                    {
                        if (sh >= 128)
                        {
                            bh = bl << (sh - 128);
                            bl = 0;
                        }
                        else if (sh > 0)
                        {
                            bh = (bh << sh) | (bl >> (128 - sh));
                            bl = bl << sh;
                        }
                    }
                    else
                    {
                        int j = -sh;
                        if (j >= 128)
                        {
                            bl = bh >> (j - 128);
                            bh = 0;
                        }
                        else
                        {
                            bl = (bh << (128 - j)) | (bl >> j);
                            bh = bh >> j;
                        }
                    }

                    rex -= (long)ex;
                    ch = ah - bh;

                    (int borrow2, cl) = Uint128.SubU128(al, bl);
                    if (borrow2 != 0)
                    {
                        ch -= 1;
                    }

                    chh = ch.hi;
                    clh = cl.hi;
                    ex = (ulong)(chh != 0 ? Polyfill.LeadingZeroCount(chh) :
                        64 + (ch != 0 ? Polyfill.LeadingZeroCount(ch.lo) :
                        64 + (clh != 0 ? Polyfill.LeadingZeroCount(clh) :
                        64 + Polyfill.LeadingZeroCount(cl.lo))));


                }

                if (ex != 0)
                {
                    ch = (ch << (int)ex) | (cl >> (128 - (int)ex));
                    cl = cl << (int)ex;
                }
                rex -= (int)ex;
            }
            else
            {
                (int carry, ch) = Uint128.AddU128(ah, bh);

                (int carry2, cl) = Uint128.AddU128(al, bl);
                if (carry2 != 0)
                {
                    carry += (ch += 1) == 0 ? 1 : 0;
                }

                if (carry != 0)
                {
                    cl = (ch << 127) | (cl >> 1);
                    ch = (Uint128.One << 127) | (ch >> 1);
                    rex++;
                }
            }

            return new Qint(cl.lo, cl.hi, ch.lo, ch.hi, rex, sgn);
        }

        public static Qint Add22(in Qint a, in Qint b)
        {
            if (a.rh == 0)
            {
                return b;
            }

            if (b.rh == 0)
            {
                return a;
            }

            Qint aa, bb;
            int cmp = Cmp22(a, b);
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

            Uint128 ah = aa.rh, bh = bb.rh;

            long mEx = aa.ex;
            int k = (int)(aa.ex - bb.ex);

            if (k > 0)
            {
                bh = (k >= 128) ? 0 : bh >> (int)k;
            }

            ulong sgn = aa.sgn;
            int ex;
            Uint128 ch;

            long rex = mEx;

            if ((aa.sgn ^ bb.sgn) != 0)
            {
                ch = ah - bh;

                ulong chh = ch.hi;
                ex = chh != 0 ? Polyfill.LeadingZeroCount(chh) : 64 + Polyfill.LeadingZeroCount(ch.lo);

                if (ex > 0)
                {
                    ah <<= ex;
                    if (ex >= k)
                    {
                        bh = bb.rh << (ex - k);
                    }
                    else
                    {
                        bh = bb.rh >> (k - ex);
                    }

                    rex -= ex;
                    ch = ah - bh;

                    chh = ch.hi;
                    ex = chh != 0 ? Polyfill.LeadingZeroCount(chh) : 64 + Polyfill.LeadingZeroCount(ch.lo);
                }

                ch <<= ex;
                rex -= ex;
            }
            else
            {
                (int cy, ch) = Uint128.AddU128(ah, bh);

                if (cy != 0)
                {
                    ch = (Uint128.One << 127) | (ch >> 1);
                    rex++;
                }
            }

            return new Qint(0, 0, ch.lo, ch.hi, rex, sgn);
        }

        public static Qint Mul(in Qint a, in Qint b)
        {
            Uint128 r33 = (Uint128)a.hh * b.hh;

            Uint128 r32 = (Uint128)a.hh * b.hl;
            Uint128 r23 = (Uint128)a.hl * b.hh;

            Uint128 r31 = (Uint128)a.hh * b.lh;
            Uint128 r13 = (Uint128)a.lh * b.hh;
            Uint128 r22 = (Uint128)a.hl * b.hl;

            Uint128 r30 = (Uint128)a.hh * b.ll;
            Uint128 r03 = (Uint128)a.ll * b.hh;
            Uint128 r21 = (Uint128)a.hl * b.lh;
            Uint128 r12 = (Uint128)a.lh * b.hl;

            Uint128 t6, t5, t4, t3, c5, c4;

            t3 = (Uint128)(r12.hi) + (r21.hi) + (r03.hi) + (r30.hi);

            c4 = t4 = r22 + t3;
            c4 += t4 = r13 + t4;
            c4 += t4 = r31 + t4;

            c5 = t5 = r23 + t4.hi;
            c5 += t5 = r32 + t5;

            t6 = r33 + ((c5 << 64) | (t5 >> 64)) + c4;

            int ex = (t6 >> 127) == 0 ? 1 : 0;

            t5 = (t5 << 64) | (t4 & 0xffffffffffffffff);
            Uint128 rrh, rrl;
            if (ex != 0)
            {
                rrh = (t6 << 1) | (t5 >> 127);
                rrl = t5 << 1;
            }
            else
            {
                rrh = t6;
                rrl = t5;
            }

            return new Qint(rrl.lo, rrl.hi, rrh.lo, rrh.hi, a.ex + b.ex + 1 - ex, a.sgn ^ b.sgn);
        }

        public static Qint Mul33(in Qint a, in Qint b)
        {
            Uint128 r33 = (Uint128)a.hh * b.hh;

            Uint128 r32 = (Uint128)a.hh * b.hl;
            Uint128 r23 = (Uint128)a.hl * b.hh;

            Uint128 r31 = (Uint128)a.hh * b.lh;
            Uint128 r13 = (Uint128)a.lh * b.hh;
            Uint128 r22 = (Uint128)a.hl * b.hl;

            Uint128 r21 = (Uint128)a.hl * b.lh;
            Uint128 r12 = (Uint128)a.lh * b.hl;

            Uint128 t6, t5, t4, t3, c5, c4;

            t3 = (r12 >> 64) + (r21 >> 64);

            c4 = t4 = r22 + t3;
            c4 += t4 = r13 + t4;
            c4 += t4 = r31 + t4;

            c5 = t5 = r23 + t4.hi;
            c5 += t5 = r32 + t5;

            t6 = r33 + ((c5 << 64) | (t5 >> 64)) + c4;

            int ex = (t6 >> 127) == 0 ? 1 : 0;

            t5 = (t5 << 64) | (t4 & 0xffffffffffffffff);
            Uint128 rrh, rrl;
            if (ex != 0)
            {
                rrh = (t6 << 1) | (t5 >> 127);
                rrl = t5 << 1;
            }
            else
            {
                rrh = t6;
                rrl = t5;
            }

            return new Qint(rrl.lo, rrl.hi, rrh.lo, rrh.hi, a.ex + b.ex + 1 - ex, a.sgn ^ b.sgn);
        }

        public static Qint Mul41(in Qint a, in Qint b)
        {
            Uint128 r33 = (Uint128)a.hh * b.hh;
            Uint128 r23 = (Uint128)a.hl * b.hh;
            Uint128 r13 = (Uint128)a.lh * b.hh;
            Uint128 r03 = (Uint128)a.ll * b.hh;

            Uint128 t6, t5, t4, t3, c5, c4;

            t3 = r03 >> 64;
            c4 = t4 = r13 + t3;
            c5 = t5 = r23 + (t4 >> 64);
            t6 = r33 + ((c5 << 64) | (t5 >> 64)) + c4;

            int ex = (t6 >> 127) == 0 ? 1 : 0;

            t5 = (t5 << 64) | (t4 & 0xffffffffffffffff);
            Uint128 rrh, rrl;
            if (ex != 0)
            {
                rrh = (t6 << 1) | (t5 >> 127);
                rrl = t5 << 1;
            }
            else
            {
                rrh = t6;
                rrl = t5;
            }

            return new Qint(rrl.lo, rrl.hi, rrh.lo, rrh.hi, a.ex + b.ex + 1 - ex, a.sgn ^ b.sgn);
        }

        public static Qint Mul31(in Qint a, in Qint b)
        {
            Uint128 r33 = (Uint128)a.hh * b.hh;
            Uint128 r23 = (Uint128)a.hl * b.hh;
            Uint128 r13 = (Uint128)a.lh * b.hh;

            Uint128 t6, t5, t4, c5;

            t4 = r13;
            c5 = t5 = r23 + (t4 >> 64);
            t6 = r33 + ((c5 << 64) | (t5 >> 64));

            int ex = (t6 >> 127) == 0 ? 1 : 0;

            t5 = (t5 << 64) | (t4 & 0xffffffffffffffff);
            Uint128 rrh, rrl;
            if (ex != 0)
            {
                rrh = (t6 << 1) | (t5 >> 127);
                rrl = t5 << 1;
            }
            else
            {
                rrh = t6;
                rrl = t5;
            }

            return new Qint(rrl.lo, rrl.hi, rrh.lo, rrh.hi, a.ex + b.ex + 1 - ex, a.sgn ^ b.sgn);
        }

        public static Qint Mul22(in Qint a, in Qint b)
        {
            Uint128 r33 = (Uint128)a.hh * b.hh;

            Uint128 r32 = (Uint128)a.hh * b.hl;
            Uint128 r23 = (Uint128)a.hl * b.hh;

            Uint128 r22 = (Uint128)a.hl * b.hl;

            Uint128 t6, t5, t4, c5;

            t4 = r22;
            c5 = t5 = r23 + (t4 >> 64);
            c5 += t5 = r32 + t5;

            t6 = r33 + ((c5 << 64) | (t5 >> 64));

            int ex = (t6 >> 127) == 0 ? 1 : 0;

            t5 = (t5 << 64) | (t4 & 0xffffffffffffffff);
            Uint128 rrh, rrl;
            if (ex != 0)
            {
                rrh = (t6 << 1) | (t5 >> 127);
                rrl = t5 << 1;
            }
            else
            {
                rrh = t6;
                rrl = t5;
            }

            return new Qint(rrl.lo, rrl.hi, rrh.lo, rrh.hi, a.ex + b.ex + 1 - ex, a.sgn ^ b.sgn);
        }

        public static Qint Mul21(in Qint a, in Qint b)
        {
            Uint128 r33 = (Uint128)a.hh * b.hh;
            Uint128 r23 = (Uint128)a.hl * b.hh;


            Uint128 t6;

            t6 = r33 + (r23 >> 64);

            int ex = (t6 >> 127) == 0 ? 1 : 0;

            Uint128 t5 = (r23 << 64);
            Uint128 rrh, rrl;
            if (ex != 0)
            {
                rrh = (t6 << 1) | (t5 >> 127);
                rrl = t5 << 1;
            }
            else
            {
                rrh = t6;
                rrl = t5;
            }

            return new Qint(rrl.lo, rrl.hi, rrh.lo, rrh.hi, a.ex + b.ex + 1 - ex, a.sgn ^ b.sgn);
        }

        public static Qint Mul11(in Qint a, in Qint b)
        {
            Uint128 t6 = (Uint128)a.hh * b.hh;

            int ex = (t6 >> 127) == 0 ? 1 : 0;

            Uint128 rrh = t6 << ex;

            return new Qint(0, 0, rrh.lo, rrh.hi, a.ex + b.ex + 1 - ex, a.sgn ^ b.sgn);
        }

        public static Qint Mul2(long b, in Qint a)
        {
            if (b == 0)
            {
                return Zero;
            }

            ulong c = (ulong)(b < 0 ? -b : b);
            if (c == 1)
            {
                return a with { sgn = (b < 0 ? 1ul : 0ul) ^ a.sgn };
            }

            ulong rsgn = (b < 0 ? 1ul : 0ul) ^ a.sgn;
            long rex = a.ex + 64;

            int k = Polyfill.LeadingZeroCount(c);
            c <<= k;
            rex -= k;

            Uint128 t3 = (Uint128)a.hh * c;
            Uint128 t2 = (Uint128)a.hl * c;
            Uint128 t1 = (Uint128)a.lh * c;
            Uint128 t0 = (Uint128)a.ll * c;

            Uint128 cy;
            Uint128 t = t0 >> 64;

            cy = t1 = t + t1;
            t = (cy << 64) | (t1 >> 64);
            cy = t2 = t + t2;

            t3 += ((cy << 64) | (t2 >> 64));

            int ex = Polyfill.LeadingZeroCount(t3.hi);

            t2 = (t2 << 64) | (t1 & 0xffffffffffffffff);
            Uint128 rrh, rrl;
            if (ex != 0)
            {
                rrh = (t3 << 1) | (t2 >> 127);
                rrl = t2 << 1;
                rex--;
            }
            else
            {
                rrh = t3;
                rrl = t2;
            }

            return new Qint(rrl.lo, rrl.hi, rrh.lo, rrh.hi, rex, rsgn);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (long e, ulong m) fastExtract(double x)
        {
            ulong _x = Polyfill.DoubleToUInt64Bits(x);

            long e = (long)(_x >> 52) & 0x7ff;
            ulong m = (_x & (~0ul >> 12)) + (e != 0 ? (1ul << 52) : 0);
            e = e - 0x3ff;
            return (e, m);
        }

        public static Qint FromDouble(double b)
        {
            var (ex, hh) = fastExtract(b);

            int t = Polyfill.LeadingZeroCount(hh);

            ulong sgn = b < 0.0 ? 1ul : 0ul;
            ex = ex - (t > 11 ? t - 12 : 0);
            hh = hh << t;

            return new Qint(0, 0, 0, hh, ex, sgn);
        }

        public long Toi()
        {
            if (ex < 0)
            {
                return 0;
            }

            long r = (long)(hh >> (63 - (int)ex));
            return sgn != 0 ? -r : r;
        }

        public Qint Subnormalize()
        {
            if (ex > -1023)
            {
                return this;
            }

            long rex = -(1011 + ex);

            ulong rhi = hh >> (int)ex;
            ulong rmd = (hh >> ((int)ex - 1)) & 0x1;
            ulong rlo = (hh & (~0ul >> (int)ex));
            if (rlo == 0)
            {
                rlo = hl;
                if (rlo == 0)
                {
                    rlo = lh;
                    if (rlo == 0)
                    {
                        rlo = ll;
                    }
                }
            }

            rhi += rlo != 0 ? rmd : rhi & rmd;

            ulong rhh = rhi << (int)ex;
            if (rhh == 0)
            {
                return new Qint(0, 0, 0, 1ul << 63, ex + 1, sgn);
            }
            return new Qint(0, 0, 0, rhh, ex, sgn);
        }

        public double ToDouble()
        {
            Qint a = Subnormalize();

            ulong r = (a.hh >> 11) | (0x3fful << 52);

            double rd = 0.0;
            if ((a.hh & 0x400) != 0)
            {
                rd += 1.1102230246251565e-16;
            }

            if ((a.hh & 0x3ff) != 0 || a.hl != 0 || a.lh != 0 || a.ll != 0)
            {
                rd += 5.5511151231257827e-17;
            }

            if (a.sgn != 0)
            {
                rd = -rd;
            }

            r = r | a.sgn << 63;
            r = Polyfill.DoubleToUInt64Bits(Polyfill.UInt64BitsToDouble(r) + rd);

            ulong e;

            if (a.ex > -1023)
            {
                if (a.ex > 1023)
                {
                    if (a.ex == 1024)
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
                    e = (ulong)((a.ex + 1023) & 0x7ff) << 52;
                }
            }
            else
            {
                if (a.ex < -1074)
                {
                    if (a.ex == -1075)
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
                    e = 1ul << (int)(a.ex + 1074);
                }
            }

            return Polyfill.UInt64BitsToDouble(r) * Polyfill.UInt64BitsToDouble(e);
        }
    }




    public static double Pow(double x, double y)
    {
        const int PowIteration = 15;
        const bool EnableFP = (PowIteration & 0x1) != 0;
        const bool EnableZiv2 = (PowIteration & 0x2) != 0;
        const bool EnableExact = (PowIteration & 0x4) != 0;
        const bool EnableZiv3 = (PowIteration & 0x8) != 0;



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double RoundEvenFinite(double x)
        {
#if NETCOREAPP3_0_OR_GREATER
            if (Avx.IsSupported)
            {
                return Avx.RoundToNearestInteger(Vector256.CreateScalarUnsafe(x)).ToScalar();
            }
            if (AdvSimd.IsSupported)
            {
                return AdvSimd.RoundToNearestScalar(Vector64.CreateScalarUnsafe(x)).ToScalar();
            }
            if (Sse41.IsSupported)
            {
                return Sse41.RoundToNearestIntegerScalar(Vector128.CreateScalarUnsafe(x)).ToScalar();
            }
#endif

            double ix = BuiltinRound(x);
            if (Abs(ix - x) == 0.5)
            {
                double u = ix;
                double v = ix - CopySign(1.0, x);
                if (Polyfill.TrailingZeroCount(Polyfill.DoubleToUInt64Bits(v)) > Polyfill.TrailingZeroCount(Polyfill.DoubleToUInt64Bits(u)))
                {
                    ix = v;
                }
            }

            return ix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool isSignaling(double x)
        {
            ulong _x = Polyfill.DoubleToUInt64Bits(x);
            return (_x & (1ul << 51)) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double s, double e) fastTwoSum(double x, double y)
        {
            double s = x + y, z = s - x;
            return (s, y - z);
        }

        /*
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double s, double t) twoSum(double a, double b)
        {
            double s = a + b;
            double aPrime = s - b;
            double bPrime = s - aPrime;
            double deltaA = a - aPrime;
            double deltaB = b - bPrime;
            double t = deltaA + deltaB;
            return (s, t);
        }
        //*/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double s, double e) fastSum(double a, double bh, double bl)
        {
            var (hi, lo) = fastTwoSum(a, bh);
            return (hi, lo + bl);
        }

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

        /*
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double hi, double lo) dSquare(double ah, double al)
        {
            double s, b = al + al;

            (double hi, s) = aMul(ah, ah);
            double lo = FusedMultiplyAdd(ah, b, s);

            return (hi, lo);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long dtoi(double x) => (long)x;
        //*/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool isInt(double x) => x == RoundEvenFinite(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (long e, ulong m) extract(double x)
        {
            ulong _x = Polyfill.DoubleToUInt64Bits(x);

            long e = (long)(_x >> 52) & 0x7ff;
            ulong m = (_x & (~0ul >> 12)) + (e != 0 ? (1ul << 52) : 0);

            int t = Polyfill.TrailingZeroCount(m);
            m >>= t;
            e = e + t - (0x433 - (e == 0 ? 1 : 0));

            return (e, m);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (long g, long k) Round54(in Dint x)
        {
            long g = x.ex - 53;
            long k = (long)((x.hi >> 10) + ((x.hi >> 9) & 1));
            return (g, k);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double pow2(double x, long e)
        {
            if ((e & 0x1) != 0)
            {
                x *= 2.0;
            }

            ulong e2 = ((ulong)((e >> 1) + 0x3ff) & 0x7ff) << 52;
            x = (x * Polyfill.UInt64BitsToDouble(e2)) * Polyfill.UInt64BitsToDouble(e2);
            return x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long dintToi(in Dint a)
        {
            if (a.ex < 0)
            {
                return 0;
            }

            long r = (long)(a.hi >> (63 - (int)a.ex));
            return a.sgn != 0 ? -r : r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double dintTodSubnormal(in Dint a)
        {
            double ret = 0;

            ulong ex = (ulong)-(1011 + a.ex);
            ulong rb, sb;

            if (ex >= 64)
            {
                rb = (a.hi >> 63);
                sb = (a.hi << 1) | a.lo;
                ret = (ex > 64 || rb == 0 || sb == 0) ? 0.0 : 4.9406564584124654e-324;
                ret = a.sgn != 0 ? -ret : ret;
            }

            ulong hi;
            hi = a.hi >> (int)ex;
            rb = (a.hi >> ((int)ex - 1)) & 0x1;
            sb = (a.hi << (65 - (int)ex));
            if (sb == 0)
            {
                sb = a.lo;
            }

            hi += sb != 0 ? rb : hi & rb;


            ulong v = hi;
            v |= a.sgn << 63;
            ret = Polyfill.UInt64BitsToDouble(v);

            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double dintTod(in Dint a)
        {
            if (a.ex < -1022)
            {
                return dintTodSubnormal(a);
            }

            ulong r = (a.hi >> 11) | (0x3fful << 52);

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

            r |= a.sgn << 63;
            r = Polyfill.DoubleToUInt64Bits(Polyfill.UInt64BitsToDouble(r) + rd);

            ulong e;

            if (a.ex > -1023)
            {
                if (a.ex > 1023)
                {
                    if (a.ex == 1024)
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
                    e = (ulong)((a.ex + 1023) & 0x7ff) << 52;
                }
            }
            else
            {
                if (a.ex < -1074)
                {
                    if (a.ex == -1075)
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
                    e = 1ul << ((int)a.ex + 1074);
                }
            }

            return Polyfill.UInt64BitsToDouble(r) * Polyfill.UInt64BitsToDouble(e);
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double qh, double ql) q1(double z)
        {
            ReadOnlySpan<double> Q1 = [1, 1, 0.5, 0.16666666679061554, 0.041666666688168076];

            double q, h0, h1, l1;
            q = FusedMultiplyAdd(Q1[4], z, Q1[3]);
            q = FusedMultiplyAdd(q, z, Q1[2]);
            h0 = FusedMultiplyAdd(q, z, Q1[1]);
            (h1, l1) = aMul(z, h0);
            return fastSum(Q1[0], h1, l1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Dint q2(in Dint y)
        {
            ReadOnlySpan<ulong> Q2Hi = [0xd00d00cd98416862, 0xb60b60b932146a54, 0x8888888888888897, 0xaaaaaaaaaaaaaaa3, 0xaaaaaaaaaaaaaaaa, 0x8000000000000000, 0x8000000000000000, 0xffffffffffffffff,];
            ReadOnlySpan<ulong> Q2Lo = [0x0, 0x0, 0x0, 0x0, 0xaaaaaa6a1e0776ae, 0xc06f3cd29, 0x88, 0xffffffffffffffd0,];
            ReadOnlySpan<int> Q2Ex = [-13, -10, -7, -5, -3, -1, 0, -1,];
            ReadOnlySpan<byte> Q2Sgn = [0, 0, 0, 0, 0, 0, 0, 0,];


            Dint r = new Dint(Q2Lo[0], Q2Hi[0], Q2Ex[0], Q2Sgn[0]);

            r = Dint.Mul11(y, r);
            r = Dint.Add11(new Dint(Q2Lo[1], Q2Hi[1], Q2Ex[1], Q2Sgn[1]), r);

            r = Dint.Mul11(y, r);
            r = Dint.Add11(new Dint(Q2Lo[2], Q2Hi[2], Q2Ex[2], Q2Sgn[2]), r);

            r = Dint.Mul11(y, r);
            r = Dint.Add(new Dint(Q2Lo[3], Q2Hi[3], Q2Ex[3], Q2Sgn[3]), r);

            r = Dint.MulPow(y, r);
            r = Dint.Add(new Dint(Q2Lo[4], Q2Hi[4], Q2Ex[4], Q2Sgn[4]), r);

            r = Dint.MulPow(y, r);
            r = Dint.Add(new Dint(Q2Lo[5], Q2Hi[5], Q2Ex[5], Q2Sgn[5]), r);

            r = Dint.MulPow(y, r);
            r = Dint.Add(new Dint(Q2Lo[6], Q2Hi[6], Q2Ex[6], Q2Sgn[6]), r);

            r = Dint.MulPow(y, r);
            r = Dint.Add(new Dint(Q2Lo[7], Q2Hi[7], Q2Ex[7], Q2Sgn[7]), r);

            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Qint q3(in Qint y)
        {
            ReadOnlySpan<ulong> Q3Hh = [0xc9cba547af749429, 0xb092309ec73dd7db, 0x8f76c77fc6c4bda8, 0xd7322b3faa271c7d, 0x93f27dbbc4fae397, 0xb8ef1d2ab6399c7d, 0xd00d00d00d00d00d, 0xd00d00d00d00d00d, 0xb60b60b60b60b60b, 0x8888888888888888, 0xaaaaaaaaaaaaaaaa, 0xaaaaaaaaaaaaaaaa, 0x8000000000000000, 0x8000000000000000, 0x8000000000000000,];
            ReadOnlySpan<ulong> Q3Hl = [0x0, 0x0, 0xcd9aab7578033f6d, 0xb3537cbfd60dcb9, 0x780b69f6554de3d9, 0x560e44741a6a8e66, 0xd00d00d00d00cf, 0xd00d00d00d00ce, 0x60b60b60b60b60b6, 0x8888888888888888, 0xaaaaaaaaaaaaaaaa, 0xaaaaaaaaaaaaaaaa, 0x0, 0x0, 0x0,];
            ReadOnlySpan<ulong> Q3Lh = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xca5a80878f19216b, 0xb60b60be6ac2e60, 0x888888890ac16c5a, 0xaaaaaaaaaaaaaaaa, 0xaaaaaaaaaaaaaaaa, 0x0, 0x0, 0x0,];
            ReadOnlySpan<ulong> Q3Ll = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xd236a7fa15252936, 0x800a9987617257e3, 0xf78e687c535a714, 0x6b3ad4c251cd03d5, 0x4df8c3de374c499e, 0x1446e270, 0x262ce809, 0x0,];
            ReadOnlySpan<int> Q3Ex = [-37, -33, -29, -26, -22, -19, -16, -13, -10, -7, -5, -3, -1, 0, 0,];
            ReadOnlySpan<byte> Q3Sgn = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0];


            Qint r = Qint.Mul11(y, new Qint(Q3Ll[0], Q3Lh[0], Q3Hl[0], Q3Hh[0], Q3Ex[0], Q3Sgn[0]));
            r = Qint.Add22(new Qint(Q3Ll[1], Q3Lh[1], Q3Hl[1], Q3Hh[1], Q3Ex[1], Q3Sgn[1]), r);

            for (int k = 2; k < 7; k++)
            {
                r = Qint.Mul22(y, r);
                r = Qint.Add22(new Qint(Q3Ll[k], Q3Lh[k], Q3Hl[k], Q3Hh[k], Q3Ex[k], Q3Sgn[k]), r);
            }

            for (int k = 7; k < 12; k++)
            {
                r = Qint.Mul33(y, r);
                r = Qint.Add(new Qint(Q3Ll[k], Q3Lh[k], Q3Hl[k], Q3Hh[k], Q3Ex[k], Q3Sgn[k]), r);
            }

            for (int k = 12; k < 15; k++)
            {
                r = Qint.Mul(y, r);
                r = Qint.Add(new Qint(Q3Ll[k], Q3Lh[k], Q3Hl[k], Q3Hh[k], Q3Ex[k], Q3Sgn[k]), r);
            }

            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double ph, double pl) p1(double z)
        {
            ReadOnlySpan<double> P1 = [0.33333333333333348, -0.25000000000000017, 0.19999999995699516, -0.16666666662262275, 0.14286102679996321, -0.12500370131039634];

            double wh, wl;
            (wh, wl) = aMul(z, z);
            double t = FusedMultiplyAdd(P1[5], z, P1[4]);
            double u = FusedMultiplyAdd(P1[3], z, P1[2]);
            double v = FusedMultiplyAdd(P1[1], z, P1[0]);
            u = FusedMultiplyAdd(t, wh, u);
            v = FusedMultiplyAdd(u, wh, v);
            u = v * wh;
            return (-0.5 * wh, FusedMultiplyAdd(u, z, -0.5 * wl));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Dint p2(in Dint z)
        {
            ReadOnlySpan<ulong> P2Hi = [0xe38e3954a09e560e, 0x800000399d09d767, 0x9249249249248676, 0xaaaaaaaaaaaa9fdd, 0xcccccccccccccccc, 0x8000000000000000, 0xaaaaaaaaaaaaaaaa, 0xffffffffffffffff, 0x8000000000000000,];
            ReadOnlySpan<ulong> P2Lo = [0x0, 0x0, 0x0, 0x0, 0xcccdc5fe0ef93b8d, 0x600135b960d8, 0xaaaaaaaaaaa77b5e, 0xfffffffffffe33ca, 0x0,];
            ReadOnlySpan<int> P2Ex = [-4, -3, -3, -3, -3, -2, -2, -2, 0,];
            ReadOnlySpan<byte> P2Sgn = [0x0, 0x1, 0x0, 0x1, 0x0, 0x1, 0x0, 0x1, 0x0];

            Dint r = Dint.Mul11(z, new Dint(P2Lo[0], P2Hi[0], P2Ex[0], P2Sgn[0]));
            r = Dint.Add11(new Dint(P2Lo[1], P2Hi[1], P2Ex[1], P2Sgn[1]), r);

            r = Dint.Mul11(z, r);
            r = Dint.Add11(new Dint(P2Lo[2], P2Hi[2], P2Ex[2], P2Sgn[2]), r);

            r = Dint.Mul11(z, r);
            r = Dint.Add11(new Dint(P2Lo[3], P2Hi[3], P2Ex[3], P2Sgn[3]), r);

            r = Dint.Mul11(z, r);
            r = Dint.Add(new Dint(P2Lo[4], P2Hi[4], P2Ex[4], P2Sgn[4]), r);

            r = Dint.Mul21(r, z);
            r = Dint.Add(new Dint(P2Lo[5], P2Hi[5], P2Ex[5], P2Sgn[5]), r);

            r = Dint.Mul21(r, z);
            r = Dint.Add(new Dint(P2Lo[6], P2Hi[6], P2Ex[6], P2Sgn[6]), r);

            r = Dint.Mul21(r, z);
            r = Dint.Add(new Dint(P2Lo[7], P2Hi[7], P2Ex[7], P2Sgn[7]), r);

            r = Dint.Mul21(r, z);
            r = Dint.Add(new Dint(P2Lo[8], P2Hi[8], P2Ex[8], P2Sgn[8]), r);

            r = Dint.Mul21(r, z);
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Qint p3(Qint z)
        {
            ReadOnlySpan<ulong> P3Hh = [0xe38e39d490f62b2f, 0xf0f0f1e1e1d4e1cf, 0xffffffffffff88b8, 0x8888888888885088, 0x9249249249249249, 0x9d89d89d89d89d89, 0xaaaaaaaaaaaaaaaa, 0xba2e8ba2e8ba2e8b, 0xcccccccccccccccc, 0xe38e38e38e38e38e, 0xffffffffffffffff, 0x9249249249249249, 0xaaaaaaaaaaaaaaaa, 0xcccccccccccccccc, 0xffffffffffffffff, 0xaaaaaaaaaaaaaaaa, 0x8000000000000000, 0x8000000000000000,];
            ReadOnlySpan<ulong> P3Hl = [0x0, 0xbbb343000334fd0f, 0xc17633c5a3181e76, 0x8f6a4426b02f93be, 0x24a2676c009fc980, 0xd8ab89d5a96621f1, 0xaaaaaaaaa815192a, 0xa2e8ba2e899ae964, 0xcccccccccccccccd, 0x38e38e38e38e38e3, 0xffffffffffffffff, 0x2492492492492492, 0xaaaaaaaaaaaaaaaa, 0xcccccccccccccccc, 0xffffffffffffffff, 0xaaaaaaaaaaaaaaaa, 0x0, 0x0,];
            ReadOnlySpan<ulong> P3Lh = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xcc491481418dc51, 0xba38ce3dcedbed7d, 0xfffffffc6072860a, 0x492492481c930545, 0xaaaaaaaaaaaaaab8, 0xccccccccccccccd2, 0xffffffffffffffff, 0xaaaaaaaaaaaaaaaa, 0x0, 0x0,];
            ReadOnlySpan<ulong> P3Ll = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1cebff4e21be093e, 0xfa2cc6f77565683f, 0x786bd58754911c58, 0xf0298bcd6e1b2310, 0xd8b61a619485f089, 0xccc65d183d01d5ef, 0xffffffcdc3a6a23c, 0xaaaaaaa4aab50b70, 0xd, 0x0,];
            ReadOnlySpan<int> P3Ex = [-5, -5, -5, -4, -4, -4, -4, -4, -4, -4, -4, -3, -3, -3, -3, -2, -1, 0,];
            ReadOnlySpan<byte> P3Sgn = [0x1, 0x0, 0x1, 0x0, 0x1, 0x0, 0x1, 0x0, 0x1, 0x0, 0x1, 0x0, 0x1, 0x0, 0x1, 0x0, 0x1, 0x0];


            Qint r = Qint.Mul11(new Qint(P3Ll[0], P3Lh[0], P3Hl[0], P3Hh[0], P3Ex[0], P3Sgn[0]), z);
            r = Qint.Add22(new Qint(P3Ll[1], P3Lh[1], P3Hl[1], P3Hh[1], P3Ex[1], P3Sgn[1]), r);

            for (int k = 2; k < 4; k++)
            {
                r = Qint.Mul11(r, z);
                r = Qint.Add22(new Qint(P3Ll[k], P3Lh[k], P3Hl[k], P3Hh[k], P3Ex[k], P3Sgn[k]), r);
            }

            for (int k = 4; k < 8; k++)
            {
                r = Qint.Mul21(r, z);
                r = Qint.Add22(new Qint(P3Ll[k], P3Lh[k], P3Hl[k], P3Hh[k], P3Ex[k], P3Sgn[k]), r);
            }

            for (int k = 8; k < 14; k++)
            {
                r = Qint.Mul31(r, z);
                r = Qint.Add(new Qint(P3Ll[k], P3Lh[k], P3Hl[k], P3Hh[k], P3Ex[k], P3Sgn[k]), r);
            }

            for (int k = 14; k < 18; k++)
            {
                r = Qint.Mul41(r, z);
                r = Qint.Add(new Qint(P3Ll[k], P3Lh[k], P3Hl[k], P3Hh[k], P3Ex[k], P3Sgn[k]), r);
            }

            r = Qint.Mul41(r, z);
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (int e, double h, double l) log1(double x)
        {
            ReadOnlySpan<double> Inverse = [1.41015625, 1.40234375, 1.39453125, 1.38671875, 1.37890625, 1.37109375, 1.3671875, 1.359375, 1.3515625, 1.34375, 1.3359375, 1.328125, 1.32421875, 1.31640625, 1.30859375, 1.3046875, 1.296875, 1.2890625, 1.28125, 1.27734375, 1.26953125, 1.265625, 1.2578125, 1.25, 1.24609375, 1.23828125, 1.234375, 1.2265625, 1.22265625, 1.21484375, 1.2109375, 1.203125, 1.19921875, 1.1953125, 1.1875, 1.18359375, 1.17578125, 1.171875, 1.16796875, 1.16015625, 1.15625, 1.15234375, 1.14453125, 1.140625, 1.13671875, 1.12890625, 1.125, 1.12109375, 1.1171875, 1.109375, 1.10546875, 1.1015625, 1.09765625, 1.08984375, 1.0859375, 1.08203125, 1.078125, 1.07421875, 1.0703125, 1.0625, 1.05859375, 1.0546875, 1.05078125, 1.046875, 1.04296875, 1.0390625, 1.03515625, 1.03125, 1.02734375, 1.0234375, 1.01953125, 1.015625, 1.01171875, 1.0078125, 1, 1, 0.994140625, 0.990234375, 0.986328125, 0.982421875, 0.978515625, 0.974609375, 0.970703125, 0.96875, 0.96484375, 0.9609375, 0.95703125, 0.953125, 0.94921875, 0.947265625, 0.943359375, 0.939453125, 0.935546875, 0.931640625, 0.9296875, 0.92578125, 0.921875, 0.919921875, 0.916015625, 0.912109375, 0.91015625, 0.90625, 0.90234375, 0.900390625, 0.896484375, 0.892578125, 0.890625, 0.88671875, 0.884765625, 0.880859375, 0.87890625, 0.875, 0.873046875, 0.869140625, 0.8671875, 0.86328125, 0.861328125, 0.857421875, 0.85546875, 0.8515625, 0.849609375, 0.845703125, 0.84375, 0.83984375, 0.837890625, 0.8359375, 0.83203125, 0.830078125, 0.826171875, 0.82421875, 0.822265625, 0.818359375, 0.81640625, 0.814453125, 0.810546875, 0.80859375, 0.806640625, 0.8046875, 0.80078125, 0.798828125, 0.796875, 0.79296875, 0.791015625, 0.7890625, 0.787109375, 0.783203125, 0.78125, 0.779296875, 0.77734375, 0.775390625, 0.771484375, 0.76953125, 0.767578125, 0.765625, 0.763671875, 0.76171875, 0.7578125, 0.755859375, 0.75390625, 0.751953125, 0.75, 0.748046875, 0.74609375, 0.7421875, 0.740234375, 0.73828125, 0.736328125, 0.734375, 0.732421875, 0.73046875, 0.728515625, 0.7265625, 0.724609375, 0.72265625, 0.720703125, 0.71875, 0.716796875, 0.71484375, 0.712890625, 0.7109375, 0.708984375, 0.70703125];
            ReadOnlySpan<double> LogInv = [-0.34370051385326406, -5.4388883298990648e-14, -0.33814494400871808, 1.686950122813039e-15, -0.33255833730004269, -3.3906861336722287e-14, -0.32694034499581903, -3.4288400126669462e-14, -0.32129061245382218, 8.7885427699715446e-14, -0.31560877898641593, 1.1259274624680829e-13, -0.31275571000378477, -1.1211800740360982e-13, -0.30702503529482783, -8.4031563047924246e-14, -0.3012613305781997, 3.7923164802093147e-14, -0.29546421289387581, 3.9934163843878439e-14, -0.28963329258294834, -9.4333981895126903e-14, -0.28376817313073843, 9.3834172236637e-14, -0.2808226629008459, -4.1886091378637011e-14, -0.27490548587275043, -4.8816703646769986e-14, -0.26895308734560786, 1.0389630784002988e-13, -0.26596354849721138, 7.3435913698677971e-14, -0.25995752443691345, -1.2621729398885316e-14, -0.25391520998095984, -3.6001767326373346e-15, -0.24783616390459429, 1.3029797173308663e-14, -0.24478272641772492, 3.3999811083618331e-14, -0.23864773785021498, 3.9970509095301341e-14, -0.23556607131286, 9.3094594951968895e-14, -0.22937410106487732, 3.1492650651914838e-14, -0.22314355131425145, 4.1697965845271953e-14, -0.22001365830533359, 5.1496672341414078e-14, -0.21372432939779173, 7.3595801864405143e-14, -0.21056476910735, 3.6507188831790577e-16, -0.2042155414287663, 7.5409165119561888e-14, -0.20102574606062262, 3.1881849375437737e-14, -0.19461546769957749, -9.4165381457182504e-14, -0.19139485299956505, -6.4408561506968921e-14, -0.18492233849406148, 4.9485167661250996e-14, -0.18167030310769405, 5.9375063333847015e-14, -0.17840765747291698, 9.8683503867349494e-14, -0.17185025692674571, 8.6492396072120709e-14, -0.16855536102980295, -3.7143977541704719e-15, -0.16193282026938505, 7.1793900192956773e-14, -0.15860503017665906, 2.0472357800461955e-14, -0.15526612891108016, -4.3792508292406054e-14, -0.14855469432313839, 1.24915489807516e-15, -0.14518200984457508, 7.7180013368280985e-14, -0.14179791186029433, 3.6984595066970968e-14, -0.13499516453748583, -1.8996158041578768e-14, -0.13157635778861732, -1.0195735223708473e-13, -0.12814582269197672, 4.6680314039457961e-14, -0.12124924363297396, 1.0427241278273008e-13, -0.11778303565643, 4.6547297475984447e-14, -0.11430477128010352, 4.4889533522386993e-14, -0.11081436634026431, -2.5799991283069902e-14, -0.10379679368156758, -7.5986365971941414e-14, -0.10026945316371894, 4.3786376170783979e-14, -0.096729626458454732, -9.6380676585522774e-14, -0.093177224854116503, -6.6787085171628983e-14, -0.086034337341743594, -5.9559229876256426e-14, -0.082443669210988446, -8.6145129360878145e-14, -0.078840061707751374, -2.4650189061766119e-14, -0.075223421237524235, -6.329065958724544e-14, -0.071593653186937445, -7.137308225343178e-14, -0.067950661908525944, 1.8195060030168815e-14, -0.060624621816486979, 5.2136206391365041e-14, -0.056941376400118315, -2.0109399435564958e-14, -0.053244514518837605, 2.532168943117445e-14, -0.049533935122326511, 4.9880309107981426e-14, -0.045809536031356402, 6.2198341994757923e-14, -0.042071213920735318, 4.8263140005511282e-14, -0.038318864302027578, -1.0902154302203302e-13, -0.034552381506728125, 6.8391397423287774e-14, -0.03077165866670839, -4.5298142577909288e-14, -0.026976587698300136, 9.8060505168431766e-14, -0.023167059281604452, 7.0073597043100357e-14, -0.019342962843211353, 8.0418538505225864e-14, -0.015504186535963527, -1.7274567499706107e-15, -0.011650617220084314, 1.0903974971735932e-13, -0.0077821404420319595, -2.2989410046203511e-14, 0, 0, 0, 0, 0.0058766084889612102, 2.3831678683970623e-14, 0.0098136214483020012, 2.2620026175581269e-14, 0.013766195764219447, -7.1487303274964921e-14, 0.017734454939727584, 4.0994203403301545e-14, 0.021718523954632474, 1.0512339808596024e-14, 0.025718529288042191, -5.3071713196562948e-14, 0.029734598942923185, -4.4126629622863087e-14, 0.031748698314686408, -1.0610652735224087e-13, 0.035789107851542212, 4.3066973476878145e-14, 0.039845908547249564, -4.9893776716773285e-14, 0.043919233934730073, 1.0541743854342862e-13, 0.048009219186269547, 9.1060543791309292e-14, 0.052116001138983847, 3.0171021061886944e-14, 0.054175734102045681, -2.1092591481336484e-14, 0.058307971386966528, -3.1430923238167113e-14, 0.06245735493371285, 3.3754590773038421e-14, 0.06662402762867714, -8.4590817281744797e-14, 0.070808134151093327, 7.3240729054000268e-14, 0.072906770808003785, 8.3995942740443368e-14, 0.077117303344493848, -6.255850200176405e-14, 0.081345639453957119, -4.7133707783009839e-15, 0.08346653102307755, 1.2485591229479935e-14, 0.087721856593134362, 9.4058078343836257e-14, 0.091995367370600434, 1.003943822681932e-14, 0.094138990913961607, -9.9696530230797061e-14, 0.098440072813218649, 3.3871241029241416e-14, 0.10275973395778237, -1.3438406228830954e-14, 0.10492658204293548, -7.6212584319249484e-14, 0.10927441497892687, 3.5759929977267538e-14, 0.11364123414523419, 6.8892141920084745e-14, 0.11583181552509814, 2.3568822182038756e-14, 0.12022742699809896, 6.0837384199725735e-14, 0.12243249955645297, 2.0797337102277177e-14, 0.1268572855367438, 8.5634458672565039e-14, 0.12907704227518479, -4.2451216089619995e-14, 0.13353139262449076, 3.1859736349078334e-14, 0.13576603042588431, 5.4642560554597724e-14, 0.14025034287328708, -1.9509765515283165e-14, 0.14250006260726877, 1.4256439478199035e-14, 0.14701474296180095, 8.7107837961224781e-15, 0.14927974959255152, 1.1026677017414112e-13, 0.15382521196443122, -9.4784041051961182e-14, 0.15610571466299916, 6.2492749316065374e-14, 0.16068238169054894, -7.5471060282448067e-14, 0.16297859395081105, 1.2645632984431417e-14, 0.16758689703692653, 9.1408198894914123e-14, 0.16989903679541385, -1.6376276414097503e-14, 0.17453941635199044, -9.0762315566997956e-14, 0.17686770611157954, -8.8723565173151846e-14, 0.17920142945763473, 7.6261536774293392e-14, 0.1838852787700489, 8.8463735581208695e-14, 0.18623545611512782, -3.6862307174239401e-14, 0.19095244599316175, 6.8069424967627335e-14, 0.1933193110035063, -1.0320443688698849e-14, 0.19569179135714876, -2.2391667855054851e-14, 0.20045370511729743, 7.2622151677387761e-14, 0.20284319251481975, -6.8276617871854977e-14, 0.20523840324062803, 7.8304972192271964e-14, 0.21004610480872543, 8.4055466633470345e-14, 0.21245865121409224, 1.0115944196590467e-13, 0.21487703207844788, 2.7146365570777349e-14, 0.21730127569003344, -5.2040087434058838e-14, 0.22216746534104459, 1.0970699320566433e-13, 0.22460946899673218, -2.6136541452968644e-14, 0.22705745063535687, -1.078736749871691e-14, 0.23197146543770941, 6.573097737831975e-14, 0.23443755793300625, -3.7601884458907501e-14, 0.2369097470784709, -1.1318526912023687e-13, 0.23938806309274696, 7.7841725495748628e-14, 0.24436319773303694, -9.8342012962697782e-14, 0.2468600779316148, -8.8998513565604442e-14, 0.2493632081495889, 5.5428558531117086e-14, 0.25187261975497677, 9.3312346779459175e-14, 0.25438834435226454, 5.28323330087437e-14, 0.25943886013828887, 9.7042267920673565e-14, 0.26197371574153294, 4.1026510716984462e-14, 0.2645150131702394, 7.1500231530184015e-15, 0.26706278524898153, 6.3719472698156668e-14, 0.26961706505403527, 1.0674835088128526e-13, 0.27217788591588032, -6.4651030640052555e-14, 0.27731928541629713, -6.2790557326608443e-14, 0.27989993200981189, -8.5912983936285462e-14, 0.28248725557477883, -1.0190482133505088e-13, 0.28508129075180477, -8.1211311749625399e-14, 0.28768207245184385, -6.292357389008195e-14, 0.29028963585892598, -6.4181446376601164e-14, 0.29290401643288533, 4.7274529405144063e-14, 0.29815337231912054, -4.4204083338755686e-14, 0.30078841995714356, -6.2123230480842141e-14, 0.30343042941990461, 1.5483459934980831e-14, 0.30607943759150658, -9.5541815660011496e-15, 0.30873548164959175, 2.1522127491642888e-14, 0.31139859906920719, -1.1022412161041444e-13, 0.31406882762507848, -1.0263280755261064e-13, 0.31674620539570242, -1.0174753377507561e-14, 0.31943077076630289, 5.8343574200909236e-14, 0.32212256243201409, 5.8553167927094155e-14, 0.32482161940129117, -5.351646604259541e-14, 0.32752798099909342, -1.1281735060685524e-13, 0.33024168687052224, 5.4612144489920215e-14, 0.33296277698491394, 2.3569101751290204e-14, 0.3356912916381134, 2.8136969901227338e-14, 0.33842727145702156, -5.2801562047290642e-15, 0.34117075740277869, -1.156568624616423e-14, 0.3439217907746297, 2.7306518921347088e-14, 0.34668041321378951, -5.2778200188642693e-14];



            double h, l;
            ulong _x = Polyfill.DoubleToUInt64Bits(x);
            ulong _m = _x & (~0ul >> 12);
            long _e = (long)(_x >> 52) & 0x7ff;

            ulong _t;

            if (_e != 0)
            {
                _t = _m | (0x3fful << 52);
                _m += 1ul << 52;
                _e -= 0x3ff;
            }
            else
            {
                int k = Polyfill.LeadingZeroCount(_m) - 11;
                _e = -0x3fe - k;
                _m <<= k;
                _t = _m | (0x3fful << 52);
            }

            double t = Polyfill.UInt64BitsToDouble(_t);
            int i;

            int c = _m >= 0x16a09e667f3bcd ? 1 : 0;
            ReadOnlySpan<double> cy = [1.0, 0.5];
            ReadOnlySpan<int> cm = [44, 45];

            _e += c;
            double E = _e;
            i = (int)(_m >> cm[c]);
            t *= cy[c];

            double r = Inverse[i - 181];
            double l1 = LogInv[(i - 181) * 2 + 0];
            double l2 = LogInv[(i - 181) * 2 + 1];

            double z = FusedMultiplyAdd(r, t, -1.0);

            const double Log2H = 0.69314718055989033, Log2L = 5.4979230187083712e-14;

            double th, tl;
            th = FusedMultiplyAdd(E, Log2H, l1);
            tl = FusedMultiplyAdd(E, Log2L, l2);

            (h, l) = fastSum(th, z, tl);
            double ph, pl;
            (ph, pl) = p1(z);
            (h, l) = fastSum(h, ph, l + pl);

            if (_e == 0 && Abs(l) > Abs(h) * 5.9604644775390625e-08)
            {
                (h, l) = fastTwoSum(h, l);
                return (1, h, l);
            }

            return (0, h, l);
        }

        static (Dint r, Dint x) log2(Dint x)
        {
            ReadOnlySpan<ulong> Inverse21Hi = [0xb500000000000000, 0xb300000000000000, 0xb100000000000000, 0xaf00000000000000, 0xad80000000000000, 0xab80000000000000, 0xaa00000000000000, 0xa800000000000000, 0xa680000000000000, 0xa480000000000000, 0xa300000000000000, 0xa180000000000000, 0xa000000000000000, 0x9e80000000000000, 0x9d00000000000000, 0x9b80000000000000, 0x9a00000000000000, 0x9880000000000000, 0x9700000000000000, 0x9580000000000000, 0x9480000000000000, 0x9300000000000000, 0x9180000000000000, 0x9080000000000000, 0x8f00000000000000, 0x8e00000000000000, 0x8c80000000000000, 0x8b80000000000000, 0x8a80000000000000, 0x8900000000000000, 0x8800000000000000, 0x8700000000000000, 0x8580000000000000, 0x8480000000000000, 0x8380000000000000, 0x8280000000000000, 0x8180000000000000, 0x8000000000000000, 0x8000000000000000, 0xfd00000000000000, 0xfb00000000000000, 0xf900000000000000, 0xf780000000000000, 0xf580000000000000, 0xf380000000000000, 0xf200000000000000, 0xf000000000000000, 0xee80000000000000, 0xec80000000000000, 0xeb00000000000000, 0xe900000000000000, 0xe780000000000000, 0xe600000000000000, 0xe480000000000000, 0xe300000000000000, 0xe100000000000000, 0xdf80000000000000, 0xde00000000000000, 0xdc80000000000000, 0xdb00000000000000, 0xd980000000000000, 0xd880000000000000, 0xd700000000000000, 0xd580000000000000, 0xd400000000000000, 0xd280000000000000, 0xd180000000000000, 0xd000000000000000, 0xce80000000000000, 0xcd80000000000000, 0xcc00000000000000, 0xcb00000000000000, 0xc980000000000000, 0xc880000000000000, 0xc700000000000000, 0xc600000000000000, 0xc500000000000000, 0xc380000000000000, 0xc280000000000000, 0xc180000000000000, 0xc000000000000000, 0xbf00000000000000, 0xbe00000000000000, 0xbd00000000000000, 0xbc00000000000000, 0xba80000000000000, 0xb980000000000000, 0xb880000000000000, 0xb780000000000000, 0xb680000000000000, 0xb580000000000000, 0xb480000000000000,];
            ReadOnlySpan<ulong> Inverse21Lo = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,];
            ReadOnlySpan<int> Inverse21Ex = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,];
            ReadOnlySpan<byte> Inverse21Sgn = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,];

            ReadOnlySpan<ulong> Inverse22Hi = [0x8100000000000000, 0x80fc000000000000, 0x80f8000000000000, 0x80f4000000000000, 0x80f0000000000000, 0x80ec000000000000, 0x80e8000000000000, 0x80e4000000000000, 0x80e0000000000000, 0x80dc000000000000, 0x80d8000000000000, 0x80d4000000000000, 0x80d0000000000000, 0x80cc000000000000, 0x80c8000000000000, 0x80c4000000000000, 0x80c0000000000000, 0x80bc000000000000, 0x80b8000000000000, 0x80b4000000000000, 0x80b0000000000000, 0x80ac000000000000, 0x80a8000000000000, 0x80a4000000000000, 0x80a0000000000000, 0x809c000000000000, 0x8098000000000000, 0x8094000000000000, 0x8090000000000000, 0x808c000000000000, 0x8088000000000000, 0x8084000000000000, 0x8080000000000000, 0x807c000000000000, 0x8078000000000000, 0x8074000000000000, 0x8070000000000000, 0x806c000000000000, 0x8068000000000000, 0x8064000000000000, 0x8060000000000000, 0x805c000000000000, 0x8058000000000000, 0x8054000000000000, 0x8050000000000000, 0x804c000000000000, 0x8048000000000000, 0x8044000000000000, 0x8040000000000000, 0x803c000000000000, 0x8038000000000000, 0x8034000000000000, 0x8030000000000000, 0x802c000000000000, 0x8028000000000000, 0x8024000000000000, 0x8020000000000000, 0x801c000000000000, 0x8018000000000000, 0x8014000000000000, 0x8010000000000000, 0x800c000000000000, 0x8008000000000000, 0x8000000000000000, 0x8000000000000000, 0xfff4000000000000, 0xffec000000000000, 0xffe4000000000000, 0xffdc000000000000, 0xffd4000000000000, 0xffcc000000000000, 0xffc4000000000000, 0xffbc000000000000, 0xffb4000000000000, 0xffac000000000000, 0xffa4000000000000, 0xff9c000000000000, 0xff94000000000000, 0xff8c000000000000, 0xff84000000000000, 0xff7c000000000000, 0xff74000000000000, 0xff6c000000000000, 0xff64000000000000, 0xff5c000000000000, 0xff54000000000000, 0xff4c000000000000, 0xff44000000000000, 0xff3c000000000000, 0xff34000000000000, 0xff2c000000000000, 0xff24000000000000, 0xff1c000000000000, 0xff14000000000000, 0xff0c000000000000, 0xff04000000000000, 0xfefc000000000000, 0xfef4000000000000, 0xfeec000000000000, 0xfee4000000000000, 0xfedc000000000000, 0xfed4000000000000, 0xfecc000000000000, 0xfec4000000000000, 0xfebc000000000000, 0xfeb4000000000000, 0xfeac000000000000, 0xfea4000000000000, 0xfe9c000000000000, 0xfe98000000000000, 0xfe90000000000000, 0xfe88000000000000, 0xfe80000000000000, 0xfe78000000000000, 0xfe70000000000000, 0xfe68000000000000, 0xfe60000000000000, 0xfe58000000000000, 0xfe50000000000000, 0xfe48000000000000, 0xfe40000000000000, 0xfe38000000000000, 0xfe30000000000000, 0xfe28000000000000, 0xfe20000000000000, 0xfe18000000000000, 0xfe10000000000000, 0xfe08000000000000, 0xfe00000000000000,];
            ReadOnlySpan<ulong> Inverse22Lo = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,];
            ReadOnlySpan<int> Inverse22Ex = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,];
            ReadOnlySpan<byte> Inverse22Sgn = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,];

            ReadOnlySpan<ulong> LogInv21Hi = [0xb1641795ce3ca97b, 0xabb3b8ba2ad362a4, 0xa5f2fcabbbc506da, 0xa0218434353f1de8, 0x9bb93315fec2d792, 0x95c981d5c4e924ed, 0x914a0fde7bcb2d12, 0x8b3ae55d5d30701c, 0x86a35abcd5ba5903, 0x8073622d6a80e634, 0xf7856e5ee2c9b290, 0xee0de5055f63eb06, 0xe47fbe3cd4d10d61, 0xdada8cf47dad2374, 0xd11de0ff15ab18c9, 0xc74946f4436a0552, 0xbd5c481086c848df, 0xb3566a13956a86f6, 0xa9372f1d0da1bd17, 0x9efe158766314e54, 0x981eb8c723fe97f4, 0x8db956a97b3d0148, 0x8338a89652cb7150, 0xf85186008b15330b, 0xe2f2a47ade3a18ae, 0xd49369d256ab1b28, 0xbed3b36bd8966422, 0xb032c549ba861d8e, 0xa176e5f5323781dd, 0x8b29b7751bd70743, 0xf85186008b15330b, 0xda16eb88cb8df614, 0xac52dd7e4726a463, 0x8d86cc491ecbfe16, 0xdcfe013d7c8cbfde, 0x9e75221a352ba779, 0xbee23afc0853b6e9, 0x0, 0x0, 0xc122451c45155104, 0xa195492cc06604e6, 0xe31e9760a5578c63, 0x8a4f1f2002d46756, 0xab8ae2601e777722, 0xcd0c3dab9ef3dd1b, 0xe65b9e6eed965c36, 0x842cc5acf1d03445, 0x9103dae3c2a4ec67, 0xa242f01edefd6a37, 0xaf4ad26cbc8e5be7, 0xc0cbf17a071f80dc, 0xce06196a692a41fb, 0xdb56446d6ad8deff, 0xe8bcbc410c9b219d, 0xf639cc185088fe5d, 0x842cc5acf1d03445, 0x8b064012593d85a5, 0x91eb89524e100d23, 0x98dcca69d27c263b, 0x9fda2d2cc9465c4f, 0xa6e3dc4bde0e3cdb, 0xab9be6480c66ea9e, 0xb2ba75f46099cf8b, 0xb9e5c83a7e8a655b, 0xc11e0b2a8d1e0ddb, 0xc8636dcfe5e6ca0a, 0xcd43bc6f5d51c3e8, 0xd49f69e456cf1b79, 0xdc08b985c11e9068, 0xe1014558bfcda3e2, 0xe881bf932af3dac0, 0xed89ed86a44a01aa, 0xf52224f82557a459, 0xfa3a589a6f9146d8, 0x80f572b1363487b9, 0x8389c3026ac3139b, 0x86216b3b0b17188b, 0x8a0b3f79b3bc180f, 0x8cab69dcde17d2f7, 0x8f4f0b3c44cfa2a2, 0x934b1089a6dc93c1, 0x95f783e6e49a9cfa, 0x98a78f0e9ae71d85, 0x9b5b3bb5f088b766, 0x9e1293b9998c1daa, 0xa22c8f029cfa45a9, 0xa4ed3f9de620f666, 0xa7b1bf5dd4c07d4e, 0xaa7a18dbdf0d44aa, 0xad4656ddf6fd070c, 0xb0168457848f5f48, 0xb2eaac6a67005513,];
            ReadOnlySpan<ulong> LogInv21Lo = [0x7af915300e517391, 0xd5b6506cc17a01f1, 0x64ca4fb7ec323d73, 0x6093efa632530ac8, 0xa7589fba0865790e, 0x29404f5aa577d6b2, 0x1429ed3aea197a5d, 0xe63eab883717047e, 0xec81c3cbd925cccf, 0x6a97009015316071, 0xc6f2a1b84190a7d7, 0x98a33316df83ba57, 0x2ec0f797fdcd1257, 0x4ffb833c3409ee78, 0xb88d83d4cc613f20, 0xc4f5cb531201c0d1, 0x1b596b5030403240, 0xff1b1e1574d9fd54, 0x200eb71e58cd36de, 0xc571827efe892fc4, 0xa31c134fb702d432, 0x3023472cd739f9de, 0xc647eb86498c2ce1, 0xe64b8b775997898d, 0xb0bf7c0b0d8bb4ed, 0x5e9154e1d5263cd5, 0x240644d7d9ed08af, 0xf74e27bc92ce336a, 0xd4f935996c92e8cc, 0x12e0b9ee992f236d, 0xe64b8b775997898d, 0x68a63ecfb66e94ac, 0x547a963a91bb3012, 0x51776453b7e8254d, 0xa32dbac46f30cfff, 0xa52b7ea62f2198d0, 0x289782c20df350a1, 0x0, 0x0, 0xb16137f09a002b3c, 0x4a18dff7cdb4ae5c, 0xf9eb2f284f31c35c, 0x5be970314148c645, 0x3b89d7f254f8d4d, 0x13b26f298aa357c8, 0xe09f5fe2058d6006, 0x1fecdfa819b96098, 0xe0863df62ab5671a, 0x469355b78dc796e3, 0xe8b8b88a14ff0ce, 0xf96ffdf76a147ccc, 0xbe3ccc15326765f, 0xa8112e35a60e6375, 0xaf7df76ad29e5b60, 0x4066e87f2c0f7340, 0x1fecdfa819b96098, 0x52013c7a80ad089b, 0x8fd3df5c52d67e7b, 0x8e94203f336fc8c5, 0x32b9565f5355182, 0x570ff874170d2a9, 0x9ae21fd871b8d27c, 0x2c3c2e77904afa78, 0xcbffe9661fe72421, 0x9a631e830fd30904, 0x88e72835b3292d50, 0xfbfb0e3f0fd23074, 0x5f53bd2e406e66e7, 0x3b9cd767c3b1ac53, 0x35470a74be1230ec, 0xc524848e3443e040, 0x11d49f96cb88317b, 0x8dcca8d7f17fa2a9, 0x388212895529a6fb, 0xf5bd0b5b3479d5f4, 0x62dda9d2270fa1f4, 0x163ceae88f720f1e, 0x49b55ea7d3730d7, 0x3ad1aa142b94f16a, 0x586e9343c9cfdbac, 0xdf5bb3b60554e152, 0x4a5004f3ef063313, 0x2cdec34784707839, 0xd878bbe3d392be25, 0x5b035eae273a855f, 0xdb5b709e0b69e773, 0x9b5e973353638c11, 0x699db68db75e9a7f, 0x604884a8dd76d08a, 0x9ea10260fe452ba2, 0xbb6f9fb246068d52, 0xf4b716f6fec8156b,];
            ReadOnlySpan<int> LogInv21Ex = [-2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -4, -4, -4, -4, -4, -4, -4, -5, -5, -5, -5, -6, -6, -7, 127, 127, -7, -6, -6, -5, -5, -5, -5, -4, -4, -4, -4, -4, -4, -4, -4, -4, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2,];
            ReadOnlySpan<byte> LogInv21Sgn = [0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,];

            ReadOnlySpan<ulong> LogInv22Hi = [0xff015358833c47e1, 0xfb0933b732572a6d, 0xf710f492711d9d26, 0xf31895e84b1a6be6, 0xef2017b6cba9cf9a, 0xeb2779fbfdf96874, 0xe72ebcb5ed08382b, 0xe335dfe2a3a69c2b, 0xdf3ce3802c7647cd, 0xdb43c78c91ea3e8c, 0xd74a8c05de46ce3a, 0xd35130ea1ba18930, 0xcf57b63753e14083, 0xcb5e1beb90bdfe33, 0xc7646204dbc0ff5e, 0xc36a88813e44ae6a, 0xbf708f5ec1749d3c, 0xbb76769b6e4d7f5c, 0xb77c3e354d9d242b, 0xb381e62a68027106, 0xaf876e78c5ed5b77, 0xab8cd71e6f9ee35d, 0xa79220196d290d15, 0xa3974967c66edba1, 0x9f9c530783244ad2, 0x9ba13cf6aace496c, 0x97a6073344c2b34b, 0x93aab1bb58284b8b, 0x8faf3c8cebf6b6a8, 0x8bb3a7a606f674a0, 0x87b7f304afc0db1a, 0x83bc1ea6ecc00f81, 0xff805515885e0250, 0xf7882d5c7832c6cc, 0xef8fc61eb4b74f6e, 0xe7971f584945efae, 0xdf9e390540da5fbe, 0xd7a51321a611b0c1, 0xcfabada9832a4101, 0xc7b20898e203b01e, 0xbfb823ebcc1ed344, 0xb7bdff9e4a9da959, 0xafc39bac66434f27, 0xa7c8f8122773f38d, 0x9fce14cb9634cba6, 0x97d2f1d4ba2c06f0, 0x8fd78f299aa0c375, 0x87dbecc63e7b01ed, 0xffc0154d588733c5, 0xefc7d18dd4485b9e, 0xdfcf0e45fbce3e80, 0xcfd5cb6dd9ef05dd, 0xbfdc08fd78c229b9, 0xafe1c6ece1a058dd, 0x9fe705341d236102, 0x8febc3cb332616ff, 0xffe0055455887de0, 0xdfe7839214b4e8ae, 0xbfee023faf0c2480, 0x9ff3814d2e4a36b2, 0xfff0015535588833, 0xbff7008ff5e0c257, 0xfff8005551558885, 0x0, 0x0, 0xc004802401440c26, 0xa00640535a37a37a, 0xe00c40e4bd6e4efd, 0x900a20f319a3e273, 0xb00f21bbe3e388ee, 0xd01522dcc4f87991, 0xf01c2465c5e61b6f, 0x881213337898871e, 0x98169478296fad41, 0xa81b9608fc3c50ec, 0xb82117edf8832797, 0xc8271a2f2689e388, 0xd82d9cd48f574c00, 0xe8349fe63cb35564, 0xf83c236c39273972, 0x842213b747fec7bb, 0x8c2655faa6a1323f, 0x942ad8843ee1a9cd, 0x9c2f9b581787cf0d, 0xa4349e7a37bc21ed, 0xac39e1eea7080dbc, 0xb43f65b96d55f55a, 0xbc4529de92f13f58, 0xc44b2e6220866227, 0xcc5173481f22f03f, 0xd457f8949835a44e, 0xdc5ebe4b958e6d6b, 0xe465c471215e7b41, 0xec6d0b0946384a46, 0xf47492180f0fafef, 0xfc7c59a18739e6e7, 0x824230d4dd36cda4, 0x8646551a5a617b6b, 0x8a4a99a34159d69f, 0x8e4efe71988d8426, 0x92538387669afa1b, 0x965828e6b25185ec, 0x9a5cee9182b15280, 0x9e61d489deeb6e53, 0xa266dad1ce61d1a3, 0xa66c016b58a7648c, 0xaa71485885800538, 0xae76af9b5ce08dfb, 0xb27c3735e6eedb86, 0xb47f0724b1906935, 0xb884bf4697559ffa, 0xbc8a97c544fdd5eb, 0xc09090a2c35aa070, 0xc496a9e11b6eb30c, 0xc89ce382566de587, 0xcca33d887dbd3a1a, 0xd0a9b7f59af2e3a2, 0xd4b052cbb7d64bcf, 0xd8b70e0cde601954, 0xdcbde9bb18ba361b, 0xe0c4e5d8713fd576, 0xe4cc0266f27d7a57, 0xe8d33f68a730fd7f, 0xecda9cdf9a4993ba, 0xf0e21acdd6e7d412, 0xf4e9b935685dbe0b, 0xf8f178185a2ebfd9, 0xfcf95778b80fbc98, 0x8080abac46f38946,];
            ReadOnlySpan<ulong> LogInv22Lo = [0xbb481c8ee141695a, 0x214cca3dd1d4796a, 0xfbc7b38b17b2019, 0xb76782b9e88c84cb, 0x2dc85881664025b5, 0xce4ab4e678d0ed03, 0xb60585f4c4bb6062, 0x59bcffe9d5650564, 0x3602021fa93b1e18, 0x9944002534d09b3d, 0x87aa95782311a277, 0xb88be10313a1303d, 0xad54bc31433dddba, 0xe1b7d813e3f825e1, 0x14f8c1be7370f219, 0xac27c5a6139cd30c, 0x2d23a0744e00f594, 0xd235e25fb9644c31, 0x361ee0bcb5db0449, 0x18660815da3d7963, 0x39c357b6bfdf81b5, 0x5076c62c951204f6, 0x146244d643f7fa2b, 0x62bb0f3208d9a1bb, 0x7926e92808bd580d, 0x4819e620d5fcc068, 0xdc494943d427214e, 0xdf0805c4161e404c, 0x2d615caaa0514c3c, 0x85c60c12eca0aedc, 0x4c207a522524f8de, 0x64243e02c6215a4f, 0x435ab4da6a5bb48d, 0x9e06fc84b6ea5e24, 0x91ab122ee427cfb5, 0x5f832513e3211643, 0x5e7b48cfeeb85aa8, 0xb36a9f58eb4ccd08, 0x3360751e43c7af35, 0x6fab78aca91193cb, 0xeb432409cffdad8d, 0x793b5acf3a336462, 0xc3ea2cd93f316b34, 0xfc679a28e9d9f212, 0xb20f215bd3b58c61, 0xd1aacedcefe9d377, 0xcbef6fac33691e95, 0xe2f1775134c8da75, 0x3c742a7c76356396, 0xca47c52b7d7ffce2, 0x7e4cfbd830393b88, 0x7370ae83f9e72748, 0xe6dbb624f9739782, 0x97fa2fd0c9dc723e, 0x7199cd06ae5d39b3, 0x7b6d1248c3e1fd40, 0x26828c92649a3a39, 0xda6959f7f0e01bf0, 0xb47505bfa5a03b06, 0xa8740b91c95df537, 0x3c56c598c659c2a3, 0x379eba7e6465ff63, 0xde026e271ee0549d, 0x0, 0x0, 0xdfeb485085f6f454, 0x6bc1e20eac8448b4, 0xc72446cc1bf728bd, 0x569b26aaa485ea5c, 0x5f69768284463b9b, 0x14d9d76196d8043a, 0x661e135f49a47c40, 0x9a31ba0cbc030353, 0x7ad1e9c315328f7e, 0xf105b66ec4703ede, 0xd6aef30cd312169a, 0xe6e2acf8f4d4c24a, 0x28bb3cd9f2a65fb5, 0x224a96f5a7471c46, 0xd462b63756c87e80, 0x3ff51287882500ed, 0x1ab9679b55f78a6b, 0x17e4b7ac6c600cb4, 0xfd1a09c848e3950e, 0x318b2ddd9d0a33b4, 0x9dd91e52c79fd070, 0x72de1d99ce252efd, 0xd7bd1d62ef25480d, 0x7f921124f1ecb59e, 0x271ee1cd6d5cdf9e, 0xfad0cc8b5faea8cc, 0xe57a0acb9d5cd4df, 0xc81bb5a8d789f444, 0x9b1beb40437575f5, 0x7944509046652d99, 0x94e51ebff53a2f15, 0x8bbc7f765b13ebbe, 0xf61305ef7390939c, 0x3abc32a78afd4b7b, 0x17596a598cb29436, 0x1c890bee9a9d743c, 0xeaafbd07b543145d, 0x6517bc4112d64b17, 0xdb94a1dfd653d3a5, 0x2ada01ce7ed36080, 0xd3b36c029ea7bb5d, 0x94c529f32403828, 0xb6b6676248bba139, 0x7bdd0c2a9c7a679a, 0x23deb274e953a259, 0xdae7e343fa859415, 0x17759bff5c717993, 0x52e7e4dde874dace, 0xa88971f8277a4d11, 0x269de85f0df92588, 0x180d255422c3377c, 0x46da70925ee85c05, 0x37968ceafaf7b453, 0x5dfba4cfdd38a059, 0x4ae21abe75d5a19b, 0xd3bd4fd98a1e6fe5, 0x33cf7d5ebfb93ad3, 0x2743c805a4928087, 0x5dbeb9795455a5, 0xb6ed80852ae6fd63, 0xf237cff1acb306b3, 0xd81648249cece4c, 0x176cd56887ac7fe9, 0x662d417ced007a46,];
            ReadOnlySpan<int> LogInv22Ex = [-8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -10, -10, -10, -10, -10, -10, -10, -10, -11, -11, -11, -11, -12, -12, -13, 127, 127, -13, -12, -12, -11, -11, -11, -11, -10, -10, -10, -10, -10, -10, -10, -10, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -7,];
            ReadOnlySpan<byte> LogInv22Sgn = [0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0];



            long E = x.ex;
            ushort i, j;

            if (x.hi > 0xb504f333f9de6484)
            {
                E++;
                i = (ushort)(x.hi >> (63 + 1 - 7));
            }
            else
            {
                i = (ushort)(x.hi >> (63 - 7));
            }

            Dint xx = x with { ex = x.ex - E };

            Dint z = Dint.Mul11(xx, new Dint(Inverse21Lo[i - 90], Inverse21Hi[i - 90], Inverse21Ex[i - 90], Inverse21Sgn[i - 90]));

            j = (ushort)(z.hi >> (63 - 13 - (int)z.ex));

            z = Dint.Mul11(z, new Dint(Inverse22Lo[j - 8128], Inverse22Hi[j - 8128], Inverse22Ex[j - 8128], Inverse22Sgn[j - 8128]));

            z = Dint.Add(Dint.MinusOne, z);

            Dint r = Dint.MulInt64(Dint.Log2, E);

            Dint p = p2(z);

            p = Dint.Add(new Dint(LogInv22Lo[j - 8128], LogInv22Hi[j - 8128], LogInv22Ex[j - 8128], LogInv22Sgn[j - 8128]), p);
            p = Dint.Add(new Dint(LogInv21Lo[i - 90], LogInv21Hi[i - 90], LogInv21Ex[i - 90], LogInv21Sgn[i - 90]), p);

            r = Dint.Add(p, r);

            return (r, xx);
        }

        static (Qint r, Qint x) log3(in Qint x)
        {
            ReadOnlySpan<ulong> Inverse31Hh = [0xb500000000000000, 0xb300000000000000, 0xb100000000000000, 0xaf00000000000000, 0xad80000000000000, 0xab80000000000000, 0xaa00000000000000, 0xa800000000000000, 0xa680000000000000, 0xa480000000000000, 0xa300000000000000, 0xa180000000000000, 0xa000000000000000, 0x9e80000000000000, 0x9d00000000000000, 0x9b80000000000000, 0x9a00000000000000, 0x9880000000000000, 0x9700000000000000, 0x9580000000000000, 0x9480000000000000, 0x9300000000000000, 0x9180000000000000, 0x9080000000000000, 0x8f00000000000000, 0x8e00000000000000, 0x8c80000000000000, 0x8b80000000000000, 0x8a80000000000000, 0x8900000000000000, 0x8800000000000000, 0x8700000000000000, 0x8580000000000000, 0x8480000000000000, 0x8380000000000000, 0x8280000000000000, 0x8180000000000000, 0x8000000000000000, 0x8000000000000000, 0xfd00000000000000, 0xfb00000000000000, 0xf900000000000000, 0xf780000000000000, 0xf580000000000000, 0xf380000000000000, 0xf200000000000000, 0xf000000000000000, 0xee80000000000000, 0xec80000000000000, 0xeb00000000000000, 0xe900000000000000, 0xe780000000000000, 0xe600000000000000, 0xe480000000000000, 0xe300000000000000, 0xe100000000000000, 0xdf80000000000000, 0xde00000000000000, 0xdc80000000000000, 0xdb00000000000000, 0xd980000000000000, 0xd880000000000000, 0xd700000000000000, 0xd580000000000000, 0xd400000000000000, 0xd280000000000000, 0xd180000000000000, 0xd000000000000000, 0xce80000000000000, 0xcd80000000000000, 0xcc00000000000000, 0xcb00000000000000, 0xc980000000000000, 0xc880000000000000, 0xc700000000000000, 0xc600000000000000, 0xc500000000000000, 0xc380000000000000, 0xc280000000000000, 0xc180000000000000, 0xc000000000000000, 0xbf00000000000000, 0xbe00000000000000, 0xbd00000000000000, 0xbc00000000000000, 0xba80000000000000, 0xb980000000000000, 0xb880000000000000, 0xb780000000000000, 0xb680000000000000, 0xb580000000000000, 0xb480000000000000,];
            ReadOnlySpan<ulong> Inverse31Hl = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0];
            ReadOnlySpan<ulong> Inverse31Lh = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0];
            ReadOnlySpan<ulong> Inverse31Ll = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0];
            ReadOnlySpan<int> Inverse31Ex = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,];
            ReadOnlySpan<byte> Inverse31Sgn = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0];

            ReadOnlySpan<ulong> Inverse32Hh = [0x8100000000000000, 0x80fc000000000000, 0x80f8000000000000, 0x80f4000000000000, 0x80f0000000000000, 0x80ec000000000000, 0x80e8000000000000, 0x80e4000000000000, 0x80e0000000000000, 0x80dc000000000000, 0x80d8000000000000, 0x80d4000000000000, 0x80d0000000000000, 0x80cc000000000000, 0x80c8000000000000, 0x80c4000000000000, 0x80c0000000000000, 0x80bc000000000000, 0x80b8000000000000, 0x80b4000000000000, 0x80b0000000000000, 0x80ac000000000000, 0x80a8000000000000, 0x80a4000000000000, 0x80a0000000000000, 0x809c000000000000, 0x8098000000000000, 0x8094000000000000, 0x8090000000000000, 0x808c000000000000, 0x8088000000000000, 0x8084000000000000, 0x8080000000000000, 0x807c000000000000, 0x8078000000000000, 0x8074000000000000, 0x8070000000000000, 0x806c000000000000, 0x8068000000000000, 0x8064000000000000, 0x8060000000000000, 0x805c000000000000, 0x8058000000000000, 0x8054000000000000, 0x8050000000000000, 0x804c000000000000, 0x8048000000000000, 0x8044000000000000, 0x8040000000000000, 0x803c000000000000, 0x8038000000000000, 0x8034000000000000, 0x8030000000000000, 0x802c000000000000, 0x8028000000000000, 0x8024000000000000, 0x8020000000000000, 0x801c000000000000, 0x8018000000000000, 0x8014000000000000, 0x8010000000000000, 0x800c000000000000, 0x8008000000000000, 0x8000000000000000, 0x8000000000000000, 0xfff4000000000000, 0xffec000000000000, 0xffe4000000000000, 0xffdc000000000000, 0xffd4000000000000, 0xffcc000000000000, 0xffc4000000000000, 0xffbc000000000000, 0xffb4000000000000, 0xffac000000000000, 0xffa4000000000000, 0xff9c000000000000, 0xff94000000000000, 0xff8c000000000000, 0xff84000000000000, 0xff7c000000000000, 0xff74000000000000, 0xff6c000000000000, 0xff64000000000000, 0xff5c000000000000, 0xff54000000000000, 0xff4c000000000000, 0xff44000000000000, 0xff3c000000000000, 0xff34000000000000, 0xff2c000000000000, 0xff24000000000000, 0xff1c000000000000, 0xff14000000000000, 0xff0c000000000000, 0xff04000000000000, 0xfefc000000000000, 0xfef4000000000000, 0xfeec000000000000, 0xfee4000000000000, 0xfedc000000000000, 0xfed4000000000000, 0xfecc000000000000, 0xfec4000000000000, 0xfebc000000000000, 0xfeb4000000000000, 0xfeac000000000000, 0xfea4000000000000, 0xfe9c000000000000, 0xfe98000000000000, 0xfe90000000000000, 0xfe88000000000000, 0xfe80000000000000, 0xfe78000000000000, 0xfe70000000000000, 0xfe68000000000000, 0xfe60000000000000, 0xfe58000000000000, 0xfe50000000000000, 0xfe48000000000000, 0xfe40000000000000, 0xfe38000000000000, 0xfe30000000000000, 0xfe28000000000000, 0xfe20000000000000, 0xfe18000000000000, 0xfe10000000000000, 0xfe08000000000000, 0xfe00000000000000,];
            ReadOnlySpan<ulong> Inverse32Hl = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,];
            ReadOnlySpan<ulong> Inverse32Lh = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,];
            ReadOnlySpan<ulong> Inverse32Ll = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,];
            ReadOnlySpan<int> Inverse32Ex = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,];
            ReadOnlySpan<byte> Inverse32Sgn = [0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,];

            ReadOnlySpan<ulong> LogInv31Hh = [0xb1641795ce3ca97b, 0xabb3b8ba2ad362a4, 0xa5f2fcabbbc506da, 0xa0218434353f1de8, 0x9bb93315fec2d792, 0x95c981d5c4e924ed, 0x914a0fde7bcb2d12, 0x8b3ae55d5d30701c, 0x86a35abcd5ba5903, 0x8073622d6a80e634, 0xf7856e5ee2c9b290, 0xee0de5055f63eb06, 0xe47fbe3cd4d10d61, 0xdada8cf47dad2374, 0xd11de0ff15ab18c9, 0xc74946f4436a0552, 0xbd5c481086c848df, 0xb3566a13956a86f6, 0xa9372f1d0da1bd17, 0x9efe158766314e54, 0x981eb8c723fe97f4, 0x8db956a97b3d0148, 0x8338a89652cb7150, 0xf85186008b15330b, 0xe2f2a47ade3a18ae, 0xd49369d256ab1b28, 0xbed3b36bd8966422, 0xb032c549ba861d8e, 0xa176e5f5323781dd, 0x8b29b7751bd70743, 0xf85186008b15330b, 0xda16eb88cb8df614, 0xac52dd7e4726a463, 0x8d86cc491ecbfe16, 0xdcfe013d7c8cbfde, 0x9e75221a352ba779, 0xbee23afc0853b6e9, 0x0, 0x0, 0xc122451c45155104, 0xa195492cc06604e6, 0xe31e9760a5578c63, 0x8a4f1f2002d46756, 0xab8ae2601e777722, 0xcd0c3dab9ef3dd1b, 0xe65b9e6eed965c36, 0x842cc5acf1d03445, 0x9103dae3c2a4ec67, 0xa242f01edefd6a37, 0xaf4ad26cbc8e5be7, 0xc0cbf17a071f80dc, 0xce06196a692a41fb, 0xdb56446d6ad8deff, 0xe8bcbc410c9b219d, 0xf639cc185088fe5d, 0x842cc5acf1d03445, 0x8b064012593d85a5, 0x91eb89524e100d23, 0x98dcca69d27c263b, 0x9fda2d2cc9465c4f, 0xa6e3dc4bde0e3cdb, 0xab9be6480c66ea9e, 0xb2ba75f46099cf8b, 0xb9e5c83a7e8a655b, 0xc11e0b2a8d1e0ddb, 0xc8636dcfe5e6ca0a, 0xcd43bc6f5d51c3e8, 0xd49f69e456cf1b79, 0xdc08b985c11e9068, 0xe1014558bfcda3e2, 0xe881bf932af3dac0, 0xed89ed86a44a01aa, 0xf52224f82557a459, 0xfa3a589a6f9146d8, 0x80f572b1363487b9, 0x8389c3026ac3139b, 0x86216b3b0b17188b, 0x8a0b3f79b3bc180f, 0x8cab69dcde17d2f7, 0x8f4f0b3c44cfa2a2, 0x934b1089a6dc93c1, 0x95f783e6e49a9cfa, 0x98a78f0e9ae71d85, 0x9b5b3bb5f088b766, 0x9e1293b9998c1daa, 0xa22c8f029cfa45a9, 0xa4ed3f9de620f666, 0xa7b1bf5dd4c07d4e, 0xaa7a18dbdf0d44aa, 0xad4656ddf6fd070c, 0xb0168457848f5f48, 0xb2eaac6a67005513,];
            ReadOnlySpan<ulong> LogInv31Hl = [0x7af915300e517391, 0xd5b6506cc17a01f1, 0x64ca4fb7ec323d72, 0x6093efa632530ac8, 0xa7589fba0865790d, 0x29404f5aa577d6b1, 0x1429ed3aea197a5d, 0xe63eab883717047e, 0xec81c3cbd925cccf, 0x6a97009015316070, 0xc6f2a1b84190a7d6, 0x98a33316df83ba56, 0x2ec0f797fdcd1257, 0x4ffb833c3409ee78, 0xb88d83d4cc613f1f, 0xc4f5cb531201c0d0, 0x1b596b503040323f, 0xff1b1e1574d9fd53, 0x200eb71e58cd36de, 0xc571827efe892fc4, 0xa31c134fb702d431, 0x3023472cd739f9de, 0xc647eb86498c2ce1, 0xe64b8b775997898d, 0xb0bf7c0b0d8bb4ec, 0x5e9154e1d5263cd4, 0x240644d7d9ed08ae, 0xf74e27bc92ce336a, 0xd4f935996c92e8cb, 0x12e0b9ee992f236d, 0xe64b8b775997898d, 0x68a63ecfb66e94ab, 0x547a963a91bb3012, 0x51776453b7e8254d, 0xa32dbac46f30cffe, 0xa52b7ea62f2198d0, 0x289782c20df350a1, 0x0, 0x0, 0xb16137f09a002b3c, 0x4a18dff7cdb4ae5c, 0xf9eb2f284f31c35c, 0x5be970314148c644, 0x3b89d7f254f8d4d, 0x13b26f298aa357c8, 0xe09f5fe2058d6005, 0x1fecdfa819b96097, 0xe0863df62ab56719, 0x469355b78dc796e2, 0xe8b8b88a14ff0cd, 0xf96ffdf76a147ccc, 0xbe3ccc15326765f, 0xa8112e35a60e6374, 0xaf7df76ad29e5b5f, 0x4066e87f2c0f733f, 0x1fecdfa819b96097, 0x52013c7a80ad089b, 0x8fd3df5c52d67e7b, 0x8e94203f336fc8c4, 0x32b9565f5355181, 0x570ff874170d2a8, 0x9ae21fd871b8d27c, 0x2c3c2e77904afa78, 0xcbffe9661fe72421, 0x9a631e830fd30903, 0x88e72835b3292d4f, 0xfbfb0e3f0fd23074, 0x5f53bd2e406e66e7, 0x3b9cd767c3b1ac52, 0x35470a74be1230ec, 0xc524848e3443e03f, 0x11d49f96cb88317a, 0x8dcca8d7f17fa2a9, 0x388212895529a6fa, 0xf5bd0b5b3479d5f4, 0x62dda9d2270fa1f4, 0x163ceae88f720f1d, 0x49b55ea7d3730d7, 0x3ad1aa142b94f169, 0x586e9343c9cfdbac, 0xdf5bb3b60554e151, 0x4a5004f3ef063312, 0x2cdec34784707839, 0xd878bbe3d392be25, 0x5b035eae273a855e, 0xdb5b709e0b69e773, 0x9b5e973353638c10, 0x699db68db75e9a7e, 0x604884a8dd76d08a, 0x9ea10260fe452ba2, 0xbb6f9fb246068d52, 0xf4b716f6fec8156b,];
            ReadOnlySpan<ulong> LogInv31Lh = [0x362aee92bfa25a80, 0x706866327ef7c050, 0xa68b0ce7a5e0a7ea, 0x304fb3b2345b41a9, 0x82b75e91fcdfa14e, 0xba0ea3f2ae1e1d07, 0x355a6f4f0ec5ce8f, 0xcfa09487833ea69, 0x6a2f869f2c41ea0a, 0x9f1d0d49f7cf8122, 0x94261a0e91f0e8f2, 0xa28f0225cea42f20, 0x1d97a9d046b706c5, 0x3713df786be7d79f, 0x8db36c5996f30e02, 0xe377c62941756dda, 0xf0a4a6c408595abb, 0xd790e4993973cb21, 0x631daa222aa1cc5e, 0x5fb87ab4717a500, 0xa1267633d7a950a6, 0x3f642654cbb04a9b, 0x6fdaaacd24ed99fc, 0x3474d3375b525967, 0xb357c6e1bb965608, 0xfb3f11769cc680ef, 0x8bd331e0f0163a57, 0x476c441f8cbfb247, 0xb1ed0cd9e5eb16c4, 0x21482d3342d35569, 0x3474d3375b525967, 0xce26340fc53dc9e7, 0x6146c24c8704d774, 0x1fe3399d400c4228, 0xda998fa29b9bb98b, 0x797189a4ceffb772, 0x4943001d3f0647d1, 0x0, 0x0, 0x114425f06f494d45, 0x1b120e15ca3dceb7, 0x739276a47bc0067f, 0xd7177b23dafc1e78, 0x7fea49aded4406bd, 0x307b8ee396d79ef6, 0xb58f9a65c1043b41, 0xe362c7f8dd18e5cb, 0xe0c7d4d12db021b8, 0xb3c575a2031956ec, 0x9ad6b7f2deaa8ae6, 0x3700761ca4fb5278, 0x733187c6d6f39bb8, 0xdd62571dda9ce602, 0xe6a09f8913389334, 0x8296a39b87519924, 0xe362c7f8dd18e5cb, 0x42ada32f6b02af2d, 0x2024f18ebc9b8af6, 0xcfe4e777e9932f10, 0xdc751798a72b3dc9, 0xc7be23c834886156, 0x7851f4e516e5c9bf, 0x77fa400e7e689a3, 0x2096b17331fac5dc, 0xd59edb68f6b3f63b, 0xd07fb98b088395ee, 0x435c6598be364fab, 0x7188af8f4f45b9ee, 0x81fdd139ee15996c, 0x7ea4f73313d9cef6, 0xc22bd8fede6ee351, 0xb09cac07eab378a8, 0x330d7e7fe8c1c62a, 0x937d820ed16d615e, 0x501b8b4a63fd6f67, 0x29aec44c9ebb0731, 0x9a8ffa0ca490b651, 0x1f95d048ae9871b6, 0x82d7e38d0ce7657c, 0x1fee2e686760d584, 0x87a486e65aa1bcd5, 0xcac9f0589aff46a5, 0x4861cab8c5ee1c94, 0x24f04843d0f8f41, 0xf58182e4db06261c, 0x5e171935b5381c36, 0xd81763d28db5c039, 0x8e98852150ea76b5, 0x6c40e044972eeeb2, 0x59b0b64abac9cb07, 0x3e567a3312c2443d, 0x2c74b8f4ed61d394,];
            ReadOnlySpan<ulong> LogInv31Ll = [0x1646679ea2568305, 0x628f2d55f109eac9, 0xd6cad0f0b6c847ef, 0xe440d92b32eac488, 0x98d12c3138e33333, 0x758e4ab7f718ea9f, 0xc4eca5fff76cbf20, 0x8791b8732b281e2f, 0x261913d1bbc49faf, 0xc0baf08f2dd617cf, 0x960d286867d7da8d, 0xc8c87785c07b059d, 0xc3c4cfd592ff1d1b, 0xdfdccbdb9cc5e4fc, 0x67caaac70b1e203f, 0xbf31ff26e7952aa7, 0x18b2e81de5a7413d, 0xb570c1978023c83, 0xc53df36a99bd161e, 0x73890974d65b5cfd, 0xf1d435478be2e98d, 0x501b839196278b37, 0x73619b3ac0a2580e, 0x1851f0a96f698496, 0xb1f3fb65de3326ac, 0x5588fd21488d3117, 0xad0c8b665d0ba662, 0xb421b4cceddb6dec, 0xd070037b7a65dbb6, 0xbf365c4132567724, 0x1851f0a96f698496, 0x7778bea9e4485112, 0x9181c8e24fdd9bf3, 0x939ae69b03a586bc, 0x121d35ae45b4e2e1, 0x67b1205aea8ed5f1, 0x907db46b91a9be11, 0x0, 0x0, 0xe2fa6c6fbca3a43a, 0x62148cc46c40db43, 0x13d3be097b445a02, 0x62496477de5b9b70, 0xbd68cbc383b2b959, 0x801ddc72263552e3, 0xe2faca238c300fa9, 0x2c885ebb0a67bc24, 0x55776e4da79f922a, 0x395fd54d8695b022, 0x47e37e4affafd5a1, 0xf6961709acabf991, 0xe3f54edc6e2e0350, 0xe44b58c81d361f18, 0x7de7a2535752b786, 0x5a18333a98b0adbc, 0x2c885ebb0a67bc24, 0x88cfab3b8ffc09bc, 0x81f936979eefae26, 0x9ed7ba1820b6ff5, 0x56eca518e7d2fe44, 0xbd706a5a2627f5aa, 0xc1c5a79f56e96b76, 0x30a5f043d61bad59, 0x7775b110db2591bd, 0x9ede162ed215bb6b, 0x10b2726db3c15971, 0x938d2827ce902e72, 0x7ec0d7bfb6b65f0d, 0xdf9da9ae69110ecb, 0x7a6dd1127f07bf9a, 0x93f48308bc589a07, 0xe6342851611cc8be, 0x18d25c613e0e9a6d, 0x6bf1e0ae92585a10, 0x6e9f54a7361289b3, 0xee2b9479c54b01fb, 0x815baea454ee87ca, 0xb7eb0007a2dfe5c0, 0x800bbe6769b633e0, 0xc0243088b56a6c0f, 0xad047f998c197d96, 0x3b2136f4975a446e, 0xb0ddc91e9b86138f, 0xd27746bfed0adcfe, 0x73db477d896b83f5, 0x930e7dec83978f45, 0xaef301c1c91f3649, 0x2d4a158846053fd7, 0x8d4e70541a83bec5, 0x30a999bf35a66756, 0xe963d8ddfd9f7f8b, 0x1166335bb2b54fe0,];
            ReadOnlySpan<int> LogInv31Ex = [-2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -4, -4, -4, -4, -4, -4, -4, -5, -5, -5, -5, -6, -6, -7, 255, 255, -7, -6, -6, -5, -5, -5, -5, -4, -4, -4, -4, -4, -4, -4, -4, -4, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2,];
            ReadOnlySpan<byte> LogInv31Sgn = [0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,];

            ReadOnlySpan<ulong> LogInv32Hh = [0xff015358833c47e1, 0xfb0933b732572a6d, 0xf710f492711d9d26, 0xf31895e84b1a6be6, 0xef2017b6cba9cf9a, 0xeb2779fbfdf96874, 0xe72ebcb5ed08382b, 0xe335dfe2a3a69c2b, 0xdf3ce3802c7647cd, 0xdb43c78c91ea3e8c, 0xd74a8c05de46ce3a, 0xd35130ea1ba18930, 0xcf57b63753e14083, 0xcb5e1beb90bdfe33, 0xc7646204dbc0ff5e, 0xc36a88813e44ae6a, 0xbf708f5ec1749d3c, 0xbb76769b6e4d7f5c, 0xb77c3e354d9d242b, 0xb381e62a68027106, 0xaf876e78c5ed5b77, 0xab8cd71e6f9ee35d, 0xa79220196d290d15, 0xa3974967c66edba1, 0x9f9c530783244ad2, 0x9ba13cf6aace496c, 0x97a6073344c2b34b, 0x93aab1bb58284b8b, 0x8faf3c8cebf6b6a8, 0x8bb3a7a606f674a0, 0x87b7f304afc0db1a, 0x83bc1ea6ecc00f81, 0xff805515885e0250, 0xf7882d5c7832c6cc, 0xef8fc61eb4b74f6e, 0xe7971f584945efae, 0xdf9e390540da5fbe, 0xd7a51321a611b0c1, 0xcfabada9832a4101, 0xc7b20898e203b01e, 0xbfb823ebcc1ed344, 0xb7bdff9e4a9da959, 0xafc39bac66434f27, 0xa7c8f8122773f38d, 0x9fce14cb9634cba6, 0x97d2f1d4ba2c06f0, 0x8fd78f299aa0c375, 0x87dbecc63e7b01ed, 0xffc0154d588733c5, 0xefc7d18dd4485b9e, 0xdfcf0e45fbce3e80, 0xcfd5cb6dd9ef05dd, 0xbfdc08fd78c229b9, 0xafe1c6ece1a058dd, 0x9fe705341d236102, 0x8febc3cb332616ff, 0xffe0055455887de0, 0xdfe7839214b4e8ae, 0xbfee023faf0c2480, 0x9ff3814d2e4a36b2, 0xfff0015535588833, 0xbff7008ff5e0c257, 0xfff8005551558885, 0x0, 0x0, 0xc004802401440c26, 0xa00640535a37a37a, 0xe00c40e4bd6e4efd, 0x900a20f319a3e273, 0xb00f21bbe3e388ee, 0xd01522dcc4f87991, 0xf01c2465c5e61b6f, 0x881213337898871e, 0x98169478296fad41, 0xa81b9608fc3c50ec, 0xb82117edf8832797, 0xc8271a2f2689e388, 0xd82d9cd48f574c00, 0xe8349fe63cb35564, 0xf83c236c39273972, 0x842213b747fec7bb, 0x8c2655faa6a1323f, 0x942ad8843ee1a9cd, 0x9c2f9b581787cf0d, 0xa4349e7a37bc21ed, 0xac39e1eea7080dbc, 0xb43f65b96d55f55a, 0xbc4529de92f13f58, 0xc44b2e6220866227, 0xcc5173481f22f03f, 0xd457f8949835a44e, 0xdc5ebe4b958e6d6b, 0xe465c471215e7b41, 0xec6d0b0946384a46, 0xf47492180f0fafef, 0xfc7c59a18739e6e7, 0x824230d4dd36cda4, 0x8646551a5a617b6b, 0x8a4a99a34159d69f, 0x8e4efe71988d8426, 0x92538387669afa1b, 0x965828e6b25185ec, 0x9a5cee9182b15280, 0x9e61d489deeb6e53, 0xa266dad1ce61d1a3, 0xa66c016b58a7648c, 0xaa71485885800538, 0xae76af9b5ce08dfb, 0xb27c3735e6eedb86, 0xb47f0724b1906935, 0xb884bf4697559ffa, 0xbc8a97c544fdd5eb, 0xc09090a2c35aa070, 0xc496a9e11b6eb30c, 0xc89ce382566de587, 0xcca33d887dbd3a1a, 0xd0a9b7f59af2e3a2, 0xd4b052cbb7d64bcf, 0xd8b70e0cde601954, 0xdcbde9bb18ba361b, 0xe0c4e5d8713fd576, 0xe4cc0266f27d7a57, 0xe8d33f68a730fd7f, 0xecda9cdf9a4993ba, 0xf0e21acdd6e7d412, 0xf4e9b935685dbe0b, 0xf8f178185a2ebfd9, 0xfcf95778b80fbc98, 0x8080abac46f38946,];
            ReadOnlySpan<ulong> LogInv32Hl = [0xbb481c8ee1416959, 0x214cca3dd1d4796a, 0xfbc7b38b17b2019, 0xb76782b9e88c84cb, 0x2dc85881664025b4, 0xce4ab4e678d0ed03, 0xb60585f4c4bb6062, 0x59bcffe9d5650564, 0x3602021fa93b1e18, 0x9944002534d09b3d, 0x87aa95782311a277, 0xb88be10313a1303c, 0xad54bc31433dddba, 0xe1b7d813e3f825e0, 0x14f8c1be7370f218, 0xac27c5a6139cd30c, 0x2d23a0744e00f594, 0xd235e25fb9644c30, 0x361ee0bcb5db0449, 0x18660815da3d7962, 0x39c357b6bfdf81b4, 0x5076c62c951204f5, 0x146244d643f7fa2a, 0x62bb0f3208d9a1ba, 0x7926e92808bd580c, 0x4819e620d5fcc067, 0xdc494943d427214e, 0xdf0805c4161e404b, 0x2d615caaa0514c3b, 0x85c60c12eca0aedb, 0x4c207a522524f8de, 0x64243e02c6215a4e, 0x435ab4da6a5bb48c, 0x9e06fc84b6ea5e24, 0x91ab122ee427cfb4, 0x5f832513e3211642, 0x5e7b48cfeeb85aa7, 0xb36a9f58eb4ccd07, 0x3360751e43c7af35, 0x6fab78aca91193cb, 0xeb432409cffdad8d, 0x793b5acf3a336461, 0xc3ea2cd93f316b33, 0xfc679a28e9d9f212, 0xb20f215bd3b58c60, 0xd1aacedcefe9d376, 0xcbef6fac33691e95, 0xe2f1775134c8da75, 0x3c742a7c76356395, 0xca47c52b7d7ffce2, 0x7e4cfbd830393b87, 0x7370ae83f9e72748, 0xe6dbb624f9739781, 0x97fa2fd0c9dc723d, 0x7199cd06ae5d39b3, 0x7b6d1248c3e1fd3f, 0x26828c92649a3a38, 0xda6959f7f0e01bf0, 0xb47505bfa5a03b06, 0xa8740b91c95df537, 0x3c56c598c659c2a2, 0x379eba7e6465ff63, 0xde026e271ee0549c, 0x0, 0x0, 0xdfeb485085f6f453, 0x6bc1e20eac8448b4, 0xc72446cc1bf728bd, 0x569b26aaa485ea5b, 0x5f69768284463b9b, 0x14d9d76196d8043a, 0x661e135f49a47c40, 0x9a31ba0cbc030352, 0x7ad1e9c315328f7d, 0xf105b66ec4703ede, 0xd6aef30cd312169a, 0xe6e2acf8f4d4c249, 0x28bb3cd9f2a65fb4, 0x224a96f5a7471c45, 0xd462b63756c87e80, 0x3ff51287882500ed, 0x1ab9679b55f78a6a, 0x17e4b7ac6c600cb4, 0xfd1a09c848e3950d, 0x318b2ddd9d0a33b3, 0x9dd91e52c79fd070, 0x72de1d99ce252efd, 0xd7bd1d62ef25480d, 0x7f921124f1ecb59e, 0x271ee1cd6d5cdf9d, 0xfad0cc8b5faea8cb, 0xe57a0acb9d5cd4de, 0xc81bb5a8d789f443, 0x9b1beb40437575f4, 0x7944509046652d98, 0x94e51ebff53a2f15, 0x8bbc7f765b13ebbe, 0xf61305ef7390939c, 0x3abc32a78afd4b7a, 0x17596a598cb29436, 0x1c890bee9a9d743c, 0xeaafbd07b543145c, 0x6517bc4112d64b17, 0xdb94a1dfd653d3a5, 0x2ada01ce7ed3607f, 0xd3b36c029ea7bb5d, 0x94c529f32403828, 0xb6b6676248bba138, 0x7bdd0c2a9c7a679a, 0x23deb274e953a258, 0xdae7e343fa859415, 0x17759bff5c717992, 0x52e7e4dde874dacd, 0xa88971f8277a4d10, 0x269de85f0df92587, 0x180d255422c3377c, 0x46da70925ee85c05, 0x37968ceafaf7b452, 0x5dfba4cfdd38a058, 0x4ae21abe75d5a19a, 0xd3bd4fd98a1e6fe5, 0x33cf7d5ebfb93ad3, 0x2743c805a4928086, 0x5dbeb9795455a5, 0xb6ed80852ae6fd62, 0xf237cff1acb306b3, 0xd81648249cece4c, 0x176cd56887ac7fe8, 0x662d417ced007a45,];
            ReadOnlySpan<ulong> LogInv32Lh = [0xed961f7cd039d43b, 0x63275973180916, 0x53eb4b80e74f4d9f, 0x3765e2cb07bc7842, 0xce96efdd54fdcc41, 0x7c9ecdfd8f89db96, 0x1caec15031f4dc54, 0x50c342f5fce295a8, 0x4f178f6a8fbb9ca5, 0x2354c6de776e85ab, 0x2ba2456733804a75, 0xc17153c5ab3b0225, 0x3d675807b776c8c3, 0xd24faf76303783f0, 0xb165fd239443b62c, 0x3e814a96bc97e05e, 0x68f9c68b2e5248, 0x869c4dd8468b27de, 0x567cab7f031b369, 0xd14621e31d9f0de1, 0xf2591df10cea40d0, 0xb058f99c8186daa4, 0x90fee0b93d40db31, 0xcdd5aab43dfdb463, 0x9a874314df180c72, 0x9066b677760637d4, 0x423f0610339ed04b, 0xb467a7c6839cc262, 0xf729c68cd270f129, 0xa3fb0e5c1d39e0e8, 0x3d54277ac7f378a6, 0xb38c33565546e35c, 0xcd29dd6d72582491, 0x699801dab452e328, 0x8862e24ccd48f678, 0xa65998dde4dd76e0, 0xbcaca74cb74df3d2, 0x8770a2e82b32cf54, 0x5d10db5217ec2ab8, 0x5e4d97ba155a9de6, 0x568dc2013b32ced6, 0x9cffddada1c113a2, 0xbfcaabcf0318ef95, 0x487785d971aec0af, 0xd8ba6eedf272eeb0, 0xb21c7fe4cdbc5967, 0x466fab846a1e3e70, 0x134f09715ee9c6cf, 0xb1d845d134023d8e, 0x13c8ea71e3d8fe30, 0x83304a61505642d8, 0x140f4a016e0c0d28, 0xbbd85b81581c98f8, 0x998985ef1e4636e0, 0x67da60b1a110ff9, 0xd8ceb8a313143c9c, 0xc3585d8bbd3ac1b8, 0x3ff9151061ec91aa, 0x2b8ff7c8377c9037, 0x5526081fe3d93a56, 0xf5c74f2f07e4f272, 0x3535a7e74bbb0089, 0x8cd0b8002d083c9b, 0x0, 0x0, 0xb62f8fe41e621f91, 0xdf9f60ec72f7062, 0x5e7d3be7e456c8a7, 0xf5e4d6d9243bca81, 0x18baa187613466f, 0x179e870f7485c2eb, 0x42882a135aa4aa82, 0xdc58134f3ce2ed6e, 0xcc0b6a758f391573, 0x76406288c82a0e9c, 0x24ed9892618f8da1, 0x830dad0cbcb297de, 0x9c8f264a305434a2, 0xd39ed746e5b2ba94, 0x157ee3bffb879ef4, 0x124d4848d57cf1e2, 0x84963a91b59a785c, 0x6767118f1a71745b, 0xe0e66fad558345db, 0x9dd7a4359a20f4ae, 0x184596be172aa3d1, 0x149ec47368e2b10f, 0x7b1a44d708e0875b, 0x7696655fbc6715a6, 0x808228274a503b4b, 0xa39dee1517d770f7, 0xd926113c7cf9cae4, 0x8c30a4efa2085a91, 0x86e839aee0bd3623, 0xb65fa4f60aca7f76, 0x12a476e601733b9f, 0x68772abd1ce3258c, 0x1819673e1d680b66, 0xea68f0383acd9ff4, 0x3dddd5d01649b409, 0x7139a25d67ed976c, 0xb28914b789a13376, 0x3e099f46da82a60f, 0x46503739bf42fb2e, 0x99d41a958f292523, 0x36f49e73617ad152, 0x253f332743a0ff31, 0xb6c98155115da0b7, 0x5fb1c32c4c750e98, 0x990d7e42e0f2ed1b, 0x5e4e41c1b5e85e4a, 0x949294749166f218, 0xa952b40de6b303a6, 0xcc6e1825f856fe70, 0xe65771fd08853e31, 0x35564b2dfc6a51cc, 0x7b96716bbe5fc916, 0x866ac7791182f8a4, 0xc04093a8c6018f91, 0xd62f6d57c561e135, 0x625d637fcaa83aa9, 0x6a8d8f7f711ab809, 0x8923300474737280, 0x5b168ca67fe36520, 0xdfbedfa9caad16a4, 0x8557e05d2318ee0, 0x3f4609735b1102e2, 0xaa02e8447626768d, 0xc0be1062bd88c8e8,];
            ReadOnlySpan<ulong> LogInv32Ll = [0x3813c435abc461e9, 0xce33d61e9d12b379, 0xec0f1ece69a0881, 0x1b2970050550b17b, 0xc953aa9864a2c806, 0xa637f86e778c350c, 0x197d0a42fdccbac4, 0xac842dc0defcda11, 0x4f07c79076670299, 0x7bfb018bb7288d07, 0x33512ca3a3f6fe2b, 0xe1f11ff3205d196e, 0x50075ac879e03385, 0x55ff6c2398186fbe, 0x237e837cc1f0bdfb, 0xa2417a23566b2cd7, 0x1d701478d6f6c2b6, 0x605b72cc1d8025bc, 0xd9bec487ee14afeb, 0x8fbb93f3b4e5b10e, 0x730872806a501e6e, 0xae0d7dcbc2a675ab, 0xe67bce480f351d7b, 0x9338ef1dac85e113, 0x2272c79abfdb422c, 0x308f6e39ffa6ca4c, 0x185c41879a1a0e44, 0x831c3f31eb48d551, 0xaab9342f19cba76c, 0x1dca7781b6e7202f, 0xa5d1bbca5392d277, 0x19646b6a5fd02489, 0xba6e335a1a33227f, 0x96dc3d1e75ba8032, 0x95fa76270a5d0366, 0xbbb102dc658c60, 0x55bfb0c4fdd76008, 0xc9fdca283fb5d971, 0xc60248447782dce7, 0xcdd79f978d1afd7b, 0x72b53615be59209e, 0xb54d414e644993a8, 0x85a480f694e7b857, 0x61cbdae5b7255821, 0x574dfcf4f87b33e8, 0xd35c35c4241c3714, 0xef6ef4000ffc2f4b, 0x48be7593379645aa, 0x66ad982559cdd0ce, 0x8edd8e108ea7f135, 0xceeccccf5069ebf8, 0x5aea5efc41fa18fd, 0xe1340de77cd6a600, 0x306c5597c846f0b9, 0x14c940c27f248ced, 0xe1449e20d24508b2, 0xd315929badc83115, 0x8f307c96ffa40bba, 0x6cd9392c0d17528b, 0x3f68c3cd00e45f8b, 0xc451b2e04ebd63ee, 0x937a324b4307d36, 0x2e9198222f25f83c, 0x0, 0x0, 0x274178d2188f9ea7, 0x6f190de53deca5e8, 0x180e4b0fc869da77, 0x2714208ba14d04c8, 0x5b94041491764cd7, 0x7931552a9aec92fb, 0x82166812bed98c2d, 0x6c0826bdc4127939, 0xa0bdfad5f77c4a7f, 0xc06e5c3eaccc9b65, 0x6091c8f53e42724e, 0x6f8eeb67afc8978d, 0x719267af576b8da7, 0xdb742c4b8aeaa82e, 0x9e384ccad23b6068, 0xca3575b044a7e9d6, 0xf77577a5e4380677, 0x98a85668eda9496f, 0xd0914651b6655eb2, 0x176ea6faed5bf263, 0x8a86b1bb367fee8, 0xb294ccdf83577dc0, 0x83a3462cb58566da, 0xb820b79a7e7be96f, 0x3ec027918b227262, 0x87b62eb6a5071ae6, 0x2cc6a626c5980669, 0xd8416fa14d8a0531, 0x447c2a52014276c5, 0x4229769700e69546, 0xe7eb2f0e037ceedb, 0xe7cf18b488213d56, 0x3bb660c119045dcb, 0xe7d3305627ebab51, 0x314d41b7d5759903, 0x715b9888be5228e7, 0xc286d314221430a0, 0x5ac8c579eeb8644c, 0xeabea04e3ce90eed, 0x2b08613964628dc6, 0x967313472d10eeee, 0x6548b3ed5877119a, 0x6fcf6732c378b707, 0xb5b84cc7549d184a, 0xe6c4e93f7ba138fe, 0xf82d9009ecaad498, 0x780bea6b173a668d, 0x46e9f250087d1466, 0x18193291e2b36381, 0xa9b4b0681339900, 0xad4c9d66d29442ba, 0x949df14f9331856d, 0x879a1365f37550b8, 0x343b553466b61759, 0x49b45337d2f46eb8, 0x32bb34856444782c, 0xe15b1d297f6f796c, 0xd02a66f2f0de4bef, 0xaf0629427a97e44f, 0x5f822584035372e8, 0x5453667bec383296, 0x295ae12732b36daf, 0x553f90bb90928bc4, 0xe925964e76028722,];
            ReadOnlySpan<int> LogInv32Ex = [-8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -10, -10, -10, -10, -10, -10, -10, -10, -11, -11, -11, -11, -12, -12, -13, 255, 255, -13, -12, -12, -11, -11, -11, -11, -10, -10, -10, -10, -10, -10, -10, -10, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -9, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -8, -7,];
            ReadOnlySpan<byte> LogInv32Sgn = [0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0];



            long E = x.ex;
            ushort i, j;

            if (x.hh > 0xb504f333f9de6484)
            {
                E++;
                i = (ushort)(x.hh >> (63 + 1 - 7));
            }
            else
            {
                i = (ushort)(x.hh >> (63 - 7));
            }

            Qint xx = x with { ex = x.ex - E };

            Qint z = Qint.Mul(xx, new Qint(Inverse31Ll[i - 90], Inverse31Lh[i - 90], Inverse31Hl[i - 90], Inverse31Hh[i - 90], Inverse31Ex[i - 90], Inverse31Sgn[i - 90]));

            j = (ushort)(z.hh >> (63 - 13 - (int)z.ex));

            z = Qint.Mul(z, new Qint(Inverse32Ll[j - 8128], Inverse32Lh[j - 8128], Inverse32Hl[j - 8128], Inverse32Hh[j - 8128], Inverse32Ex[j - 8128], Inverse32Sgn[j - 8128]));
            z = Qint.Add(Qint.MinusOne, z);

            Qint r = Qint.Mul2(E, Qint.Log2);

            Qint p = p3(z);

            p = Qint.Add(new Qint(LogInv32Ll[j - 8128], LogInv32Lh[j - 8128], LogInv32Hl[j - 8128], LogInv32Hh[j - 8128], LogInv32Ex[j - 8128], LogInv32Sgn[j - 8128]), p);
            p = Qint.Add(new Qint(LogInv31Ll[i - 90], LogInv31Lh[i - 90], LogInv31Hl[i - 90], LogInv31Hh[i - 90], LogInv31Ex[i - 90], LogInv31Sgn[i - 90]), p);
            r = Qint.Add(p, r);

            return (r, xx);
        }

        static (double eh, double el) exp1(double rh, double rl, double s)
        {
            ReadOnlySpan<double> T1 = [1, 0, 1.0108892860517005, -1.5234778603368577e-17, 1.0218971486541166, 5.1092250289734439e-17, 1.0330248790212284, 7.6008388740270885e-18, 1.0442737824274138, 8.5518897055379649e-17, 1.0556451783605572, 1.759325738772092e-18, 1.0671404006768237, -7.8998539668415821e-17, 1.0787607977571199, -6.6566604360565926e-17, 1.0905077326652577, -3.0467820798124711e-17, 1.1023825833078409, 5.2660368715706944e-17, 1.1143867425958924, 1.0410278456845571e-16, 1.1265216186082418, 5.1658567587954567e-17, 1.1387886347566916, 8.9128126760254078e-17, 1.1511892299529827, 3.2507102188638272e-17, 1.1637248587775775, 3.8292048369240935e-17, 1.1763969916502812, 5.554203254218079e-17, 1.189207115002721, 3.9820152314656461e-17, 1.2021567314527031, 6.6449814992523012e-17, 1.215247359980469, -7.7126306926814881e-17, 1.22848053610687, -1.89878163130253e-17, 1.241857812073484, 4.6580275918369368e-17, 1.2553807570246911, -6.7113898212968784e-18, 1.2690509571917332, 2.6679321313421861e-18, 1.2828700160787783, 1.713594918243561e-17, 1.2968395546510096, 2.5382502794888315e-17, 1.3109612115247644, -7.1815361355194539e-17, 1.3252366431597413, -2.8587312100388614e-17, 1.3396675240533029, 8.927282594831732e-17, 1.3542555469368927, 7.7009483798029895e-17, 1.3690024229745905, 9.5937979191188488e-17, 1.383909881963832, -6.7705116587947863e-17, 1.3989796725383112, -9.6142132090513231e-17, 1.4142135623730951, -9.6672933134529135e-17, 1.42961333839197, -1.2031642489053655e-17, 1.4451808069770467, -3.0237581349939873e-17, 1.460917794180647, -5.6003771860752158e-17, 1.4768261459394993, -3.4839945568927958e-17, 1.4929077282912648, 1.4192920154284036e-17, 1.5091644275934228, -1.016455327754295e-16, 1.5255981507445384, -1.1024941712342561e-16, 1.5422108254079407, 7.9498348096976209e-17, 1.5590044002378369, 3.7812070533575275e-17, 1.5759808451078865, -1.0136916471278304e-17, 1.593142151342267, -1.0094406542311964e-16, 1.6104903319492543, 2.4707192569797888e-17, 1.6280274218573478, -6.7129550847070841e-17, 1.6457554781539649, -1.0125679913674773e-16, 1.6636765803267364, 5.8909926967130997e-17, 1.681792830507429, 8.1990100205814965e-17, 1.7001063537185235, -8.0237193703977002e-18, 1.7186192981224779, -1.851380418263111e-17, 1.7373338352737062, 3.1643892992929569e-17, 1.7562521603732995, 2.9601406954488733e-17, 1.7753764925265212, 6.429731796556572e-17, 1.7947090750031072, 1.8227458427912087e-17, 1.8142521755003989, -9.9695315389203488e-17, 1.8340080864093424, 3.2831072242456272e-17, 1.8539791250833855, 9.7618874907275935e-17, 1.8741676341103, -6.1227634130041426e-17, 1.8945759815869656, 3.4034035352165297e-17, 1.9152065613971474, -1.0619946056195963e-16, 1.9360617934922943, 1.0332385960676326e-16, 1.9571441241754002, 8.9607677910366678e-17, 1.9784560263879509, 4.0388753109278167e-17];
            ReadOnlySpan<double> T2 = [1, 0, 1.0001692397053021, 9.336185335478462e-17, 1.0003385080526823, -5.1413339313189571e-18, 1.0005078050469876, 6.9624240220205726e-17, 1.0006771306930664, -5.1151232976856676e-17, 1.0008464849957674, 8.4229900245864866e-17, 1.001015867959941, -2.8245220747761678e-17, 1.0011852795904375, -7.1804245655921317e-17, 1.0013547198921082, -1.8973728416792993e-17, 1.0015241888698057, 9.0604410672691217e-17, 1.0016936865283832, -7.17327634990032e-17, 1.0018632128726943, -1.3307196246722661e-17, 1.002032767907594, 2.5726925943221118e-17, 1.0022023516379379, -3.9299377854845172e-17, 1.0023719640685822, 8.4613772479947175e-17, 1.0025416052043845, -4.1948832416399403e-17, 1.0027112750502025, -3.6366159286922639e-17, 1.0028809736108952, -2.6109440632439383e-17, 1.0030507008913223, 1.7530784779823321e-17, 1.0032204568963443, 5.7539235256282674e-17, 1.0033902416308227, -8.6849220051179562e-18, 1.0035600550996193, 9.4900354309817776e-17, 1.0037298973075977, -8.7103806058184224e-17, 1.0038997682596209, 3.4958916958571545e-17, 1.0040696679605541, 9.753787549840241e-17, 1.0042395964152628, -1.0576221196292857e-16, 1.0044095536286128, 4.2091887381271259e-17, 1.0045795396054717, -1.6700166857554788e-17, 1.0047495543507072, -1.6231463554124514e-17, 1.0049195978691881, 2.3028539278028117e-17, 1.0050896701657839, 1.6418046976773032e-17, 1.005259771245365, 3.7266984318284137e-17, 1.0054299011128027, 9.4991865354550318e-17, 1.0056000597729693, -8.6809313144445816e-17, 1.005770247230737, 4.0005474910301169e-17, 1.0059404634909801, 7.190499111509974e-17, 1.006110708558573, -1.3908068671065783e-17, 1.0062809824383909, -8.1402086425730496e-17, 1.00645128513531, -5.7621510437495342e-17, 1.0066216166542072, 6.7452784773104568e-17, 1.0067919769999607, 1.8998557240346296e-17, 1.0069623661774489, -9.6374300323164072e-17, 1.0071327841915512, -1.2528654462453979e-17, 1.0073032310471479, 3.0205788878436942e-17, 1.0074737067491204, -4.8693942586085649e-17, 1.0076442113023503, 5.2240299376874532e-17, 1.0078147447117207, -9.3615435514784559e-17, 1.007985306982115, -8.6525132330619496e-17, 1.0081558981184175, -3.2520587560843081e-17, 1.0083265181255139, -9.9172322680609143e-17, 1.0084971670082898, -7.1360474041625228e-17, 1.0086678447716324, -1.726868371224322e-17, 1.0088385514204294, -6.6199546936739401e-17, 1.0090092869595693, 3.5654569015130204e-17, 1.0091800513939415, 3.7173100137088179e-17, 1.0093508447284363, 7.0625724068255277e-17, 1.0095216669679448, -1.4321412303428819e-17, 1.0096925181173586, 1.566818801313411e-17, 1.0098633981815708, -1.1043695780393688e-16, 1.0100343071654745, -5.767317427160398e-17, 1.0102052450739643, 4.8354849784403827e-18, 1.0103762119119353, 7.0151212897154421e-17, 1.0105472076842836, 7.1618028736195738e-17, 1.0107182323959061, 1.050465913408405e-16];


            const double Rho0 = -745.13326703414066, Rho1 = -656.46523407199663, Rho2 = 709.7826672320183, Rho3 = 709.78275855159075;
            double eh, el;

            if (!(rh <= Rho2))
            {
                if (rh > Rho3)
                {
                    (eh, el) = (1.7976931348623157e+308 * s, 1.7976931348623157e+308 * s);
                }
                else
                {
                    (eh, el) = (double.NaN, double.NaN);
                }
                return (eh, el);
            }

            if (rh < Rho1)
            {
                if (rh < Rho0)
                {
                    (eh, el) = (0.0 * s, 4.9406564584124654e-324 * (0.5 * s));
                }
                else
                {
                    (eh, el) = (double.NaN, double.NaN);
                }
                return (eh, el);
            }

            const double InvLog2 = 5909.278887481194;

            double k = RoundEvenFinite(rh * InvLog2);

            const double Log2H = 0.00016922538587889289, Log2L = 5.6617353853669423e-21;

            double zh, zl;
            zh = FusedMultiplyAdd(Log2H, -k, rh);
            zl = FusedMultiplyAdd(Log2L, -k, rl);

            long K = (long)k;
            long M = (K >> 12) + 0x3ff;
            int i2 = (int)(K >> 6) & 0x3f;
            int i1 = (int)K & 0x3f;

            double t1h = T1[i2 * 2 + 0], t1l = T1[i2 * 2 + 1], t2h = T2[i1 * 2 + 0], t2l = T2[i1 * 2 + 1];
            (eh, el) = dMul(t2h, t2l, t1h, t1l);

            double qh, ql;
            (qh, ql) = q1(zh + zl);

            (eh, el) = dMul(eh, el, qh, ql);

            ulong _d = (ulong)M << 52;
            _d = Polyfill.DoubleToUInt64Bits(Polyfill.UInt64BitsToDouble(_d) * s);

            eh *= Polyfill.UInt64BitsToDouble(_d);
            el *= Polyfill.UInt64BitsToDouble(_d);

            return (eh, el);
        }

        static Dint exp2(in Dint x)
        {
            ReadOnlySpan<ulong> T12Hi = [0x8000000000000000, 0x8164d1f3bc030773, 0x82cd8698ac2ba1d7, 0x843a28c3acde4046, 0x85aac367cc487b14, 0x871f61969e8d1010, 0x88980e8092da8527, 0x8a14d575496efd9a, 0x8b95c1e3ea8bd6e6, 0x8d1adf5b7e5ba9e5, 0x8ea4398b45cd53c0, 0x9031dc431466b1dc, 0x91c3d373ab11c336, 0x935a2b2f13e6e92b, 0x94f4efa8fef70961, 0x96942d3720185a00, 0x9837f0518db8a96f, 0x99e0459320b7fa64, 0x9b8d39b9d54e5538, 0x9d3ed9a72cffb750, 0x9ef5326091a111ad, 0xa0b0510fb9714fc2, 0xa27043030c496818, 0xa43515ae09e6809e, 0xa5fed6a9b15138ea, 0xa7cd93b4e9653569, 0xa9a15ab4ea7c0ef8, 0xab7a39b5a93ed337, 0xad583eea42a14ac6, 0xaf3b78ad690a4374, 0xb123f581d2ac258f, 0xb311c412a9112489, 0xb504f333f9de6484, 0xb6fd91e328d17791, 0xb8fbaf4762fb9ee9, 0xbaff5ab2133e45fb, 0xbd08a39f580c36be, 0xbf1799b67a731082, 0xc12c4cca66709456, 0xc346ccda24976407, 0xc5672a115506dadd, 0xc78d74c8abb9b15c, 0xc9b9bd866e2f27a2, 0xcbec14fef2727c5c, 0xce248c151f8480e3, 0xd06333daef2b2594, 0xd2a81d91f12ae45a, 0xd4f35aabcfedfa1f, 0xd744fccad69d6af4, 0xd99d15c278afd7b5, 0xdbfbb797daf23755, 0xde60f4825e0e9123, 0xe0ccdeec2a94e111, 0xe33f8972be8a5a51, 0xe5b906e77c8348a8, 0xe8396a503c4bdc68, 0xeac0c6e7dd24392e, 0xed4f301ed9942b84, 0xefe4b99bdcdaf5cb, 0xf281773c59ffb139, 0xf5257d152486cc2c, 0xf7d0df730ad13bb8, 0xfa83b2db722a033a, 0xfd3e0c0cf486c174,];
            ReadOnlySpan<ulong> T12Lo = [0x0, 0x7be56527bd14def5, 0x3e2a475b46520bff, 0x1af92eca13fd1582, 0xc5c95b8c2154c1b2, 0x3a1727c57b52a956, 0x5df8d76c98c67563, 0x80ca1d92c3680c2, 0xfbe4628758a53c90, 0xb4c7b4968e41ad36, 0x2dc0144c8783d4c6, 0x775814a8494e87e2, 0xfd6d8e0ae5ac9d8, 0xd339940e9d924ee7, 0x2e8afad12551de54, 0x48ea9b683a9c22c5, 0x46ad23182e42f6f6, 0xe43086cb34b5fcaf, 0xa2a817a2a3cc3f1f, 0xde494cf050e99b0b, 0xa0911f09ebb9fdd1, 0x192dc79edb0fd9a9, 0x9b7a04ef80cfdea8, 0xd1db4831781e1ef, 0x1cbd7f621710701b, 0x9ec5b4d5039f72af, 0x541e24ec3531fa73, 0x658023b2759e0079, 0x4980a8c8f59a2ec4, 0xdf26101ccbb35033, 0x87d037e96d215d8e, 0x3ecf14dc798a519c, 0x597d89b3754abe9f, 0x7165f0ddd541a5a, 0x1b879778566b65a2, 0x74d519d24593838c, 0xa8811fb66d0faf7a, 0xe815d0abcbf0b851, 0x7c457d59a50087b5, 0x20ec856128b83a42, 0x3e2ad0c964dd9f37, 0xc13a2e3976c0277e, 0x80e1f92a0511697e, 0xf4907c8f45ebf6dd, 0xe235838f95f2c6ed, 0xd6d45c6559a4d502, 0x12248e57c3de4028, 0x5921deffa6262c5b, 0x39a68bb9902d3fde, 0xfe873deca3e12bac, 0x3d840d5a9e29aa64, 0xdd07a2d9e8466859, 0x65895048dd333ca, 0x9bfe90795980eed, 0x1e5e8f4a4edbb0ed, 0x791790d0ac70c7de, 0xd02d75b3706e54fb, 0x600d2db6a64bfb12, 0x46561cf6948db913, 0xe8980a9cc8f47a4b, 0x7b9d0c7aed980fc3, 0xfe90d496d60fb6eb, 0x7c25bb14315d7fcd, 0x853f3a5931e0ee03,];
            ReadOnlySpan<int> T12Ex = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,];
            ReadOnlySpan<byte> T12Sgn = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,];

            ReadOnlySpan<ulong> T22Hi = [0x8000000000000000, 0x80058baf7fee3b5d, 0x800b179c82028fd0, 0x8010a3c708e73282, 0x8016302f17467628, 0x801bbcd4afcacb08, 0x802149b7d51ebefb, 0x8026d6d889ecfd69, 0x802c6436d0e04f50, 0x8031f1d2aca39b43, 0x80377fac1fe1e56a, 0x803d0dc32d464f85, 0x80429c17d77c18ed, 0x80482aaa212e9e95, 0x804db97a0d095b0c, 0x805348879db7e67d, 0x8058d7d2d5e5f6b0, 0x805e675bb83f5f0f, 0x8063f722477010a1, 0x8069872686241a12, 0x806f17687707a7af, 0x8074a7e81cc7036b, 0x807a38a57a0e94dc, 0x807fc9a0918ae142, 0x80855ad965e88b83, 0x808aec4ff9d45430, 0x80907e044ffb1984, 0x80960ff66b09d765, 0x809ba2264dada76a, 0x80a13493fa93c0d4, 0x80a6c73f74697897, 0x80ac5a28bddc4157, 0x80b1ed4fd999ab6c, 0x80b780b4ca4f64df, 0x80bd145792ab3970, 0x80c2a838355b1297, 0x80c83c56b50cf77f, 0x80cdd0b3146f0d11, 0x80d3654d562f95ec, 0x80d8fa257cfcf26e, 0x80de8f3b8b85a0af, 0x80e4248f84783c87, 0x80e9ba216a837f8c, 0x80ef4ff140564116, 0x80f4e5ff089f763e, 0x80fa7c4ac60e31e1, 0x810012d47b51a4a0, 0x8105a99c2b191ce1, 0x810b40a1d81406d4, 0x8110d7e584f1ec6d, 0x81166f673462756d, 0x811c0726e9156760, 0x81219f24a5baa59d, 0x812737606d023148, 0x812ccfda419c2956, 0x813268922638ca8b, 0x813801881d886f7b, 0x813d9abc2a3b9090, 0x8143342e4f02c405, 0x8148cdde8e8ebdec, 0x814e67cceb90502c, 0x815401f968b86a87, 0x81599c6408b81a94, 0x815f370cce408bc8,];
            ReadOnlySpan<ulong> T22Lo = [0x0, 0x1c718b38e549cb93, 0x945e54e2ae18f2f0, 0x2b96d62d51c15a07, 0x3690dfe44d11d008, 0xe23a986bd3e626f0, 0x7bdbadbc888aeb29, 0xb904bbfb40d3a2b7, 0xff8ce94a6797b3ce, 0xad9db772901d96b6, 0x61cd0bffd7cfc683, 0x43456f71b96affd4, 0x49fc841afba9c3c6, 0x86f7b54f6c45c85e, 0x6c9f1f7d1efcfe68, 0x171eb1ceef1d1f28, 0x94d589f608ee4aa2, 0x2ed38ab8472b2144, 0xb1652de1378af1a1, 0xb4ad9233a0390cad, 0xe54ec5f966eb1872, 0x4d204ecfc11f4aab, 0x9bf3ef4d9be2d1e4, 0x7068ab2230585d13, 0xa0cc0a49c10ea66b, 0x84099bf6830f2768, 0x3aa8b9cbbc65a8ab, 0xf7d88c0928ba3947, 0x4a8a4f44bb703db6, 0x6699dc50dd96b774, 0x6e0472ed4ccfa2e0, 0xba2dc7e0c72e51ba, 0x25335719b6e6fd20, 0x534dfa7417846aa4, 0xfc41c5c2d5336ccc, 0x34dc28baed8f3fde, 0xb880575ea03548c1, 0x32c1f98704428c71, 0x890e222a5eb95372, 0x24628efd9ca9d59b, 0x3b13310f5ad57fb1, 0x1a9dfefaeb616564, 0x718d1151d109bf98, 0x996709da2e25f04c, 0xe0adc640acaa6b0b, 0xd4eb5edc6b341283, 0x8ccd7223820719e3, 0xf24ebd6eb9ca4292, 0xcef03ab14a66550, 0x4bf94297d1519822, 0xd0d8372f966cf15e, 0xb97931db7b7be2ec, 0x6abd3b0eab9c7048, 0xdaf888e96508151a, 0xdc8046821f46122e, 0x6846ad73a8d9027f, 0xe885724f14131287, 0x83768490519df895, 0x661b22b45e25de18, 0xf11430fef78c6ee, 0x99775205944eadc4, 0x7de463a40d18261, 0x8f4a0b6748df7960, 0xe2404468cfe5ab9f,];
            ReadOnlySpan<int> T22Ex = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,];
            ReadOnlySpan<byte> T22Sgn = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,];



            Dint K, y, r;

            if (x.ex >= 10)
            {
                r = x with { ex = x.sgn != 0 ? -1076 : 1025, sgn = 0 };
                return r;
            }

            K = Dint.Mul11(x, Dint.Log2InvPow);

            long k = dintToi(K);

            K = Dint.MulInt64(Dint.Log2, k);

            K = K with { ex = K.ex - 12, sgn = K.sgn ^ 1 };

            y = Dint.Add(x, K);

            long M = k >> 12;
            int i2 = (int)(k >> 6) & 0x3f;
            int i1 = (int)k & 0x3f;

            r = q2(y);

            r = Dint.MulPow(new Dint(T12Lo[i2], T12Hi[i2], T12Ex[i2], T12Sgn[i2]), r);
            r = Dint.MulPow(new Dint(T22Lo[i1], T22Hi[i1], T22Ex[i1], T22Sgn[i1]), r);

            r = r with { ex = r.ex + M };

            return r;
        }

        static Qint exp3(in Qint x)
        {
            ReadOnlySpan<ulong> T13Hh = [0x8000000000000000, 0x8164d1f3bc030773, 0x82cd8698ac2ba1d7, 0x843a28c3acde4046, 0x85aac367cc487b14, 0x871f61969e8d1010, 0x88980e8092da8527, 0x8a14d575496efd9a, 0x8b95c1e3ea8bd6e6, 0x8d1adf5b7e5ba9e5, 0x8ea4398b45cd53c0, 0x9031dc431466b1dc, 0x91c3d373ab11c336, 0x935a2b2f13e6e92b, 0x94f4efa8fef70961, 0x96942d3720185a00, 0x9837f0518db8a96f, 0x99e0459320b7fa64, 0x9b8d39b9d54e5538, 0x9d3ed9a72cffb750, 0x9ef5326091a111ad, 0xa0b0510fb9714fc2, 0xa27043030c496818, 0xa43515ae09e6809e, 0xa5fed6a9b15138ea, 0xa7cd93b4e9653569, 0xa9a15ab4ea7c0ef8, 0xab7a39b5a93ed337, 0xad583eea42a14ac6, 0xaf3b78ad690a4374, 0xb123f581d2ac258f, 0xb311c412a9112489, 0xb504f333f9de6484, 0xb6fd91e328d17791, 0xb8fbaf4762fb9ee9, 0xbaff5ab2133e45fb, 0xbd08a39f580c36be, 0xbf1799b67a731082, 0xc12c4cca66709456, 0xc346ccda24976407, 0xc5672a115506dadd, 0xc78d74c8abb9b15c, 0xc9b9bd866e2f27a2, 0xcbec14fef2727c5c, 0xce248c151f8480e3, 0xd06333daef2b2594, 0xd2a81d91f12ae45a, 0xd4f35aabcfedfa1f, 0xd744fccad69d6af4, 0xd99d15c278afd7b5, 0xdbfbb797daf23755, 0xde60f4825e0e9123, 0xe0ccdeec2a94e111, 0xe33f8972be8a5a51, 0xe5b906e77c8348a8, 0xe8396a503c4bdc68, 0xeac0c6e7dd24392e, 0xed4f301ed9942b84, 0xefe4b99bdcdaf5cb, 0xf281773c59ffb139, 0xf5257d152486cc2c, 0xf7d0df730ad13bb8, 0xfa83b2db722a033a, 0xfd3e0c0cf486c174,];
            ReadOnlySpan<ulong> T13Hl = [0x0, 0x7be56527bd14def4, 0x3e2a475b46520bff, 0x1af92eca13fd1582, 0xc5c95b8c2154c1b2, 0x3a1727c57b52a956, 0x5df8d76c98c67562, 0x80ca1d92c3680c2, 0xfbe4628758a53c90, 0xb4c7b4968e41ad36, 0x2dc0144c8783d4c5, 0x775814a8494e87e2, 0xfd6d8e0ae5ac9d8, 0xd339940e9d924ee7, 0x2e8afad12551de54, 0x48ea9b683a9c22c4, 0x46ad23182e42f6f6, 0xe43086cb34b5fcae, 0xa2a817a2a3cc3f1f, 0xde494cf050e99b0b, 0xa0911f09ebb9fdd1, 0x192dc79edb0fd9a9, 0x9b7a04ef80cfdea7, 0xd1db4831781e1ee, 0x1cbd7f621710701b, 0x9ec5b4d5039f72af, 0x541e24ec3531fa73, 0x658023b2759e0079, 0x4980a8c8f59a2ec4, 0xdf26101ccbb35032, 0x87d037e96d215d8e, 0x3ecf14dc798a519b, 0x597d89b3754abe9f, 0x7165f0ddd541a59, 0x1b879778566b65a1, 0x74d519d24593838c, 0xa8811fb66d0faf7a, 0xe815d0abcbf0b850, 0x7c457d59a50087b5, 0x20ec856128b83a42, 0x3e2ad0c964dd9f37, 0xc13a2e3976c0277e, 0x80e1f92a0511697e, 0xf4907c8f45ebf6dc, 0xe235838f95f2c6ed, 0xd6d45c6559a4d502, 0x12248e57c3de4028, 0x5921deffa6262c5a, 0x39a68bb9902d3fde, 0xfe873deca3e12bab, 0x3d840d5a9e29aa64, 0xdd07a2d9e8466859, 0x65895048dd333ca, 0x9bfe90795980eec, 0x1e5e8f4a4edbb0ec, 0x791790d0ac70c7dd, 0xd02d75b3706e54fa, 0x600d2db6a64bfb12, 0x46561cf6948db912, 0xe8980a9cc8f47a4b, 0x7b9d0c7aed980fc3, 0xfe90d496d60fb6ea, 0x7c25bb14315d7fcc, 0x853f3a5931e0ee03,];
            ReadOnlySpan<ulong> T13Lh = [0x0, 0x9eb851655e2e5c4d, 0x29f1a4afbefa5d7c, 0xd96b414ec4c9d06, 0x148a0459e7585151, 0x259ac58894f4fcb3, 0xe623d58b3772ba13, 0x259c4df53d76e910, 0x1aa84ffbebac349f, 0x183926ae7d718dc2, 0xa11037230b367828, 0x43e90e15c2002132, 0x1942b34816fb4f26, 0x2748c36eeaffa273, 0x4856046901ff6c05, 0xe0e68d9f200c5358, 0x5e139a1b14fa8178, 0x8ac981ca9ceca6b3, 0x928b5fce34cdf21, 0x1ff17c29677589a0, 0x65c15c122133e2a2, 0x782a0735d02b1a20, 0x9da4384dbc2c8eae, 0xbae743abfbc07376, 0x1dd170ace2bcfc17, 0x1424bd194d3999e, 0x3951f214c02d824a, 0x7ad59ec00ebe6393, 0x6be409407034fded, 0xa4502c14f429ded9, 0x757cfb9913adc577, 0xfa6e051d6f8bc3ff, 0x1d6f60ba893ba84c, 0xf88abbe777df360e, 0xa5ab16cf451056ed, 0x2f30d0bdcaa516d, 0x15b34bbcb0298f41, 0xa13fc7e6faf9c830, 0x6b2e5dd607a9969c, 0x6b9f89b7dabbcb2b, 0x6b0f939998251a36, 0x4da570a2c574a304, 0x257ac0db1f419377, 0xeb8a25b7b40c0426, 0x6f28610b8c36485a, 0x11546d3ea28976d6, 0x52029c0b81f7be57, 0xb8e7a32e5783da5c, 0x1d733af522058b16, 0xc0edda4d891be43d, 0x481e1ab725b12d56, 0x1438495eacdf256, 0x224b251b33092002, 0xf358a8d368fceaea, 0xaacd6065b6e9f6ac, 0xfe312f84fa665204, 0xc4faace043b7f91c, 0x3787630a764ae4c9, 0xd4a277eaddaa925c, 0x2cf0b49df0bd70e9, 0x6f510308677709f5, 0xe914ffb4723793f1, 0x8006fe21a95d14dc, 0x61b7bb285a60791,];
            ReadOnlySpan<ulong> T13Ll = [0x0, 0xd08075ac1f200e4c, 0x2502f15067378a17, 0x806bddad09d9c4a3, 0x5d42b362af1ee859, 0x5229a7352c9b247b, 0x8bc3587fb118c94d, 0xe9c32d22e935007d, 0x91e135ee84a3f734, 0x724a166325437476, 0xeb90ce3700bf59b6, 0x6f398dfe3f7903f1, 0xf1203caf65bfb9b9, 0x583eab6852a22bb1, 0x35fb634c2e63a0f, 0x9a22b1526bb6a2e4, 0xd78b65cbefa7bb70, 0x1560e51a5df911dc, 0x9769d9b0a908a786, 0x33a6fe2d4fd53e8a, 0x21f977fe7c7fa118, 0x9f33f7bc78dc629f, 0x5a7a799221808de9, 0x4c72418596cc5bd0, 0x2589c98a8290d3f0, 0xdd30939a1d1e929c, 0x325c9e2203504517, 0x967357d6b36df9f8, 0xb165f141833a67da, 0x5a8c73beaa946990, 0x97ced890d5b0b0c0, 0xba1e54cf684354df, 0xed17ac8583339915, 0x20850e774a86cd8f, 0x322d7893ed4da9a8, 0x6c373a75c2828202, 0xd9a4be023ece032, 0x83ea957596be426d, 0xdefefee72ae7a33d, 0x5b718d616c4fef19, 0xc7686006e4e6c093, 0xcea65224bc9900d0, 0xf4dd023ff93c7ffb, 0x639aa6f940962626, 0x2bbd398af35c079f, 0x2a33269ab05c3e5d, 0xfa7663033f05357b, 0xfa628009459a2417, 0xb5c13ada0e77829a, 0xb70cfbb1bdf6eb5d, 0x613b0d1dbfa0d717, 0xcc2490c8643ef6b4, 0x1cb99d3f1ff298a2, 0xfa8fcbb2e85b853f, 0xcefcd5b62a14b818, 0x3a1c6473409c261d, 0x17d8d1e8ca31880b, 0xc8e7c95b06416e6d, 0x9392870834f21a53, 0x7c43b0ea5d43228d, 0xbdd80329364aa2a0, 0xef6797b5a11efb7c, 0x4844b29bf4af18e8, 0x9d2285b6754edd61,];
            ReadOnlySpan<int> T13Ex = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,];
            ReadOnlySpan<byte> T13Sgn = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,];

            ReadOnlySpan<ulong> T23Hh = [0x8000000000000000, 0x80058baf7fee3b5d, 0x800b179c82028fd0, 0x8010a3c708e73282, 0x8016302f17467628, 0x801bbcd4afcacb08, 0x802149b7d51ebefb, 0x8026d6d889ecfd69, 0x802c6436d0e04f50, 0x8031f1d2aca39b43, 0x80377fac1fe1e56a, 0x803d0dc32d464f85, 0x80429c17d77c18ed, 0x80482aaa212e9e95, 0x804db97a0d095b0c, 0x805348879db7e67d, 0x8058d7d2d5e5f6b0, 0x805e675bb83f5f0f, 0x8063f722477010a1, 0x8069872686241a12, 0x806f17687707a7af, 0x8074a7e81cc7036b, 0x807a38a57a0e94dc, 0x807fc9a0918ae142, 0x80855ad965e88b83, 0x808aec4ff9d45430, 0x80907e044ffb1984, 0x80960ff66b09d765, 0x809ba2264dada76a, 0x80a13493fa93c0d4, 0x80a6c73f74697897, 0x80ac5a28bddc4157, 0x80b1ed4fd999ab6c, 0x80b780b4ca4f64df, 0x80bd145792ab3970, 0x80c2a838355b1297, 0x80c83c56b50cf77f, 0x80cdd0b3146f0d11, 0x80d3654d562f95ec, 0x80d8fa257cfcf26e, 0x80de8f3b8b85a0af, 0x80e4248f84783c87, 0x80e9ba216a837f8c, 0x80ef4ff140564116, 0x80f4e5ff089f763e, 0x80fa7c4ac60e31e1, 0x810012d47b51a4a0, 0x8105a99c2b191ce1, 0x810b40a1d81406d4, 0x8110d7e584f1ec6d, 0x81166f673462756d, 0x811c0726e9156760, 0x81219f24a5baa59d, 0x812737606d023148, 0x812ccfda419c2956, 0x813268922638ca8b, 0x813801881d886f7b, 0x813d9abc2a3b9090, 0x8143342e4f02c405, 0x8148cdde8e8ebdec, 0x814e67cceb90502c, 0x815401f968b86a87, 0x81599c6408b81a94, 0x815f370cce408bc8,];
            ReadOnlySpan<ulong> T23Hl = [0x0, 0x1c718b38e549cb93, 0x945e54e2ae18f2f0, 0x2b96d62d51c15a07, 0x3690dfe44d11d008, 0xe23a986bd3e626f0, 0x7bdbadbc888aeb29, 0xb904bbfb40d3a2b6, 0xff8ce94a6797b3ce, 0xad9db772901d96b5, 0x61cd0bffd7cfc682, 0x43456f71b96affd4, 0x49fc841afba9c3c5, 0x86f7b54f6c45c85e, 0x6c9f1f7d1efcfe68, 0x171eb1ceef1d1f28, 0x94d589f608ee4aa2, 0x2ed38ab8472b2143, 0xb1652de1378af1a0, 0xb4ad9233a0390cac, 0xe54ec5f966eb1872, 0x4d204ecfc11f4aaa, 0x9bf3ef4d9be2d1e4, 0x7068ab2230585d12, 0xa0cc0a49c10ea66a, 0x84099bf6830f2767, 0x3aa8b9cbbc65a8aa, 0xf7d88c0928ba3946, 0x4a8a4f44bb703db6, 0x6699dc50dd96b773, 0x6e0472ed4ccfa2df, 0xba2dc7e0c72e51ba, 0x25335719b6e6fd20, 0x534dfa7417846aa4, 0xfc41c5c2d5336ccc, 0x34dc28baed8f3fde, 0xb880575ea03548c1, 0x32c1f98704428c71, 0x890e222a5eb95372, 0x24628efd9ca9d59a, 0x3b13310f5ad57fb0, 0x1a9dfefaeb616563, 0x718d1151d109bf97, 0x996709da2e25f04b, 0xe0adc640acaa6b0a, 0xd4eb5edc6b341283, 0x8ccd7223820719e3, 0xf24ebd6eb9ca4292, 0xcef03ab14a6654f, 0x4bf94297d1519822, 0xd0d8372f966cf15d, 0xb97931db7b7be2ec, 0x6abd3b0eab9c7047, 0xdaf888e965081519, 0xdc8046821f46122d, 0x6846ad73a8d9027f, 0xe885724f14131286, 0x83768490519df895, 0x661b22b45e25de17, 0xf11430fef78c6ee, 0x99775205944eadc4, 0x7de463a40d18260, 0x8f4a0b6748df795f, 0xe2404468cfe5ab9f,];
            ReadOnlySpan<ulong> T23Lh = [0x0, 0x34a318717a85d198, 0x36ee988aaff03620, 0x68b51f6090715cda, 0x403605216aed73f0, 0x5bdd95c213fb273c, 0x201cf874aa8cafc4, 0x84a6d5d525029ce2, 0x345f82f5b1fae20e, 0x8f6321e8e84c97d3, 0xc0432e96c959387b, 0x34c51656768b5277, 0xaedee98517f79365, 0x14747b1b6977fb14, 0x6b994b07993e3561, 0x5629bb4d6d20a74a, 0x2adc0c3f864ba0f5, 0xc40f99da125c266f, 0x8e5b66f89923f0ce, 0x930d2b4079a002bd, 0x76754509f037248a, 0xf02c00376690ea79, 0x6dbfe64309a2b072, 0x9fe6067d9e828773, 0xf0eb8fefacaf32d8, 0x9a875f4408858619, 0x8b22713e014be438, 0xd1441da0989f9760, 0x212bb24b9d533796, 0x8712128a139dc866, 0xc2857930dae5bef1, 0x6765fb22ac558aca, 0x1f60261b05f1202, 0x68164a4ae2414ea4, 0x65250abea5b33d49, 0x533c9eca3a17497d, 0x4704388d9f1b3cd2, 0x7e5ed5955b2d4887, 0x1197e58ebf689d43, 0xc5f4be776ef6a61a, 0x9bad68937edd6b38, 0x94426c99024f23f0, 0x85189bdd7ac4b012, 0xe18453f8dafeabf1, 0x8b6d28b5eb20d2f2, 0x370761b5ce7d7e44, 0x118525e07f78529c, 0x70f4efb7d5c90568, 0xa9c9ffc2ca67ffde, 0x9ee96b903910b0f, 0xb70c0ef050a08aa9, 0x135c526104fa1c29, 0xa7712808fe956328, 0xada38ad7502e18a9, 0x8b2f742bd9d4370a, 0x1163a8bcf6bffce3, 0x9cea3c3530355654, 0x605362ea89eb07d4, 0xa82b3121936ae61d, 0x932801def6b0fb, 0x2555ab2151b96f7c, 0xdc941f1fd7a051c0, 0x988da3f28bde163d, 0x4db5f07dc6319207,];
            ReadOnlySpan<ulong> T23Ll = [0x0, 0x945b3ca6120b7d55, 0x76cc37ff9584ce15, 0xe7a99fea0d150e10, 0x49b8f71dcaa49423, 0x9e66ef0d411e38d0, 0x92f52199af16c4de, 0x32bdce1e8420f0a8, 0x3d0b18c06975c162, 0x3cc84ae246bf5abb, 0xb9c4ef247a66c427, 0xb09b272d97fcc8, 0x836514d2d81fdf14, 0xbec69e11682e0863, 0x9e86080e1001781d, 0x7407003ab22ffa82, 0x9dc70119154b8f9a, 0x94d103f4365ed44b, 0x4c9bb4d5be541cc3, 0x5cf73638639bbbd6, 0xd762ffd79b46d451, 0x233e0911cc8de5f9, 0x3c5cacc11f785d53, 0xdc2ac4e4e300cb2c, 0x78899d5679c5b99e, 0xdfa0d299eae4aa3a, 0x47da7d37d3e079f6, 0xff7e8daa7390655a, 0xfcdbcb683daab7f0, 0xab9445c9a773244f, 0x95df5640f17d2dbe, 0x9a33e936c809a0e7, 0x3c355acba4df4fa, 0x2c64cb5808ef6fa6, 0xad82dbaac7bfa2e3, 0xa1b8b14b109d4838, 0x86f4e188e2ca8a59, 0x5b7e292a686df542, 0x3896d92dd4431f8b, 0x101735de189170ec, 0xeae4250b29447d4b, 0x235b5252cafbaa02, 0x9bea88f10391b325, 0xf5867174289d8d94, 0xb125fb6305bf7e6d, 0xf1eb5df89b727f7c, 0x97f6dffe47385081, 0x5301745d3b39c4d0, 0xada5b6f36036c85a, 0x45fe2b1237a101fc, 0xd499953c6b9aa8f0, 0x215ef11d179cc996, 0x717f9b1d39438323, 0x6ebf0e93981c95f2, 0x5718a10a231edabf, 0x2e1b37721d94b76, 0xf9c7f1fa9145fa7f, 0x4794c4e3bcb98244, 0x91fe35aa6124aefb, 0x8aa35adbcc33b28e, 0x1ab6ca4ae6eda941, 0xdddfd0f8f59dec56, 0x951855dd23786b9c, 0x221ce2379e877086,];
            ReadOnlySpan<int> T23Ex = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,];
            ReadOnlySpan<byte> T23Sgn = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,];



            Qint K, y, r;

            K = Qint.Mul11(x, Qint.Log2Inv);

            long k = K.Toi();

            K = Qint.Mul2(k, Qint.Log2);

            K = K with { ex = K.ex - 12, sgn = K.sgn ^ 1 };

            y = Qint.Add(x, K);

            long M = k >> 12;
            int i2 = (int)(k >> 6) & 0x3f;
            int i1 = (int)k & 0x3f;

            r = q3(y);

            r = Qint.Mul(new Qint(T13Ll[i2], T13Lh[i2], T13Hl[i2], T13Hh[i2], T13Ex[i2], T13Sgn[i2]), r);
            r = Qint.Mul(new Qint(T23Ll[i1], T23Lh[i1], T23Hl[i1], T23Hh[i1], T23Ex[i1], T23Sgn[i1]), r);

            r = r with { ex = r.ex + M };

            return r;
        }


        static (int flag, double r) exactPow(double x, double y, in Dint z)
        {
            double r = double.NaN;      // TODO originally undefined

            long _s = z.sgn != 0 ? -1 : 1;

            ulong m;
            long E;
            (E, m) = extract(x);

            if (m == 1)
            {
                double G = (double)E * y;

                if (isInt(G))
                {
                    r = z.sgn != 0 ? -1.0 : 1.0;
                    long g = (long)G;
                    r = pow2(r, g);

                    return (1, r);
                }

                return (0, r);
            }

            if (y < 0.0 || y > 34.0)
            {
                return (0, r);
            }

            ulong n;
            long F;
            (F, n) = extract(y);

            if (n > 34 || F < -5)
            {
                return (0, r);
            }

            if (F < 0)
            {
                if ((E & (long)(~0ul >> (64 + (int)F))) != 0)
                {
                    return (0, r);
                }

                long G, g = (E >> (int)-F) * (long)n;
                long k;

                (G, k) = Round54(z);

                int cnt = Polyfill.LeadingZeroCount((ulong)k);
                Dint d = new Dint(0, (ulong)k << cnt, G + 63 - cnt, 1 - z.sgn);
                d = Dint.Add(z, d);

                d = d with { ex = d.ex + 116 };

                if (Dint.CmpDintAbs(d, z) >= 0)
                {
                    return (0, r);
                }

                if (G > g)
                {
                    return (0, r);
                }

                if ((((ulong)k & ~(~1ul << (int)(g - G))) == (1ul << (int)(g - G))))
                {
                    r = (double)((k >> (int)(g - G)) * _s);
                    r = pow2(r, g);
                    return (1, r);
                }

                return (0, r);
            }

            {
                ulong t = n << (int)F;
                long k = 1;

                while (t != 0)
                {
                    if ((t & 0x1) != 0)
                    {
                        if (Polyfill.BigMul((long)m, k, out k) != 0)
                        {
                            return (0, r);
                        }
                    }

                    t >>= 1;

                    if (t != 0 && Polyfill.BigMul(m, m, out m) != 0)
                    {
                        return (0, r);
                    }
                }

                if (k >> 54 != 0)
                {
                    return (0, r);
                }

                r = (double)(k * _s);
                long G = E * (long)(n << (int)F);
                r = pow2(r, G);

                return (1, r);
            }
        }

        static bool isExact(double x, double y)
        {
            ulong v = Polyfill.DoubleToUInt64Bits(x), w = Polyfill.DoubleToUInt64Bits(y);
            if ((v << 1) != 0x7fe0000000000000ul && (w << 22) != 0)
            {
                return false;
            }

            if ((v << 1) == 0x7fe0000000000000ul)
            {
                return true;
            }

            ReadOnlySpan<ulong> xmax = [0, 0xffffffffffffffff, 94906265, 208063, 9741, 1551, 455, 189, 97, 59, 39, 27, 21, 15, 13, 11, 9, 7, 7, 5, 5, 5, 5, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3];

            if (y >= 0.0 && isInt(y))
            {
                ulong m = v & 0xffffffffffffful;
                long e = (long)((v << 1) >> 53) - 0x433;
                if (e >= -1074)
                {
                    m |= 0x10000000000000ul;
                }
                else
                {
                    e++;
                }

                int t = Polyfill.TrailingZeroCount(m);
                m >>= t;
                e += t;

                if (y == 0.0 || y == 1.0)
                {
                    return true;
                }
                if (m == 1)
                {
                    return -1074 <= y * e && y * e < 1024;
                }

                if (y < 0 || 33 < y)
                {
                    return false;
                }

                int yInt = (int)y;
                if (m > xmax[yInt])
                {
                    return false;
                }

                ulong my = m * m;
                for (int i = 2; i < yInt; i++)
                {
                    my *= m;
                }

                t = 64 - Polyfill.LeadingZeroCount(m);

                long ez = e * yInt + t;
                if (ez <= -1074 || 1024 < ez)
                {
                    return false;
                }

                return e * yInt >= -1074;
            }

            ulong n = w & 0xffffffffffffful;
            long f = (long)((w << 1) >> 53) - 0x433;
            if (f >= -1074)
            {
                n |= 0x10000000000000ul;
            }
            else
            {
                f++;
            }


            {
                int t = Polyfill.TrailingZeroCount(n);
                n >>= t;
                f += t;

                ulong m = v & 0xffffffffffffful;
                long e = (long)((v << 1) >> 53) - 0x433;
                if (e >= -1074)
                {
                    m |= 0x10000000000000ul;
                }
                else
                {
                    e++;
                }

                t = Polyfill.TrailingZeroCount(m);
                m >>= t;
                e += t;

                if (y < 0.0)
                {
                    if (m != 1)
                    {
                        return false;
                    }

                    long ez;
                    if (f >= 0)
                    {
                        ez = (f < 12) ? ((long)(0 - n) * e) << (int)f : 1024;
                    }
                    else
                    {
                        t = Polyfill.TrailingZeroCount((ulong)e);
                        if (-f > t)
                        {
                            return false;
                        }
                        ez = (-e >> (int)(-f)) * (long)n;
                    }

                    return -1074 <= ez && ez < 1024;
                }

                while (f++ != 0)
                {
                    if ((e & 1) != 0)
                    {
                        return false;
                    }

                    e /= 2;
                    double dm = (double)m;
                    double s = BuiltinRound(Sqrt(dm));

                    if (s * s != dm)
                    {
                        return false;
                    }
                    m = (ulong)s;
                }

                if (m > 1)
                {
                    if (33 < n)
                    {
                        return false;
                    }
                    if (m > xmax[(int)n])
                    {
                        return false;
                    }
                }


                ulong my = m, n0 = n;
                while (n0-- > 1)
                {
                    my += m;
                }

                t = 64 - Polyfill.LeadingZeroCount(my);

                return -1074 <= e * (int)n && e * (int)n + t <= 1024;
            }
        }












        double s = 1.0;
        ulong _x = Polyfill.DoubleToUInt64Bits(x);
        ulong _y = Polyfill.DoubleToUInt64Bits(y);

        if (_x >= 0x7ff0000000000000 || _y >= 0x7ff0000000000000)
        {
            if (double.IsNaN(x))
            {
                if (y == 0.0 && !isSignaling(x))
                {
                    return 1.0;
                }

                return x + x;
            }

            if (double.IsNaN(y))
            {
                if (x == 1.0 && !isSignaling(y))
                {
                    return 1.0;
                }

                return y + y;
            }

            if (_x == 0x7ff0000000000000)
            {
                if (y == 0.0)
                {
                    return 1.0;
                }
                if (y < 0.0)
                {
                    return 0.0;
                }
                if (y > 0.0)
                {
                    return double.PositiveInfinity;
                }
            }
            else if (_x == 0xfff0000000000000)
            {
                if (isInt(y) && !isInt(y * 0.5))
                {
                    if (y < 0.0)
                    {
                        return -0.0;
                    }
                    else
                    {
                        return double.NegativeInfinity;
                    }
                }

                if (y < 0.0)
                {
                    return 0.0;
                }

                if (y > 0.0)
                {
                    return double.PositiveInfinity;
                }
            }

            if (_y == 0x7ff0000000000000)
            {
                if (x == 0.0)
                {
                    return 0.0;
                }

                if (x == -1.0 || x == 1.0)
                {
                    return 1.0;
                }

                if (-1.0 < x && x < 1.0)
                {
                    return 0.0;
                }

                if (x < -1.0 || 1.0 < x)
                {
                    return double.PositiveInfinity;
                }
            }
            else if (_y == 0xfff0000000000000)
            {
                if (x == 0.0)
                {
                    return double.PositiveInfinity;
                }

                if (x == -1.0 || x == 1.0)
                {
                    return 1.0;
                }

                if (-1.0 < x && x < 1.0)
                {
                    return double.PositiveInfinity;
                }

                if (x < -1.0 || 1.0 < x)
                {
                    return 0.0;
                }
            }
        }



        if (x <= 0.0)
        {
            if (y == 0.0)
            {
                return 1.0;
            }

            if (_x == 0)
            {
                if (isInt(y) && !isInt(y * 0.5))
                {
                    if (y < 0.0)
                    {
                        return double.PositiveInfinity;
                    }

                    return 0.0;
                }

                if (y > 0.0)
                {
                    return 0.0;
                }

                return double.PositiveInfinity;
            }
            else if (_x == 0x8000000000000000)
            {
                if (isInt(y) && !isInt(y * 0.5))
                {
                    if (y < 0.0)
                    {
                        return double.NegativeInfinity;
                    }

                    return -0.0;
                }

                if (y > 0.0)
                {
                    return 0.0;
                }

                return double.PositiveInfinity;
            }

            if (!isInt(y))
            {
                return double.NaN;
            }

            ReadOnlySpan<double> cs = [1.0, -1.0];
            int yParity = Abs(y) >= 9007199254740992.0 ? 0 : (int)((long)y & 1);
            s = cs[yParity];
            x = -x;
        }


        bool exact;

        if (EnableFP)
        {
            double resH, resL, lh, ll;

            (int cancel, lh, ll) = log1(x);

            int ey = (int)(_y >> 52) & 0x7ff;
            if (ey < 0x36 || ey >= 0x7f5)
            {
                lh = ll = double.NaN;
            }

            double rh, rl;
            (rh, rl) = sMul(y, lh, ll);

            (resH, resL) = exp1(rh, rl, s);

            ReadOnlySpan<double> err = [6.246867986000465e-20, 4.6485168145316003e-18];

            double resMin, resMax;
            resMin = resH + FusedMultiplyAdd(err[cancel], -resH, resL);
            resMax = resH + FusedMultiplyAdd(err[cancel], resH, resL);

            exact = isExact(x, y);

            if (resMin == resMax)
            {
                return resMax;
            }

            if (y == 1.0)
            {
                return s * x;
            }

            if (y == 2.0)
            {
                double z = x * x;
                return z;
            }

            if (y == 0.5)
            {
                return Sqrt(x);
            }

            if (y == 0.0)
            {
                return 1.0;
            }
        }


        ulong rd;

        if (EnableZiv2)
        {
            Dint X = Dint.FromDoubleLog(x), Y = Dint.FromDoubleLog(y);

            X = X with { sgn = 0 };

            (Dint R, X) = log2(X);

            R = Dint.Mul21(R, Y);

            R = exp2(R);

            if (R.ex < -1075)
            {
                return 0.5 * (s * 4.9406564584124654e-324);
            }

            if (R.ex < -1022)
            {
                ulong ex = (ulong)-(1022 + R.ex);
                ulong m = R.lo >> (10 + (int)ex) | R.hi << (54 - (int)ex);

                rd = m + 14 > (2 * 14) ? 1ul : 0ul;
            }
            else
            {
                const int ErrBnd2 = 28;

                ulong lo = R.lo >> 10 | R.hi << 54;
                rd = lo + ErrBnd2 > (2 * ErrBnd2) ? 1ul : 0ul;
            }

            R = R with { sgn = s == -1.0 ? 1ul : 0ul };

            if (rd != 0)
            {
                return dintTod(R);
            }

            if (EnableExact)
            {
                (int flag, double e) = exactPow(x, y, R);
                if (flag != 0)
                {
                    return e;
                }
            }
        }


        if (EnableZiv3)
        {
            Qint qX = Qint.FromDouble(x), qY = Qint.FromDouble(y);

            qX = qX with { sgn = 0 };

            (Qint qR, qX) = log3(qX);

            qR = Qint.Mul41(qR, qY);

            Qint qZ = exp3(qR);

            const uint ErrBnd3 = 60;
            ulong r1 = qZ.hh << 54 | qZ.hl >> 10;
            ulong r2 = qZ.hl << 54 | qZ.lh >> 10;
            ulong r3 = qZ.lh << 54 | qZ.ll >> 10;

            rd = !((r1 == 0 && r2 == 0 && r3 <= ErrBnd3) || (~r1 == 0 && ~r2 == 0 && r3 + (2 * ErrBnd3) <= ErrBnd3)) ? 1ul : 0ul;

            if (rd != 0)
            {
                qZ = qZ with { sgn = s == -1.0 ? 1ul : 0ul, ll = qZ.ll & (~0ul << 10) };

                return qZ.ToDouble();
            }

            if (qR.ex < -56)
            {
                return (qR.sgn == 0) ? 1.0 + 7.8886090522101181e-31 : 1.0 - 7.8886090522101181e-31;
            }

            throw new InvalidOperationException($"Unexpected worst-case found. Please report to the developer. Worst-case of pow found: x, y = {x:g17}, {y:g17}");
        }
    }
}
