using System;
using System.Runtime.CompilerServices;

#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
#endif

namespace CoreMathSharp;

public static partial class StrictMathF
{
    /// <summary>
    /// Computes compound interest.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    /// <remarks>
    /// Mathematically, returns pow(x + 1, y).
    /// This method is more accurate than simply pow(x + 1, y).
    /// </remarks>
    public static float Compound(float x, float y)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool isSignalingF(float x)
        {
            uint u = Polyfill.SingleToUInt32Bits(x);
            u ^= 0x00400000;
            return (u & 0x7fffffff) > 0x7fc00000;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool isInt(float y)
        {
            uint wy = Polyfill.SingleToUInt32Bits(y);
            int ey = (int)(wy >> 23 & 0xff) - 127, s = ey + 9;
            if (ey >= 0)
            {
                if (s >= 32)
                {
                    return true;
                }
                return (wy << s) == 0;
            }
            return (wy << 1) == 0;
        }

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

            double ix = StrictMath.BuiltinRound(x);
            if (StrictMath.Abs(ix - x) == 0.5)
            {
                double u = ix;
                double v = ix - StrictMath.CopySign(1.0, x);
                if (Polyfill.TrailingZeroCount(Polyfill.DoubleToUInt64Bits(v)) > Polyfill.TrailingZeroCount(Polyfill.DoubleToUInt64Bits(u)))
                {
                    ix = v;
                }
            }

            return ix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double s, double t) fastTwoSum(double a, double b)
        {
            double s = a + b;
            double e = s - a;
            return (s, b - e);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double hi, double lo) aMul(double a, double b)
        {
            double hi = a * b;
            double lo = StrictMath.FusedMultiplyAdd(a, b, -hi);
            return (hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double hi, double lo) sMul(double a, double bh, double bl)
        {
            var (hi, lo) = aMul(a, bh);
            lo = StrictMath.FusedMultiplyAdd(a, bl, lo);
            return (hi, lo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double hi, double lo) dMul(double ah, double al, double bh, double bl)
        {
            var (hi, s) = aMul(ah, bh);
            double t = StrictMath.FusedMultiplyAdd(al, bh, s);
            double lo = StrictMath.FusedMultiplyAdd(ah, bl, t);
            return (hi, lo);
        }


        const double InvLog2 = 1.4426950408889634;
        //const double Log2 = 0.69314718055994529;


        static double p1(double z)
        {
            ReadOnlySpan<double> P = [0.0, 1.4426950408889634, -0.72134752044476802, 0.48089834696406908, -0.36067375082452474, 0.28853899226737745, -0.24052620964966426, 0.2061866781489112];

            double z2 = z * z;
            double c5 = StrictMath.FusedMultiplyAdd(P[6], z, P[5]);
            double c3 = StrictMath.FusedMultiplyAdd(P[4], z, P[3]);
            double c1 = StrictMath.FusedMultiplyAdd(P[2], z, P[1]);
            double z4 = z2 * z2;
            c5 = StrictMath.FusedMultiplyAdd(P[7], z2, c5);
            c1 = StrictMath.FusedMultiplyAdd(c3, z2, c1);
            c1 = StrictMath.FusedMultiplyAdd(c5, z4, c1);
            return z * c1;
        }

        static (double h, double l) p2(double zh, double zl)
        {
            ReadOnlySpan<double> P2 = [1.4426950408889634, 2.0355273740931317e-17, -0.72134752044448169, -1.0177636800051583e-17, 0.48089834696298778, 2.5288808125554186e-17, -0.36067376022224085, -5.0968567808439639e-18, 0.28853900817779266, 2.6289836446187457e-17, -0.24044917348149364, 0.20609929155556583, -0.18033688011479601, 0.16029944899207862, -0.14426947904986057, 0.13115406763151405, -0.12030649210107214, 0.11105797113583867];

            double t;
            double h = P2[4 + 13];
            for (int i = 12; i >= 7; i--)
            {
                h = StrictMath.FusedMultiplyAdd(h, zh, P2[4 + i]);
            }

            (h, double l) = sMul(h, zh, zl);
            (h, t) = fastTwoSum(P2[10], h);
            l += t;

            (h, l) = dMul(h, l, zh, zl);
            (h, t) = fastTwoSum(P2[8], h);
            l += t + P2[9];

            (h, l) = dMul(h, l, zh, zl);
            (h, t) = fastTwoSum(P2[6], h);
            l += t + P2[7];

            (h, l) = dMul(h, l, zh, zl);
            (h, t) = fastTwoSum(P2[4], h);
            l += t + P2[5];

            (h, l) = dMul(h, l, zh, zl);
            (h, t) = fastTwoSum(P2[2], h);
            l += t + P2[3];

            (h, l) = dMul(h, l, zh, zl);
            (h, t) = fastTwoSum(P2[0], h);
            l += t + P2[1];

            return dMul(h, l, zh, zl);
        }

        static double q1(double z)
        {
            ReadOnlySpan<double> Q = [1.0, 0.69314718053510949, 0.24022650695393627, 0.055504515574106836, 0.0096181873974531783];

            double z2 = z * z;
            double c3 = StrictMath.FusedMultiplyAdd(Q[4], z, Q[3]);
            double c0 = StrictMath.FusedMultiplyAdd(Q[1], z, Q[0]);
            double c2 = StrictMath.FusedMultiplyAdd(c3, z, Q[2]);
            return StrictMath.FusedMultiplyAdd(c2, z2, c0);
        }

        static (double qh, double ql) q2(double h, double l)
        {
            ReadOnlySpan<double> Q2 = [1.0, 0.69314718055994529, 2.3190455425771328e-17, 0.24022650695910072, -9.493917425934395e-18, 0.055504108664821583, -2.4715450854778426e-18, 0.0096181291076284769, 0.0013333558146326069, 0.00015403530393530196, 1.5252789714172188e-05, 1.3215480820210429e-06];

            double h2 = h * h;
            double c7 = StrictMath.FusedMultiplyAdd(Q2[11], h, Q2[10]);
            double c5 = StrictMath.FusedMultiplyAdd(Q2[9], h, Q2[8]);
            c5 = StrictMath.FusedMultiplyAdd(c7, h2, c5);

            double qh = c5 * h;
            (qh, double ql) = fastTwoSum(Q2[7], qh);
            double t;

            (qh, ql) = dMul(qh, ql, h, l);
            (qh, t) = fastTwoSum(Q2[5], qh);
            ql += t + Q2[6];

            (qh, ql) = dMul(qh, ql, h, l);
            (qh, t) = fastTwoSum(Q2[3], qh);
            ql += t + Q2[4];

            (qh, ql) = dMul(qh, ql, h, l);
            (qh, t) = fastTwoSum(Q2[1], qh);
            ql += t + Q2[2];

            (qh, ql) = dMul(qh, ql, h, l);
            (qh, t) = fastTwoSum(Q2[0], qh);
            ql += t;

            return (qh, ql);
        }



        static double log2p1(double x)
        {
            ReadOnlySpan<double> inv = [1.40625, 1.375, 1.34375, 1.3125, 1.296875, 1.265625, 1.25, 1.21875, 1.203125, 1.171875, 1.15625, 1.125, 1.109375, 1.09375, 1.078125, 1.0625, 1.046875, 1.03125, 1, 1, 0.9765625, 0.9609375, 0.9453125, 0.9375, 0.921875, 0.90625, 0.89453125, 0.8828125, 0.87109375, 0.859375, 0.84765625, 0.8359375, 0.828125, 0.81640625, 0.8046875, 0.796875, 0.78515625, 0.7734375, 0.765625, 0.7578125, 0.75, 0.7421875, 0.73046875, 0.72265625, 0.71484375, 0.70703125];
            ReadOnlySpan<double> log2inv = [-0.49185309632967472, 1.0820682119194486e-17, -0.45943161863729726, 3.8053583859449705e-19, -0.42626475470209796, 1.9932012137193316e-17, -0.39231742277876031, 1.6328502208352762e-17, -0.37503943134692475, -1.099000777384843e-17, -0.33985000288462475, 2.0897960245560436e-17, -0.32192809488736235, 3.7170199641426819e-19, -0.28540221886224837, 2.726283638197372e-17, -0.26678654069490138, 1.148454798555715e-17, -0.22881869049588088, 5.9678940542186452e-18, -0.20945336562894978, 1.7478015391165941e-18, -0.16992500144231237, 1.0448980122780218e-17, -0.14974711950468206, -3.3957331682262494e-18, -0.12928301694496647, 1.147571414337692e-17, -0.10852445677816905, -5.4046572138033075e-18, -0.087462841250339401, -6.7653212269912753e-18, -0.066089190457772437, 4.1302478527567341e-18, -0.044394119358453436, -1.3338680039226223e-18, 0, 0, 0, 0, 0.034215715337912955, 1.1151059892428047e-18, 0.057485494660760125, 1.1745696149950948e-19, 0.081136762725405487, 7.610716771889941e-19, 0.093109404391481465, 5.5961920578043772e-18, 0.11735695063815874, 5.4590525294637497e-18, 0.14201900487242788, -4.8982940096825213e-18, 0.16079621190305607, -7.5185647499571466e-18, 0.1798210375848123, -7.1448096253247018e-18, 0.19910010007969528, -1.3263604826229526e-17, 0.2186402864753404, 7.5223783500876519e-19, 0.23844876755552069, -7.1675723371070307e-18, 0.25853301359885306, -4.3007535189465375e-18, 0.2720795454368008, 2.4764753568785879e-17, 0.29264086791911725, -3.4856825155657363e-18, 0.31349947281678164, -2.4630201066282264e-17, 0.32757465802850438, 2.6214744450027748e-17, 0.34894830882107136, 2.3232525721961299e-17, 0.37064337992039037, 1.0829515961374715e-17, 0.38529015588479176, 2.2208024293925304e-17, 0.4000871578128723, 2.4103897311490816e-17, 0.41503749927884381, 5.2244900613901091e-18, 0.43014439166905216, -3.4945163577459646e-18, 0.45310554011236331, 2.1370790227232135e-17, 0.46861853948368787, 2.1195035355308622e-18, 0.48430016171595752, 1.9794762178834054e-17, 0.50015411291679468, -4.0288695985439377e-17];


            double u = 1.0 + x;

            ulong v = Polyfill.DoubleToUInt64Bits(u);
            ulong m = v & 0xffffffffffffful;
            int e = (int)(v >> 52) - 0x3ff + (m >= 0x6a09e667f3bcdul ? 1 : 0);
            v -= (ulong)(e * 0x10000000000000L);

            double t = Polyfill.UInt64BitsToDouble(v);
            v = Polyfill.DoubleToUInt64Bits(Polyfill.UInt64BitsToDouble(v) + 2.0);
            int i = (int)(v >> 45) - 0x2002d;

            double r = inv[i];
            double z = StrictMath.FusedMultiplyAdd(r, t, -1.0);
            double p = p1(z);

            return (double)e + (log2inv[i * 2 + 0] + p);
        }




        static float exp2_1(double t)
        {
            ReadOnlySpan<double> exp2T = [-0.5, -0.46875, -0.4375, -0.40625, -0.375, -0.34375, -0.3125, -0.28125, -0.25, -0.21875, -0.1875, -0.15625, -0.125, -0.09375, -0.0625, -0.03125, 0, 0.03125, 0.0625, 0.09375, 0.125, 0.15625, 0.1875, 0.21875, 0.25, 0.28125, 0.3125, 0.34375, 0.375, 0.40625, 0.4375, 0.46875, 0.5];
            ReadOnlySpan<double> exp2U = [0.70710678118654757, -4.8336466567264567e-17, 0.72259040348852333, -1.5118790674969937e-17, 0.73841307296974967, -1.7419972784463979e-17, 0.75458221379671142, -5.0822766387714752e-17, 0.77110541270397037, 3.9749174048488104e-17, 0.78799042255394325, -5.068458235639152e-18, 0.80524516597462714, 1.2353596284898944e-17, 0.82287773907698247, -5.0628399568373863e-17, 0.8408964152537145, 4.0995050102907483e-17, 0.85930964906123897, -9.2569020913155549e-18, 0.87812608018664973, 1.4800703477244367e-17, 0.89735453750155358, 9.1137292139560434e-18, 0.91700404320467122, 1.6415536121228136e-17, 0.93708381705514998, -3.0613817065020713e-17, 0.9576032806985737, -5.3099730280979813e-17, 0.97857206208770009, 4.4803838955183339e-17, 1, 0, 1.0218971486541166, 5.1092250289734439e-17, 1.0442737824274138, 8.5518897055379649e-17, 1.0671404006768237, -7.8998539668415821e-17, 1.0905077326652577, -3.0467820798124711e-17, 1.1143867425958924, 1.0410278456845571e-16, 1.1387886347566916, 8.9128126760254078e-17, 1.1637248587775775, 3.8292048369240935e-17, 1.189207115002721, 3.9820152314656461e-17, 1.215247359980469, -7.7126306926814881e-17, 1.241857812073484, 4.6580275918369368e-17, 1.2690509571917332, 2.6679321313421861e-18, 1.2968395546510096, 2.5382502794888315e-17, 1.3252366431597413, -2.8587312100388614e-17, 1.3542555469368927, 7.7009483798029895e-17, 1.383909881963832, -6.7705116587947863e-17, 1.4142135623730951, -9.6672933134529135e-17];


            double k = RoundEvenFinite(t);
            double r = t - k;
            ulong v = Polyfill.DoubleToUInt64Bits(3.015625 + r);
            int i = (int)(v >> 46) - 0x10010;
            r -= exp2T[i];
            v = Polyfill.DoubleToUInt64Bits(exp2U[i * 2 + 0] * q1(r));

            ulong err = Polyfill.DoubleToUInt64Bits(5.0626169922907138e-13);
            v += (ulong)((long)k * 0x10000000000000L);

            if (Polyfill.UInt64BitsToDouble(v) < 1.175494350822881e-38)
            {
                return -1.0f;
            }

            err += (ulong)((long)k * 0x10000000000000L);
            float lb = (float)(Polyfill.UInt64BitsToDouble(v) - Polyfill.UInt64BitsToDouble(err));
            float rb = (float)(Polyfill.UInt64BitsToDouble(v) + Polyfill.UInt64BitsToDouble(err));

            if (lb != rb)
            {
                return -1.0f;
            }

            return lb;
        }

        static (bool exact, bool midpoint) isExactOrMidpoint(float x, float y)
        {
            uint v = Polyfill.SingleToUInt32Bits(x), w = Polyfill.SingleToUInt32Bits(y);
            if ((v << 1) != 0 && (w << (32 - 16)) != 0)
            {
                return (false, false);
            }
            if ((v << 1) == 0)
            {
                return (true, false);
            }

            int e = (int)((v << 1) >> 24) - 0x96;
            if (e < -76 || 30 < e)
            {
                return (false, false);
            }

            ulong vd = Polyfill.DoubleToUInt64Bits(1.0 + x);
            e = (int)((vd << 1) >> 53) - 0x433;


            ReadOnlySpan<ulong> xmax = [0, 0xffffff, 5791, 321, 75, 31, 17, 11, 7, 5, 5, 3, 3, 3, 3, 3];

            if (y >= 0 && isInt(y))
            {
                ulong m = vd & 0xffffffffffffful;
                if (e >= -1074)
                {
                    m |= 0x10000000000000ul;
                }
                else
                {
                    e++;
                }
                int t = Polyfill.TrailingZeroCount(m);
                m = m >> t;
                e += t;

                if (y == 0.0f || y == 1.0f)
                {
                    return (true, m > 0x1000000u);
                }
                if (m == 1)
                {
                    return (-149f <= y * e && y * e < 128f, false);
                }
                if (y < 0.0f || 15.0f < y)
                {
                    return (false, false);
                }

                int yInt = (int)y;
                if (m > xmax[yInt])
                {
                    return (false, false);
                }

                ulong my = m * m;
                for (int i = 2; i < yInt; i++)
                {
                    my = my * m;
                }

                t = 64 - Polyfill.LeadingZeroCount(my);
                int ez = e * yInt + t;
                if (ez <= -149 || 128 < ez)
                {
                    return (false, false);
                }

                return (e * yInt >= -149, my > 0x1000000u);
            }


            {
                uint n = w & 0x7fffff;
                int f = (int)((w << 1) >> 24) - 0x96;
                if (f >= -149)
                {
                    n |= 0x800000;
                }
                else
                {
                    f++;
                }
                int t = Polyfill.TrailingZeroCount(n);
                n = n >> t;
                f += t;

                ulong m = vd & 0xffffffffffffful;
                if (e >= -1074)
                {
                    m |= 0x10000000000000ul;
                }
                else
                {
                    e++;
                }
                t = Polyfill.TrailingZeroCount(m);
                m = m >> t;
                e += t;


                if (y < 0.0f)
                {
                    if (m != 1)
                    {
                        return (false, false);
                    }

                    t = Polyfill.TrailingZeroCount((uint)e);
                    if (-f > t)
                    {
                        return (false, false);
                    }

                    int ez;
                    if (e >= 0)
                    {
                        ez = ((f >= 0) ? -(e << f) : -(e >> -f)) * (int)n;
                    }
                    else
                    {
                        ez = ((f >= 0) ? (-e << f) : (-e >> -f)) * (int)n;
                    }

                    return (-149 <= ez && ez < 128, false);
                }

                while (f++ != 0)
                {
                    if ((e & 1) != 0)
                    {
                        return (false, false);
                    }

                    e /= 2;
                    double dm = (double)m;
                    double s = StrictMath.BuiltinRound(StrictMath.Sqrt(dm));
                    if (s * s != dm)
                    {
                        return (false, false);
                    }

                    m = (ulong)s;
                }

                if (m > 1)
                {
                    if (15 < n)
                    {
                        return (false, false);
                    }

                    if (m > xmax[(int)n])
                    {
                        return (false, false);
                    }
                }

                uint my = (uint)m, n0 = n;
                while (n0-- > 1)
                {
                    my = my * (uint)m;
                }

                t = 32 - Polyfill.LeadingZeroCount(my);

                return (-149 <= e * (int)n && e * (int)n + t <= 128, false);
            }
        }

        static float exp2_2(double h, double l, float x, float y, bool exact)
        {
            ReadOnlySpan<double> exp2T = [-0.5, -0.46875, -0.4375, -0.40625, -0.375, -0.34375, -0.3125, -0.28125, -0.25, -0.21875, -0.1875, -0.15625, -0.125, -0.09375, -0.0625, -0.03125, 0, 0.03125, 0.0625, 0.09375, 0.125, 0.15625, 0.1875, 0.21875, 0.25, 0.28125, 0.3125, 0.34375, 0.375, 0.40625, 0.4375, 0.46875, 0.5];
            ReadOnlySpan<double> exp2U = [0.70710678118654757, -4.8336466567264567e-17, 0.72259040348852333, -1.5118790674969937e-17, 0.73841307296974967, -1.7419972784463979e-17, 0.75458221379671142, -5.0822766387714752e-17, 0.77110541270397037, 3.9749174048488104e-17, 0.78799042255394325, -5.068458235639152e-18, 0.80524516597462714, 1.2353596284898944e-17, 0.82287773907698247, -5.0628399568373863e-17, 0.8408964152537145, 4.0995050102907483e-17, 0.85930964906123897, -9.2569020913155549e-18, 0.87812608018664973, 1.4800703477244367e-17, 0.89735453750155358, 9.1137292139560434e-18, 0.91700404320467122, 1.6415536121228136e-17, 0.93708381705514998, -3.0613817065020713e-17, 0.9576032806985737, -5.3099730280979813e-17, 0.97857206208770009, 4.4803838955183339e-17, 1, 0, 1.0218971486541166, 5.1092250289734439e-17, 1.0442737824274138, 8.5518897055379649e-17, 1.0671404006768237, -7.8998539668415821e-17, 1.0905077326652577, -3.0467820798124711e-17, 1.1143867425958924, 1.0410278456845571e-16, 1.1387886347566916, 8.9128126760254078e-17, 1.1637248587775775, 3.8292048369240935e-17, 1.189207115002721, 3.9820152314656461e-17, 1.215247359980469, -7.7126306926814881e-17, 1.241857812073484, 4.6580275918369368e-17, 1.2690509571917332, 2.6679321313421861e-18, 1.2968395546510096, 2.5382502794888315e-17, 1.3252366431597413, -2.8587312100388614e-17, 1.3542555469368927, 7.7009483798029895e-17, 1.383909881963832, -6.7705116587947863e-17, 1.4142135623730951, -9.6672933134529135e-17];


            if (y == 1.0f)
            {
                return 1.0f + x;
            }

            double k = RoundEvenFinite(h);

            if (k == 0.0 && StrictMath.Abs(h) <= 4.2995663356387357e-08)
            {
                return (float)(1.0 + h * 0.5);
            }

            double r = h - k;
            (h, l) = fastTwoSum(r, l);
            ulong v = Polyfill.DoubleToUInt64Bits(3.015625 + h);
            int i = (int)(v >> 46) - 0x10010;
            h -= exp2T[i];

            (h, l) = fastTwoSum(h, l);
            double qh, ql;
            (qh, ql) = q2(h, l);

            (qh, ql) = dMul(exp2U[i * 2 + 0], exp2U[i * 2 + 1], qh, ql);
            (qh, ql) = fastTwoSum(qh, ql);

            ulong w = Polyfill.DoubleToUInt64Bits(qh);
            if (((w + 1) & 0xffffffful) <= 2)
            {
                ReadOnlySpan<double> Err = [8.7443653621938717e-26, 4.8613553284244853e-29];

                int small = (k == 0 && i == 16 && StrictMath.Abs(h) <= 3.814697265625e-06) ? 1 : 0;
                double err = Err[small];

                v = Polyfill.DoubleToUInt64Bits(qh + (ql - err));
                v += (ulong)((long)k * 0x10000000000000L);
                w = Polyfill.DoubleToUInt64Bits(qh + (ql + err));
                w += (ulong)((long)k * 0x10000000000000L);

                float left, right;

                if (exact)
                {
                    int vtz = Polyfill.TrailingZeroCount(v);
                    int wtz = Polyfill.TrailingZeroCount(w);
                    return vtz >= wtz ? (float)Polyfill.UInt64BitsToDouble(v) : (float)Polyfill.UInt64BitsToDouble(w);
                }

                left = (float)Polyfill.UInt64BitsToDouble(v);
                right = (float)Polyfill.UInt64BitsToDouble(w);

                if (left != right)
                {
                    throw new InvalidOperationException($"Rounding test of accurate path failed for compound({x:g9}, {y:g9}). Please report the above to the developer.");
                }
            }

            v = Polyfill.DoubleToUInt64Bits(qh + ql);
            if ((w << 36) == 0 && Polyfill.UInt64BitsToDouble(v) == qh && ql != 0)
            {
                v += (ql > 0) ? 1ul : ~0ul;
            }
            v += (ulong)((long)k * 0x10000000000000L);

            float res = (float)Polyfill.UInt64BitsToDouble(v);
            return res;
        }

        static (double h, double l) log2p1Accurate(double x)
        {
            ReadOnlySpan<double> inv = [1.40625, 1.375, 1.34375, 1.3125, 1.296875, 1.265625, 1.25, 1.21875, 1.203125, 1.171875, 1.15625, 1.125, 1.109375, 1.09375, 1.078125, 1.0625, 1.046875, 1.03125, 1, 1, 0.9765625, 0.9609375, 0.9453125, 0.9375, 0.921875, 0.90625, 0.89453125, 0.8828125, 0.87109375, 0.859375, 0.84765625, 0.8359375, 0.828125, 0.81640625, 0.8046875, 0.796875, 0.78515625, 0.7734375, 0.765625, 0.7578125, 0.75, 0.7421875, 0.73046875, 0.72265625, 0.71484375, 0.70703125];
            ReadOnlySpan<double> log2inv = [-0.49185309632967472, 1.0820682119194486e-17, -0.45943161863729726, 3.8053583859449705e-19, -0.42626475470209796, 1.9932012137193316e-17, -0.39231742277876031, 1.6328502208352762e-17, -0.37503943134692475, -1.099000777384843e-17, -0.33985000288462475, 2.0897960245560436e-17, -0.32192809488736235, 3.7170199641426819e-19, -0.28540221886224837, 2.726283638197372e-17, -0.26678654069490138, 1.148454798555715e-17, -0.22881869049588088, 5.9678940542186452e-18, -0.20945336562894978, 1.7478015391165941e-18, -0.16992500144231237, 1.0448980122780218e-17, -0.14974711950468206, -3.3957331682262494e-18, -0.12928301694496647, 1.147571414337692e-17, -0.10852445677816905, -5.4046572138033075e-18, -0.087462841250339401, -6.7653212269912753e-18, -0.066089190457772437, 4.1302478527567341e-18, -0.044394119358453436, -1.3338680039226223e-18, 0, 0, 0, 0, 0.034215715337912955, 1.1151059892428047e-18, 0.057485494660760125, 1.1745696149950948e-19, 0.081136762725405487, 7.610716771889941e-19, 0.093109404391481465, 5.5961920578043772e-18, 0.11735695063815874, 5.4590525294637497e-18, 0.14201900487242788, -4.8982940096825213e-18, 0.16079621190305607, -7.5185647499571466e-18, 0.1798210375848123, -7.1448096253247018e-18, 0.19910010007969528, -1.3263604826229526e-17, 0.2186402864753404, 7.5223783500876519e-19, 0.23844876755552069, -7.1675723371070307e-18, 0.25853301359885306, -4.3007535189465375e-18, 0.2720795454368008, 2.4764753568785879e-17, 0.29264086791911725, -3.4856825155657363e-18, 0.31349947281678164, -2.4630201066282264e-17, 0.32757465802850438, 2.6214744450027748e-17, 0.34894830882107136, 2.3232525721961299e-17, 0.37064337992039037, 1.0829515961374715e-17, 0.38529015588479176, 2.2208024293925304e-17, 0.4000871578128723, 2.4103897311490816e-17, 0.41503749927884381, 5.2244900613901091e-18, 0.43014439166905216, -3.4945163577459646e-18, 0.45310554011236331, 2.1370790227232135e-17, 0.46861853948368787, 2.1195035355308622e-18, 0.48430016171595752, 1.9794762178834054e-17, 0.50015411291679468, -4.0288695985439377e-17];

            double h, l;

            if (1.0 >= x)
            {
                if (StrictMath.Abs(x) >= 1.1102230246251565e-16)
                {
                    (h, l) = fastTwoSum(1.0, x);
                }
                else
                {
                    (h, l) = (1.0, x);
                }
            }
            else
            {
                (h, l) = fastTwoSum(x, 1.0);
            }

            ulong v = Polyfill.DoubleToUInt64Bits(h);
            ulong m = v & 0xffffffffffffful;
            int e = (int)(v >> 52) - 0x3ff + (m >= 0x6a09e667f3bcdul ? 1 : 0);

            ReadOnlySpan<double> scale = [536870912, 268435456, 134217728, 67108864, 33554432, 16777216, 8388608, 4194304, 2097152, 1048576, 524288, 262144, 131072, 65536, 32768, 16384, 8192, 4096, 2048, 1024, 512, 256, 128, 64, 32, 16, 8, 4, 2, 1, 0.5, 0.25, 0.125, 0.0625, 0.03125, 0.015625, 0.0078125, 0.00390625, 0.001953125, 0.0009765625, 0.00048828125, 0.000244140625, 0.0001220703125, 6.103515625e-05, 3.0517578125e-05, 1.52587890625e-05, 7.62939453125e-06, 3.814697265625e-06, 1.9073486328125e-06, 9.5367431640625e-07, 4.76837158203125e-07, 2.384185791015625e-07, 1.1920928955078125e-07, 5.9604644775390625e-08, 2.9802322387695312e-08, 1.4901161193847656e-08, 7.4505805969238281e-09, 3.7252902984619141e-09, 1.862645149230957e-09, 9.3132257461547852e-10, 4.6566128730773926e-10, 2.3283064365386963e-10, 1.1641532182693481e-10, 5.8207660913467407e-11, 2.9103830456733704e-11, 1.4551915228366852e-11, 7.2759576141834259e-12, 3.637978807091713e-12, 1.8189894035458565e-12, 9.0949470177292824e-13, 4.5474735088646412e-13, 2.2737367544323206e-13, 1.1368683772161603e-13, 5.6843418860808015e-14, 2.8421709430404007e-14, 1.4210854715202004e-14, 7.1054273576010019e-15, 3.5527136788005009e-15, 1.7763568394002505e-15, 8.8817841970012523e-16, 4.4408920985006262e-16, 2.2204460492503131e-16, 1.1102230246251565e-16, 5.5511151231257827e-17, 2.7755575615628914e-17, 1.3877787807814457e-17, 6.9388939039072284e-18, 3.4694469519536142e-18, 1.7347234759768071e-18, 8.6736173798840355e-19, 4.3368086899420177e-19, 2.1684043449710089e-19, 1.0842021724855044e-19, 5.4210108624275222e-20, 2.7105054312137611e-20, 1.3552527156068805e-20, 6.7762635780344027e-21, 3.3881317890172014e-21, 1.6940658945086007e-21, 8.4703294725430034e-22, 4.2351647362715017e-22, 2.1175823681357508e-22, 1.0587911840678754e-22, 5.2939559203393771e-23, 2.6469779601696886e-23, 1.3234889800848443e-23, 6.6174449004242214e-24, 3.3087224502121107e-24, 1.6543612251060553e-24, 8.2718061255302767e-25, 4.1359030627651384e-25, 2.0679515313825692e-25, 1.0339757656912846e-25, 5.169878828456423e-26, 2.5849394142282115e-26, 1.2924697071141057e-26, 6.4623485355705287e-27, 3.2311742677852644e-27, 1.6155871338926322e-27, 8.0779356694631609e-28, 4.0389678347315804e-28, 2.0194839173657902e-28, 1.0097419586828951e-28, 5.0487097934144756e-29, 2.5243548967072378e-29, 1.2621774483536189e-29, 6.3108872417680944e-30, 3.1554436208840472e-30, 1.5777218104420236e-30, 7.8886090522101181e-31, 3.944304526105059e-31, 1.9721522630525295e-31, 9.8607613152626476e-32, 4.9303806576313238e-32, 2.4651903288156619e-32, 1.2325951644078309e-32, 6.1629758220391547e-33, 3.0814879110195774e-33, 1.5407439555097887e-33, 7.7037197775489434e-34, 3.8518598887744717e-34, 1.9259299443872359e-34, 9.6296497219361793e-35, 4.8148248609680896e-35, 2.4074124304840448e-35, 1.2037062152420224e-35, 6.018531076210112e-36, 3.009265538105056e-36, 1.504632769052528e-36, 7.5231638452626401e-37, 3.76158192263132e-37, 1.88079096131566e-37, 9.4039548065783001e-38, 4.70197740328915e-38, 2.350988701644575e-38, 1.1754943508222875e-38, 5.8774717541114375e-39, 2.9387358770557188e-39];

            h *= scale[e + 29];
            l *= scale[e + 29];

            v = Polyfill.DoubleToUInt64Bits(2.0 + h);
            int i = (int)(v >> 45) - 0x2002d;

            double r = inv[i];
            double zh = StrictMath.FusedMultiplyAdd(r, h, -1.0);
            double zl = r * l;

            (zh, zl) = fastTwoSum(zh, zl);

            double ph, pl;
            (ph, pl) = p2(zh, zl);

            (h, l) = fastTwoSum((double)e, log2inv[i * 2 + 0]);
            l += log2inv[i * 2 + 1];

            double t;
            (h, t) = fastTwoSum(h, ph);
            l += t + pl;

            return (h, l);
        }

        static double accuratePath(float x, float y, bool exact)
        {
            double h, l;

            (h, l) = log2p1Accurate(x);
            (h, l) = sMul(y, h, l);

            return exp2_2(h, l, x, y, exact);
        }

        static float asCompoundFSpecial(float x, float y)
        {
            uint nx = Polyfill.SingleToUInt32Bits(x), ny = Polyfill.SingleToUInt32Bits(y);
            uint ax = nx << 1, ay = ny << 1;

            if (ax == 0 || ay == 0)
            {
                if (ax == 0)
                {
                    return isSignalingF(y) ? x + y : 1.0f;
                }

                if (ay == 0)
                {
                    if (isSignalingF(x))
                    {
                        return x + y;
                    }
                    if (x < -1.0f)
                    {
                        return 0.0f / 0.0f;
                    }
                    else
                    {
                        return 1.0f;
                    }
                }
            }

            uint mone = Polyfill.SingleToUInt32Bits(-1.0f);
            if (ay >= 0xffu << 24)
            {
                if (ax > 0xffu << 24)
                {
                    return x + y;
                }
                if (ay == 0xffu << 24)
                {
                    if (nx > mone)
                    {
                        return 0.0f / 0.0f;
                    }

                    int sy = (int)(ny >> 31);
                    if (nx == mone)
                    {
                        if (sy == 0)
                        {
                            return 0.0f;
                        }
                        else
                        {
                            return 1.0f / 0.0f;
                        }
                    }
                    if (x < 0.0f)
                    {
                        if (sy == 0)
                        {
                            return 0.0f;
                        }
                        else
                        {
                            return 1.0f / 0.0f;
                        }
                    }
                    if (x > 0.0f)
                    {
                        if (sy != 0)
                        {
                            return 0.0f;
                        }
                        else
                        {
                            return 1.0f / 0.0f;
                        }
                    }

                    return 1.0f;
                }

                return x + y;
            }

            if (nx >= 0xffu << 23)
            {
                if (ax == 0xffu << 24)
                {
                    if (nx >> 31 != 0)
                    {
                        return 0.0f / 0.0f;
                    }
                    return (ny >> 31) != 0 ? 1.0f / x : x;
                }
                if (ax > 0xffu << 24)
                {
                    return x + y;
                }
                if (nx > mone)
                {
                    return 0.0f / 0.0f;
                }
                if (ny >> 31 != 0)
                {
                    return 1.0f / 0.0f;
                }
                else
                {
                    return 0.0f;
                }
            }

            return 0.0f;
        }



        uint mone = Polyfill.SingleToUInt32Bits(-1.0f);
        uint nx = Polyfill.SingleToUInt32Bits(x), ny = Polyfill.SingleToUInt32Bits(y);
        if (nx >= mone)
        {
            return asCompoundFSpecial(x, y);
        }

        uint ax = nx << 1, ay = ny << 1;
        if (ax == 0 || ax >= 0xffu << 24 || ay == 0 || ay >= 0xffu << 24)
        {
            return asCompoundFSpecial(x, y);
        }

        double xd = x, yd = y;
        ulong tx = Polyfill.DoubleToUInt64Bits(xd), ty = Polyfill.DoubleToUInt64Bits(yd);

        double l;
        if (ax < 0x62000000u)
        {
            double t1 = xd - (xd * xd) * 0.5;
            l = InvLog2 * t1;
        }
        else
        {
            l = log2p1(Polyfill.UInt64BitsToDouble(tx));
        }

        ulong t = Polyfill.DoubleToUInt64Bits(l * Polyfill.UInt64BitsToDouble(ty));

        if ((t << 1) >= 0x406ul << 53)
        {
            if (t >= 0x3018bul << 46)
            {
                return 1.1754943508222875e-38f * 1.1754943508222875e-38f;
            }
            else if ((t >> 63) == 0)
            {
                return 8.5070591730234616e+37f * 8.5070591730234616e+37f;
            }
        }

        if ((t << 1) <= 0x7cce2a8ed5e1a9b2ul)
        {
            return (t >> 63) != 0 ? 1.0f - 2.9802322387695312e-08f : 1.0f + 2.9802322387695312e-08f;
        }


        var (exact, midpoint) = isExactOrMidpoint(x, y);
        float res = exp2_1(Polyfill.UInt64BitsToDouble(t));
        if (res != -1.0f)
        {
            return res;
        }

        return (float)accuratePath(x, y, exact);
    }
}
