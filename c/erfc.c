// how to compile:
// clang -O2 erfc.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/erfc/erfc.c"
#include "testvec.h"

void printErfc()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_erfc(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextSignedDouble();
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_erfc(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextDoubleRange(-5.0, 5.0);
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_erfc(x)));
    }
}

int main(void)
{
    printErfc();

    return 0;
}
