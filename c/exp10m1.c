// how to compile:
// clang -O2 exp10m1.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/exp10m1/exp10m1.c"
#include "testvec.h"

void printExp10m1()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_exp10m1(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextDoubleRange(-256.0, 256.0);
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_exp10m1(x)));
    }
}

int main(void)
{
    printExp10m1();

    return 0;
}
