// how to compile:
// clang -O2 erff.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/erf/erff.c"
#include "testvec.h"

void printErff()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_erff(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextFloatRange(-4.0f, 4.0f);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_erff(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat(next());
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_erff(x)));
    }
}

int main(void)
{
    printErff();

    return 0;
}
