// how to compile:
// clang -O2 log2.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/log2/log2.c"
#include "testvec.h"

void printLog2()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_log2(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = toDouble(next() >> 1);
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_log2(x)));
    }
}

int main(void)
{
    printLog2();

    return 0;
}
