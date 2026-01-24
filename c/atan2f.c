// how to compile:
// clang -O2 atan2f.c -o ./bin/testvec -lm -march=native

#include "testvec.h"
#include "core-math/src/binary32/atan2/atan2f.c"

void printAtan2f()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        for (int j = 0; j < TEST_FLOATS_LENGTH; j++)
        {
            float x = testFloats[i];
            float y = testFloats[j];

            printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(y), toUint32(cr_atan2f(y, x)));
        }
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat(next());
        float y = toFloat(next());

        printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(y), toUint32(cr_atan2f(y, x)));
    }
}

int main(void)
{
    printAtan2f();

    return 0;
}
