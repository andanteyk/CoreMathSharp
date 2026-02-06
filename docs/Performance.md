# Performance

## TL;DR

* The speed of this library is at a level that is practical despite the high-precision calculations it performs.
* In both Managed and Unity, it was even faster than the standard library implementation in some cases.
* However, in the Unity environment, the `double` version tends to be slightly slower.


## TOC

* [Managed](#managed) - vs. `System.Math(F)`
* [Unity](#unity) - vs. `Mathf`, `math`, `[BurstCompile]` -ed `math`, Native library with P/Invoke

## Managed

### Summary

The vertical axis represents the mean time (smaller is faster), and the horizontal axis represents the function.

#### MicroBenchmark, vs. `MathF` , .NET 10

![](StrictMathF_Net10.png)

`MathF` is a native method with no accuracy guarantee, while `CoreF` (this library) is managed and has accuracy guarantees, putting it at a performance disadvantage.  
Despite this, it appears to be performing well. Some methods are even faster than `MathF`.

`Compound` seems a little slower, but this may be due to the range of input random numbers - the fast or slow path is determined depending on the input.

#### MicroBenchmark, vs. `Math` , .NET 10

![](StrictMath_Net10.png)

In comparison with `Math`, faster methods are observed as well.

Although it includes some slightly slower methods such as `AtanPi`, overall it can be said that the speed is practical.

### Details

For the comparison of managed code, I used [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet), which is considered to be the most reliable.

```
BenchmarkDotNet v0.15.8, Windows 11 (10.0.26100.7623/24H2/2024Update/HudsonValley)
12th Gen Intel Core i7-12700F 2.10GHz, 1 CPU, 20 logical and 12 physical cores
.NET SDK 10.0.102
  [Host]    : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.23 (8.0.23, 8.0.2325.60607), X64 RyuJIT x86-64-v3

Affinity=00001111111111111111  
```

#### Contenders

This benchmark compares:

* `Math` - `System.Math(F)` ; Standard library
* `Core` - This library 

For methods that are not directly supported in `Math(F)`, I performed measurements by mapping those that can be mapped naively (e.g., `Compound(x, y) == Pow(x + 1, y)`).

#### Benchmarking Code (Example)

Two benchmark patterns were prepared.

The first, `MicroBenchmark`, simply calls the function once.

```cs
// MicroBenchmark
[Benchmark]
public double CoreAcos()
{
    // X is a random number generated outside the benchmark
    return StrictMath.Acos(X);
}
```

The second, `MacroBenchmark` returns the sum of the array passed through a function.  
This is because a single function call may be too short a measurement.

```cs
// MacroBenchmark
[Benchmark]
public double CoreAcos()
{
    double sum = 0.0;

    // X is a double[5000] filled with random numbers
    foreach (var x in X)
    {
        sum += StrictMath.Acos(x);
    }

    return sum;
}
```

#### Results

##### MicroBenchmark, vs. `MathF` , .NET 10

![](StrictMathF_Net10.png)

##### MicroBenchmark, vs. `Math` , .NET 10

![](StrictMath_Net10.png)

##### MacroBenchmark, vs. `MathF` , .NET 10

![](StrictMathFMacro_Net10.png)

##### MacroBenchmark, vs. `Math` , .NET 10

![](StrictMathMacro_Net10.png)

##### MicroBenchmark, vs. `MathF` , .NET Standard 2.1

![](StrictMathF_NetStd.png)

##### MicroBenchmark, vs. `Math` , .NET Standard 2.1

![](StrictMath_NetStd.png)

##### MicroBenchmark, vs. `MathF` , .NET Standard 2.1

![](StrictMathFMacro_NetStd.png)

##### MicroBenchmark, vs. `Math` , .NET Standard 2.1

![](StrictMathMacro_NetStd.png)

##### Observations

When comparing .NET Standard 2.1 and .NET 10, we can see that `Core(F)` (this library) has a significant speed difference.

This is thought to be mainly due to the speed difference of `FusedMultiplyAdd`.  
This method is an important calculation that is frequently used by other methods, but its API is not implemented in .NET Standard 2.1.  
Therefore, a software fallback is implemented, but it is much slower than the intrinsic.  

This doesn't seem to have much of an effect on `float`, but it is more noticeable in the implementation of `double`.

##### Raw Data

* [CoreMathSharp.Benchmarks.StrictMathBenchmark-report-github.md](RawData/CoreMathSharp.Benchmarks.StrictMathBenchmark-report-github.md)
* [CoreMathSharp.Benchmarks.StrictMathMacroBenchmark-report-github.md](RawData/CoreMathSharp.Benchmarks.StrictMathMacroBenchmark-report-github.md)
* [CoreMathSharp.Benchmarks.StrictMathBenchmark-report.csv](RawData/CoreMathSharp.Benchmarks.StrictMathBenchmark-report.csv)
* [CoreMathSharp.Benchmarks.StrictMathMacroBenchmark-report.csv](RawData/CoreMathSharp.Benchmarks.StrictMathMacroBenchmark-report.csv)

## Unity

### Summary

#### MicroBenchmark(float), Summary, Player(IL2CPP)

![](UnityFMicro_IL2CPP_Summary.png)

`Compound` and `Pow` are slightly slower, but otherwise `Core` (this library) does not appear to be slower than the other methods.

#### MicroBenchmark(double), Summary, Player(IL2CPP)

![](UnityMicro_IL2CPP_Summary.png)

Unlike `float`, it tends to be slower than other methods, possibly due to the time required for high-precision calculations.

### Details

For comparison in Unity, the [Performance Testing Package for Unity Test Framework](https://docs.unity3d.com/Packages/com.unity.test-framework.performance@3.2/manual/index.html) was used.

(That seems like a pretty simple way to measure benchmarks, but BenchmarkDotNet doesn't currently work with Unity, so that's a no-brainer.)

Unity 6000.5.0a5 was used for the measurements.

The measurements were taken for both PlayMode (Mono) and Player (IL2CPP).

#### Contenders

This benchmark compares:

* `Unity` - [`UnityEngine.Mathf`](https://docs.unity3d.com/ja/current/ScriptReference/Mathf.html) ; Unity's standard library
* `Math` - [`Unity.Mathematics.math`](https://github.com/Unity-Technologies/Unity.Mathematics)
    * Burst is not used. As far as I can tell from reading the source code, `System.Math` is used internally.
* `Low` - `math` with `[BurstCompile(FloatMode = FloatMode.Fast, FloatPrecision = FloatPrecision.Low)]`
    * Relaxes the requirements for some math functions to 350.0 ulps, while allowing risky operations such as reordering floating-point operations.
* `Medium` - `math` with `[BurstCompile(FloatMode = FloatMode.Strict, FloatPrecision = FloatPrecision.Medium)]`
    * Relaxes the requirements for some math functions to 3.5 ulps. This is the default setting.
* `High` - `math` with `[BurstCompile(FloatMode = FloatMode.Deterministic, FloatPrecision = FloatPrecision.High)]`
    * Some mathematical functions have an error of 1.0 ulp. Consistent behavior can be expected across all platforms.
    * Note that Flush to Zero is enabled.
* `Core` - This library
* `PInvoke` - [The CORE-MATH project](https://core-math.gitlabpages.inria.fr/) 's original C library
    * Compiling a C library into a DLL (Native Plugin) and calling it with P/Invoke.

More information about `[BurstCompile]` can be found [here](https://docs.unity3d.com/Packages/com.unity.burst@1.8/manual/compilation-burstcompile.html).

#### Benchmarking Code (Example)

As with the managed code comparison, I prepared two benchmark patterns.

```cs
// UnityBenchmark
[Test, Performance]
public void CoreAcosF()
{
    Measure.Method(() =>
    {
        // X is a random number generated outside the benchmark
        // Store it in the external variable Result to prevent it from being deleted as dead code.
        Result = StrictMath.Acos(X);
    })
        .WarmupCount(10)
        .MeasurementCount(100)
        .IterationsPerMeasurement(5000)
        .SampleGroup("Acos")
        .Run();
}
```

```cs
// UnityMacroBenchmark
[Test, Performance]
public void CoreAcos()
{
    Measure.Method(() =>
    {
        double sum = 0.0;

        // X is a double[5000] filled with random numbers
        foreach (var x in X)
        {
            sum += StrictMath.Acos(x);
        }

        Result = sum;
    })
        .WarmupCount(10)
        .MeasurementCount(100)
        .IterationsPerMeasurement(16)
        .SampleGroup("Acos")
        .Run();
}
```

#### Results

##### MicroBenchmark(float), Summary, PlayMode(Mono)

![](UnityFMicro_PlayMode_Summary.png)

##### MicroBenchmark(float), vs. `Mathf` and `math`, PlayMode(Mono)

![](UnityFMicro_PlayMode_Unity.png)

##### MicroBenchmark(float), vs. `[BurstCompile]` -ed `math`, PlayMode(Mono)

![](UnityFMicro_PlayMode_Burst.png)

##### MicroBenchmark(float), vs. P/Invoke, PlayMode(Mono)

![](UnityFMicro_PlayMode_PInvoke.png)

##### MicroBenchmark(double), Summary, PlayMode(Mono)

![](UnityMicro_PlayMode_Summary.png)

---


##### MacroBenchmark(float), Summary, PlayMode(Mono)

![](UnityFMacro_PlayMode_Summary.png)

##### MacroBenchmark(float), vs. `Mathf` and `math`, PlayMode(Mono)

![](UnityFMacro_PlayMode_Unity.png)

##### MacroBenchmark(float), vs. `[BurstCompile]` -ed `math`, PlayMode(Mono)

![](UnityFMacro_PlayMode_Burst.png)

##### MacroBenchmark(float), vs. P/Invoke, PlayMode(Mono)

![](UnityFMacro_PlayMode_PInvoke.png)

##### MacroBenchmark(double), Summary, PlayMode(Mono)

![](UnityMacro_PlayMode_Summary.png)

---


##### MicroBenchmark(float), Summary, Player(IL2CPP)

![](UnityFMicro_IL2CPP_Summary.png)

##### MicroBenchmark(float), vs. `Mathf` and `math`, Player(IL2CPP)

![](UnityFMicro_IL2CPP_Unity.png)

##### MicroBenchmark(float), vs. `[BurstCompile]` -ed `math`, Player(IL2CPP)

![](UnityFMicro_IL2CPP_Burst.png)

##### MicroBenchmark(float), vs. P/Invoke, Player(IL2CPP)

![](UnityFMicro_IL2CPP_PInvoke.png)

##### MicroBenchmark(double), Summary, Player(IL2CPP)

![](UnityMicro_IL2CPP_Summary.png)

---


##### MacroBenchmark(float), Summary, Player(IL2CPP)

![](UnityFMacro_IL2CPP_Summary.png)

##### MacroBenchmark(float), vs. `Mathf` and `math`, Player(IL2CPP)

![](UnityFMacro_IL2CPP_Unity.png)

##### MacroBenchmark(float), vs. `[BurstCompile]` -ed `math`, Player(IL2CPP)

![](UnityFMacro_IL2CPP_Burst.png)

##### MacroBenchmark(float), vs. P/Invoke, Player(IL2CPP)

![](UnityFMacro_IL2CPP_PInvoke.png)

##### MacroBenchmark(double), Summary, Player(IL2CPP)

![](UnityMacro_IL2CPP_Summary.png)

---


##### Observations

###### float <-> double

In the Unity environment, the slowness of `double` calculations was evident.

This is because, as mentioned above, `FusedMultiplyAdd` is implemented in software, and also because error-free calculations in high-precision doubles are expensive.

.NET Standard 2.1, which can be used for Unity implementation, does not support intrinsics (`Vector128`, `LeadingZeroCount`, etc.), which results in a significant slowdown in speed.

In terms of source code size, some functions may require 10 times the number of lines.  
This is also thought to be a factor in the difference between float and double.

Therefore, if you don't need it, it's better to use the float version of the function.

###### Micro <-> Macro

We found that the trends were generally the same for microbenchmarks and macrobenchmarks.  
Therefore, it can be said that there are no major problems with the benchmarking methodology.

###### Mono <-> IL2CPP

First, please note that the vertical axis is 1/10.
The impact of IL2CPP on speed is likely to be very large.

The overall trend doesn't seem to have changed much, but High appears to be slightly slower in comparison.

###### vs. `Mathf`, `math`

`Compound` and `Pow` were relatively slower, but the other functions were roughly comparable in speed.

For inverse trigonometric functions and `Cbrt`, the results showed that despite being managed code, they were faster than Unity's implementation.

###### vs. `[BurstCompile]` -ed `math`

`Core` is often faster than `[BurstCompile]` -ed `math`.

In particular, it can be seen that it has a speed advantage in most cases compared to High.  
High is considered to be a large overhead in order to obtain deterministic results.

It also showed that simply applying `[BurstCompile]` does not necessarily make the program faster.  
This is likely because it makes little use of SIMD operations, where Burst's true value can be realized.

From what I measured, the only visible difference between Low and Medium was `Pow`; everything else was almost the same.  
Considering the risk to accuracy, the benefits of using Low are likely smaller than the disadvantages.

Furthermore, if you utilize Burst Intrinsics in Unity, you can use `fma` and `clz`, for example, which is likely to make things even faster.　　
However, since this is an implementation specific to Burst, the implementation cost is very high, and as will be described later, it is thought that using a native library would be faster, so this is difficult.

###### vs. P/Invoke

In most cases, P/Invoke is slightly faster.  
This is especially noticeable in the slower implementations `Compound` and `Pow`.

It is believed that the speedup achieved by native compilation has a much greater impact than the overhead of P/Invoke.

However, it should be noted that native libraries require the effort of being built for each environment.  
It is certainly fast, but there are many points to be aware of. Managed code is easy and convenient.

##### Raw Data

* [PerformanceTestResults_playmode.csv](RawData/PerformanceTestResults_playmode.csv)
* [PerformanceTestResults_il2cpp.csv](RawData/PerformanceTestResults_il2cpp.csv)

