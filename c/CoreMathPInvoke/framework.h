#pragma once

#if _MSC_VER
#define EXPORT_API __declspec(dllexport)
#else
#define EXPORT_API
#endif

extern "C" 
{
	EXPORT_API float cr_acosf(float x);
	EXPORT_API float cr_acoshf(float x);
	EXPORT_API float cr_acospif(float x);
	EXPORT_API float cr_asinf(float x);
	EXPORT_API float cr_asinhf(float x);
	EXPORT_API float cr_asinpif(float x);
	EXPORT_API float cr_atanf(float x);
	EXPORT_API float cr_atan2f(float y, float x);
	EXPORT_API float cr_atan2pif(float y, float x);
	EXPORT_API float cr_atanhf(float x);
	EXPORT_API float cr_atanpif(float x);
	EXPORT_API float cr_cbrtf(float x);
	EXPORT_API float cr_compoundf(float x, float y);
	EXPORT_API float cr_cosf(float x);
	EXPORT_API float cr_coshf(float x);
	EXPORT_API float cr_cospif(float x);
	EXPORT_API float cr_erff(float x);
	EXPORT_API float cr_erfcf(float x);
	EXPORT_API float cr_expf(float x);
	EXPORT_API float cr_exp10f(float x);
	EXPORT_API float cr_exp10m1f(float x);
	EXPORT_API float cr_exp2f(float x);
	EXPORT_API float cr_exp2m1f(float x);
	EXPORT_API float cr_expm1f(float x);
	EXPORT_API float cr_hypotf(float x, float y);
	EXPORT_API float cr_lgammaf(float x);
	EXPORT_API int cr_lgammaf_signgam();
	EXPORT_API float cr_logf(float x);
	EXPORT_API float cr_log10f(float x);
	EXPORT_API float cr_log10p1f(float x);
	EXPORT_API float cr_log2f(float x);
	EXPORT_API float cr_log2p1f(float x);
	EXPORT_API float cr_log1pf(float x);
	EXPORT_API float cr_powf(float x, float y);
	EXPORT_API float cr_rsqrtf(float x);
	EXPORT_API float cr_sinf(float x);
	EXPORT_API void cr_sincosf(float x, float * sin, float * cos);
	EXPORT_API float cr_sinhf(float x);
	EXPORT_API float cr_sinpif(float x);
	EXPORT_API float cr_sqrtf(float x);
	EXPORT_API float cr_tanf(float x);
	EXPORT_API float cr_tanhf(float x);
	EXPORT_API float cr_tanpif(float x);
	EXPORT_API float cr_tgammaf(float x);

	EXPORT_API double cr_acos(double x);
	EXPORT_API double cr_acosh(double x);
	EXPORT_API double cr_acospi(double x);
	EXPORT_API double cr_asin(double x);
	EXPORT_API double cr_asinh(double x);
	EXPORT_API double cr_asinpi(double x);
	EXPORT_API double cr_atan(double x);
	EXPORT_API double cr_atan2(double y, double x);
	EXPORT_API double cr_atan2pi(double y, double x);
	EXPORT_API double cr_atanh(double x);
	EXPORT_API double cr_atanpi(double x);
	EXPORT_API double cr_cbrt(double x);
	EXPORT_API double cr_cos(double x);
	EXPORT_API double cr_cosh(double x);
	EXPORT_API double cr_cospi(double x);
	EXPORT_API double cr_erf(double x);
	EXPORT_API double cr_erfc(double x);
	EXPORT_API double cr_exp(double x);
	EXPORT_API double cr_exp10(double x);
	EXPORT_API double cr_exp10m1(double x);
	EXPORT_API double cr_exp2(double x);
	EXPORT_API double cr_exp2m1(double x);
	EXPORT_API double cr_expm1(double x);
	EXPORT_API double cr_hypot(double x, double y);
	EXPORT_API double cr_lgamma(double x);
	EXPORT_API int cr_lgamma_signgam();
	EXPORT_API double cr_log(double x);
	EXPORT_API double cr_log10(double x);
	EXPORT_API double cr_log10p1(double x);
	EXPORT_API double cr_log2(double x);
	EXPORT_API double cr_log2p1(double x);
	EXPORT_API double cr_log1p(double x);
	EXPORT_API double cr_pow(double x, double y);
	EXPORT_API double cr_rsqrt(double x);
	EXPORT_API double cr_sin(double x);
	EXPORT_API void cr_sincos(double x, double* sin, double* cos);
	EXPORT_API double cr_sinh(double x);
	EXPORT_API double cr_sinpi(double x);
	EXPORT_API double cr_sqrt(double x);
	EXPORT_API double cr_tan(double x);
	EXPORT_API double cr_tanh(double x);
	EXPORT_API double cr_tanpi(double x);
	EXPORT_API double cr_tgamma(double x);
}
