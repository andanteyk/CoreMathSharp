using System;
using Unity.Burst;
using Unity.Mathematics;

namespace CoreMathSharpUnity.Tests;

[BurstCompile]
public static class BurstMath
{
    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double AbsLow(double x) => Math.Abs(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double AbsMedium(double x) => Math.Abs(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double AbsHigh(double x) => Math.Abs(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double AcosLow(double x) => Math.Acos(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double AcosMedium(double x) => Math.Acos(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double AcosHigh(double x) => Math.Acos(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double AcoshLow(double x) => Math.Acosh(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double AcoshMedium(double x) => Math.Acosh(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double AcoshHigh(double x) => Math.Acosh(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double AcosPiLow(double x) => Math.Acos(x) / Math.PI;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double AcosPiMedium(double x) => Math.Acos(x) / Math.PI;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double AcosPiHigh(double x) => Math.Acos(x) / Math.PI;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double AsinLow(double x) => Math.Asin(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double AsinMedium(double x) => Math.Asin(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double AsinHigh(double x) => Math.Asin(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double AsinhLow(double x) => Math.Asinh(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double AsinhMedium(double x) => Math.Asinh(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double AsinhHigh(double x) => Math.Asinh(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double AsinPiLow(double x) => Math.Asin(x) / Math.PI;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double AsinPiMedium(double x) => Math.Asin(x) / Math.PI;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double AsinPiHigh(double x) => Math.Asin(x) / Math.PI;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double AtanLow(double x) => Math.Atan(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double AtanMedium(double x) => Math.Atan(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double AtanHigh(double x) => Math.Atan(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double AtanPiLow(double x) => Math.Atan(x) / Math.PI;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double AtanPiMedium(double x) => Math.Atan(x) / Math.PI;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double AtanPiHigh(double x) => Math.Atan(x) / Math.PI;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double Atan2Low(double y, double x) => Math.Atan2(y, x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double Atan2Medium(double y, double x) => Math.Atan2(y, x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double Atan2High(double y, double x) => Math.Atan2(y, x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double Atan2PiLow(double y, double x) => Math.Atan2(y, x) / Math.PI;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double Atan2PiMedium(double y, double x) => Math.Atan2(y, x) / Math.PI;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double Atan2PiHigh(double y, double x) => Math.Atan2(y, x) / Math.PI;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double AtanhLow(double x) => Math.Atanh(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double AtanhMedium(double x) => Math.Atanh(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double AtanhHigh(double x) => Math.Atanh(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double CbrtLow(double x) => Math.Cbrt(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double CbrtMedium(double x) => Math.Cbrt(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double CbrtHigh(double x) => Math.Cbrt(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double CeilingLow(double x) => Math.Ceiling(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double CeilingMedium(double x) => Math.Ceiling(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double CeilingHigh(double x) => Math.Ceiling(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double CosLow(double x) => Math.Cos(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double CosMedium(double x) => Math.Cos(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double CosHigh(double x) => Math.Cos(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double CosPiLow(double x) => Math.Cos(x * Math.PI);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double CosPiMedium(double x) => Math.Cos(x * Math.PI);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double CosPiHigh(double x) => Math.Cos(x * Math.PI);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double CoshLow(double x) => Math.Cosh(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double CoshMedium(double x) => Math.Cosh(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double CoshHigh(double x) => Math.Cosh(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double ExpLow(double x) => Math.Exp(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double ExpMedium(double x) => Math.Exp(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double ExpHigh(double x) => Math.Exp(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double ExpM1Low(double x) => Math.Exp(x) - 1.0;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double ExpM1Medium(double x) => Math.Exp(x) - 1.0;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double ExpM1High(double x) => Math.Exp(x) - 1.0;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double Exp10Low(double x) => math.exp10(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double Exp10Medium(double x) => math.exp10(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double Exp10High(double x) => math.exp10(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double Exp10M1Low(double x) => math.exp10(x) - 1.0;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double Exp10M1Medium(double x) => math.exp10(x) - 1.0;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double Exp10M1High(double x) => math.exp10(x) - 1.0;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double Exp2Low(double x) => math.exp2(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double Exp2Medium(double x) => math.exp2(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double Exp2High(double x) => math.exp2(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double Exp2M1Low(double x) => math.exp2(x) - 1.0;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double Exp2M1Medium(double x) => math.exp2(x) - 1.0;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double Exp2M1High(double x) => math.exp2(x) - 1.0;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double FloorLow(double x) => Math.Floor(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double FloorMedium(double x) => Math.Floor(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double FloorHigh(double x) => Math.Floor(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double FusedMultiplyAddLow(double x, double y, double z) => math.mad(x, y, z);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double FusedMultiplyAddMedium(double x, double y, double z) => math.mad(x, y, z);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double FusedMultiplyAddHigh(double x, double y, double z) => math.mad(x, y, z);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double HypotLow(double x, double y) => math.length(new double2(x, y));

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double HypotMedium(double x, double y) => math.length(new double2(x, y));

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double HypotHigh(double x, double y) => math.length(new double2(x, y));


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double LogLow(double x) => Math.Log(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double LogMedium(double x) => Math.Log(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double LogHigh(double x) => Math.Log(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double LogLow(double x, double newBase) => Math.Log(x, newBase);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double LogMedium(double x, double newBase) => Math.Log(x, newBase);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double LogHigh(double x, double newBase) => Math.Log(x, newBase);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double Log1PLow(double x) => Math.Log(x + 1.0);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double Log1PMedium(double x) => Math.Log(x + 1.0);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double Log1PHigh(double x) => Math.Log(x + 1.0);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double Log10Low(double x) => Math.Log10(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double Log10Medium(double x) => Math.Log10(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double Log10High(double x) => Math.Log10(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double Log10P1Low(double x) => Math.Log10(x + 1.0);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double Log10P1Medium(double x) => Math.Log10(x + 1.0);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double Log10P1High(double x) => Math.Log10(x + 1.0);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double Log2Low(double x) => math.log2(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double Log2Medium(double x) => math.log2(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double Log2High(double x) => math.log2(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double Log2P1Low(double x) => math.log2(x + 1.0);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double Log2P1Medium(double x) => math.log2(x + 1.0);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double Log2P1High(double x) => math.log2(x + 1.0);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double PowLow(double x, double y) => Math.Pow(x, y);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double PowMedium(double x, double y) => Math.Pow(x, y);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double PowHigh(double x, double y) => Math.Pow(x, y);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double ReciprocalLow(double x) => math.rcp(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double ReciprocalMedium(double x) => math.rcp(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double ReciprocalHigh(double x) => math.rcp(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double ReciprocalSqrtLow(double x) => math.rsqrt(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double ReciprocalSqrtMedium(double x) => math.rsqrt(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double ReciprocalSqrtHigh(double x) => math.rsqrt(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double RoundLow(double x) => Math.Round(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double RoundMedium(double x) => Math.Round(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double RoundHigh(double x) => Math.Round(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static int SignLow(double x) => Math.Sign(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static int SignMedium(double x) => Math.Sign(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static int SignHigh(double x) => Math.Sign(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double SinLow(double x) => Math.Sin(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double SinMedium(double x) => Math.Sin(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double SinHigh(double x) => Math.Sin(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static void SinCosLow(double x, out double sin, out double cos) => math.sincos(x, out sin, out cos);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static void SinCosMedium(double x, out double sin, out double cos) => math.sincos(x, out sin, out cos);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static void SinCosHigh(double x, out double sin, out double cos) => math.sincos(x, out sin, out cos);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double SinhLow(double x) => Math.Sinh(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double SinhMedium(double x) => Math.Sinh(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double SinhHigh(double x) => Math.Sinh(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double SinPiLow(double x) => Math.Sin(x * Math.PI);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double SinPiMedium(double x) => Math.Sin(x * Math.PI);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double SinPiHigh(double x) => Math.Sin(x * Math.PI);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double SqrtLow(double x) => Math.Sqrt(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double SqrtMedium(double x) => Math.Sqrt(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double SqrtHigh(double x) => Math.Sqrt(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double TanLow(double x) => Math.Tan(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double TanMedium(double x) => Math.Tan(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double TanHigh(double x) => Math.Tan(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double TanhLow(double x) => Math.Tanh(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double TanhMedium(double x) => Math.Tanh(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double TanhHigh(double x) => Math.Tanh(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double TanPiLow(double x) => Math.Tan(x * Math.PI);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double TanPiMedium(double x) => Math.Tan(x * Math.PI);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double TanPiHigh(double x) => Math.Tan(x * Math.PI);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static double TruncateLow(double x) => Math.Truncate(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static double TruncateMedium(double x) => Math.Truncate(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static double TruncateHigh(double x) => Math.Truncate(x);
}


[BurstCompile]
public static class BurstMathF
{
    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float AbsLow(float x) => math.abs(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float AbsMedium(float x) => math.abs(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float AbsHigh(float x) => math.abs(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float AcosLow(float x) => math.acos(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float AcosMedium(float x) => math.acos(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float AcosHigh(float x) => math.acos(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float AcoshLow(float x) => (float)Math.Acosh(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float AcoshMedium(float x) => (float)Math.Acosh(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float AcoshHigh(float x) => (float)Math.Acosh(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float AcosPiLow(float x) => math.acos(x) / math.PI;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float AcosPiMedium(float x) => math.acos(x) / math.PI;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float AcosPiHigh(float x) => math.acos(x) / math.PI;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float AsinLow(float x) => math.asin(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float AsinMedium(float x) => math.asin(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float AsinHigh(float x) => math.asin(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float AsinhLow(float x) => (float)Math.Asinh(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float AsinhMedium(float x) => (float)Math.Asinh(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float AsinhHigh(float x) => (float)Math.Asinh(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float AsinPiLow(float x) => math.asin(x) / math.PI;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float AsinPiMedium(float x) => math.asin(x) / math.PI;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float AsinPiHigh(float x) => math.asin(x) / math.PI;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float AtanLow(float x) => math.atan(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float AtanMedium(float x) => math.atan(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float AtanHigh(float x) => math.atan(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float AtanPiLow(float x) => math.atan(x) / math.PI;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float AtanPiMedium(float x) => math.atan(x) / math.PI;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float AtanPiHigh(float x) => math.atan(x) / math.PI;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float Atan2Low(float y, float x) => math.atan2(y, x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float Atan2Medium(float y, float x) => math.atan2(y, x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float Atan2High(float y, float x) => math.atan2(y, x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float Atan2PiLow(float y, float x) => math.atan2(y, x) / math.PI;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float Atan2PiMedium(float y, float x) => math.atan2(y, x) / math.PI;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float Atan2PiHigh(float y, float x) => math.atan2(y, x) / math.PI;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float AtanhLow(float x) => (float)Math.Atanh(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float AtanhMedium(float x) => (float)Math.Atanh(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float AtanhHigh(float x) => (float)Math.Atanh(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float CbrtLow(float x) => (float)Math.Cbrt(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float CbrtMedium(float x) => (float)Math.Cbrt(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float CbrtHigh(float x) => (float)Math.Cbrt(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float CompoundLow(float x, float y) => (float)Math.Pow(x + 1.0f, y);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float CompoundMedium(float x, float y) => (float)Math.Pow(x + 1.0f, y);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float CompoundHigh(float x, float y) => (float)Math.Pow(x + 1.0f, y);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float CeilingLow(float x) => math.ceil(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float CeilingMedium(float x) => math.ceil(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float CeilingHigh(float x) => math.ceil(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float CosLow(float x) => math.cos(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float CosMedium(float x) => math.cos(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float CosHigh(float x) => math.cos(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float CosPiLow(float x) => math.cos(x * math.PI);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float CosPiMedium(float x) => math.cos(x * math.PI);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float CosPiHigh(float x) => math.cos(x * math.PI);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float CoshLow(float x) => math.cosh(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float CoshMedium(float x) => math.cosh(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float CoshHigh(float x) => math.cosh(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float ExpLow(float x) => math.exp(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float ExpMedium(float x) => math.exp(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float ExpHigh(float x) => math.exp(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float ExpM1Low(float x) => math.exp(x) - 1f;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float ExpM1Medium(float x) => math.exp(x) - 1f;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float ExpM1High(float x) => math.exp(x) - 1f;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float Exp10Low(float x) => math.exp10(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float Exp10Medium(float x) => math.exp10(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float Exp10High(float x) => math.exp10(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float Exp10M1Low(float x) => math.exp10(x) - 1f;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float Exp10M1Medium(float x) => math.exp10(x) - 1f;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float Exp10M1High(float x) => math.exp10(x) - 1f;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float Exp2Low(float x) => math.exp2(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float Exp2Medium(float x) => math.exp2(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float Exp2High(float x) => math.exp2(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float Exp2M1Low(float x) => math.exp2(x) - 1f;

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float Exp2M1Medium(float x) => math.exp2(x) - 1f;

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float Exp2M1High(float x) => math.exp2(x) - 1f;


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float FloorLow(float x) => math.floor(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float FloorMedium(float x) => math.floor(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float FloorHigh(float x) => math.floor(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float FusedMultiplyAddLow(float x, float y, float z) => math.mad(x, y, z);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float FusedMultiplyAddMedium(float x, float y, float z) => math.mad(x, y, z);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float FusedMultiplyAddHigh(float x, float y, float z) => math.mad(x, y, z);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float LogLow(float x) => math.log(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float LogMedium(float x) => math.log(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float LogHigh(float x) => math.log(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float Log1PLow(float x) => math.log(x + 1f);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float Log1PMedium(float x) => math.log(x + 1f);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float Log1PHigh(float x) => math.log(x + 1f);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float LogLow(float x, float newBase) => (float)Math.Log(x, newBase);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float LogMedium(float x, float newBase) => (float)Math.Log(x, newBase);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float LogHigh(float x, float newBase) => (float)Math.Log(x, newBase);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float Log10Low(float x) => math.log10(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float Log10Medium(float x) => math.log10(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float Log10High(float x) => math.log10(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float Log10P1Low(float x) => math.log10(x + 1f);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float Log10P1Medium(float x) => math.log10(x + 1f);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float Log10P1High(float x) => math.log10(x + 1f);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float Log2Low(float x) => math.log2(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float Log2Medium(float x) => math.log2(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float Log2High(float x) => math.log2(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float Log2P1Low(float x) => math.log2(x + 1f);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float Log2P1Medium(float x) => math.log2(x + 1f);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float Log2P1High(float x) => math.log2(x + 1f);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float PowLow(float x, float y) => math.pow(x, y);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float PowMedium(float x, float y) => math.pow(x, y);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float PowHigh(float x, float y) => math.pow(x, y);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float ReciprocalLow(float x) => math.rcp(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float ReciprocalMedium(float x) => math.rcp(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float ReciprocalHigh(float x) => math.rcp(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float ReciprocalSqrtLow(float x) => math.rsqrt(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float ReciprocalSqrtMedium(float x) => math.rsqrt(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float ReciprocalSqrtHigh(float x) => math.rsqrt(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float RoundLow(float x) => math.round(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float RoundMedium(float x) => math.round(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float RoundHigh(float x) => math.round(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static int SignLow(float x) => Math.Sign(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static int SignMedium(float x) => Math.Sign(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static int SignHigh(float x) => Math.Sign(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float SinLow(float x) => math.sin(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float SinMedium(float x) => math.sin(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float SinHigh(float x) => math.sin(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float SinPiLow(float x) => math.sin(x * math.PI);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float SinPiMedium(float x) => math.sin(x * math.PI);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float SinPiHigh(float x) => math.sin(x * math.PI);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static void SinCosLow(float x, out float sin, out float cos) => math.sincos(x, out sin, out cos);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static void SinCosMedium(float x, out float sin, out float cos) => math.sincos(x, out sin, out cos);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static void SinCosHigh(float x, out float sin, out float cos) => math.sincos(x, out sin, out cos);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float SinhLow(float x) => math.sinh(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float SinhMedium(float x) => math.sinh(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float SinhHigh(float x) => math.sinh(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float SqrtLow(float x) => math.sqrt(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float SqrtMedium(float x) => math.sqrt(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float SqrtHigh(float x) => math.sqrt(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float TanLow(float x) => math.tan(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float TanMedium(float x) => math.tan(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float TanHigh(float x) => math.tan(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float TanPiLow(float x) => math.tan(x * math.PI);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float TanPiMedium(float x) => math.tan(x * math.PI);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float TanPiHigh(float x) => math.tan(x * math.PI);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float TanhLow(float x) => math.tanh(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float TanhMedium(float x) => math.tanh(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float TanhHigh(float x) => math.tanh(x);


    [BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]
    public static float TruncateLow(float x) => math.trunc(x);

    [BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]
    public static float TruncateMedium(float x) => math.trunc(x);

    [BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]
    public static float TruncateHigh(float x) => math.trunc(x);

}
