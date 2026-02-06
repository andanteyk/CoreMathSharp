// how to compile:
// clang -O2 compoundf.c -o ./bin/testvec -lm -march=native

#include "testvec.h"
#include "core-math/src/binary32/compound/compoundf.c"

void printCompoundf()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        for (int j = 0; j < TEST_FLOATS_LENGTH; j++)
        {
            float x = testFloats[i];
            float y = testFloats[j];

            printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(y), toUint32(cr_compoundf(x, y)));
        }
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = nextSignedFloat();
        float y = nextFloatRange(-10.0f, 10.0f);

        printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(y), toUint32(cr_compoundf(x, y)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat(next());
        float y = toFloat(next());

        printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(y), toUint32(cr_compoundf(x, y)));
    }
}

int main(void)
{
    printCompoundf();

    return 0;
}
