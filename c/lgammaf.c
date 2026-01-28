// how to compile:
// clang -O2 lgammaf.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/lgamma/lgammaf.c"
#include "testvec.h"
#include <math.h>

extern int signgam;

void printLgammaf()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        signgam = 1;
        printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_lgammaf(x)), (uint32_t)signgam);
    }

    for (int i = 0; i < ITERATION_LENGTH * 4; i++)
    {
        float x = toFloat(next());
        signgam = 1;
        printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_lgammaf(x)), (uint32_t)signgam);
    }
}

int main(void)
{
    printLgammaf();

    return 0;
}
