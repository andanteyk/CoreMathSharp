// how to compile:
// clang -O2 cospif.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/cospi/cospif.c"
#include "testvec.h"

void printCospif()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_cospif(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextSignedFloat();
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_cospif(x)));
    }
}

int main(void)
{
    printCospif();

    return 0;
}
