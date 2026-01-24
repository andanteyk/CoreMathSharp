// how to compile:
// clang -O2 atan2.c -o ./bin/testvec -lm -march=native

#include "testvec.h"
#include "core-math/src/binary64/atan2/atan2.c"

void printAtan2()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        for (int j = 0; j < TEST_DOUBLES_LENGTH; j++)
        {
            double x = testDoubles[i];
            double y = testDoubles[j];
            printf("%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(y), toUint64(cr_atan2(y, x)));
        }
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextSignedDouble();
        double y = nextSignedDouble();
        printf("%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(y), toUint64(cr_atan2(y, x)));
    }
}

int main(void)
{
    printAtan2();

    return 0;
}
