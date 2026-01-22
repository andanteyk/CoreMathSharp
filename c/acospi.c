// how to compile:
// clang -O2 acospi.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/acospi/acospi.c"
#include "testvec.h"

void printAcospi()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_acospi(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextSignedDouble();
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_acospi(x)));
    }
}

int main(void)
{
    printAcospi();

    return 0;
}
