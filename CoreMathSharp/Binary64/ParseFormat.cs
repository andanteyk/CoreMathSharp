using System;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreMathSharp;

public static partial class StrictMath
{
#if NET7_0_OR_GREATER

    [GeneratedRegex(@"^(?<sign>[+\-]?)0x((?<predot>[0-9a-f]*)\.(?<postdot>[0-9a-f]+)|(?<predot>[0-9a-f]+)\.(?<postdot>[0-9a-f]*)|(?<predot>[0-9a-f]+)(?<postdot>))p(?<expsign>[+\-]?)(?<exp>[0-9]+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex DoubleHexRegex { get; }

    internal static double ParseHex(string str)
    {
        if (str == "NaN")
        {
            return double.NaN;
        }
        if (str == "∞" || str == "+∞")
        {
            return double.PositiveInfinity;
        }
        if (str == "-∞")
        {
            return double.NegativeInfinity;
        }

        var match = DoubleHexRegex.Match(str);
        if (!match.Success)
        {
            throw new FormatException();
        }

        ulong resultBits = 0;

        var matchSign = match.Groups["sign"].ValueSpan;
        if (matchSign.Length >= 1 && matchSign[0] == '-')
        {
            resultBits |= 1ul << 63;
        }

        ulong mantissa;
        int exponent;

        var matchPreDot = match.Groups["predot"].ValueSpan;
        var matchPostDot = match.Groups["postdot"].ValueSpan;
        if (matchPostDot.Length > 13)
        {
            matchPostDot = matchPostDot[..13];
        }
        ulong preDot = ulong.Parse(matchPreDot, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        ulong postDot = matchPostDot.Length == 0 ? 0ul : ulong.Parse(matchPostDot, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
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


        int topMantissa = 64 - BitOperations.LeadingZeroCount(mantissa);
        if (topMantissa == 0)
        {
            return BitConverter.UInt64BitsToDouble(resultBits);
        }
        mantissa = topMantissa >= 53 ? (mantissa >> (topMantissa - 53)) : (mantissa << (53 - topMantissa));
        mantissa &= (1ul << 52) - 1;
        exponent += topMantissa - shift - 1;


        if (exponent < -1022)
        {
            mantissa |= 1ul << 52;
            mantissa >>= -(exponent + 1022);
            exponent = -1023;
        }

        resultBits |= (ulong)(exponent + 1023) << 52 | mantissa;

        return BitConverter.UInt64BitsToDouble(resultBits);
    }

    internal static string FormatHex(double value)
    {
        if (double.IsPositiveInfinity(value))
        {
            return "∞";
        }
        if (double.IsNegativeInfinity(value))
        {
            return "-∞";
        }
        if (double.IsNaN(value))
        {
            return "NaN";
        }


        ulong bits = BitConverter.DoubleToUInt64Bits(value);
        int exponent = (int)((bits >> 52) & 0x7ff);
        ulong mantissa = bits & ((1ul << 52) - 1);

        var sb = new StringBuilder();
        if (double.IsNegative(value))
        {
            sb.Append('-');
        }

        sb.Append("0x");

        if (exponent == 0)
        {
            if (mantissa == 0)
            {
                sb.Append("0.0000000000000p+0");
                return sb.ToString();
            }

            int topMantissa = 64 - BitOperations.LeadingZeroCount(mantissa);
            mantissa <<= 53 - topMantissa;
            mantissa &= (1ul << 52) - 1;
            exponent += 1 - (53 - topMantissa);
        }

        sb.Append($"1.{mantissa:x13}p{exponent - 1023:+0;-0}");

        return sb.ToString();
    }

#endif
}
