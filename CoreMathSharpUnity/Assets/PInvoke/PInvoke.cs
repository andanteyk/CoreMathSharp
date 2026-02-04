using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CoreMathSharpUnity.PInvoke;

public static class PInvoke
{
    [DllImport("CoreMathPInvoke")]
    static extern float cr_acosf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AcosF(float x) => cr_acosf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_acoshf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AcoshF(float x) => cr_acoshf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_acospif(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AcosPiF(float x) => cr_acospif(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_asinf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AsinF(float x) => cr_asinf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_asinhf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AsinhF(float x) => cr_asinhf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_asinpif(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AsinPiF(float x) => cr_asinpif(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_atan2f(float y, float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Atan2F(float y, float x) => cr_atan2f(y, x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_atan2pif(float y, float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Atan2PiF(float y, float x) => cr_atan2pif(y, x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_atanf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AtanF(float x) => cr_atanf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_atanhf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AtanhF(float x) => cr_atanhf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_atanpif(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float AtanPiF(float x) => cr_atanpif(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_cbrtf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float CbrtF(float x) => cr_cbrtf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_compoundf(float x, float y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float CompoundF(float x, float y) => cr_compoundf(x, y);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_cosf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float CosF(float x) => cr_cosf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_coshf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float CoshF(float x) => cr_coshf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_cospif(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float CosPiF(float x) => cr_cospif(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_erff(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ErfF(float x) => cr_erff(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_erfcf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ErfcF(float x) => cr_erfcf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_exp10f(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Exp10F(float x) => cr_exp10f(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_exp10m1f(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Exp10M1F(float x) => cr_exp10m1f(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_exp2f(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Exp2F(float x) => cr_exp2f(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_exp2m1f(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Exp2M1F(float x) => cr_exp2m1f(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_expf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ExpF(float x) => cr_expf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_expm1f(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ExpM1F(float x) => cr_expm1f(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_hypotf(float x, float y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float HypotF(float x, float y) => cr_hypotf(x, y);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_lgammaf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float LGammaF(float x) => cr_lgammaf(x);


    [DllImport("CoreMathPInvoke")]
    static extern int cr_lgammaf_signgam();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LGammaFSigngam() => cr_lgammaf_signgam();


    [DllImport("CoreMathPInvoke")]
    static extern float cr_logf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float LogF(float x) => cr_logf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_log1pf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Log1PF(float x) => cr_log1pf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_log2f(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Log2F(float x) => cr_log2f(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_log2p1f(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Log2P1F(float x) => cr_log2p1f(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_log10f(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Log10F(float x) => cr_log10f(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_log10p1f(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Log10P1F(float x) => cr_log10p1f(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_powf(float x, float y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float PowF(float x, float y) => cr_powf(x, y);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_rsqrtf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReciprocalSqrtF(float x) => cr_rsqrtf(x);


    [DllImport("CoreMathPInvoke")]
    static extern void cr_sincosf(float x, out float sin, out float cos);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SinCosF(float x, out float sin, out float cos) => cr_sincosf(x, out sin, out cos);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_sinf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SinF(float x) => cr_sinf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_sinhf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SinhF(float x) => cr_sinhf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_sinpif(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SinPiF(float x) => cr_sinpif(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_sqrtf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SqrtF(float x) => cr_sqrtf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_tanf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float TanF(float x) => cr_tanf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_tanhf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float TanhF(float x) => cr_tanhf(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_tanpif(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float TanPiF(float x) => cr_tanpif(x);


    [DllImport("CoreMathPInvoke")]
    static extern float cr_tgammaf(float x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float TGammaF(float x) => cr_tgammaf(x);



    [DllImport("CoreMathPInvoke")]
    static extern double cr_acos(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Acos(double x) => cr_acos(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_acosh(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Acosh(double x) => cr_acosh(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_acospi(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double AcosPi(double x) => cr_acospi(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_asin(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Asin(double x) => cr_asin(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_asinh(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Asinh(double x) => cr_asinh(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_asinpi(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double AsinPi(double x) => cr_asinpi(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_atan2(double y, double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Atan2(double y, double x) => cr_atan2(y, x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_atan2pi(double y, double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Atan2Pi(double y, double x) => cr_atan2pi(y, x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_atan(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Atan(double x) => cr_atan(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_atanh(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Atanh(double x) => cr_atanh(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_atanpi(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double AtanPi(double x) => cr_atanpi(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_cbrt(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Cbrt(double x) => cr_cbrt(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_cos(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Cos(double x) => cr_cos(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_cosh(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Cosh(double x) => cr_cosh(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_cospi(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double CosPi(double x) => cr_cospi(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_erf(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Erf(double x) => cr_erf(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_erfc(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Erfc(double x) => cr_erfc(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_exp10(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Exp10(double x) => cr_exp10(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_exp10m1(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Exp10M1(double x) => cr_exp10m1(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_exp2(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Exp2(double x) => cr_exp2(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_exp2m1(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Exp2M1(double x) => cr_exp2m1(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_exp(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Exp(double x) => cr_exp(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_expm1(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ExpM1(double x) => cr_expm1(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_hypot(double x, double y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Hypot(double x, double y) => cr_hypot(x, y);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_lgamma(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double LGamma(double x) => cr_lgamma(x);


    [DllImport("CoreMathPInvoke")]
    static extern int cr_lgamma_signgam();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LGammaSigngam() => cr_lgamma_signgam();


    [DllImport("CoreMathPInvoke")]
    static extern double cr_log(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Log(double x) => cr_log(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_log1p(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Log1P(double x) => cr_log1p(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_log2(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Log2(double x) => cr_log2(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_log2p1(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Log2P1(double x) => cr_log2p1(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_log10(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Log10(double x) => cr_log10(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_log10p1(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Log10P1(double x) => cr_log10p1(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_pow(double x, double y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Pow(double x, double y) => cr_pow(x, y);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_rsqrt(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ReciprocalSqrt(double x) => cr_rsqrt(x);


    [DllImport("CoreMathPInvoke")]
    static extern void cr_sincos(double x, out double sin, out double cos);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SinCos(double x, out double sin, out double cos) => cr_sincos(x, out sin, out cos);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_sin(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Sin(double x) => cr_sin(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_sinh(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Sinh(double x) => cr_sinh(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_sinpi(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double SinPi(double x) => cr_sinpi(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_sqrt(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Sqrt(double x) => cr_sqrt(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_tan(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Tan(double x) => cr_tan(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_tanh(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Tanh(double x) => cr_tanh(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_tanpi(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double TanPi(double x) => cr_tanpi(x);


    [DllImport("CoreMathPInvoke")]
    static extern double cr_tgamma(double x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double TGamma(double x) => cr_tgamma(x);

}
