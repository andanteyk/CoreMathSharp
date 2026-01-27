// how to compile:
// clang -O2 exp10.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/exp10/exp10.c"
#include "testvec.h"

void printExp10()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_exp10(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextDoubleRange(-256.0, 256.0);
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_exp10(x)));
    }
}

int main(void)
{
    printExp10();

    return 0;
}
