// how to compile:
// clang -O2 tanhf.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/tanh/tanhf.c"
#include "testvec.h"

void printTanhf()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_tanhf(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat(next());
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_tanhf(x)));
    }
}

int main(void)
{
    printTanhf();

    return 0;
}
