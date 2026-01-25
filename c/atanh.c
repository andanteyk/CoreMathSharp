// how to compile:
// clang -O2 atanh.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/atanh/atanh.c"
#include "testvec.h"

void printAtanh()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_atanh(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextSignedDouble();
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_atanh(x)));
    }
}

int main(void)
{
    printAtanh();

    return 0;
}
