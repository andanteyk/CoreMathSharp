// how to compile:
// clang -O2 asinh.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/asinh/asinh.c"
#include "testvec.h"

void printAsinh()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_asinh(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextDoubleRange(1.0, 10.0);
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_asinh(x)));
    }
}

int main(void)
{
    printAsinh();

    return 0;
}
