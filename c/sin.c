// how to compile:
// clang -O2 sin.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/sin/sin.c"
#include "testvec.h"

void printSin()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_sin(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = toDouble(next());
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_sin(x)));
    }
}

int main(void)
{
    printSin();

    return 0;
}
