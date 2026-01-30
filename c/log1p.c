// how to compile:
// clang -O2 log1p.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/log1p/log1p.c"
#include "testvec.h"

void printLog1p()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_log1p(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = toDouble(next() >> 1);
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_log1p(x)));
    }
}

int main(void)
{
    printLog1p();

    return 0;
}
