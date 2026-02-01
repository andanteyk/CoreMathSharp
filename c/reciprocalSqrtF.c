// how to compile:
// clang -O2 reciprocalSqrtF.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/rsqrt/rsqrtf.c"
#include "testvec.h"

void printReciprocalSqrtF()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_rsqrtf(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat((uint32_t)next() >> 1);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_rsqrtf(x)));
    }
}

int main(void)
{
    printReciprocalSqrtF();

    return 0;
}
