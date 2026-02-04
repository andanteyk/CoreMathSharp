using System;
using System.Runtime.CompilerServices;

namespace CoreMathSharp;

public static partial class StrictMath
{
    /// <summary>
    /// Computes the arc-sine of a value and divides the result by <see cref="Math.PI"/>.
    /// </summary>
    /// <returns>[-0.5, 0.5]</returns>
    /// <remarks>
    /// Mathematically, returns asin(x) / PI.
    /// </remarks>
    public static double AsinPi(double x)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (ulong lo, ulong hi) shl(ulong lo, ulong hi, int n)
        {
            ulong rlo, rhi;
            if (n < 64)
            {
                rlo = lo << n;
                rhi = hi << n | lo >> -n;
                return (rlo, rhi);
            }
            if (n < 128)
            {
                rlo = 0;
                rhi = lo << n;
                return (rlo, rhi);
            }
            return (0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (ulong lo, ulong hi) shr(ulong lo, ulong hi, int n)
        {
            ulong rlo, rhi;
            if (n < 64)
            {
                rhi = hi >> n;
                rlo = lo >> n | hi << -n;
                return (rlo, rhi);
            }
            if (n < 128)
            {
                rhi = 0;
                rlo = hi >> n;
                return (rlo, rhi);
            }
            return (0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ulong muuh(ulong a, ulong b)
        {
            return Polyfill.BigMul(a, b, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long mh(long a, long b)
        {
            return Polyfill.BigMul(a, b, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (long lo, long hi) imul(long a, long b)
        {
            long hi = Polyfill.BigMul(a, b, out long lo);
            return (lo, hi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (ulong lo, ulong hi) mUU(ulong alo, ulong ahi, ulong blo, ulong bhi)
        {
            ulong ohi = Polyfill.BigMul(ahi, bhi, out ulong olo);

            ulong olo2;

            olo2 = olo + Polyfill.BigMul(alo, bhi, out _);
            if (olo2 < olo)
            {
                ohi++;
            }
            olo = olo2;

            olo2 = olo + Polyfill.BigMul(ahi, blo, out _);
            if (olo2 < olo)
            {
                ohi++;
            }
            olo = olo2;

            return (olo, ohi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (ulong lo, ulong hi) muU(ulong a, ulong blo, ulong bhi)
        {
            var (olo, ohi) = mul128(a, bhi);
            (olo, ohi) = add128(olo, ohi, Polyfill.BigMul(a, blo, out _), 0);
            return (olo, ohi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (ulong lo, ulong hi) sqrU(ulong lo, ulong hi)
        {
            ulong oshi = Polyfill.BigMul(lo, hi, out ulong oslo);
            oslo = oslo >> 63 | oshi << 1;
            oshi >>= 63;

            ulong ohi = Polyfill.BigMul(hi, hi, out ulong olo);

            olo += oslo;
            ulong carry = olo < oslo ? 1ul : 0ul;
            ohi += oshi + carry;

            return (olo, ohi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (ulong lo, ulong hi) add128(ulong alo, ulong ahi, ulong blo, ulong bhi)
        {
            ulong lo = alo + blo;
            ulong carry = lo < alo ? 1ul : 0ul;
            ulong hi = ahi + bhi + carry;
            return (lo, hi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (ulong lo, ulong hi) sub128(ulong alo, ulong ahi, ulong blo, ulong bhi)
        {
            ulong lo = alo - blo;
            ulong borrow = lo > alo ? 1ul : 0ul;
            ulong hi = ahi - bhi - borrow;
            return (lo, hi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (ulong lo, ulong hi) mul128(ulong a, ulong b)
        {
            ulong hi = Polyfill.BigMul(a, b, out ulong lo);
            return (lo, hi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (ulong lo, ulong hi) mul128and64(ulong alo, ulong ahi, ulong b)
        {
            ulong thi = Polyfill.BigMul(alo, b, out ulong tlo);
            ulong ulo = ahi * b;
            return (tlo, thi + ulo);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (ulong lo, ulong hi) pasin(ulong xlo, ulong xhi)
        {
            ReadOnlySpan<ulong> b = [0x5ba2e8ba2e8ad9b7, 0x0004713b13b29079, 0x000000393331e196, 0x0000000002f5c315];
            ReadOnlySpan<ulong> ch = [0xaaaaaaaaaaaaaaa5, 0x0002aaaaaaaaaaaa, 0x3333333333333484, 0x0000001333333333, 0xb6db6db6db6da950, 0x0000000000b6db6d, 0x1c71c71c71c76217, 0x00000000000007c7];

            ulong tlo = ch[6], thi = ch[7];
            tlo += muuh(xhi, b[0] + muuh(xhi, b[1] + muuh(xhi, b[2] + muuh(xhi, b[3]))));

            ulong rlo, rhi;
            (rlo, rhi) = mUU(xlo, xhi, tlo, thi);
            (rlo, rhi) = add128(ch[4], ch[5], rlo, rhi);
            (rlo, rhi) = mUU(xlo, xhi, rlo, rhi);
            (rlo, rhi) = add128(ch[2], ch[3], rlo, rhi);
            (rlo, rhi) = mUU(xlo, xhi, rlo, rhi);
            (rlo, rhi) = add128(ch[0], ch[1], rlo, rhi);
            (rlo, rhi) = mUU(xlo, xhi, rlo, rhi);

            return (rlo, rhi);
        }


        const ulong InvPiH = 0x517cc1b727220a94ul;
        const ulong InvPiL = 0xfe13abe8fa9a6ee0ul;
        const double OneOverPiH = 0.31830988618379069;
        const double OneOverPiL = -1.9678676675182486e-17;


        static double asinPiAcc(double x)
        {
            ReadOnlySpan<ulong> s = [0x4e29cf6e5fed0679, 0x648557de8d99f7e, 0x76a17954b2b7c517, 0xc8fb2f886ec09f3, 0xbeeeae8129a786b9, 0x12d52092ce19f5cc, 0xd8e72d912977ee71, 0x1917a6bc29b42be1, 0x4e08e535cadaf147, 0x1f564e56a9730e34, 0xc002a2684781f080, 0x259020dd1cc27444, 0x8ffbbceed62c7c43, 0x2bc42889167f8ca9, 0x9732300393f33614, 0x31f17078d34c156c, 0x43af186b79b2a0f3, 0x381704d4fc9ec5f9, 0x90887712e9dc9663, 0x3e33f2f642be355e, 0x4c20ab7aa99a2183, 0x4447498ac7d9dd82, 0xd725d3b9ed35fbaa, 0x4a5018bb567c16a2, 0x97c4afa25181e605, 0x504d72505d98050c, 0x408fca9cc277fc1f, 0x563e69d6ac7f73f8, 0x4e61f79b3a36f1dc, 0x5c2214c3e9167abb, 0x98916152cf7eee1c, 0x61f78a9abaa58b46, 0xd409485edd56b172, 0x67bde50ea3b628b6, 0x9b165cba0c171818, 0x6d744027857300ad, 0x1439670dfe3d68e6, 0x7319ba64c711785a, 0x362474f1a105878f, 0x78ad74e01bd8ec78, 0x13e03e4889485c69, 0x7e2e936fe26ae7ed, 0xbfd79717f2880abf, 0x839c3cc917ff6cb4, 0xb892ca8361d8c84c, 0x88f59aa0da591421, 0xbba4cfecbff54867, 0x8e39d9cd73464364, 0xb17821911e71c16e, 0x93682a66e896f544, 0x19cec845ac87a5c6, 0x987fbfe70b81a708, 0xe25e39549638ae68, 0x9d7fd1490285c9e3, 0x3b5167ee359a234e, 0xa267992848eeb0c0, 0x149f6e75993468a3, 0xa73655df1f2f489e, 0x1becda8089c1a94c, 0xabeb49a46764fd15, 0xe4cad00d5c94bcd2, 0xb085baa8e966f6da, 0x597d89b3754abe9f, 0xb504f333f9de6484, 0x9de1e3b22b8bf4db, 0xb96841bf7ffcb21a, 0xac85320f528d6d5d, 0xbdaef913557d76f0, 0xbdf0715cb8b20bd7, 0xc1d8705ffcbb6e90, 0x43da25d99267326b, 0xc5e40358a8ba05a7, 0x8335241be1693225, 0xc9d1124c931fda7a, 0x23af31db7179a4aa, 0xcd9f023f9c3a059e, 0x744fea20e8abef92, 0xd14d3d02313c0eed, 0xf630e8b6dac83e69, 0xd4db3148750d1819, 0x24b9fe00663574a4, 0xd84852c0a80ffcdb, 0x2c19b63253da43fc, 0xdb941a28cb71ec87, 0x4b19aa71fec3ae6d, 0xdebe05637ca94cfb, 0xf4e8a8372f8c5810, 0xe1c5978c05ed8691, 0x122785ae67f5515d, 0xe4aa5909a08fa7b4, 0x125129529d48a92f, 0xe76bd7a1e63b9786, 0x15ad45b4a1b5e823, 0xea09a68a6e49cd62, 0x7e610231ac1d6181, 0xec835e79946a3145, 0x86f8c20fb664b01b, 0xeed89db66611e307, 0x67127db35b287316, 0xf1090827b43725fd, 0xa5486bdc455d56a2, 0xf314476247088f74, 0x163c5c7f03b718c5, 0xf4fa0ab6316ed2ec, 0x2c791f59cc1ffc23, 0xf6ba073b424b19e8, 0xc7adc6b4988891bb, 0xf853f7dc9186b952, 0x4504ae08d19b2980, 0xf9c79d63272c4628, 0x2172a361fd2a722f, 0xfb14be7fbae58156, 0x256778ffcb5c1769, 0xfc3b27d38a5d49ab, 0xeae6bd951c1dabbe, 0xfd3aabf84528b50b, 0x90cd1d959db674ef, 0xfe1323870cfe9a3d, 0x41390efdc726e9ef, 0xfec46d1e89292cf0, 0xf668633f1ab858a, 0xff4e6d680c41d0a9, 0x421e8edaaf59453e, 0xffb10f1bcb6bef1d, 0x5657552366961732, 0xffec4304266865d9];
            //const double X1 = 0.99993953791419843;

            ulong t = Polyfill.DoubleToUInt64Bits(x);
            int se = (int)(((long)t >> 52) & 0x7ff) - 0x3ff;
            long xsign = (long)(t & (1ul << 63));
            double ax = Abs(x);
            ulong filo, fihi;
            ulong sm = t << 11 | 1ul << 63;
            var (sm2l, sm2h) = mul128(sm, sm);

            if (ax < 0.0131875)
            {
                int ss = 2 * se;
                (sm2l, sm2h) = shr(sm2l, sm2h, -14 - ss);
                ulong Smh = sm >> 1;

                var (templ, temph) = pasin(sm2l, sm2h);
                (templ, temph) = muU(sm >> 1, templ, temph);
                (filo, fihi) = add128(0, Smh, templ, temph);
                se += 0x3ff;
            }
            else
            {
                double xx = FusedMultiplyAdd(x, -x, 1.0);
                ulong ixx = Polyfill.DoubleToUInt64Bits(1.0 / xx), c = Polyfill.DoubleToUInt64Bits(Sqrt(xx));
                ixx = Polyfill.DoubleToUInt64Bits(Polyfill.UInt64BitsToDouble(ixx) * Polyfill.UInt64BitsToDouble(c));
                double x2 = x * x;

                ReadOnlySpan<double> ch = [63.964595846054479, -39.390650807299593, 23.579467473196825, -7.4451509880590248];
                double c0 = ch[0] + ax * ch[1];
                double c2 = ch[2] + ax * ch[3];
                c0 += x2 * c2;
                ulong ic = Polyfill.DoubleToUInt64Bits(c0 * Polyfill.UInt64BitsToDouble(c) + 64.0);
                int indx = (int)(((ic & (~0ul >> 12)) + (1ul << (52 - 7))) >> (52 - 6));
                ulong cm = c << 11 | 1ul << 63;
                int ce = (int)((long)c >> 52) - 0x3ff;

                var (cm2l, cm2h) = mul128(cm, cm);
                const int off = 36 - 22 + 14;
                int ss = 128 - 104 + 2 * se + off;
                (sm2l, sm2h) = shl(sm2l, sm2h, ss);
                int sc = 128 - 104 + 2 * ce + off;
                (cm2l, cm2h) = shl(cm2l, cm2h, sc);

                (sm2l, sm2h) = add128(sm2l, sm2h, cm2l, cm2h);
                long h = (long)sm2h;
                ulong ixm = (ixx & (~0ul >> 12)) | 1ul << 52;
                int ixe = (int)((long)ixx >> 52) - 0x3ff;

                long dc = mh(h, (long)ixm);
                var (templ, temph) = imul(dc, (long)(cm >> 1));
                var (dsm2l, dsm2h) = ((ulong)templ, (ulong)temph);
                (dsm2l, dsm2h) = shl(dsm2l, dsm2h, 13);
                (sm2l, sm2h) = sub128(sm2l, sm2h, dsm2l, dsm2h);

                (templ, temph) = imul(dc, dc);
                var (dsm3l, dsm3h) = ((ulong)templ, (ulong)temph);
                sc = 28 - ixe * 2;
                if (sc >= 0)
                {
                    (dsm3l, dsm3h) = shr(dsm3l, dsm3h, sc);
                }
                else
                {
                    (dsm3l, dsm3h) = shl(dsm3l, dsm3h, -sc);
                }
                (sm2l, sm2h) = add128(sm2l, sm2h, dsm3l, dsm3h);

                int k = ixe - ce;
                ss = 24 + k;

                ulong Cml = 0, Cmh = cm, Dl = (ulong)dc << ss, Dh = (ulong)(dc >> (64 - ss));
                (Cml, Cmh) = sub128(Cml, Cmh, Dl, Dh);
                var (temp2l, temp2h) = shr(sm2l, sm2h, 14);
                h = (long)temp2l;
                dc = mh(h, (long)ixm);
                ss = 26 - k;

                if (ss >= 0)
                {
                    long temp3 = ss >= 64 ? 0L : dc >> ss;
                    (Cml, Cmh) = sub128(Cml, Cmh, (ulong)temp3, 0);
                }
                else
                {
                    var (temp3l, temp3h) = shl((ulong)dc, 0, -ss);
                    (Cml, Cmh) = sub128(Cml, Cmh, temp3l, temp3h);
                }


                filo = 0xd313198a2e037073;
                fihi = 0x3243f6a8885a308;
                (filo, fihi) = mul128and64(filo, fihi, (ulong)(64 - indx));
                if (indx == 0)
                {
                    (Cml, Cmh) = shr(Cml, Cmh, -ce - 7);
                    var (c2al, c2ah) = sqrU(Cml, Cmh);
                    var (zl, zh) = pasin(c2al, c2ah);

                    var (temp3l, temp3h) = mUU(Cml, Cmh, zl, zh);
                    (Cml, Cmh) = add128(Cml, Cmh, temp3l, temp3h);
                    (temp3l, temp3h) = shr(Cml, Cmh, 7);
                    (filo, fihi) = sub128(filo, fihi, temp3l, temp3h);
                }
                else
                {
                    var (vl, vh) = muU(sm >> -se, s[(indx - 1) * 2 + 0], s[(indx - 1) * 2 + 1]);
                    var (temp3l, temp3h) = mUU(Cml, Cmh, s[(63 - indx) * 2 + 0], s[(63 - indx) * 2 + 1]);
                    (temp3l, temp3h) = shr(temp3l, temp3h, -ce);
                    (vl, vh) = sub128(vl, vh, temp3l, temp3h);

                    ulong msk = (ulong)((long)vh >> 63);
                    (temp3l, temp3h) = add128(vl, vh, vl, vh);
                    temp3l &= msk;
                    temp3h &= msk;
                    var (temp4l, temp4h) = sqrU(vl, vh);
                    var (v2l, v2h) = sub128(temp4l, temp4h, temp3l, temp3h);

                    (v2l, v2h) = shl(v2l, v2h, 14);
                    var (pl, ph) = pasin(v2l, v2h);
                    (temp3l, temp3h) = (pl & msk, ph & msk);
                    (temp4l, temp4h) = mUU(pl, ph, vl, vh);
                    (temp4l, temp4h) = sub128(temp4l, temp4h, temp3l, temp3h);
                    (vl, vh) = add128(vl, vh, temp4l, temp4h);
                    (filo, fihi) = add128(filo, fihi, vl, vh);
                }
                se = 0x3fe;
            }


            // asinpi mod
            var (m1l, m1h) = mul128(InvPiH, filo);
            var (m2l, m2h) = mul128(InvPiL, fihi);
            (filo, fihi) = mul128(InvPiH, fihi);
            (filo, fihi) = add128(filo, fihi, m1h, 0);
            (filo, fihi) = add128(filo, fihi, m2h, 0);
            // asinpi mod end


            int nz = Polyfill.LeadingZeroCount(fihi);
            ulong rnd = (fihi >> (10 - nz)) & 1;        // rm == FE_TONEAREST
            // volatile double k0 = 1.0, k = k0 + 2.2250738585072014e-308;
            t = (fihi >> (11 - nz)) + (((ulong)se - (ulong)nz) << 52 | (ulong)xsign | rnd);

            return Polyfill.UInt64BitsToDouble(t);
        }


        static double asinPiTiny(double x)
        {
            double h, l;
            ulong t = Polyfill.DoubleToUInt64Bits(x);
            ulong au = t & 0x7ffffffffffffffful;

            if (x == 0.0)
            {
                return x;
            }

            if ((au << 12) == 0x59af9a1194efe000ul)
            {
                int e = (int)((t >> 52) & 0x7ff);
                h = 2.3860092221731257e-17;
                l = 1.5407439555097885e-33;
                t = Polyfill.DoubleToUInt64Bits((x > 0.0) ? 1.0 : -1.0);
                t -= (ulong)(0x3c9 - e) << 52;
                return FusedMultiplyAdd(l, Polyfill.UInt64BitsToDouble(t), h * Polyfill.UInt64BitsToDouble(t));
            }

            if (au == 0x35cba89af1f855ul)
            {
                return (x > 0.0) ?
                    FusedMultiplyAdd(2.4099198651028841e-181, -2.4099198651028841e-181, 3.8592439318736722e-308) :
                    FusedMultiplyAdd(2.4099198651028841e-181, 2.4099198651028841e-181, -3.8592439318736722e-308);
            }

            if (au == 0x15cba89af1f855ul)
            {
                return (x > 0.0) ?
                    FusedMultiplyAdd(2.4099198651028841e-181, 2.4099198651028841e-181, 9.984835630268913e-309) :
                    FusedMultiplyAdd(-2.4099198651028841e-181, 2.4099198651028841e-181, -9.984835630268913e-309);
            }

            if (au == 0x25cba89af1f855ul)
            {
                return (x > 0.0) ?
                    FusedMultiplyAdd(-2.4099198651028841e-181, 2.4099198651028841e-181, 1.9296219659368361e-308) :
                    FusedMultiplyAdd(2.4099198651028841e-181, 2.4099198651028841e-181, -1.9296219659368361e-308);
            }

            h = x * OneOverPiH;
            double sx = x * 8.1129638414606682e+31;
            l = FusedMultiplyAdd(sx, OneOverPiH, -h * 8.1129638414606682e+31);
            l = FusedMultiplyAdd(sx, OneOverPiL, l);

            double res = FusedMultiplyAdd(l, 1.2325951644078309e-32, h);
            return res;
        }

        static double asinPiSmall(double x)
        {
            ReadOnlySpan<double> exceptions = [4.2845220716184517e-12, 1.3638057329688084e-12, 8.2277181776488646e-45, 1.7931717895994545e-11, 5.7078430825538661e-12, 4.2565530544112804e-44, 1.4859781196792462e-16, 4.7300152614670414e-17, 4.4651584405213146e-49, 6.3133917728469883e-16, 2.0096150166486054e-16, -1.2325951644078308e-32, 2.0849332543072404e-13, 6.6365486687933792e-14, 6.3108872417680937e-30, 1.548626481412716e-09, 4.929431190396858e-10, 5.1698788284564224e-26, 2.7291755775177452e-09, 8.6872356745525469e-10, -5.1698788284564224e-26, 4.482285451958768e-14, 1.4267557720562562e-14, 2.3721022297911018e-47, 2.0935071781603503e-10, 6.6638403160516987e-11, 2.2916217905433142e-43, 2.2044858709812636e-13, 7.0170964668582067e-14, 5.3958624674692736e-47, 6.4491208666098256e-09, 2.0528189290360831e-09, -4.4397830830992395e-42, 3.0752846321857495e-10, 9.7889350125380648e-11, 9.9907793915936632e-43, 1.0933420934173983e-13, 3.4802159731563947e-14, 2.4203066072680676e-46, 6.2606090165427021e-16, 1.9928137434969211e-16, 1.8969414350953222e-48, 4.2871032938023594e-10, 1.3646273615083831e-10, 3.846001517535187e-43, 2.702477122779453e-15, 8.6022518536622577e-16, 5.0909531941233871e-48, 4.6653504455649961e-11, 1.485027169335291e-11, 5.5471754214373479e-44, 1.3280261640404512e-09, 4.2272385712481216e-10, 5.2483458614003428e-42];

            for (int i = 0; i < exceptions.Length / 3; i++)
            {
                if (x == exceptions[i * 3 + 0])
                {
                    return exceptions[i * 3 + 1] + exceptions[i * 3 + 2];
                }
                if (x == -exceptions[i * 3 + 0])
                {
                    return -exceptions[i * 3 + 1] - exceptions[i * 3 + 2];
                }
            }

            const double c1h = 0.31830988618379069, c1l = -1.9678676675182489e-17;
            const double c3 = 0.053051647697298462;

            double h = c1h, l = FusedMultiplyAdd(c3, x * x, c1l);
            double hh, ll;
            hh = h * x;
            ll = FusedMultiplyAdd(h, x, -hh);
            ll = FusedMultiplyAdd(l, x, ll);
            return hh + ll;
        }





        ReadOnlySpan<ulong> s = [
            0, 0x3242abef46ccfbf, 0x647d97c437604f9, 0x96a9049670cfae6,
            0xc8bd35e14da15f0, 0xfab272b54b9871a, 0x12c8106e8e613a22, 0x15e214448b3fc654,
            0x18f8b83c69a60ab6, 0x1c0b826a7e4f62fc, 0x1f19f97b215f1aaf, 0x2223a4c563eceec1,
            0x25280c5dab3e0b51, 0x2826b9282ecc0286, 0x2b1f34eb563fb9fc, 0x2e110a61f48b3d5d,
            0x30fbc54d5d52c5a3, 0x33def28751db145b, 0x36ba2013c2b98056, 0x398cdd326388bc2d,
            0x3c56ba700dec763c, 0x3f1749b7f13573f6, 0x41ce1e648bffb65a, 0x447acd506d2c8a10,
            0x471cece6b9a321b2, 0x49b41533744b7aa2, 0x4c3fdff385c0d384, 0x4ebfe8a48142e4f1,
            0x5133cc9424775860, 0x539b2aef8f97a44f, 0x55f5a4d233b27e8a, 0x5842dd5474b37b6d,
            0x5a827999fcef3242, 0x5cb420dfbffe590d, 0x5ed77c89aabebb78, 0x60ec382ffe5db748,
            0x62f201ac545d02d3, 0x64e88926498fed3d, 0x66cf811fce1d02cf, 0x68a69e81189e0776,
            0x6a6d98a43a868c0c, 0x6c2429605407fe6d, 0x6dca0d1465b8f643, 0x6f5f02b1be54a67d,
            0x70e2cbc602f6c348, 0x72552c84d047d3da, 0x73b5ebd0f31dcbc3, 0x7504d3453724e6b1,
            0x7641af3cca3518a2, 0x776c4edb3308f183, 0x78848413da1b92fe, 0x798a23b1238447ba,
            0x7a7d055b18b76976, 0x7b5d039da1258cf4, 0x7c29fbee48c35ca9, 0x7ce3ceb193962314,
            0x7d8a5f3fdd72c0ab, 0x7e1d93e9c52ea4d5, 0x7e9d55fc22945a85, 0x7f0991c3867f4d1e,
            0x7f62368f44949678, 0x7fa736b40620e854, 0x7fd8878de5b5f78e, 0x7ff62182133432ec,
            ~0ul >> 1];

        ReadOnlySpan<ulong> sh = [
            0, 0xc90aafbd1b33efca, 0x91f65f10dd813e6f, 0x5aa41259c33eb998,
            0x22f4d78536857c3b, 0xeac9cad52e61c68a, 0xb2041ba3984e8898, 0x78851122cff19532,
            0x3e2e0f1a6982ad93, 0x2e09a9f93d8bf28, 0xc67e5ec857c6abd2, 0x88e93158fb3bb04a,
            0x4a03176acf82d45b, 0x9ae4a0bb300a193, 0xc7cd3ad58fee7f08, 0x8442987d22cf576a,
            0x3ef1535754b168d3, 0xf7bca1d476c516db, 0xae8804f0ae6015b3, 0x63374c98e22f0b43,
            0x15ae9c037b1d8f07, 0xc5d26dfc4d5cfda2, 0x73879922ffed9698, 0x1eb3541b4b228437,
            0xc73b39ae68c86c97, 0x6d054cdd12dea896, 0xff7fce17034e103, 0xaffa292050b93c7c,
            0x4cf325091dd61807, 0xe6cabbe3e5e913c3, 0x7d69348cec9fa2a3, 0x10b7551d2cdedb5d,
            0xa09e667f3bcc908b, 0x2d0837efff964354, 0xb5df226aafaede16, 0x3b0e0bff976dd218,
            0xbc806b151740b4e8, 0x3a22499263fb4f50, 0xb3e047f38740b3c4, 0x29a7a0462781ddaf,
            0x9b66290ea1a3033f, 0x90a581501ff9b65, 0x728345196e3d90e6, 0xd7c0ac6f95299f69,
            0x38b2f180bdb0d23f, 0x954b213411f4f682, 0xed7af43cc772f0c2, 0x4134d14dc939ac43,
            0x906bcf328d4628b0, 0xdb13b6ccc23c60f1, 0x212104f686e4bfad, 0x6288ec48e111ee95,
            0x9f4156c62dda5d83, 0xd740e76849633d06, 0xa7efb9230d72a59, 0x38f3ac64e588c509,
            0x6297cff75cb02ac4, 0x8764fa714ba93565, 0xa7557f08a516a17d, 0xc26470e19fd347b2,
            0xd88da3d125259e08, 0xe9cdad01883a1522, 0xf621e3796d7de3a8, 0xfd886084cd0cbb2b,
            0];

        ReadOnlySpan<ulong> a = [0x002aaaaaaaaaaaaa, 0x0000133333333344, 0x0000000b6db6d69d, 0x0000000007c7aa6f];
        ReadOnlySpan<ulong> b = [0xaaaaaaaaaaaaaaaa, 0x0004cccccccccccc, 0x0000002db6db6db6, 0x0000000001f1c71c, 0x00000000000016e8];
        ReadOnlySpan<double> ch = [63.964595846054479, -39.390650807299593, 23.579467473196825, -7.4451509880590248];



        ulong t = Polyfill.DoubleToUInt64Bits(x);
        int e = ((int)(t >> 52) & 0x7ff) - 0x3ff;
        long xsign = (long)(t & (1ul << 63));
        ulong sm = (t << 11) | (1ul << 63);

        ulong fiLo, fiHi;
        if (e >= 0)
        {
            ulong m = t << 12;
            if (e == 0 && m == 0)
            {
                return x * 0.5;
            }
            if (e == 0x400 && m != 0)
            {
                return x + x;
            }
            return double.NaN;
        }
        else if (e < -6)
        {
            if (e < -26)
            {
                // asinpi begin
                if (e < -53)
                {
                    return asinPiTiny(x);
                }
                return asinPiSmall(x);
                // asinpi end
            }

            ulong v2 = muuh(sm, sm), v3 = muuh(sm, v2);
            v2 >>= -2 * e - 14;

            ulong d = muuh(v3, b[0] + muuh(v2, b[1] + muuh(v2, b[2] + muuh(v2, b[3] + muuh(v2, b[4])))));

            int ss = 63 + 2 * e;
            fiLo = d << ss;
            fiHi = (d >> (64 - ss)) + (sm >> 1);



            // asinpi begin
            var (m1l, m1h) = mul128(InvPiH, fiLo);
            var (m2l, m2h) = mul128(InvPiL, fiHi);
            (fiLo, fiHi) = mul128(InvPiH, fiHi);
            (fiLo, fiHi) = add128(fiLo, fiHi, m1h, 0);
            (fiLo, fiHi) = add128(fiLo, fiHi, m2h, 0);
            // asinpi end



            int nz = Polyfill.LeadingZeroCount(fiHi) + 1;       // rounding mode == FE_TONEAREST

            ulong ulo = fiLo, uhi = fiHi;
            ulo += 15ul << ss;      // e < -6; ss < 51; no overflow
            if (ulo < fiLo)
            {
                uhi++;
            }

            if ((((fiHi ^ uhi) >> (11 - nz)) & 1) != 0)
            {
                return asinPiAcc(x);
            }
            e += 0x3ff;
        }
        else
        {
            double xx = FusedMultiplyAdd(x, -x, 1.0);
            ulong ixx = Polyfill.DoubleToUInt64Bits(1.0 / xx), c = Polyfill.DoubleToUInt64Bits(Sqrt(xx));
            ixx = Polyfill.DoubleToUInt64Bits(Polyfill.UInt64BitsToDouble(ixx) * Polyfill.UInt64BitsToDouble(c));

            double ax = Abs(x), x2 = x * x;
            double c0 = ch[0] + ax * ch[1];
            double c2 = ch[2] + ax * ch[3];
            c0 += x2 * c2;
            c0 *= Polyfill.UInt64BitsToDouble(c);
            c0 += 64;

            ulong ic = Polyfill.DoubleToUInt64Bits(c0);
            int indx = (int)(((ic & (~0ul >> 12)) + (1ul << (52 - 7))) >> (52 - 6));
            ulong cm = c << 11 | 1ul << 63;
            int ce = (int)((long)c >> 52) - 0x3ff;

            ulong sm2l, sm2h;
            (sm2l, sm2h) = mul128(sm, sm);
            ulong cm2l, cm2h;
            (cm2l, cm2h) = mul128(cm, cm);

            const int off = 36 - 22 + 14;
            int ss = 128 - 104 + 2 * e + off;
            (sm2l, sm2h) = shl(sm2l, sm2h, ss);

            int sc = 128 - 104 + 2 * ce + off;
            if (sc >= 0)
            {
                (cm2l, cm2h) = shl(cm2l, cm2h, sc);
            }
            else
            {
                (cm2l, cm2h) = shr(cm2l, cm2h, -sc);
            }

            (sm2l, sm2h) = add128(sm2l, sm2h, cm2l, cm2h);

            long h = (long)sm2h;
            ulong ixm = (ixx & (~0ul >> 12)) | 1ul << 52;
            int ixe = (int)((long)ixx >> 52) - 0x3ff;

            long Smh;
            ss = 6 + e;
            Smh = (long)((sm << ss) - sh[64 - indx]);

            long Cmh;
            sc = 6 + ce;

            if (sc >= 0)
            {
                Cmh = (long)(cm << sc);
            }
            else
            {
                Cmh = (long)(cm >> -sc);
            }
            Cmh -= (long)sh[indx];

            Cmh -= mh(h, (long)ixm) >> (34 - ixe);
            long v = mh(Smh, (long)s[indx]) - mh(Cmh, (long)s[64 - indx]), v2 = mh(v, v), v3 = mh(v2, v);
            v += mh(v3, (long)(a[0] + muuh((ulong)v2, a[1] + muuh((ulong)v2, a[2] + muuh((ulong)v2, a[3])))));

            fiLo = 0xd313198a2e037073;
            fiHi = 0x3243f6a8885a308;
            (fiLo, fiHi) = mul128and64(fiLo, fiHi, (ulong)(64 - indx));
            ulong Vh = (ulong)(v >> 5), Vl = (ulong)(v << 59);
            (fiLo, fiHi) = add128(fiLo, fiHi, Vl, Vh);



            // asinpi begin
            var (m1l, m1h) = mul128(InvPiH, fiLo);
            var (m2l, m2h) = mul128(InvPiL, fiHi);
            (fiLo, fiHi) = mul128(InvPiH, fiHi);
            (fiLo, fiHi) = add128(fiLo, fiHi, m1h, 0);
            (fiLo, fiHi) = add128(fiLo, fiHi, m2h, 0);
            // asinpi end



            int nz = Polyfill.LeadingZeroCount(fiHi) + 1;       // rm == FE_TONEAREST
            ulong ulo = fiLo, uhi = fiHi, dlo = fiLo, dhi = fiHi;

            (ulo, uhi) = add128(ulo, uhi, 124ul << 55, 0);
            (dlo, dhi) = sub128(dlo, dhi, 124ul << 55, 0);

            if ((((dhi ^ uhi) >> (11 - nz)) & 1) != 0)
            {
                return asinPiAcc(x);
            }
            e = 0x3fe;
        }

        {
            int nz = Polyfill.LeadingZeroCount(fiHi);
            ulong rnd = (fiHi >> (10 - nz)) & 1;        // assumes rm == FE_TONEAREST
            //volatile double k0 = 1.0, k = k0 + 2.2250738585072014e-308;
            t = ((fiHi >> (11 - nz)) + ((ulong)(e - nz) << 52 | rnd)) | (ulong)xsign;
            return Polyfill.UInt64BitsToDouble(t);
        }


    }
}
