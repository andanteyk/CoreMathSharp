// how to compile:
// clang -O2 cospi.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/cospi/cospi.c"
#include "testvec.h"

void printCospi()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_cospi(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextSignedDouble();
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_cospi(x)));
    }
}

int main(void)
{
    printCospi();

    return 0;
}
