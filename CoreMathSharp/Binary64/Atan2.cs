using System;
using System.Runtime.CompilerServices;

namespace CoreMathSharp;

public static partial class StrictMath
{

    private readonly record struct Tint(ulong m, ulong h, ulong l, long ex, ulong sgn)
    {
        public static Tint Zero => new Tint(0, 0, 0, -1076, 0);
        public static Tint One => new Tint(0, 0x8000000000000000ul, 0, 1, 0);
        public static Tint Pi => new Tint(0xc4c6628b80dc1cd1, 0xc90fdaa22168c234, 0x29024e088a67cc74, 2, 0);
        public static Tint Pi2 => new Tint(0xc4c6628b80dc1cd1, 0xc90fdaa22168c234, 0x29024e088a67cc74, 1, 0);

        public override string ToString()
        {
            return $"{m:x16} {h:x16} {l:x16} {ex:x16} {sgn:x16}";
        }

        public readonly bool IsNormalized()
        {
            if (h == 0 && m == 0 && l == 0)
            {
                return true;
            }

            return (h >> 63) != 0;
        }

        private static (ulong lo, ulong hi) Add128(ulong al, ulong ah, ulong bl, ulong bh)
        {
            ulong l = al + bl;
            ulong carry = l < al ? 1ul : 0ul;
            ulong h = ah + bh + carry;
            return (l, h);
        }

        private static (ulong lo, ulong hi) Sub128(ulong al, ulong ah, ulong bl, ulong bh)
        {
            ulong l = al - bl;
            ulong borrow = l > al ? 1ul : 0ul;
            ulong h = ah - bh - borrow;
            return (l, h);
        }

        private static (ulong lo, ulong hi) Mul128(ulong a, ulong b)
        {
            ulong hi = Polyfill.BigMul(a, b, out ulong lo);
            return (lo, hi);
        }

        public static Tint Mul(in Tint a, in Tint b)
        {
            ulong r_m, r_h, r_l, r_sgn;
            long r_ex;

            r_ex = a.ex + b.ex;
            r_sgn = a.sgn ^ b.sgn;

            var (rhl, rhh) = Mul128(a.h, b.h);
            var (rm1l, rm1h) = Mul128(a.h, b.m);
            var (rm2l, rm2h) = Mul128(a.m, b.h);
            var (rl1l, rl1h) = Mul128(a.h, b.l);
            var (rl2l, rl2h) = Mul128(a.m, b.m);
            var (rl3l, rl3h) = Mul128(a.l, b.h);

            ulong h, l, cm;

            r_h = rhh;
            r_m = rhl;

            r_l = rm1l;
            h = rm1h;
            r_m += h;
            r_h += r_m < h ? 1ul : 0ul;

            l = rm2l;
            h = rm2h;
            r_l += l;
            cm = r_l < l ? 1ul : 0ul;
            r_m += h;
            r_h += r_m < h ? 1ul : 0ul;

            (rl1l, rl1h) = Add128(rl1h, 0, rl2h, 0);
            (rl1l, rl1h) = Add128(rl1l, rl1h, rl3h, 0);
            l = rl1l;
            cm += rl1h;
            r_l += l;
            cm += r_l < l ? 1ul : 0ul;

            r_m += cm;
            r_h += r_m < cm ? 1ul : 0ul;

            if ((r_h >> 63) == 0)
            {
                r_h = r_h << 1 | r_m >> 63;
                r_m = r_m << 1 | r_l >> 63;
                r_l = r_l << 1;
                r_ex--;
            }

            return new Tint(r_m, r_h, r_l, r_ex, r_sgn);
        }

        public readonly bool IsZero() => h == 0;

        public static int Cmp(long a, long b) => (a > b ? 1 : 0) - (a < b ? 1 : 0);
        public static int Cmpu(ulong a, ulong b) => (a > b ? 1 : 0) - (a < b ? 1 : 0);
        public static int Cmpu128(ulong al, ulong ah, ulong bl, ulong bh)
        {
            int gt = ah > bh ? 1 : ah < bh ? 0 : al > bl ? 1 : 0;
            int lt = ah < bh ? 1 : ah > bh ? 0 : al < bl ? 1 : 0;
            return gt - lt;
        }

        public static int CmpAbs(in Tint a, in Tint b)
        {
            if (a.IsZero())
            {
                return b.IsZero() ? 0 : -1;
            }
            if (b.IsZero())
            {
                return 1;
            }

            int c = Cmp(a.ex, b.ex);
            if (c != 0)
            {
                return c;
            }

            c = Cmpu128(a.m, a.h, b.m, b.h);
            if (c != 0)
            {
                return c;
            }

            return Cmpu(a.l, b.l);
        }

        public static Tint Rshift(in Tint a, in Tint b, int k)
        {
            ulong a_m, a_h, a_l;

            if (k == 0)
            {
                a_l = b.l;
                a_m = b.m;
                a_h = b.h;
            }
            else if (k < 64)
            {
                a_l = b.l >> k | b.m << -k;
                a_m = b.m >> k | b.h << -k;
                a_h = b.h >> k;
            }
            else if (k == 64)
            {
                a_l = b.m;
                a_m = b.h;
                a_h = 0;
            }
            else if (k < 128)
            {
                a_l = b.m >> k | b.h << -k;
                a_m = b.h >> k;
                a_h = 0;
            }
            else if (k < 192)
            {
                a_l = b.h >> k;
                a_m = 0;
                a_h = 0;
            }
            else
            {
                a_m = 0;
                a_h = 0;
                a_l = 0;
            }

            return new Tint(a_m, a_h, a_l, a.ex, a.sgn);
        }

        public static Tint Lshift(in Tint a, in Tint b, int k)
        {
            ulong a_m, a_h, a_l;

            if (k == 0)
            {
                a_l = b.l;
                a_m = b.m;
                a_h = b.h;
            }
            else if (k < 64)
            {
                a_l = b.l << k;
                a_m = b.m << k | b.l >> -k;
                a_h = b.h << k | b.m >> -k;
            }
            else if (k == 64)
            {
                a_l = 0;
                a_m = b.l;
                a_h = b.m;
            }
            else if (k < 128)
            {
                a_l = 0;
                a_m = b.l << k;
                a_h = b.m << k;
            }
            else if (k < 192)
            {
                a_l = 0;
                a_m = 0;
                a_h = b.l << k;
            }
            else
            {
                a_l = 0;
                a_m = 0;
                a_h = 0;
            }

            return new Tint(a_m, a_h, a_l, a.ex, a.sgn);
        }

        public static Tint Add(in Tint a, in Tint b)
        {
            Tint aa, bb;

            int cmp = CmpAbs(a, b);
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


            int sh = (int)(aa.ex - bb.ex);
            Tint t = Rshift(new Tint(), bb, sh);
            Tint r;

            if ((aa.sgn ^ bb.sgn) != 0)
            {
                t = t with { l = aa.l - t.l };
                var (tempm, temph) = Sub128(aa.m, aa.h, t.m, t.h);
                if (t.l > aa.l)
                {
                    (tempm, temph) = Sub128(tempm, temph, 1ul, 0);
                }
                t = t with { m = tempm, h = temph };

                ulong th = t.h;
                int ex = th != 0 ? Polyfill.LeadingZeroCount(th) :
                    (t.m != 0 || t.h != 0 ? 64 + Polyfill.LeadingZeroCount(t.m) : 128 + Polyfill.LeadingZeroCount(t.l));

                if (ex <= 1 || sh == 0)
                {
                    r = Lshift(new Tint(), t, ex);
                    r = r with { ex = aa.ex - ex };
                }
                else
                {
                    t = Lshift(t, bb, ex - sh);
                    r = Lshift(new Tint(), aa, ex);

                    t = t with { l = r.l - t.l };
                    (tempm, temph) = Sub128(r.m, r.h, t.m, t.h);
                    if (t.l > r.l)
                    {
                        (tempm, temph) = Sub128(tempm, temph, 1, 0);
                    }
                    t = t with { m = tempm, h = temph };

                    th = t.h;
                    int ex1 = th != 0 ? Polyfill.LeadingZeroCount(th) :
                        (t.m != 0 || t.h != 0 ? 64 + Polyfill.LeadingZeroCount(t.m) : 128 + Polyfill.LeadingZeroCount(t.l));
                    r = Lshift(r, t, ex1);
                    r = r with { ex = aa.ex - (ex + ex1) };
                }
            }
            else
            {
                ulong ahl = aa.m, ahh = aa.h;
                ulong al = aa.l;
                r = new Tint() with { l = al + t.l };

                ulong cl = r.l < al ? 1ul : 0ul;
                var (tempm, temph) = Add128(ahl, ahh, t.m, t.h);
                r = r with { m = tempm, h = temph };

                ulong ch = r.h < ahh ? 1ul : r.h > ahh ? 0ul : r.m < ahl ? 1ul : 0ul;
                (tempm, temph) = Add128(r.m, r.h, cl, 0);
                r = r with { m = tempm, h = temph };

                ch += r.h == 0 && r.m < cl ? 1ul : 0ul;

                if (ch != 0)
                {
                    r = r with { ex = aa.ex + 1 };
                    r = r with { l = r.l >> 1 | r.m << 63 };
                    r = r with { m = r.m >> 1 | r.h << 63, h = r.h >> 1 | ch << 63 };
                }
                else
                {
                    r = r with { ex = aa.ex };
                }
            }

            r = r with { sgn = aa.sgn };
            return r;
        }

        public static Tint FromD(double x)
        {
            ulong u = Polyfill.DoubleToUInt64Bits(x);
            ulong a_sgn = u >> 63;
            ulong ax = u & 0x7ffffffffffffffful;
            int e = (int)(ax >> 52);

            long a_ex;
            ulong a_h;
            if (e != 0)
            {
                a_ex = e - 0x3fe;
                a_h = 1ul << 63 | ax << 11;
            }
            else
            {
                e = Polyfill.LeadingZeroCount(ax);
                a_ex = -0x3f2 - e;
                a_h = ax << e;
            }

            return new Tint(0, a_h, 0, a_ex, a_sgn);
        }

        public static double AsLdexp(double x, long i)
        {
            ulong ix = Polyfill.DoubleToUInt64Bits(x);
            ix += (ulong)i << 52;
            return Polyfill.UInt64BitsToDouble(ix);
        }

        public readonly double ToDouble(ulong err, double y, double x)
        {
            if (ex >= 1025)
            {
                return sgn != 0 ? -8.9884656743115795e+307 - 8.9884656743115795e+307 : 8.9884656743115795e+307 + 8.9884656743115795e+307;
            }
            if (ex <= -1074)
            {
                if (ex < -1074)
                {
                    return (sgn != 0 ? -4.9406564584124654e-324 : 4.9406564584124654e-324) * 0.5;
                }

                int mid = h == (1ul << 63) && m == 0 && l == 0 ? 1 : 0;
                return (sgn != 0 ? -4.9406564584124654e-324 : 4.9406564584124654e-324) * (mid != 0 ? 0.5 : 0.75);
            }


            ulong hh = h, mm = m, ll = l;
            int eex = (int)ex;
            ulong low = hh & 0x7ff;

            if (mm == 0 || ~mm == 0)
            {
                if ((mm == 0 && (low == 0 || low == 0x400) && ll < err) ||
                    (~mm == 0 && (low == 0x3ff || low == 0x7ff) && ~ll < err))
                {
                    throw new InvalidOperationException($"Unexpected worst-case found, please report to the developer; Worst-case of atan2 found: y,x={y},{x}");
                }
            }
            if (eex <= -1022)
            {
                int sh = -1021 - eex;
                ll = (mm << -sh) | (ll >> sh) | (ll > 0 ? 1ul : 0ul);
                mm = (hh << -sh) | (mm >> sh);
                hh = hh >> sh;
                low = hh & 0x7ff;
                eex += sh;
            }

            double dh = hh >> 11, dl;
            if (err == 0)
            {
                dl = 0;
            }
            else if (low < 0x400)
            {
                dl = 0.25;
            }
            else if (low > 0x400)
            {
                dl = 0.75;
            }
            else
            {
                if (mm == 0 && ll == 0)
                {
                    dl = 0.5;
                }
                else
                {
                    dl = 0.75;
                }
            }

            double s = sgn != 0 ? -1.0 : 1.0;
            dh = FusedMultiplyAdd(dl, s, s * dh);
            dh *= 2.2204460492503131e-16;
            return dh * AsLdexp(1.0, eex - 1);
        }

        public readonly Tint Inv()
        {
            double a = ToDouble(0, 0, 0);

            bool subnormal = Abs(a) < 2.2250738585072014e-308;
            if (subnormal)
            {
                a *= 9007199254740992;
            }
            Tint r = FromD(1.0 / a);
            if (subnormal)
            {
                r = r with { ex = r.ex + 53 };
            }

            var q = Mul(this, r);
            q = q with { sgn = 1 - q.sgn };
            q = Add(One, q);
            q = Mul(r, q);
            r = Add(r, q);

            return r;
        }

        public static Tint Div(in Tint b, in Tint a)
        {
            Tint Y = a.Inv();
            Tint r = Mul(Y, b);

            Tint Z = Mul(a, r);
            Z = Z with { sgn = 1 - Z.sgn };
            Z = Add(b, Z);
            Z = Mul(Y, Z);
            r = Add(r, Z);

            return r;
        }

        public static Tint DivD(double b, double a)
        {
            return Div(FromD(b), FromD(a));
        }
    }

