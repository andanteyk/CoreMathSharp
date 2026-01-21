using System;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreMathSharp;

public static partial class StrictMathF
{
#if NET7_0_OR_GREATER

    [GeneratedRegex(@"^(?<sign>[+\-]?)0x((?<predot>[0-9a-f]*)\.(?<postdot>[0-9a-f]+)|(?<predot>[0-9a-f]+)\.(?<postdot>[0-9a-f]*)|(?<predot>[0-9a-f]+)(?<postdot>))p(?<expsign>[+\-]?)(?<exp>[0-9]+)f$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex FloatHexRegex { get; }

    internal static float ParseHex(string str)
    {
        if (str == "NaN")
        {
            return float.NaN;
        }
        if (str == "∞" || str == "+∞")
        {
            return float.PositiveInfinity;
        }
        if (str == "-∞")
        {
            return float.NegativeInfinity;
        }


        var match = FloatHexRegex.Match(str);
        if (!match.Success)
        {
            throw new FormatException();
        }

        uint resultBits = 0;

        var matchSign = match.Groups["sign"].ValueSpan;
        if (matchSign.Length >= 1 && matchSign[0] == '-')
        {
            resultBits |= 1u << 31;
        }

        uint mantissa;
        int exponent;

        var matchPreDot = match.Groups["predot"].ValueSpan;
        var matchPostDot = match.Groups["postdot"].ValueSpan;
        uint preDot = uint.Parse(matchPreDot, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        uint postDot = matchPostDot.Length == 0 ? 0u : uint.Parse(matchPostDot, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        int shift = matchPostDot.Length * 4;
        mantissa = preDot << shift | postDot;


        int exponentSign = 1;
        var matchExponentSign = match.Groups["expsign"].ValueSpan;
        if (matchExponentSign.Length >= 1 && matchExponentSign[0] == '-')
        {
            exponentSign = -1;
        }
        var matchExponent = match.Groups["exp"].ValueSpan;
        exponent = exponentSign * int.Parse(matchExponent, NumberStyles.Number, CultureInfo.InvariantCulture);


        int topMantissa = 32 - BitOperations.LeadingZeroCount(mantissa);
        if (topMantissa == 0)
        {
            return BitConverter.UInt32BitsToSingle(resultBits);
        }
        mantissa = topMantissa >= 24 ? (mantissa >> (topMantissa - 24)) : (mantissa << (24 - topMantissa));
        mantissa &= (1u << 23) - 1;
        exponent += topMantissa - shift - 1;


        if (exponent < -126)
        {
            mantissa |= 1u << 23;
            mantissa >>= -(exponent + 126);
            exponent = -127;
        }

        resultBits |= (uint)(exponent + 127) << 23 | mantissa;

        return BitConverter.UInt32BitsToSingle(resultBits);
    }

    internal static string FormatHex(float value)
    {
        if (float.IsPositiveInfinity(value))
        {
            return "∞";
        }
        if (float.IsNegativeInfinity(value))
        {
            return "-∞";
        }
        if (float.IsNaN(value))
        {
            return "NaN";
        }


        uint bits = BitConverter.SingleToUInt32Bits(value);
        int exponent = (int)((bits >> 23) & 0xff);
        uint mantissa = bits & ((1u << 23) - 1);

        var sb = new StringBuilder();
        if (float.IsNegative(value))
        {
            sb.Append('-');
        }

        sb.Append("0x");

        if (exponent == 0)
        {
            if (mantissa == 0)
            {
                sb.Append("0.000000p+0f");
                return sb.ToString();
            }

            int topMantissa = 32 - BitOperations.LeadingZeroCount(mantissa);
            mantissa <<= 24 - topMantissa;
            mantissa &= (1u << 23) - 1;
            exponent += 1 - (24 - topMantissa);
        }

        sb.Append($"1.{mantissa << 1:x6}p{exponent - 127:+0;-0}f");

        return sb.ToString();
    }

#endif
}
