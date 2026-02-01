// how to compile:
// clang -O2 reciprocalSqrt.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/rsqrt/rsqrt.c"
#include "testvec.h"

void printReciprocalSqrt()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_rsqrt(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = toDouble(next() >> 1);
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_rsqrt(x)));
    }
}

int main(void)
{
    printReciprocalSqrt();

    return 0;
}
