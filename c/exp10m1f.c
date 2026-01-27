// how to compile:
// clang -O2 exp10m1f.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/exp10m1/exp10m1f.c"
#include "testvec.h"

void printExp10m1f()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_exp10m1f(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextFloatRange(-40.0f, 40.0f);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_exp10m1f(x)));
    }
}

int main(void)
{
    printExp10m1f();

    return 0;
}
