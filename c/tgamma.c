// how to compile:
// clang -O2 tgamma.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/tgamma/tgamma.c"
#include "testvec.h"

const static uint64_t errorVectors[] = {
    0xc0648ba8e27d09ad,
    0xc061fe464bbe8b7a,
    0};

void printTgamma()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_tgamma(x)));
    }

    for (int i = 0; errorVectors[i] != 0; i++)
    {
        double x = toDouble(errorVectors[i]);
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_tgamma(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH * 4; i++)
    {
        double x = nextDoubleRange(-184.0, 171.0);
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_tgamma(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = toDouble(next());
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_tgamma(x)));
    }
}

int main(void)
{
    printTgamma();

    return 0;
}
