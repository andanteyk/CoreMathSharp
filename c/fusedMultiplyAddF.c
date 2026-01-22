// how to compile:
// clang -O2 fusedMultiplyAddF.c -o ./testvec -lm -march=native

#include "testvec.h"

void printFusedMultiplyAddF()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        for (int j = 0; j < TEST_FLOATS_LENGTH; j++)
        {
            for (int k = 0; k < TEST_FLOATS_LENGTH; k++)
            {
                float x = testFloats[i];
                float y = testFloats[j];
                float z = testFloats[k];
                printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(y), toUint32(z), toUint32(fmaf(x, y, z)));
            }
        }
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextSignedFloat();
        float y = nextSignedFloat();
        float z = nextSignedFloat();
        printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(y), toUint32(z), toUint32(fmaf(x, y, z)));
    }
}

int main(void)
{
    printFusedMultiplyAddF();

    return 0;
}
