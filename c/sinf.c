// how to compile:
// clang -O2 sinf.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/sin/sinf.c"
#include "testvec.h"

void printSinf()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_sinf(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat(next());
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_sinf(x)));
    }
}

int main(void)
{
    printSinf();

    return 0;
}
