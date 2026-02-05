// how to compile:
// clang -O2 fusedMultiplyAddF.c -o ./bin/testvec -lm -march=native

#include "testvec.h"

const static uint32_t errorVectors[] = {
    0xb8f7344c, 0x51a00000, 0xce2d049,
    0, 0, 0};

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

    for (int i = 0; errorVectors[i] != 0; i += 3)
    {
        float x = toFloat(errorVectors[i + 0]);
        float y = toFloat(errorVectors[i + 1]);
        float z = toFloat(errorVectors[i + 2]);
        printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(y), toUint32(z), toUint32(fmaf(x, y, z)));
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
