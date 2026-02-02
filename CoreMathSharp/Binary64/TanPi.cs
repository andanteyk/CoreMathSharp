using System;
using System.Runtime.CompilerServices;

namespace CoreMathSharp;

public static partial class StrictMath
{
    public static double TanPi(double x)
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double s, double e) fastTwoSum(double x, double y)
        {
            double s = x + y, z = s - x;
            return (s, y - z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double s, double e) fastTwoSub(double x, double y)
        {
            double s = x - y, z = x - s;
            return (s, z - y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double s, double l) mulddAcc(double xh, double xl, double ch, double cl)
        {
            double ahlh = ch * xl, alhh = cl * xh, ahhh = ch * xh, ahhl = FusedMultiplyAdd(ch, xh, -ahhh);
            ahhl += alhh + ahlh;
            return fastTwoSum(ahhh, ahhl);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double hh, double l) mulddd(double xh, double xl, double ch)
        {
            double ahlh = ch * xl, ahhh = ch * xh, ahhl = FusedMultiplyAdd(ch, xh, -ahhh);
            ahhl += ahlh;
            ch = ahhh + ahhl;
            return (ch, (ahhh - ch) + ahhl);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double h, double l) fastsumddd(double xh, double xl, double ch)
        {
            var (th, tl) = fastTwoSum(xh, ch);
            return (th, xl + tl);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double polydd(double xh, double xl, int n, ReadOnlySpan<double> c, ref double l)
        {
            int i = n - 1;
            double ch, cl;
            (ch, cl) = fastTwoSum(c[i * 2 + 0], l);
            cl += c[i * 2 + 1];

            while (--i >= 0)
            {
                (ch, cl) = mulddAcc(xh, xl, ch, cl);
                double th, tl;
                (th, tl) = fastTwoSum(c[i * 2 + 0], ch);
                ch = th;
                cl += tl + c[i * 2 + 1];
            }

            l = cl;
            return ch;
        }


        static double asTanPiDatabase(double x, double f)
        {
            ReadOnlySpan<double> db = [4.7300152614670414e-17, 1.4859781196792462e-16, 6.1629758220391547e-33, 7.0530197093213519e-17, 2.2157714904427979e-16, -6.1629758220391547e-33, 9.2920177914172418e-17, 2.9191734830542063e-16, 1.2325951644078309e-32, 9.4600305229340828e-17, 2.9719562393584925e-16, 1.2325951644078309e-32, 3.0447561904995328e-16, 9.5653836800453774e-16, 4.9303806576313238e-32, 5.1545969980349888e-15, 1.6193644061242723e-14, -7.8886090522101181e-31, 6.3248045871211923e-15, 1.9869959626290963e-14, 7.8886090522101181e-31, 5.3227224240105025e-14, 1.6721825664369051e-13, 6.3108872417680944e-30, 6.5984838363181829e-14, 2.0729748345008199e-13, -6.3108872417680944e-30, 7.2308766664812431e-14, 2.2716469014431327e-13, 6.3108872417680944e-30, 2.1523112061720291e-13, 6.7616850735490335e-13, -2.5243548967072378e-29, 3.683189778661054e-13, 1.1571081950418585e-12, 5.0487097934144756e-29, 5.0631441043358743e-13, 1.5906336322248056e-12, -5.0487097934144756e-29, 3.720782895071451e-12, 1.1689184208759033e-11, 4.0389678347315804e-28, 2.6600735696506734e-11, 8.3568675844229328e-11, 3.2311742677852644e-27, 9.4164182991307565e-11, 2.9582550551677683e-10, 1.2924697071141057e-26, 2.8415272826066846e-10, 8.9269212360121287e-10, 2.5849394142282115e-26, 1.2298183289334222e-09, 3.8635882274273149e-09, -2.0679515313825692e-25, 1.9638891552115813e-07, 6.1697397424781517e-07, 2.6469779601696886e-23, 2.2980997412817642e-07, 7.2196932644286487e-07, 2.6469779601696886e-23, 3.2361236028460906e-07, 1.0166582136813314e-06, 5.2939559203393771e-23, 7.9241726834681773e-07, 2.4894522688211969e-06, 1.0587911840678754e-22, 5.6524469972948246e-06, 1.7757685963373651e-05, 8.4703294725430034e-22, 2.4971583686115285e-05, 7.8450544017743474e-05, 3.3881317890172014e-21, 5.8897270460573793e-05, 0.00018503123430704056, -6.7762635780344027e-21, 5.8918876140153057e-05, 0.0001850991105536073, 6.7762635780344027e-21, 0.00013550697375431335, 0.00042570773897332296, 1.3552527156068805e-20, 0.00017833292991614478, 0.00056024948113463167, -2.7105054312137611e-20, 0.00040192521437566866, 0.0012626859718398071, -5.4210108624275222e-20, 0.00080340841282529687, 0.0025239873272409111, 1.0842021724855044e-19, 0.00090686029414165678, 0.0028489933460715097, 1.0842021724855044e-19, 0.0013333836111766196, 0.0041889726590195195, -2.1684043449710089e-19, 0.010396726272931965, 0.032673898811717457, -1.7347234759768071e-18, 0.0118869508517391, 0.037361326760578371, 1.7347234759768071e-18, 0.016905797689754469, 0.053161124721393831, 1.7347234759768071e-18, 0.037184182685530212, 0.11735184921280624, 3.4694469519536142e-18, 0.054126302949554457, 0.17170087922733931, 6.9388939039072284e-18, 0.069878512941711768, 0.22312578477570091, -6.9388939039072284e-18, 0.099667118697533266, 0.32376390542419831, 1.3877787807814457e-17, 0.10234483371468589, 0.33308360018972633, 1.3877787807814457e-17, 0.1023476186079835, 0.33309331987345858, 1.3877787807814457e-17, 0.10973245167988503, 0.35907296645266135, 1.3877787807814457e-17, 0.22378711739814902, 0.84751214860472646, 2.7755575615628914e-17, 0.23400189847406203, 0.90421443429126913, -2.7755575615628914e-17, 0.24478471169419852, 0.96775677494870571, 2.7755575615628914e-17, 0.38633689702695989, 2.680416060023711, 1.1102230246251565e-16, 0.42801245584275949, 4.3460921085500397, -2.2204460492503131e-16, 0.4664196791119597, 9.443868939328663, 4.4408920985006262e-16, 0.48292704243799284, 18.626215356684039, 8.8817841970012523e-16];

            ulong ix = Polyfill.DoubleToUInt64Bits(x);
            ulong aix = ix & (~0ul >> 1);

            ulong p = 0;
            double ax = Abs(x);
            int e = (int)(aix >> 52) - 1022;

            if (e >= 0)
            {
                ulong t = aix & ~(~0ul >> (12 + e));
                ax -= Polyfill.UInt64BitsToDouble(t);
                p = ((aix | 1ul << 52) >> (52 - e)) & 1;
                if (p != 0)
                {
                    ax = 0.5 - ax;
                }
            }

            double sgn = 1.0;
            if ((p ^ ix >> 63) != 0)
            {
                sgn = -1.0;
            }

            if (e < -54)
            {
                ulong a = Polyfill.DoubleToUInt64Bits((1023L - (54 + e)) << 52);
                ulong ia = Polyfill.DoubleToUInt64Bits((1023L + (54 + e)) << 52);
                ax *= Polyfill.UInt64BitsToDouble(a);
                sgn *= Polyfill.UInt64BitsToDouble(ia);
            }

            {
                int a = 0, b = db.Length / 3 - 1, m = (a + b) / 2;
                while (a <= b)
                {
                    if (db[m * 3 + 0] < ax)
                    {
                        a = m + 1;
                    }
                    else if (db[m * 3 + 0] == ax)
                    {
                        f = sgn * db[m * 3 + 1] + sgn * db[m * 3 + 2];
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
        }



        ReadOnlySpan<double> T = [0, 0, 9.0977656555289437e-20, 0.049126849769467254, 5.3100671162822435e-18, 0.098491403357164248, 4.0790646818000011e-18, 0.14833598753834742, 8.3917944776365378e-19, 0.19891236737965801, 9.3899343814740963e-18, 0.25048696019130545, -1.2766990847826405e-17, 0.3033466836073424, 1.0216199149473033e-17, 0.35780572131452409, 1.4349369327986523e-17, 0.41421356237309503, 1.7418847992047869e-17, 0.47296477589131991, -5.2626469389262167e-17, 0.53451113595079169, -2.9566641441253576e-17, 0.5993769336819238, 4.1042270233610004e-17, 0.66817863791929888, -4.6236658094925058e-17, 0.74165054627203542, -2.2923147594675738e-17, 0.82067879082866035, 2.1564367263640407e-17, 0.90634716901914714, 0, 1, 8.7695652727462724e-17, 1.1033299757334756, -2.1244788699148428e-17, 1.2185035255879764, 3.9251662671937787e-17, 1.3483439134867201, 6.9741008889583049e-17, 1.4966057626654889, -2.3797277757262252e-17, 1.6683992055835071, 2.9458071077957847e-17, 1.8708684117893895, 8.2594855265212044e-17, 2.1143223575486405, 1.2537167179050217e-16, 2.4142135623730949, 2.3237772982434003e-17, 2.7948127724904768, -9.5329577993691603e-17, 3.2965582089383205, -4.2716225925125868e-17, 3.9922237837700845, 2.9537918103736698e-17, 5.0273394921258481, -1.8658009130558321e-16, 6.7414524054149885, 5.362306887894472e-16, 10.15317038760886, -2.3792881581892444e-17, 20.355467624987188];
        ReadOnlySpan<double> c = [6.2810069495799273e-63, 1.7790559956752839e-104, 5.0990532156026337e-146, 1.4640479742404237e-187];
        const double ph = 2.6610324844426207e-21, pl = 1.0373161876273278e-37;

        double th, tl, res;

        ulong ix = Polyfill.DoubleToUInt64Bits(x);
        ulong ax = ix & (~0ul >> 1);

        if (ax >= 0x3f3ul << 52)
        {
            if (ax >= 0x42dul << 52)
            {
                if (ax >= 0x7fful << 52)
                {
                    if (ax > 0x7fful << 52)
                    {
                        return x + x;
                    }

                    return 0.0 / 0.0;
                }

                int e = (int)(ax >> 52), s = e - 1069;
                if (s > 6)
                {
                    return CopySign(0.0, x);
                }

                long m = (long)ax, sgn = (long)ix >> 63;
                int iq = (int)(((m ^ sgn) - sgn) << s) & 127;
                if ((iq & 31) == 0)
                {
                    long jq = (long)(iq >> 5);
                    if ((jq & 1) != 0)
                    {
                        if ((jq & 2) != 0)
                        {
                            return -1.0 / 0.0;
                        }
                        else
                        {
                            return 1.0 / 0.0;
                        }
                    }
                    else
                    {
                        if (((jq ^ sgn) & 2) != 0)
                        {
                            return -0.0;
                        }
                        else
                        {
                            return 0.0;
                        }
                    }
                }
                else
                {
                    double nl, nh;
                    if ((iq & 32) != 0)
                    {
                        nl = -T[(32 - (iq & 31)) * 2 + 0];
                        nh = -T[(32 - (iq & 31)) * 2 + 1];
                    }
                    else
                    {
                        nl = T[(iq & 31) * 2 + 0];
                        nh = T[(iq & 31) * 2 + 1];
                    }

                    return nh + nl;
                }
            }


            {
                int e = (int)(ax >> 52), s = 1068 - e, s1 = e - 1011;
                long m = (long)(ax & (~0ul >> 12)) | (1L << 52);
                long ms = (m << s1) >> 63;
                long sgn = (long)ix >> 63;

                int iq = (int)((m ^ ms) >> s) & 63;
                iq = (iq + 1) >> 1;
                ms ^= sgn;

                long sm = (m ^ sgn) - sgn;
                long k = sm << (e - 1005);

                double z = k;
                if (k << 1 == 0)
                {
                    if (k == 0)
                    {
                        if ((iq & 31) == 0)
                        {
                            long jq = sm >> (s + 6);
                            if ((jq & 1) != 0)
                            {
                                if ((jq & 2) != 0)
                                {
                                    return -1.0 / 0.0;
                                }
                                else
                                {
                                    return 1.0 / 0.0;
                                }
                            }
                            else
                            {
                                if (((jq ^ sgn) & 2) != 0)
                                {
                                    return -0.0;
                                }
                                else
                                {
                                    return 0.0;
                                }
                            }
                        }

                        ulong kq = ((ulong)m << s1) >> 58;
                        if (kq == 0x10)
                        {
                            return CopySign(1.0, x);
                        }
                        if (kq == 0x30)
                        {
                            return -CopySign(1.0, x);
                        }
                    }

                    z = CopySign(1.0, x) * z;
                }

                double z2 = z * z, z4 = z2 * z2, z3 = z * z2;
                double f = z3 * ((c[0] + z2 * c[1]) + z4 * (c[2] + z2 * c[3]));
                double eps = z3 * 8.6361685550944446e-78 + CopySign(4.9303806576313238e-32, z);

                (th, tl) = mulddd(ph, pl, z);
                (th, tl) = fastsumddd(th, tl, f);

                if (iq == 32)
                {
                    double ith = -1.0 / th;
                    tl = (FusedMultiplyAdd(ith, th, 1.0) + tl * ith) * ith;
                    th = ith;
                }
                else
                {
                    double nl = T[iq * 2 + 0], nh = T[iq * 2 + 1];

                    ReadOnlySpan<double> s2 = [-1.0, 1.0];

                    nh *= s2[(int)(ms + 1)];
                    nl *= s2[(int)(ms + 1)];

                    var (mh, ml) = mulddAcc(th, tl, nh, nl);
                    (mh, double dm) = fastTwoSub(1.0, mh);
                    ml = dm - ml;
                    (nh, double dn) = fastTwoSum(nh, th);
                    nl += dn + tl;

                    double imh = 1.0 / mh;
                    th = nh * imh;
                    tl = FusedMultiplyAdd(nh, imh, -th) + (nl + nh * (FusedMultiplyAdd(-mh, imh, 1.0) - ml * imh)) * imh;
                }

                eps += eps * (th * th);
                double lb = th + (tl - eps), ub = th + (tl + eps);
                if (lb == ub)
                {
                    return lb;
                }

                z *= 1.0842021724855044e-19;


                ReadOnlySpan<double> ch = [0.024543692606170259, 9.5675531183386969e-19, 4.9283149528979971e-06, -2.1615622354208456e-22, 1.1875126696551896e-09, -3.6931646863639206e-26, 2.8954607951315737e-13, 5.3513261844359426e-30, 7.0680197387788849e-17, 1.2048483746230456e-33];
                ReadOnlySpan<double> cl = [1.7255646049060546e-20, 4.2127955142022481e-24, 1.0295197365311714e-27];

                z2 = z * z;
                double dz2 = FusedMultiplyAdd(z, z, -z2);

                tl = z2 * (cl[0] + z2 * (cl[1] + z2 * cl[2]));
                th = polydd(z2, dz2, 5, ch, ref tl);
                (th, tl) = mulddd(th, tl, z);

                if (iq == 32)
                {
                    double ith = -1.0 / th;
                    tl = (FusedMultiplyAdd(ith, th, 1.0) + tl * ith) * ith;
                    th = ith;
                }
                else
                {
                    double nl = T[iq * 2 + 0], nh = T[iq * 2 + 1];

                    ReadOnlySpan<double> s2 = [-1.0, 1.0];

                    nh *= s2[(int)(ms + 1)];
                    nl *= s2[(int)(ms + 1)];

                    var (mh, ml) = mulddAcc(th, tl, nh, nl);
                    (mh, double dm) = fastTwoSub(1.0, mh);
                    ml = dm - ml;
                    (nh, double dn) = fastTwoSum(nh, th);
                    nl += dn + tl;

                    double imh = 1.0 / mh;
                    th = nh * imh;
                    tl = FusedMultiplyAdd(nh, imh, -th) + (nl + nh * (FusedMultiplyAdd(-mh, imh, 1.0) - ml * imh)) * imh;
                }

                (th, tl) = fastTwoSum(th, tl);
                res = th;
            }
        }
        else
        {
            if (ax == 0)
            {
                return x;
            }

            const double pi0 = 3.1415926535897931, pi1 = 1.2246467991473532e-16;

            if (ax < 0x3caul << 52)
            {
                if (ax < 0x36ul << 52)
                {
                    int e = (int)(ax >> 52);
                    ulong sc = (ulong)((2045L - e) << 52);
                    ulong isc = (ulong)((1L + e) << 52);

                    double z = x * Polyfill.UInt64BitsToDouble(sc);
                    (th, tl) = mulddd(pi0, pi1, z);
                    res = th * Polyfill.UInt64BitsToDouble(isc);

                    if (Abs(res) < 2.2250738585072014e-308)
                    {
                        double o = CopySign(2.2250738585072014e-308, x);
                        double v0b = (o + res) * Polyfill.UInt64BitsToDouble(sc), v0h = res * Polyfill.UInt64BitsToDouble(sc);
                        tl += th - v0h;
                        v0b += tl;
                        return v0b * Polyfill.UInt64BitsToDouble(isc) - o;
                    }
                }
                else
                {
                    (th, tl) = mulddd(pi0, pi1, x);
                    res = th;
                }
            }
            else
            {
                ReadOnlySpan<double> c2 = [10.335425560099941, 40.802624638036221, 163.00001026054639];
                double x2 = x * x, x3 = x * x2;
                double f = x3 * (c2[0] + x2 * (c2[1] + x2 * c2[2]));
                var (px0, px1) = mulddd(pi0, pi1, x);
                (th, tl) = fastsumddd(px0, px1, f);

                double eps = x * (x2 * 7.5495165674510645e-15 + 3.944304526105059e-31);
                double lb = th + (tl - eps), ub = th + (tl + eps);
                if (lb == ub)
                {
                    return lb;
                }

                ReadOnlySpan<double> ch = [10.335425560099941, -4.5331245651578895e-16, 40.802624638037528, -1.2689610152651054e-15, 162.99995197525544, 2.9213955720341702e-15];
                ReadOnlySpan<double> cl = [651.9097561458326, 2607.6010593946085];

                double dx2 = FusedMultiplyAdd(x, x, -x2), dx3 = FusedMultiplyAdd(x2, x, -x3) + dx2 * x;
                tl = x2 * (cl[0] + x2 * cl[1]);
                th = polydd(x2, dx2, 3, ch, ref tl);
                (th, tl) = mulddAcc(x3, dx3, th, tl);
                (th, double dv) = fastTwoSum(px0, th);
                tl = tl + px1 + dv;
                (th, tl) = fastTwoSum(th, tl);
                res = th;
            }
        }

        ulong ul = Polyfill.DoubleToUInt64Bits(tl), uh = Polyfill.DoubleToUInt64Bits(th);
        ulong er = (ul + 6) & (~0ul >> 12);
        ulong de = ((uh - ul) >> 52) & 0x7fful;
        if (er <= 12 || de > 102)
        {
            return asTanPiDatabase(x, res);
        }

        return res;
    }
}
