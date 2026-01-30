// how to compile:
// clang -O2 log2p1f.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/log2p1/log2p1f.c"
#include "testvec.h"

void printLog2p1f()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_log2p1f(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat((uint32_t)next() >> 1);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_log2p1f(x)));
    }
}

int main(void)
{
    printLog2p1f();

    return 0;
}
