// how to compile:
// clang -O2 lgamma.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/lgamma/lgamma.c"
#include "testvec.h"
#include <math.h>

extern int signgam;

void printLgamma()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_lgamma(x)), (uint64_t)signgam);
    }

    for (int i = 0; i < ITERATION_LENGTH * 4; i++)
    {
        double x = toDouble(next());
        printf("%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_lgamma(x)), (uint64_t)signgam);
    }
}

int main(void)
{
    printLgamma();

    return 0;
}
