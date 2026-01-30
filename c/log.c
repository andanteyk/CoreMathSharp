// how to compile:
// clang -O2 log.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/log/log.c"
#include "testvec.h"

void printLog()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_log(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = toDouble(next() >> 1);
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_log(x)));
    }
}

int main(void)
{
    printLog();

    return 0;
}
