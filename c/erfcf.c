// how to compile:
// clang -O2 erfcf.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/erfc/erfcf.c"
#include "testvec.h"

void printErfcf()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_erfcf(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextFloatRange(-4.0f, 4.0f);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_erfcf(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat(next());
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_erfcf(x)));
    }
}

int main(void)
{
    printErfcf();

    return 0;
}
