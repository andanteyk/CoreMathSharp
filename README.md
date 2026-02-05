# CoreMathSharp

Perfectly accurate, portable, and deterministic implementations of mathematical functions

<a href="https://www.nuget.org/packages/AndanteSoft.CoreMathSharp">![NuGet Version](https://img.shields.io/nuget/vpre/AndanteSoft.CoreMathSharp)</a>
<a href="LICENSE.md">![GitHub License](https://img.shields.io/github/license/andanteyk/CoreMathSharp)</a>
<a href="https://www.nuget.org/packages/AndanteSoft.CoreMathSharp">![NuGet Downloads](https://img.shields.io/nuget/dt/AndanteSoft.CoreMathSharp)</a>

![Logo](CoreMathSharp.png)

## Basic Usage

```cs
// All functions are accessible via StrictMath(F)
double exp = StrictMath.Exp(123.0);
float logf = StrictMathF.Log(123.0f);

// Error functions etc. are also available - see Features
double erf = StrictMath.Erf(456.0);
```

## Install

CoreMathSharp can be installed from NuGet `AndanteSoft.CoreMathSharp`.

```
dotnet add package AndanteSoft.CoreMathSharp
```

CoreMathSharp requires **.NET Standard 2.1** or **.NET 10.**
All functions are available in both, but the .NET 10 version is recommended as it runs faster.

### Installing on Unity

Supported version: 2021.2 or later. (API Compatibility Level: .NET Standard 2.1)

My test environment is 6000.5.0a5.

Use [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity) to install.

## Features

### TL;DR

* **Completely accurate.** All functions perform mathematically correct calculations and return correctly rounded results.
* **Environment independent.** `Math(F)` are environment dependent. CoreMathSharp is environment independent and produces correct results everywhere.
* **Reproducible.** Correct results are obtained in any environment, making it suitable for game replays and scientific and technical simulations.
* **Portability.** Works in .NET Standard 2.1 environments (i.e. Unity).
* **Easy to use.** Usage is the same as `Math(F)`. Some mathematical functions not found in `Math(F)` are also implemented.
* **Fully managed.** No native implementation.
* **No dependent libraries.**

### Why not use `Math(F)` ?

For example, [`Math.Sin`](https://learn.microsoft.com/en-us/dotnet/api/system.math.sin?view=net-10.0) has the following note:

> This method calls into the underlying C runtime, and **the exact result or valid input range may differ** between different operating systems or architectures.

[The help](https://learn.microsoft.com/en-us/cpp/c-runtime-library/floating-point-support?view=msvc-170) for the "underlying C runtime" says:

> The floating-point functions are implemented to balance performance with correctness. Because producing the correctly rounded result may be prohibitively expensive, these functions are designed to efficiently **produce a close approximation to the correctly rounded result.** In most cases, **the result produced is within +/-1 ULP** (unit of least precision) of the correctly rounded result, though there may be cases where there's greater inaccuracy.
> ...
> Many of the floating-point math library functions have different implementations for different CPU architectures. 

Also, for example, Unity's [`Mathf.Sin`](https://docs.unity3d.com/ScriptReference/Mathf.Sin.html) has some (slightly scary) note:

> If using very large numbers with this function, there is an acceptable range for input angle values for this method, beyond which the calculation will fail. On windows, the acceptable range is approximately between -9223372036854775295 to 9223372036854775295. **This range may differ on other platforms.** For values outside of the acceptable range, the Sin method returns the input value, rather than throwing an exception.

This information tells us:

* Not accurate. In most cases the error is ±1 [ulp](https://en.wikipedia.org/wiki/Unit_in_the_last_place), so in most cases this won't be a problem, maybe...
* It is environment-dependent, which is **problematic for terrain generation and replay in games** and for reproducibility in scientific papers.
* Not portable. It is heavily dependent on specific platforms (CRT, libm, etc.) and cannot be perfectly consistent across different platforms.

But what if there was a "perfect" mathematical function?
Perfection - accurate down to the last bit - necessarily means that the same value would be obtained in any environment.
Therefore, perfect accuracy brings benefits beyond just accuracy.

### Functions

The following functions are available:

|         Function |                                    Math | float | double |
|-----------------:|----------------------------------------:|------:|-------:|
|              Abs |                       $\lvert x \rvert$ |    ✅ |     ✅ |
|             Acos |                            $\arccos(x)$ |    ✅ |     ✅ |
|            Acosh |                    $\mathrm{arcosh}(x)$ |    ✅ |     ✅ |
|           AcosPi |                        $\arccos(x)/\pi$ |    ✅ |     ✅ |
|             Asin |                            $\arcsin(x)$ |    ✅ |     ✅ |
|            Asinh |                    $\mathrm{arsinh}(x)$ |    ✅ |     ✅ |
|           AsinPi |                        $\arcsin(x)/\pi$ |    ✅ |     ✅ |
|             Atan |                            $\arctan(x)$ |    ✅ |     ✅ |
|            Atan2 |                  $\mathrm{atan2}(y, x)$ |    ✅ |     ✅ |
|          Atan2Pi |              $\mathrm{atan2}(y, x)/\pi$ |    ✅ |     ✅ |
|            Atanh |                    $\mathrm{artanh}(x)$ |    ✅ |     ✅ |
|           AtanPi |                        $\arctan(x)/\pi$ |    ✅ |     ✅ |
|             Cbrt |                           $\sqrt[3]{x}$ |    ✅ |     ✅ |
|         Compound |                             $(1+x)^{y}$ |    ✅ |        |
|         CopySign | $\lvert x \rvert \cdot \mathrm{sgn}(y)$ |    ✅ |     ✅ |
|              Cos |                               $\cos(x)$ |    ✅ |     ✅ |
|             Cosh |                              $\cosh(x)$ |    ✅ |     ✅ |
|            CosPi |                     $\cos(x \cdot \pi)$ |    ✅ |     ✅ |
|              Erf |                       $\mathrm{erf}(x)$ |    ✅ |     ✅ |
|             Erfc |                     $1-\mathrm{erf}(x)$ |    ✅ |     ✅ |
|              Exp |                                 $e^{x}$ |    ✅ |     ✅ |
|             Exp2 |                                 $2^{x}$ |    ✅ |     ✅ |
|           Exp2M1 |                             $2^{x} - 1$ |    ✅ |     ✅ |
|            Exp10 |                                $10^{x}$ |    ✅ |     ✅ |
|          Exp10M1 |                            $10^{x} - 1$ |    ✅ |     ✅ |
|            ExpM1 |                             $e^{x} - 1$ |    ✅ |     ✅ |
| FusedMultiplyAdd |                      $(x \times y + z)$ |    ✅ |     ✅ |
|            Hypot |                  $\sqrt{x^{2} + y^{2}}$ |    ✅ |     ✅ |
|           LGamma |           $\ln \lvert \Gamma(x) \rvert$ |    ✅ |     ✅ |
|              Log |                               $\log(x)$ |    ✅ |     ✅ |
|            Log1P |                           $\log(x + 1)$ |    ✅ |     ✅ |
|             Log2 |                           $\log_{2}(x)$ |    ✅ |     ✅ |
|           Log2P1 |                       $\log_{2}(x + 1)$ |    ✅ |     ✅ |
|            Log10 |                          $\log_{10}(x)$ |    ✅ |     ✅ |
|          Log10P1 |                      $\log_{10}(x + 1)$ |    ✅ |     ✅ |
|              Max |                            $\max(x, y)$ |    ✅ |     ✅ |
|              Min |                            $\min(x, y)$ |    ✅ |     ✅ |
|              Pow |                                 $x^{y}$ |    ✅ |     ✅ |
|   ReciprocalSqrt |                            $1/\sqrt{x}$ |    ✅ |     ✅ |
|              Sin |                               $\sin(x)$ |    ✅ |     ✅ |
|           SinCos |                    $(\sin(x), \cos(x))$ |    ✅ |     ✅ |
|             Sinh |                              $\sinh(x)$ |    ✅ |     ✅ |
|            SinPi |                     $\sin(x \cdot \pi)$ |    ✅ |     ✅ |
|             Sqrt |                              $\sqrt{x}$ |    ✅ |     ✅ |
|              Tan |                               $\tan(x)$ |    ✅ |     ✅ |
|             Tanh |                              $\tanh(x)$ |    ✅ |     ✅ |
|            TanPi |                     $\tan(x \cdot \pi)$ |    ✅ |     ✅ |
|           TGamma |                             $\Gamma(x)$ |    ✅ |     ✅ |

### Performance

See [docs/Performance.md](docs/Performance.md).

## Notes

In a 32-bit environment (where the x87 FPU is used because SSE2 cannot be used for calculations), correct results may not be obtained.
This is unavoidable due to the [C# specifications](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/types#837-floating-point-types), so it cannot be supported.

> Floating-point operations may be performed with higher precision than the result type of the operation. 
> ...
> Some hardware architectures support an “extended” or “long double” floating-point type with greater range and precision than the `double` type, and implicitly perform all floating-point operations using this higher precision type.

## Fork

### Build

```
dotnet build
```

### Run Tests

```
dotnet test
```

The tests performed in `.net8.0` exist for testing DLLs for `netstandard2.1` .
(This is because the minimum requirement for xUnit is .net8.0.)

To generate test vectors (such as `acosf.txt`), see `c/***.c`.
An environment where clang can run (WSL is recommended) is required.

### Run Benchmarks

#### Managed (vs. [`Math`](https://learn.microsoft.com/en-us/dotnet/api/system.math?view=net-10.0))

```
dotnet run -c Release --project CoreMathSharp.Benchmarks -f net10.0
```

#### Unity (vs. [`Mathf`](https://docs.unity3d.com/ja/current/ScriptReference/Mathf.html), [`math`](https://github.com/Unity-Technologies/Unity.Mathematics), `[BurstCompile]` -ed `math`)

1. Open `CoreMathSharpUnity` folder with Unity 6000.5.0a5.
1. Open Window -> General -> Test Runner / Performance Test Report.
1. In Test Runner, change to `PlayMode` (Mono) or `Player` (IL2CPP), then press `Run All`.

The DLL (`Assets/Plugins/x86_64/CoreMathPInvoke.dll`) is built for Windows x64.
If you want to run it in a different environment:

1. Open `c/CoreMathPInvoke.slnx` with Visual Studio 2026 (needs Clang support)
1. Build with Release x64 mode
1. Copy the built DLL: from `c/x64/Release/` to `CoreMathSharpUnity/Assets/Plugins/`
1. Restart Unity

### Publish

```
dotnet pack
```

## License

[MIT License](LICENSE)

The implementation of CoreMathSharp is a port of the implementation in [THE CORE-MATH project](https://core-math.gitlabpages.inria.fr/).
I would like to take this opportunity to express my gratitude.

## TODO

* Benchmarking
    * Managed (vs. `Math(F)`)
    * IL2CPP (vs. `Mathf` or `Unity.Mathematics`)
    * vs. BurstCompile
    * Compare with P/Invoke
* More accurate testing
    * measure code coverage
* Fix math-dependent functions

