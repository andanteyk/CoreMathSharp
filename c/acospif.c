// how to compile:
// clang -O2 acospif.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/acospi/acospif.c"
#include "testvec.h"

void printAcospif()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_acospif(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextSignedFloat();
        printf("%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(cr_acospif(x)));
    }
}

int main(void)
{
    printAcospif();

    return 0;
}
