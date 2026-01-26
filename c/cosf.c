// how to compile:
// clang -O2 cosf.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/cos/cosf.c"
#include "testvec.h"

void printCosf()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_cosf(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat(next());
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_cosf(x)));
    }
}

int main(void)
{
    printCosf();

    return 0;
}
