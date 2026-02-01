// how to compile:
// clang -O2 powf.c -o ./bin/testvec -lm -march=native

#include "testvec.h"
#include "core-math/src/binary32/pow/powf.c"

void printPowf()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        for (int j = 0; j < TEST_FLOATS_LENGTH; j++)
        {
            float x = testFloats[i];
            float y = testFloats[j];

            printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(y), toUint32(cr_powf(x, y)));
        }
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextFloatRange(-256.0, 256.0);
        float y = nextFloatRange(-16.0, 16.0);

        printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(y), toUint32(cr_powf(x, y)));
    }
}

int main(void)
{
    printPowf();

    return 0;
}
