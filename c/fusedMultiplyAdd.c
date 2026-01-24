// how to compile:
// clang -O2 fusedMultiplyAdd.c -o ./bin/testvec -lm -march=native

#include "testvec.h"

void printFusedMultiplyAdd()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        for (int j = 0; j < TEST_DOUBLES_LENGTH; j++)
        {
            for (int k = 0; k < TEST_DOUBLES_LENGTH; k++)
            {
                double x = testDoubles[i];
                double y = testDoubles[j];
                double z = testDoubles[k];
                printf("%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(y), toUint64(z), toUint64(fma(x, y, z)));
            }
        }
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextSignedDouble();
        double y = nextSignedDouble();
        double z = nextSignedDouble();
        printf("%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(y), toUint64(z), toUint64(fma(x, y, z)));
    }
}

int main(void)
{
    printFusedMultiplyAdd();

    return 0;
}
