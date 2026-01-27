// how to compile:
// clang -O2 exp2.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary64/exp2/exp2.c"
#include "testvec.h"

void printExp2()
{
    for (int i = 0; i < TEST_DOUBLES_LENGTH; i++)
    {
        double x = testDoubles[i];
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_exp2(x)));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        double x = nextDoubleRange(-1024.0, 1024.0);
        printf("%016" PRIx64 "\t%016" PRIx64 "\n", toUint64(x), toUint64(cr_exp2(x)));
    }
}

int main(void)
{
    printExp2();

    return 0;
}
