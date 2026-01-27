// how to compile:
// clang -O2 exp10f.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/exp10/exp10f.c"
#include "testvec.h"

void printExp10f()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_exp10f(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextFloatRange(-40.0f, 40.0f);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_exp10f(x)));
    }
}

int main(void)
{
    printExp10f();

    return 0;
}
