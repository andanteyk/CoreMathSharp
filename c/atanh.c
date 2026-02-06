// how to compile:
// clang -O2 atanh.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/atanh/atanh.c"
#include "testvec.h"

const static uint64_t errorVectors[] = {
    0x3fd2dbb7b1c91363,
    0x3fdc493dc899e4a5,
    0};

void printAtanh()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_atanh(x)));
    }

    for (int i = 0; errorVectors[i] != 0; i++)
    {
        double x = toDouble(errorVectors[i]);
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
