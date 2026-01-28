// how to compile:
// clang -O2 expm1f.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/expm1/expm1f.c"
#include "testvec.h"

void printExpm1f()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_expm1f(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextFloatRange(-20.0f, 100.0f);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_expm1f(x)));
    }
}

int main(void)
{
    printExpm1f();

    return 0;
}