    public static double Atan2(double y, double x)
    {
        const ulong Mask = 0x7ffffffffffffffful;
        const double PiH = 3.1415926535897931;
        const double PiL = 1.2246467991473532e-16;
        const double PiOver2H = 1.5707963267948966;
        const double PiOver2L = 6.123233995736766e-17;



        static double atan2Accurate(double y, double x)
        {
            ReadOnlySpan<ulong> Ph = [0x82703f8b53112eca, 0xb3cf74b427d53e03, 0xb08498b17b88c39a, 0xfd4baa7bb52b0d83, 0x96541948b85fb386, 0x96aaa5d4baffc590, 0x83cd41b5fa862b11, 0xcbbc7752197cc106, 0x8cf8f7c585767042, 0xafdc36d6b36139bf, 0xc6f8fb1463822519, 0xcce79f10f754e315, 0xc090384dbd77132e, 0xa556ac9eee25e867, 0x81bd2677ddfcbc38, 0xb9ed12169d72e8d9, 0xf2df6da875b666fa, 0x902d34d4db910ed8, 0x9aefa41961ce1207, 0x95db9977c5e6e263, 0x817cb6d8236d5f4d, 0xc5eec4228c352098, 0x840c38ccfcb5dc64, 0x97074af7b286b19e, 0x9054011c32d4b7a5, 0xdddb4a2eaab2bb15, 0x80ef9a1b3e59abe5, 0xca16dd49cd30854c, 0xa0ba01b8742ba801, 0x9c0440896ce6515c];
            ReadOnlySpan<ulong> Pm = [0x71ce6279d467aa45, 0xee590b839e751890, 0x11e37821d784d8fa, 0xb25425beb0a04dc3, 0xe0b815ace425a859, 0xf23db783b6e4ce55, 0xad8f9d6e3eb9d833, 0xbc66cfb3edefa6e3, 0xcd92a5b9ac13d8c8, 0x11df63474e3859d4, 0x2f9f04cf55c10377, 0x91ce47edcbc8a4ee, 0x1dfb4d9a1d595de7, 0xd84dafa25141d203, 0x1df33f27dbb60939, 0xc81d95dcef13c227, 0xc29dd3a69c274c65, 0x6f3ecddfd6af0a9f, 0xacd0317666753751, 0x85367a63fab8a720, 0x70c7815f28909d9b, 0x789e56be1dee9b67, 0xb55a44095bf7f312, 0xc20aac21f3130f1d, 0xc68d3c5d525f4af4, 0x753761a8f0e9aa8d, 0x18e6afe0651fdf49, 0xbad8ca379401027c, 0xe6dc14a9516d7f04, 0xcf3470fa7f10ac3,];
            ReadOnlySpan<ulong> Pl = [0x19c9b28466b6fc0, 0xf2570b12bcb2381a, 0x2795b14b84b9d78c, 0x92008d9fb34b68ff, 0xbeca17c69bb4f7a0, 0x2c54e054d3b69f7e, 0x4ef470d801fa03ca, 0x568b2f9e4feb8910, 0x43bd4a023dbe04d, 0x42b30b2c089a2b6c, 0x1c44675c6d483606, 0xb2a6f721ac656b42, 0xe506ca899a394f70, 0xf1c7c2a5156b3ec3, 0xcb4c2c73286a6c29, 0x974b95796b93c332, 0x83b9260976c8f0c5, 0x8e950d7e70a8e9b2, 0x3eaf42a53fcc6534, 0xfc2b63bd115998d8, 0xc54e97155d12158, 0xba25bda81292ccc1, 0x6668b8792d4ecd08, 0x369e9fdbdd48528a, 0xeeb3c98615dee188, 0x3f20620cd3e075bb, 0xd1c0329e6a702d9d, 0x1fc7f47985486f16, 0x1f5303d878905d2f, 0xa9dc80682c111a9d,];
            ReadOnlySpan<int> Pex = [-10, -8, -6, -5, -3, -2, -1, -1, 0, 0, 0, 0, 0, 0, 0, -1, -2, -2, -3, -4, -5, -7, -8, -10, -12, -15, -17, -21, -25, -59,];
            ReadOnlySpan<byte> Psgn = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,];

            ReadOnlySpan<ulong> Qh = [0x82703f8b53112eca, 0xb3cf74b427d53e03, 0xb33c44af0d43c9e8, 0x8264d256f0bf4d61, 0x9d97c4947687b2ee, 0xa0f888dc7aab949e, 0x8fd9c7172ab0828e, 0xe38168b78a51ee15, 0xa164ccdbb65ee133, 0xced0c0cbe44fac2f, 0xf0e6660438c90371, 0x8000000000000000, 0xf8ea68f9ad849103, 0xddc64eeee3506234, 0xb52ddfba2d37c8e2, 0x87ac113d26b683a1, 0xba065070b900f3d5, 0xe902d2a4bfc22441, 0x84e405c915f7769b, 0x896da46c7e6cebb3, 0x801a0c1a0dfefc92, 0xd59d088930df07db, 0x9db56a6984b77231, 0xcb7450d9d62d2e68, 0xe12a52e02b64b23e, 0xd057b6d0900ed30d, 0x9b1f1ca22fef7ae5, 0xaeba710f053aac3b, 0x84b9ada076cd007d, 0xcca4b9046786937d,];
            ReadOnlySpan<ulong> Qm = [0x71ce6279d467aa45, 0xee590b839e751890, 0xf4426f84614701db, 0xee21431cc0f29764, 0xef4087c49dde5229, 0xf623f19ac413db23, 0x1a9f72fadafc3dfb, 0x604a47815c8091eb, 0xe623433c5e086402, 0xd7c2adfc7c5dda9e, 0x3dca1a0f58f0d943, 0x0, 0xb257e4bfc606967f, 0x47a33ea40a2b22e4, 0x24551a9b19fa064a, 0x4e97756c38785b0b, 0xe49d8f9f238f1eeb, 0x945cfe6c83a5b828, 0xfaaf34c3c51dd39, 0xe0afc0b6db3dbbf5, 0xf8e0a97902e08d47, 0xa8cb00021849fbaf, 0xcf7c2074e0480959, 0x9787b4265977d004, 0x97c9139a8ced716c, 0x5e04bb17eb879a5e, 0x19a687f0a4073eee, 0x30ea5d6fb116f775, 0x13e55006073f76b1, 0x233c1e691c127c1f,];
            ReadOnlySpan<ulong> Ql = [0x19c9b28466b6fc0, 0xf2570b12bcb238cf, 0x979e49dc5b858442, 0xce0cc1b593ac9952, 0x742bc6e02bfb6df8, 0x854744a164f70ecd, 0xaf96a180ea297178, 0x9faaa42312e8eb30, 0xcb228afd335a245f, 0xa22b4a311cf5a5b1, 0x8a92b9de68c4488c, 0x0, 0x818fa2816928c4aa, 0xa6a2c129ace2470a, 0x3d78c68b54bc3850, 0xf75f0a4506931ba8, 0x6805d48d67f1dcd9, 0x519ebb457f8ccca0, 0xa97fa0ae99382c93, 0x61ccff6e78efff4, 0x81898e0b7d8ef0bc, 0xf80cc91aa0bddf89, 0x3316ecaa6aa4604e, 0xba615cee00a841d7, 0x70a176ad2b5f16d2, 0x23ed135b60bea070, 0x5a9ccbc8c14578b, 0x8a9bcfef5698f8f1, 0x4cf4259968228f3e, 0xf493e0b7a3de459a,];
            ReadOnlySpan<int> Qex = [-10, -8, -6, -4, -3, -2, -1, -1, 0, 0, 0, 1, 0, 0, 0, 0, -1, -2, -2, -3, -4, -6, -7, -9, -11, -13, -15, -18, -21, -26,];
            ReadOnlySpan<byte> Qsgn = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,];


