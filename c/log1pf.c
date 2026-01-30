// how to compile:
// clang -O2 log1pf.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/log1p/log1pf.c"
#include "testvec.h"

void printLog1pf()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_log1pf(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat((uint32_t)next() >> 1);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_log1pf(x)));
    }
}

int main(void)
{
    printLog1pf();

    return 0;
}
