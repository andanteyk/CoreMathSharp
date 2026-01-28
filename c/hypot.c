// how to compile:
// clang -O2 hypot.c -o ./bin/testvec -lm -march=native

#include "testvec.h"
#include "core-math/src/binary64/hypot/hypot.c"

void printHypot()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        for (int j = 0; j < TEST_DOUBLES_LENGTH; j++)
        {
            double x = testDoubles[i];
            double y = testDoubles[j];
            printf("%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(y), toUint64(cr_hypot(x, y)));
        }
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = toDouble(next());
        double y = toDouble(next());
        printf("%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(y), toUint64(cr_hypot(x, y)));
    }
}

int main(void)
{
    printHypot();

    return 0;
}
