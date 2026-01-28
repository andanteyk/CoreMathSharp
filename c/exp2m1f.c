// how to compile:
// clang -O2 exp2m1f.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/exp2m1/exp2m1f.c"
#include "testvec.h"

void printExp2m1f()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_exp2m1f(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextFloatRange(-32.0f, 128.0f);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_exp2m1f(x)));
    }
}

int main(void)
{
    printExp2m1f();

    return 0;
}
