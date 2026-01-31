// how to compile:
// clang -O2 pow.c -o ./bin/testvec -lm -march=native

#include "testvec.h"
#include "core-math/src/binary64/pow/pow.c"

void printPow()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        for (int j = 0; j < TEST_DOUBLES_LENGTH; j++)
        {
            double x = testDoubles[i];
            double y = testDoubles[j];
            printf("%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(y), toUint64(cr_pow(x, y)));
        }
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextDoubleRange(-256.0, 256.0);
        double y = nextDoubleRange(-16.0, 16.0);
        printf("%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(y), toUint64(cr_pow(x, y)));
    }
}

int main(void)
{
    printPow();

    return 0;
}