            double res;

            double t = y / x;

            double u = CopySign(1.0, y);
            double v = FusedMultiplyAdd(u, -5.5511151231257827e-17, u);

            if (t == 0.0)
            {
                if (x > 0)
                {
                    return t;
                }
                res = (y > 0) ? PiH + PiL : -PiH - PiL;
                return res;
            }

            double corr = FusedMultiplyAdd(t, x, -y);
            if (corr == 0.0 && x > 0.0)
            {
                if (Abs(t) < 1.3538603431225864e-08)
                {
                    if (Abs(y) >= 3.6103887517296592e-276)
                    {
                        return FusedMultiplyAdd(t, -5.5511151231257827e-17, t);
                    }

                    corr = FusedMultiplyAdd(t * 4.0564819207303341e+31, x, -y * 4.0564819207303341e+31);
                    if (corr == 0.0)
                    {
                        res = FusedMultiplyAdd(t, -5.5511151231257827e-17, t);
                        return res;
                    }
                }
            }

            bool inv = Abs(y) > Abs(x);
            Tint z, p, q;
            if (inv)
            {
                z = Tint.DivD(x, y);
            }
            else
            {
                z = Tint.DivD(y, x);
            }

            if (!inv && x > 0.0 && z.ex <= -96)
            {
                z = z with { l = z.l - 2 };
                z = z with { m = z.m - (z.l < 2 ? 1ul : 0ul) };
                z = z with { h = z.h - (z.m < 1 ? 1ul : 0ul) };
                res = z.ToDouble(1, y, x);
                return res;
            }

