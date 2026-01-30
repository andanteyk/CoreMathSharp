// how to compile:
// clang -O2 logf.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/log/logf.c"
#include "testvec.h"

void printLogf()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_logf(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat((uint32_t)next() >> 1);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_logf(x)));
    }
}

int main(void)
{
    printLogf();

    return 0;
}
