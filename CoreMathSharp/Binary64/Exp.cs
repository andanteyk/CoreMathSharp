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
    public static double Exp(double x)
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
        static (double h, double l) opolydd(double xh, double xl, int n, ReadOnlySpan<double> c)
        {
            int i = n - 1;
            double ch = c[i * 2 + 0], cl = c[i * 2 + 1];

            while (--i >= 0)
            {
                (ch, cl) = muldd(xh, xl, ch, cl);
                double th = ch + c[i * 2 + 0], tl = (c[i * 2 + 0] - th) + ch;
                ch = th;
                cl += tl + c[i * 2 + 1];
            }

            return (ch, cl);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double asLdexp(double x, long i)
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

        static double asExpDatabase(double x, double f)
        {
            ReadOnlySpan<double> db = [2.2204460492503128e-16, 0.00010538831410138832, 0.0055282774790604916, 0.021146097432816237, 0.039418351986433779, 0.046453328449305387, 0.12953343691349475, 0.20331954196172794, 0.24555352187141782, 0.27540600958627748, 0.43424665650553418, 0.50069332895087848, 4.6130429605534751, 6.0598594832526835, 16.559833721798679, 17.835004503812709, 28.268769111813619, 60.061740396981683, 62.891319835656368, 170.40063412375622, 470.27976575414419, -8.5420057850740927e-05, -0.00019979289929051341, -0.00044083367612578553, -0.00044154821825474419, -0.00058684343543334409, -0.00089945428777803344, -0.005854738373659552, -0.0087883141399313046, -0.010385646752244758, -0.02126284029696137, -0.024190663662552927, -0.025640606111311446, -0.1037516737020303, -0.10870837302127832, -0.11595781086814885, -0.14504743082833096, -0.19283178027363737, -0.22849173530190173, -0.23930006692547043, -0.56813286662045848, -0.75981557462171057, -0.87066332589370665, -1.1664532184587633, -1.797728508222747, -2.0173718153791254, -2.7892491442501601, -15.273162747858944, -24.968964037398379, -86.48458873295607, -204.04205075712659];

            ulong ix = Polyfill.DoubleToUInt64Bits(x);
            int a = 0, b = db.Length - 1, m = (a + b) / 2;
            ReadOnlySpan<ulong> c = MemoryMarshal.Cast<double, ulong>(db);

            while (a <= b)
            {
                if (c[m] < ix)
                {
                    a = m + 1;
                }
                else if (c[m] == ix)
                {
                    ReadOnlySpan<ulong> s2 = [0x57f5fe2e5bde4075ul, 0x3c1f16b8edul];
                    const ulong s = 333811522313371;

                    ulong jf = Polyfill.DoubleToUInt64Bits(f), dr = ((s >> m) << 63) | 0x3c90000000000000ul;
                    ulong t = (s2[m >> 5] >> ((m << 1) & 63)) & 3;

                    for (int k = -1; k <= 1; k++)
                    {
                        ulong r = jf + (ulong)k;
                        if ((r & 3) == t)
                        {
                            return Polyfill.UInt64BitsToDouble(r) + Polyfill.UInt64BitsToDouble(dr);
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




        static double asExpAccurate(double x, ReadOnlySpan<double> t0, ReadOnlySpan<double> t1)
        {
            ReadOnlySpan<double> ch = [1, 0, 0.5, 2.2752803697148587e-30, 0.16666666666666666, 9.251858538539695e-18, 0.041666666666666664, 2.3113756024311491e-18, 0.0083333333333333332, 1.1645814375470794e-19, 0.0013888888892440137, -7.0227927062705387e-20, 0.0001984126983733902, -1.0602991841308864e-20];


            ulong ix = Polyfill.DoubleToUInt64Bits(x);
            if (((ix >> 52) & 0x7ff) < 0x3c9)
            {
                return 1.0 + x;
            }

            const double s = 5909.278887481194;
            double t = RoundEvenFinite(x * s);
            long jt = (long)t;
            int i0 = (int)(jt >> 6) & 0x3f, i1 = (int)jt & 0x3f;
            long ie = jt >> 12;

            double t0h = t0[i0 * 2 + 1], t0l = t0[i0 * 2 + 0];
            double t1h = t1[i1 * 2 + 1], t1l = t1[i1 * 2 + 0];
            var (th, tl) = muldd(t0h, t0l, t1h, t1l);

            const double l2h = 0.00016922538588914904, l2l = 1.0256140314162804e-14, l2ll = 3.2042720746546034e-31;
            double dx = x - l2h * t, dxl = l2l * t, dxll = l2ll * t + FusedMultiplyAdd(l2l, t, -dxl);
            double dxh = dx + dxl;
            dxl = (dx - dxh) + dxl + dxll;

            var (fh, fl) = opolydd(dxh, dxl, 7, ch);
            (fh, fl) = muldd(dxh, dxl, fh, fl);

            if (ix > 0xc086232bdd7abcd2ul)
            {
                ix = (ulong)(1 - ie) << 52;
                (fh, fl) = muldd(fh, fl, th, tl);
                (fh, fl) = fastSum(th, tl, fh, fl);

                double e;
                (fh, e) = fastTwoSum(Polyfill.UInt64BitsToDouble(ix), fh);
                fl += e;
                fh = asToDenormal(fh + fl);
            }
            else
            {
                if (th == 1.0)
                {
                    double e;
                    (fh, e) = fastTwoSum(th, fh);
                    (fl, e) = fastTwoSum(e, fl);
                    ix = Polyfill.DoubleToUInt64Bits(fl);

                    if (((ix & ~0ul) >> 12) == 0)
                    {
                        ulong v = Polyfill.DoubleToUInt64Bits(e);
                        ulong d1 = ((ulong)(((long)ix >> 63) ^ ((long)v >> 63)) << 1) + 1;
                        ix += d1;
                        fl = Polyfill.UInt64BitsToDouble(ix);
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
                    fh = asExpDatabase(x, fh);
                }
                fh = asLdexp(fh, ie);
            }

            return fh;
        }




        ReadOnlySpan<double> t0 = [0, 1, -1.523477860336858e-17, 1.0108892860517005, 5.1092250289734445e-17, 1.0218971486541166, 7.60083887402709e-18, 1.0330248790212284, 8.5518897055379637e-17, 1.0442737824274138, 1.7593257387720916e-18, 1.0556451783605572, -7.8998539668415821e-17, 1.0671404006768237, -6.6566604360565926e-17, 1.0787607977571199, -3.0467820798124711e-17, 1.0905077326652577, 5.2660368715706944e-17, 1.1023825833078409, 1.0410278456845571e-16, 1.1143867425958924, 5.1658567587954561e-17, 1.1265216186082418, 8.9128126760254065e-17, 1.1387886347566916, 3.2507102188638278e-17, 1.1511892299529827, 3.8292048369240941e-17, 1.1637248587775775, 5.5542032542180777e-17, 1.1763969916502812, 3.9820152314656461e-17, 1.189207115002721, 6.6449814992523012e-17, 1.2021567314527031, -7.7126306926814881e-17, 1.215247359980469, -1.8987816313025296e-17, 1.22848053610687, 4.6580275918369368e-17, 1.241857812073484, -6.7113898212968784e-18, 1.2553807570246911, 2.6679321313421861e-18, 1.2690509571917332, 1.713594918243561e-17, 1.2828700160787783, 2.5382502794888315e-17, 1.2968395546510096, -7.1815361355194551e-17, 1.3109612115247644, -2.8587312100388608e-17, 1.3252366431597413, 8.9272825948317308e-17, 1.3396675240533029, 7.7009483798029882e-17, 1.3542555469368927, 9.5937979191188488e-17, 1.3690024229745905, -6.7705116587947851e-17, 1.383909881963832, -9.6142132090513231e-17, 1.3989796725383112, -9.6672933134529135e-17, 1.4142135623730951, -1.2031642489053654e-17, 1.42961333839197, -3.0237581349939879e-17, 1.4451808069770467, -5.600377186075217e-17, 1.460917794180647, -3.4839945568927964e-17, 1.4768261459394993, 1.4192920154284036e-17, 1.4929077282912648, -1.016455327754295e-16, 1.5091644275934228, -1.1024941712342561e-16, 1.5255981507445384, 7.9498348096976196e-17, 1.5422108254079407, 3.7812070533575275e-17, 1.5590044002378369, -1.0136916471278304e-17, 1.5759808451078865, -1.0094406542311962e-16, 1.593142151342267, 2.4707192569797888e-17, 1.6104903319492543, -6.7129550847070829e-17, 1.6280274218573478, -1.0125679913674774e-16, 1.6457554781539649, 5.8909926967131009e-17, 1.6636765803267364, 8.1990100205814978e-17, 1.681792830507429, -8.0237193703977002e-18, 1.7001063537185235, -1.8513804182631107e-17, 1.7186192981224779, 3.1643892992929569e-17, 1.7373338352737062, 2.9601406954488739e-17, 1.7562521603732995, 6.4297317965565708e-17, 1.7753764925265212, 1.822745842791209e-17, 1.7947090750031072, -9.9695315389203488e-17, 1.8142521755003989, 3.2831072242456266e-17, 1.8340080864093424, 9.7618874907275935e-17, 1.8539791250833855, -6.1227634130041426e-17, 1.8741676341103, 3.4034035352165303e-17, 1.8945759815869656, -1.0619946056195964e-16, 1.9152065613971474, 1.0332385960676326e-16, 1.9360617934922943, 8.9607677910366665e-17, 1.9571441241754002, 4.0388753109278167e-17, 1.9784560263879509];
        ReadOnlySpan<double> t1 = [0, 1, 9.336185335478462e-17, 1.0001692397053021, -5.1413339313189571e-18, 1.0003385080526823, 6.9624240220205726e-17, 1.0005078050469876, -5.115123297685667e-17, 1.0006771306930664, 8.4229900245864878e-17, 1.0008464849957674, -2.8245220747761684e-17, 1.001015867959941, -7.1804245655921329e-17, 1.0011852795904375, -1.8973728416792996e-17, 1.0013547198921082, 9.0604410672691205e-17, 1.0015241888698057, -7.17327634990032e-17, 1.0016936865283832, -1.3307196246722662e-17, 1.0018632128726943, 2.5726925943221121e-17, 1.002032767907594, -3.9299377854845172e-17, 1.0022023516379379, 8.4613772479947175e-17, 1.0023719640685822, -4.1948832416399403e-17, 1.0025416052043845, -3.6366159286922646e-17, 1.0027112750502025, -2.610944063243938e-17, 1.0028809736108952, 1.7530784779823324e-17, 1.0030507008913223, 5.7539235256282674e-17, 1.0032204568963443, -8.6849220051179577e-18, 1.0033902416308227, 9.4900354309817764e-17, 1.0035600550996193, -8.7103806058184211e-17, 1.0037298973075977, 3.4958916958571545e-17, 1.0038997682596209, 9.753787549840241e-17, 1.0040696679605541, -1.0576221196292857e-16, 1.0042395964152628, 4.2091887381271259e-17, 1.0044095536286128, -1.6700166857554785e-17, 1.0045795396054717, -1.6231463554124514e-17, 1.0047495543507072, 2.3028539278028114e-17, 1.0049195978691881, 1.6418046976773032e-17, 1.0050896701657839, 3.7266984318284131e-17, 1.005259771245365, 9.499186535455033e-17, 1.0054299011128027, -8.6809313144445816e-17, 1.0056000597729693, 4.0005474910301175e-17, 1.005770247230737, 7.190499111509974e-17, 1.0059404634909801, -1.3908068671065786e-17, 1.006110708558573, -8.1402086425730496e-17, 1.0062809824383909, -5.7621510437495342e-17, 1.00645128513531, 6.745278477310458e-17, 1.0066216166542072, 1.8998557240346293e-17, 1.0067919769999607, -9.6374300323164059e-17, 1.0069623661774489, -1.2528654462453979e-17, 1.0071327841915512, 3.0205788878436942e-17, 1.0073032310471479, -4.8693942586085655e-17, 1.0074737067491204, 5.2240299376874538e-17, 1.0076442113023503, -9.3615435514784559e-17, 1.0078147447117207, -8.6525132330619496e-17, 1.007985306982115, -3.2520587560843081e-17, 1.0081558981184175, -9.9172322680609155e-17, 1.0083265181255139, -7.1360474041625215e-17, 1.0084971670082898, -1.7268683712243217e-17, 1.0086678447716324, -6.6199546936739413e-17, 1.0088385514204294, 3.5654569015130198e-17, 1.0090092869595693, 3.7173100137088179e-17, 1.0091800513939415, 7.0625724068255265e-17, 1.0093508447284363, -1.4321412303428819e-17, 1.0095216669679448, 1.566818801313411e-17, 1.0096925181173586, -1.1043695780393687e-16, 1.0098633981815708, -5.767317427160398e-17, 1.0100343071654745, 4.8354849784403835e-18, 1.0102052450739643, 7.0151212897154409e-17, 1.0103762119119353, 7.1618028736195726e-17, 1.0105472076842836, 1.0504659134084051e-16, 1.0107182323959061];


        ulong ix = Polyfill.DoubleToUInt64Bits(x);
        ulong aix = ix & (~0ul >> 1);

        if (aix <= 0x3c90000000000000ul)
        {
            return 1.0 + x;
        }
        if (aix >= 0x40862e42fefa39f0ul)
        {
            if (aix > 0x7ff0000000000000ul)
            {
                return x + x;
            }
            if (aix == 0x7ff0000000000000ul)
            {
                if (ix >> 63 != 0)
                {
                    return 0.0;
                }
                else
                {
                    return x;
                }
            }
            if ((ix >> 63) == 0)
            {
                double z = 8.9884656743115795e+307;
                return z * z;
            }
            if (aix >= 0x40874910d52d3052ul)
            {
                return 3.3376107877608021e-308 * 2.7755575615628914e-17;
            }
        }

        const double s = 5909.278887481194;
        double t = RoundEvenFinite(x * s);
        long jt = (long)t;
        int i0 = (int)(jt >> 6) & 0x3f, i1 = (int)jt & 0x3f;
        long ie = jt >> 12;

        double t0h = t0[i0 * 2 + 1], t0l = t0[i0 * 2 + 0];
        double t1h = t1[i1 * 2 + 1], t1l = t1[i1 * 2 + 0];

        var (th, tl) = muldd(t0h, t0l, t1h, t1l);

        const double l2h = 0.00016922538588914904, l2l = 1.0256140314162804e-14;

        double dx = (x - l2h * t) + l2l * t, dx2 = dx * dx;

        ReadOnlySpan<double> ch = [1, 0.5, 0.16666666674124284, 0.041666666654270573];
        double p = (ch[0] + dx * ch[1]) + dx2 * (ch[2] + dx * ch[3]);
        double fh = th, tx = th * dx, fl = tl + tx * p;

        double eps = 1.64e-19;
        if (ix > 0xc086232bdd7abcd2ul)
        {
            ix = (ulong)(1 - ie) << 52;
            double e;
            (fh, e) = fastTwoSum(Polyfill.UInt64BitsToDouble(ix), fh);
            fl += e;
            double ub = fh + (fl + eps), lb = fh + (fl - eps);
            if (ub != lb)
            {
                return asExpAccurate(x, t0, t1);
            }
            fh = asToDenormal(lb);
        }
        else
        {
            double ub = fh + (fl + eps), lb = fh + (fl - eps);
            if (ub != lb)
            {
                return asExpAccurate(x, t0, t1);
            }
            fh = asLdexp(lb, ie);
        }

        return fh;
    }
}
