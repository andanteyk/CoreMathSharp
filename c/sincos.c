// how to compile:
// clang -O2 sincos.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/sincos/sincos.c"
#include "testvec.h"

void printSinCos()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        double sin, cos;
        cr_sincos(x, &sin, &cos);
        printf("%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(sin), toUint64(cos));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = toDouble(next());
        double sin, cos;
        cr_sincos(x, &sin, &cos);
        printf("%016" PRIx64 "\t%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(sin), toUint64(cos));
    }
}

int main(void)
{
    printSinCos();

    return 0;
}
