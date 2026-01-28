// how to compile:
// clang -O2 exp2f.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/exp2/exp2f.c"
#include "testvec.h"

void printExp2f()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_exp2f(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextFloatRange(-128.0f, 128.0f);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_exp2f(x)));
    }
}

int main(void)
{
    printExp2f();

    return 0;
}
