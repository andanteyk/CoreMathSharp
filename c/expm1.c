// how to compile:
// clang -O2 expm1.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/expm1/expm1.c"
#include "testvec.h"

void printExpm1()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_expm1(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextDoubleRange(-40.0, 256.0);
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_expm1(x)));
    }
}

int main(void)
{
    printExpm1();

    return 0;
}
