// how to compile:
// clang -O2 log2f.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/log2/log2f.c"
#include "testvec.h"

void printLog2f()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_log2f(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat((uint32_t)next() >> 1);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_log2f(x)));
    }
}

int main(void)
{
    printLog2f();

    return 0;
}
