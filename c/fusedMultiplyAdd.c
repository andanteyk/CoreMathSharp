// how to compile:
// clang -O2 fusedMultiplyAdd.c -o ./bin/testvec -lm -march=native

#include "testvec.h"

const static uint64_t errorVectors[] = {
    0x073d9dbad09f6241, 0x3594187266071a61, 0x000e41733cb5c35b,
    0x1d31dbc72eae144b, 0xa05e40111db522df, 0x804e73f0cb21a6ff,
    0x906bf37840164858, 0x880ae1e017477079, 0x8001530570325973,
    0x1f4ca970a27428c6, 0x210dd800c23d346f, 0x8080e93d601fe465,
    0, 0, 0};

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

    for (int i = 0; errorVectors[i] != 0; i += 3)
    {
        double x = toDouble(errorVectors[i + 0]);
        double y = toDouble(errorVectors[i + 1]);
        double z = toDouble(errorVectors[i + 2]);
        printf("%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(y), toUint64(z), toUint64(fma(x, y, z)));
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