            int sz = (int)z.sgn;
            z = z with { sgn = 0 };

            p = new Tint(Pm[29], Ph[29], Pl[29], Pex[29], Psgn[29]);
            q = new Tint(Qm[29], Qh[29], Ql[29], Qex[29], Qsgn[29]);

            for (int i = 28; i >= 0; i--)
            {
                p = Tint.Mul(p, z);
                q = Tint.Mul(q, z);
                p = Tint.Add(p, new Tint(Pm[i], Ph[i], Pl[i], Pex[i], Psgn[i]));
                q = Tint.Add(q, new Tint(Qm[i], Qh[i], Ql[i], Qex[i], Qsgn[i]));
            }

            p = Tint.Mul(p, z);
            z = Tint.Div(p, q);

            ulong err = 662;
            z = z with { sgn = (ulong)sz };

            if (inv)
            {
                if (z.sgn == 0)
                {
                    z = z with { sgn = 1 };
                    z = Tint.Add(Tint.Pi2, z);
                }
                else
                {
                    z = Tint.Add(Tint.Pi2, z);
                    z = z with { sgn = 1 };
                }
                err = 524;
            }

            if (x < 0.0)
            {
                if (z.sgn == 0)
                {
                    z = z with { sgn = 1 };
                    z = Tint.Add(Tint.Pi, z);
                    z = z with { sgn = 1 };
                }
                else
                {
                    z = Tint.Add(Tint.Pi, z);
                }
                err = 266;
            }

