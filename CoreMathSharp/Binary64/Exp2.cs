using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
#endif

namespace CoreMathSharp;

public static partial class StrictMath
{
    public static double Exp2(double x)
    {

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
        static (double s, double e) fastTwoSum(double x, double y)
        {
            double s = x + y, z = s - x;
            return (s, y - z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double s, double e) fastSum(double xh, double xl, double yh, double yl)
        {
            var (sh, sl) = fastTwoSum(xh, yh);
            return (sh, (xl + yl) + sl);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double h, double l) muldd(double xh, double xl, double ch, double cl)
        {
            double ahhh = ch * xh;
            double l = (ch * xl + cl * xh) + FusedMultiplyAdd(ch, xh, -ahhh);
            return (ahhh, l);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double h, double l) mulddd(double xh, double ch, double cl)
        {
            double ahhh = ch * xh;
            double l = cl * xh + FusedMultiplyAdd(ch, xh, -ahhh);
            return (ahhh, l);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double h, double l) polydd(double xh, int n, ReadOnlySpan<double> c)
        {
            int i = n - 1;
            double ch = c[i * 2 + 0], cl = c[i * 2 + 1];

            while (--i >= 0)
            {
                (ch, cl) = mulddd(xh, ch, cl);
                double th = ch + c[i * 2 + 0], tl = (c[i * 2 + 0] - th) + ch;
                ch = th;
                cl += tl + c[i * 2 + 1];
            }

            return (ch, cl);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double asLdexp(double x, int i)
        {
            ulong ix = Polyfill.DoubleToUInt64Bits(x);
            ix += (ulong)i << 52;
            return Polyfill.UInt64BitsToDouble(ix);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double asToDenormal(double x)
        {
            ulong ix = Polyfill.DoubleToUInt64Bits(x);
            ix &= ~0ul >> 12;
            return Polyfill.UInt64BitsToDouble(ix);
        }

        static double asExp2Database(double x, double f)
        {
            ReadOnlySpan<double> db = [0.0018476455676928627, 0.00185919233062784, 0.0028484658079196202, 0.0028763223538210525, 0.0077149269583885781, 0.024231918711723883, 0.087702242567795616, 0.095788918003709367, 0.12355887769708004, 0.1778592311907721, 0.19649005786728485, 0.62149794152056237, 0.84993304531285274, 1.6214979415205624, -0.00015437939628496522, -0.00017909522604891929, -0.00018180307074928715, -0.00038686796803203853, -0.00039076062694585036, -0.00052637306201794732, -0.00068837138132979729, -0.0007454354460954939, -0.001045424250995566, -0.0012160551746951969, -0.0025002480913021924, -0.0030030613364612027, -0.0033761494469129148, -0.0041673040512283802, -0.0043900902615763623, -0.0056593440160994128, -0.0075068956271017659, -0.00805569542034636, -0.017779076602942218, -0.018912661424181635, -0.041311682156002484, -0.077798226325677039, -0.15006695468714726, -0.22876988070583898, -0.24928514095356566, -0.37850205847943763, -0.47095680395564515, -0.90421108199629063, -1.3785020584794376];
            ReadOnlySpan<ulong> idb = MemoryMarshal.Cast<double, ulong>(db);

            ulong ix = Polyfill.DoubleToUInt64Bits(x);
            int a = 0, b = db.Length - 1, m = (a + b) / 2;
            while (a <= b)
            {
                ulong t = idb[m];
                if (t < ix)
                {
                    a = m + 1;
                }
                else if (t == ix)
                {
                    ReadOnlySpan<ulong> s2 = [0x3b216fbd5fd7665f, 0x34c797];
                    const long k = 8677191773140;

                    ulong p = (s2[m >> 5] >> ((m * 2) & 63)) & 3;
                    ulong jf = Polyfill.DoubleToUInt64Bits(f), dy = (ulong)(0x3c90 | ((k >> m) << 15)) << 48;

                    for (int i = -1; i <= 1; i++)
                    {
                        ulong y = jf + (ulong)i;
                        if ((y & 3) == p)
                        {
                            return Polyfill.UInt64BitsToDouble(y) + Polyfill.UInt64BitsToDouble(dy);
                        }
                    }
                    break;
                }
                else
                {
                    b = m - 1;
                }
                m = (a + b) / 2;
            }

            return f;
        }




        static double asExp2Accurate(double x, ReadOnlySpan<double> t0, ReadOnlySpan<double> t1)
        {
            ulong ix = Polyfill.DoubleToUInt64Bits(x);
            double sx = 4096.0 * x, fx = RoundEvenFinite(sx), z = sx - fx;
            long k = (long)fx;
            int i1 = (int)k & 0x3f, i0 = (int)(k >> 6) & 0x3f;
            long ie = k >> 12;

            double t0h = t0[i0 * 2 + 1], t0l = t0[i0 * 2 + 0];
            double t1h = t1[i1 * 2 + 1], t1l = t1[i1 * 2 + 0];

            var (th, tl) = muldd(t0h, t0l, t1h, t1l);

            ReadOnlySpan<double> cd = [0.00016922538587889289, 5.6617353853682897e-21, 1.4318615612930102e-08, -5.6588239986784279e-25, 8.0769108411654573e-13, -4.611190128844e-29, 3.4170458845140945e-17, 1.0045988996358037e-33, 1.1565018170905508e-21, -8.7478972692918702e-38, 3.2618244377448612e-26, -1.8822354885763482e-42];

            var (fh, fl) = polydd(z, 6, cd);
            (fh, fl) = mulddd(z, fh, fl);

            if (ix <= 0xc08ff00000000000ul)
            {
                if (-8.0085662595372941e-17 <= x && x <= 1.6017132519074586e-16)
                {
                    return FusedMultiplyAdd(x, 0.5, 1.0);
                }
                else if ((k & 0xfff) == 0)
                {
                    double e;
                    (fh, e) = fastTwoSum(th, fh);
                    (fl, e) = fastTwoSum(e, fl);
                    ix = Polyfill.DoubleToUInt64Bits(fl);

                    if ((ix & (~0ul >> 12)) == 0)
                    {
                        if (((ix >> 52) & 0x7ff) != 0)
                        {
                            ulong v = Polyfill.DoubleToUInt64Bits(e);
                            ulong d1 = ((ulong)(((long)ix >> 63) ^ ((long)v >> 63)) << 1) + 1;
                            ix += d1;
                            fl = Polyfill.UInt64BitsToDouble(ix);
                        }
                    }
                }
                else
                {
                    (fh, fl) = muldd(fh, fl, th, tl);
                    (fh, fl) = fastSum(th, tl, fh, fl);
                }

                (fh, fl) = fastTwoSum(fh, fl);
                ix = Polyfill.DoubleToUInt64Bits(fl);
                ulong d = (ix + 2) & (~0ul >> 12);
                if (d <= 2)
                {
                    fh = asExp2Database(x, fh);
                }
                fh = asLdexp(fh, (int)ie);
            }
            else
            {
                ix = ((ulong)(1 - ie) << 52);
                (fh, fl) = muldd(fh, fl, th, tl);
                (fh, fl) = fastSum(th, tl, fh, fl);
                double e;
                (fh, e) = fastTwoSum(Polyfill.DoubleToUInt64Bits(ix), fh);
                fl += e;
                fh = asToDenormal(fh + fl);
            }

            return fh;
        }




        ReadOnlySpan<double> t0 = [0, 1, -1.523477860336858e-17, 1.0108892860517005, 5.1092250289734445e-17, 1.0218971486541166, 7.60083887402709e-18, 1.0330248790212284, 8.5518897055379637e-17, 1.0442737824274138, 1.7593257387720916e-18, 1.0556451783605572, -7.8998539668415821e-17, 1.0671404006768237, -6.6566604360565926e-17, 1.0787607977571199, -3.0467820798124711e-17, 1.0905077326652577, 5.2660368715706944e-17, 1.1023825833078409, 1.0410278456845571e-16, 1.1143867425958924, 5.1658567587954561e-17, 1.1265216186082418, 8.9128126760254065e-17, 1.1387886347566916, 3.2507102188638278e-17, 1.1511892299529827, 3.8292048369240941e-17, 1.1637248587775775, 5.5542032542180777e-17, 1.1763969916502812, 3.9820152314656461e-17, 1.189207115002721, 6.6449814992523012e-17, 1.2021567314527031, -7.7126306926814881e-17, 1.215247359980469, -1.8987816313025296e-17, 1.22848053610687, 4.6580275918369368e-17, 1.241857812073484, -6.7113898212968784e-18, 1.2553807570246911, 2.6679321313421861e-18, 1.2690509571917332, 1.713594918243561e-17, 1.2828700160787783, 2.5382502794888315e-17, 1.2968395546510096, -7.1815361355194551e-17, 1.3109612115247644, -2.8587312100388608e-17, 1.3252366431597413, 8.9272825948317308e-17, 1.3396675240533029, 7.7009483798029882e-17, 1.3542555469368927, 9.5937979191188488e-17, 1.3690024229745905, -6.7705116587947851e-17, 1.383909881963832, -9.6142132090513231e-17, 1.3989796725383112, -9.6672933134529135e-17, 1.4142135623730951, -1.2031642489053654e-17, 1.42961333839197, -3.0237581349939879e-17, 1.4451808069770467, -5.600377186075217e-17, 1.460917794180647, -3.4839945568927964e-17, 1.4768261459394993, 1.4192920154284036e-17, 1.4929077282912648, -1.016455327754295e-16, 1.5091644275934228, -1.1024941712342561e-16, 1.5255981507445384, 7.9498348096976196e-17, 1.5422108254079407, 3.7812070533575275e-17, 1.5590044002378369, -1.0136916471278304e-17, 1.5759808451078865, -1.0094406542311962e-16, 1.593142151342267, 2.4707192569797888e-17, 1.6104903319492543, -6.7129550847070829e-17, 1.6280274218573478, -1.0125679913674774e-16, 1.6457554781539649, 5.8909926967131009e-17, 1.6636765803267364, 8.1990100205814978e-17, 1.681792830507429, -8.0237193703977002e-18, 1.7001063537185235, -1.8513804182631107e-17, 1.7186192981224779, 3.1643892992929569e-17, 1.7373338352737062, 2.9601406954488739e-17, 1.7562521603732995, 6.4297317965565708e-17, 1.7753764925265212, 1.822745842791209e-17, 1.7947090750031072, -9.9695315389203488e-17, 1.8142521755003989, 3.2831072242456266e-17, 1.8340080864093424, 9.7618874907275935e-17, 1.8539791250833855, -6.1227634130041426e-17, 1.8741676341103, 3.4034035352165303e-17, 1.8945759815869656, -1.0619946056195964e-16, 1.9152065613971474, 1.0332385960676326e-16, 1.9360617934922943, 8.9607677910366665e-17, 1.9571441241754002, 4.0388753109278167e-17, 1.9784560263879509];
        ReadOnlySpan<double> t1 = [0, 1, 9.336185335478462e-17, 1.0001692397053021, -5.1413339313189571e-18, 1.0003385080526823, 6.9624240220205726e-17, 1.0005078050469876, -5.115123297685667e-17, 1.0006771306930664, 8.4229900245864878e-17, 1.0008464849957674, -2.8245220747761684e-17, 1.001015867959941, -7.1804245655921329e-17, 1.0011852795904375, -1.8973728416792996e-17, 1.0013547198921082, 9.0604410672691205e-17, 1.0015241888698057, -7.17327634990032e-17, 1.0016936865283832, -1.3307196246722662e-17, 1.0018632128726943, 2.5726925943221121e-17, 1.002032767907594, -3.9299377854845172e-17, 1.0022023516379379, 8.4613772479947175e-17, 1.0023719640685822, -4.1948832416399403e-17, 1.0025416052043845, -3.6366159286922646e-17, 1.0027112750502025, -2.610944063243938e-17, 1.0028809736108952, 1.7530784779823324e-17, 1.0030507008913223, 5.7539235256282674e-17, 1.0032204568963443, -8.6849220051179577e-18, 1.0033902416308227, 9.4900354309817764e-17, 1.0035600550996193, -8.7103806058184211e-17, 1.0037298973075977, 3.4958916958571545e-17, 1.0038997682596209, 9.753787549840241e-17, 1.0040696679605541, -1.0576221196292857e-16, 1.0042395964152628, 4.2091887381271259e-17, 1.0044095536286128, -1.6700166857554785e-17, 1.0045795396054717, -1.6231463554124514e-17, 1.0047495543507072, 2.3028539278028114e-17, 1.0049195978691881, 1.6418046976773032e-17, 1.0050896701657839, 3.7266984318284131e-17, 1.005259771245365, 9.499186535455033e-17, 1.0054299011128027, -8.6809313144445816e-17, 1.0056000597729693, 4.0005474910301175e-17, 1.005770247230737, 7.190499111509974e-17, 1.0059404634909801, -1.3908068671065786e-17, 1.006110708558573, -8.1402086425730496e-17, 1.0062809824383909, -5.7621510437495342e-17, 1.00645128513531, 6.745278477310458e-17, 1.0066216166542072, 1.8998557240346293e-17, 1.0067919769999607, -9.6374300323164059e-17, 1.0069623661774489, -1.2528654462453979e-17, 1.0071327841915512, 3.0205788878436942e-17, 1.0073032310471479, -4.8693942586085655e-17, 1.0074737067491204, 5.2240299376874538e-17, 1.0076442113023503, -9.3615435514784559e-17, 1.0078147447117207, -8.6525132330619496e-17, 1.007985306982115, -3.2520587560843081e-17, 1.0081558981184175, -9.9172322680609155e-17, 1.0083265181255139, -7.1360474041625215e-17, 1.0084971670082898, -1.7268683712243217e-17, 1.0086678447716324, -6.6199546936739413e-17, 1.0088385514204294, 3.5654569015130198e-17, 1.0090092869595693, 3.7173100137088179e-17, 1.0091800513939415, 7.0625724068255265e-17, 1.0093508447284363, -1.4321412303428819e-17, 1.0095216669679448, 1.566818801313411e-17, 1.0096925181173586, -1.1043695780393687e-16, 1.0098633981815708, -5.767317427160398e-17, 1.0100343071654745, 4.8354849784403835e-18, 1.0102052450739643, 7.0151212897154409e-17, 1.0103762119119353, 7.1618028736195726e-17, 1.0105472076842836, 1.0504659134084051e-16, 1.0107182323959061];


        ulong ix = Polyfill.DoubleToUInt64Bits(x);
        ulong ax = ix << 1;
        if (ax == 0)
        {
            return 1.0;
        }
        if (ax >= 0x8120000000000000ul)
        {
            if (ax > 0xffe0000000000000ul)
            {
                return x + x;
            }
            if (ax == 0xffe0000000000000ul)
            {
                return (ix >> 63) != 0 ? 0.0 : x;
            }
            if (ix >> 63 != 0)
            {
                if (ix >= 0xc090cc0000000000ul)
                {
                    double zz = 2.2250738585072014e-308;
                    return zz * zz;
                }
            }
            else
            {
                return 8.9884656743115795e+307 * x;
            }
        }

        if (ax <= 0x792e2a8eca5705fcul)
        {
            return 1.0 + CopySign(5.5511151231257827e-17, x);
        }


        ulong m = ix << 12, ex = (ax >> 53) - 0x3ff, frac = ex >> 63 | m << ((int)ex & 63);
        double sx = 4096.0 * x, fx = RoundEvenFinite(sx), z = sx - fx, z2 = z * z;
        long k = (long)fx;
        int i1 = (int)k & 0x3f, i0 = (int)(k >> 6) & 0x3f;
        long ie = k >> 12;

        double t0h = t0[i0 * 2 + 1], t0l = t0[i0 * 2 + 0];
        double t1h = t1[i1 * 2 + 1], t1l = t1[i1 * 2 + 0];

        var (th, tl) = muldd(t0h, t0l, t1h, t1l);

        ReadOnlySpan<double> c = [0.00016922538587889289, 1.4318615612930102e-08, 8.0769108447795267e-13, 3.4170458859405979e-17];
        double tz = th * z, fh = th, fl = tz * ((c[0] + z * c[1]) + z2 * (c[2] + z * c[3])) + tl;
        double eps = 1.64e-19;

        if (ix <= 0xc08ff00000000000ul)
        {
            if (frac != 0)
            {
                double ub = fh + (fl + eps);
                fh += (fl - eps);
                if (ub != fh)
                {
                    return asExp2Accurate(x, t0, t1);
                }
            }
            fh = asLdexp(fh, (int)ie);
        }
        else
        {
            ix = (ulong)(1 - ie) << 52;
            double e;
            (fh, e) = fastTwoSum(Polyfill.UInt64BitsToDouble(ix), fh);
            fl += e;
            if (frac != 0)
            {
                double ub = fh + (fl + eps);
                fh += (fl - eps);
                if (ub != fh)
                {
                    return asExp2Accurate(x, t0, t1);
                }
            }
            fh = asToDenormal(fh);
        }

        return fh;
    }
}
