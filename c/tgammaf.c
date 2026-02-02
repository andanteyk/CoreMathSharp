// how to compile:
// clang -O2 tgammaf.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/tgamma/tgammaf.c"
#include "testvec.h"

void printTGammaf()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_tgammaf(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH * 4; i++)
    {
        float x = nextFloatRange(-42.0f, 35.0f);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_tgammaf(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat(next());
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_tgammaf(x)));
    }
}

int main(void)
{
    printTGammaf();

    return 0;
}
