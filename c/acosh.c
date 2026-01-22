// how to compile:
// clang -O2 acosh.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/acosh/acosh.c"
#include "testvec.h"

void printAcosh()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_acosh(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextDoubleRange(1.0, 10.0);
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_acosh(x)));
    }
}

int main(void)
{
    printAcosh();

    return 0;
}
