// how to compile:
// clang -O2 acoshf.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/acosh/acoshf.c"
#include "testvec.h"

void printAcoshf()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_acoshf(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextFloatRange(1.0f, 10.0f);
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_acoshf(x)));
    }
}

int main(void)
{
    printAcoshf();

    return 0;
}
