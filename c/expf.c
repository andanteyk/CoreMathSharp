// how to compile:
// clang -O2 expf.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/exp/expf.c"
#include "testvec.h"

void printExpf()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_expf(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextFloatRange(-100.0f, 100.0f);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_expf(x)));
    }
}

int main(void)
{
    printExpf();

    return 0;
}