            res = z.ToDouble(err, y, x);
            return res;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double s, double e) fastTwoSum(double x, double y)
        {
            double s = x + y, z = s - x;
            return (s, y - z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (double sh, double e) fastSum(double xh, double xl, double yh, double yl)
        {
            var (sh, sl) = fastTwoSum(xh, yh);
            return (sh, (xl + yl) + sl);
        }

        static double asAtan2Special(double y0, double x0)
        {
            ulong iy = Polyfill.DoubleToUInt64Bits(y0), ix = Polyfill.DoubleToUInt64Bits(x0);
            ulong aiy = iy << 1, aix = ix << 1;

            if (aiy >= (0x7fful << 53) || aix >= (0x7fful << 53))
            {
                if (aiy > (0x7fful << 53) || aix > (0x7fful << 53))
                {
                    return y0 + x0;
                }
                if (aiy == (0x7fful << 53) && aix == (0x7fful << 53))
                {
                    ReadOnlySpan<double> finf = [2.7755575615628914e-17, 0.78539816339744828, 5.5511151231257827e-17, 2.3561944901923448];
                    return CopySign(finf[(int)(ix >> 63) * 2 + 1], y0) + CopySign(finf[(int)(ix >> 63) * 2 + 0], y0);
                }

                if (aix == (0x7fful << 53))
                {
                    if (x0 < 0.0)
                    {
                        return CopySign(PiH, y0) + CopySign(PiL, y0);
                    }

                    return CopySign(0.0, y0);
                }

                return CopySign(PiOver2H, y0) + CopySign(PiOver2L, y0);
            }

            if (aiy == 0 || aix == 0)
            {
                if (aiy == 0 && aix == 0)
                {
                    if (ix == 0)
                    {
                        return y0;
                    }

                    return iy == 0 ? PiH + PiL : -PiH - PiL;
                }

                if (aiy == 0)
                {
                    if (x0 > 0.0)
                    {
                        return y0 / x0;
                    }

                    return ((iy >> 63) == 0) ? PiH + PiL : -PiH - PiL;
                }

                return (y0 > 0.0) ? PiOver2H + PiOver2L : -PiOver2H - PiOver2L;
            }

            return 0.0;
        }




        ReadOnlySpan<double> asgn = [0.0, -0.0];
        ReadOnlySpan<double> T2 = [0, 0.015625, 0.03125, 0.046875, 0.0625, 0.078125, 0.09375, 0.109375, 0.125, 0.140625, 0.15625, 0.171875, 0.1875, 0.203125, 0.21875, 0.234375, 0.25, 0.265625, 0.28125, 0.296875, 0.3125, 0.328125, 0.34375, 0.359375, 0.375, 0.390625, 0.40625, 0.421875, 0.4375, 0.453125, 0.46875, 0.484375, 0.5, 0.515625, 0.53125, 0.546875, 0.5625, 0.578125, 0.59375, 0.609375, 0.625, 0.640625, 0.65625, 0.671875, 0.6875, 0.703125, 0.71875, 0.734375, 0.75, 0.765625, 0.78125, 0.796875, 0.8125, 0.828125, 0.84375, 0.859375, 0.875, 0.890625, 0.90625, 0.921875, 0.9375, 0.953125, 0.96875, 0.984375, 1];
        ReadOnlySpan<double> f2 = [0, 0, -1.7569843108731414e-16, 0.015623728620477007, 1.2718109451069599e-16, 0.031239833430268149, -1.5282123363018451e-16, 0.046840712915969807, -1.5420474151678852e-16, 0.062418809995957503, -3.5828811550300013e-17, 0.077966633831542342, 2.0188234451767475e-16, 0.093476781158589262, 4.846007563068433e-17, 0.10894195698986575, 1.9116370516694847e-16, 0.12435499454676124, 1.9133104288467082e-16, 0.13970887428916345, -1.0143688686840133e-16, 0.15499674192394108, -1.7007461777357561e-16, 0.17021192528547457, 5.9691843500100911e-17, 0.18534794999569471, 1.6967340798095792e-16, 0.20039855382587834, 5.5984967244265696e-17, 0.21535769969773799, 1.7884685822291619e-16, 0.23021958727684355, -7.2567971228152285e-17, 0.24497866312686423, 1.3026105738713097e-16, 0.2596296294082574, 8.2613535751637735e-18, 0.27416745111965879, -1.2530600203628822e-16, 0.28858736189407752, 2.1103377702202993e-16, 0.30288486837497119, 9.2083013219589226e-17, 0.31705575320914692, 1.0306969208672186e-16, 0.33109607670413199, 8.808349770693735e-17, 0.34500217720710502, 3.0887335648619192e-17, 0.35877067027057219, -3.5898839726412174e-17, 0.37239844667675426, -1.4274522636885407e-16, 0.38588266939807392, 2.2465981056170421e-17, 0.39922076957525254, -1.8240997597148038e-16, 0.41241044159738749, -1.9872907418313842e-16, 0.42544963737004249, -2.4942770306265409e-17, 0.43833655985795783, 3.2807356001837356e-17, 0.45106965598852344, -3.281237377829614e-17, 0.46364760900080615, -2.0739011759277459e-16, 0.47606933032276144, -1.1373236189329585e-17, 0.48833395105640554, -4.7181675085518756e-17, 0.50044081314729416, -2.5462781472855804e-17, 0.51238946031073773, -2.1652451080538965e-16, 0.52417962878291346, 1.069585067790331e-16, 0.53581123796046359, -6.1785205748553098e-17, 0.54728438098743704, -5.4556305485916264e-18, 0.55859931534356244, -2.0978954283997712e-16, 0.56975645348297865, 9.6607658680584982e-17, 0.5807563535676703, -1.728396503881636e-16, 0.59159971033511161, -8.1517995090231631e-17, 0.60228734613496426, 1.9049254307644509e-16, 0.61282020216524113, 2.6724038851400951e-17, 0.6231993299340659, 1.9475383748901603e-16, 0.63342588296914437, 1.2685708751395994e-16, 0.64350110879328426, 3.5800634857340095e-17, 0.65342634118076193, -1.4178285110681214e-16, 0.6632029927060934, -1.300154525596627e-16, 0.67283254759376332, 6.9432236715600077e-18, 0.68231655487474807, 2.1392745373274551e-16, 0.69165662185319965, -1.3089856480587381e-16, 0.7008544078844503, 6.5050637956666784e-17, 0.7099116184635248, -2.1478388444456983e-17, 0.71882999962162453, -1.0845297676512382e-16, 0.72761133262651079, -1.8730522844203674e-16, 0.73625742898142832, -7.393914397116018e-17, 0.74477012571607526, 8.6765367803333583e-17, 0.7531512809621943, 1.2087233279526847e-16, 0.76140276980557831, -3.7049919056027213e-17, 0.7695264804056583, 1.9536811397308681e-16, 0.77752431037334757, 3.061616997868383e-17, 0.78539816339744828];
        ReadOnlySpan<double> O = [0, 0, 1.5707963267948966, 6.123233995736766e-17, 0, 0, -1.5707963267948966, -6.123233995736766e-17, 3.1415926535897931, 1.2246467991473532e-16, 1.5707963267948966, 6.123233995736766e-17, -3.1415926535897931, -1.2246467991473532e-16, -1.5707963267948966, -6.123233995736766e-17];


        ulong iy = Polyfill.DoubleToUInt64Bits(y), ix = Polyfill.DoubleToUInt64Bits(x);
        ulong aiy = iy & Mask;
        if (aiy == 0 || aiy >= 0x7fful << 52)
        {
            return asAtan2Special(y, x);
        }
        ulong aix = ix & Mask;
        if (aix == 0 || aix >= 0x7fful << 52)
        {
            return asAtan2Special(y, x);
        }

        double ax = Abs(x), ay = Abs(y);
        double xx = Max(ax, ay), yy = Min(ax, ay);
        ulong sy = iy >> 63, sx = ix >> 63;
        ulong GT = aix < aiy ? 1ul : 0ul;
        ulong dxy = (aix - aiy) ^ (0 - GT);
        if (dxy >= 53ul << 52)
        {
            return atan2Accurate(y, x);
        }

        ulong sgn = Polyfill.DoubleToUInt64Bits(asgn[(int)(GT ^ sx ^ sy)]);
        ulong kw = sx << 2 | sy << 1 | GT;
        ulong jj = Polyfill.DoubleToUInt64Bits(yy / xx + (2.0 + 1.0 / 128.0));
        int jt = (int)((jj >> (52 - 7)) & 127);

        double fh = f2[jt * 2 + 1] * CopySign(1.0, Polyfill.UInt64BitsToDouble(sgn));
        double fl = f2[jt * 2 + 0] * CopySign(1.0, Polyfill.UInt64BitsToDouble(sgn));

        fh += O[(int)kw * 2 + 0];
        fl += O[(int)kw * 2 + 1];

        if (xx < 4.0083367200179456e-292)
        {
            xx *= 2.4948003869183998e+291;
            yy *= 2.4948003869183998e+291;
        }
        if (xx > 4.4942328371557898e+307)
        {
            if (jt != 0)
            {
                xx *= 0.5;
                yy *= 0.5;
            }
        }

        double t0 = T2[jt];
        double zn = FusedMultiplyAdd(-t0, xx, yy), zd = FusedMultiplyAdd(t0, yy, xx);
        double z = zn / zd;

        ReadOnlySpan<double> b = [-0.33333333333332604, 0.19999999943116925, -0.14284292170535642];
        double z2 = z * z;
        z *= CopySign(1.0, Polyfill.UInt64BitsToDouble(sgn));
        double dz = (z * z2) * (b[0] + z2 * (b[1] + z2 * b[2]));

        double eps = Abs(z) * 4.528712474471952e-16 + 8.0779356694631609e-28;
        (double rh, z) = fastTwoSum(fh, z);
        double rl = (fl + dz) + z;
        double lb = rh + (rl - eps), ub = rh + (rl + eps);
        if (lb != ub)
        {
            double dh = yy * t0, dl = FusedMultiplyAdd(yy, t0, -dh), e, rdh;
            (dh, e) = fastTwoSum(xx, dh);
            if (Abs(dh) <= 4.4942328371557898e+307)
            {
                rdh = 1.0 / dh;
            }
            else
            {
                // TODO: can't modify/access to flag
                rdh = 1.0 / dh;
            }

            dl += e;
            double nh = xx * t0, nl = FusedMultiplyAdd(xx, t0, -nh);
            double dt = yy - nh, y1 = dt + nh;
            if (y1 == yy)
            {
                (nh, nl) = fastTwoSum(dt, -nl);
            }
            else
            {
                (nh, nl) = fastTwoSum(dt, (yy - y1) - nl);
            }

            double zh = nh * rdh;
            z2 = zh * zh;
            double zl = rdh * (FusedMultiplyAdd(dh, -zh, nh) + (nl - (nh * rdh) * dl));

            ReadOnlySpan<double> b2 = [-0.33333333333333331, 0.19999999999974255, -0.14285713816496304, 0.11107620301764512];
            zl += zh * z2 * ((b2[0] + z2 * b2[1]) + (z2 * z2) * (b2[2] + z2 * b2[3]));
            zh *= CopySign(1.0, Polyfill.UInt64BitsToDouble(sgn));
            zl *= CopySign(1.0, Polyfill.UInt64BitsToDouble(sgn));
            eps = 1.1102230246251565e-15 * (Abs(zh) * z2 + 4.4408920985006262e-16);
            (fh, fl) = fastSum(fh, fl, zh, zl);
            lb = fh + (fl - eps);
            ub = fh + (fl + eps);
            if (lb != ub)
            {
                return atan2Accurate(y, x);
            }
        }

        return ub;
    }
}
