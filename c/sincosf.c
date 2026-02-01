// how to compile:
// clang -O2 sincosf.c -o ./bin/testvec -lm -march=native

#include "core-math/src/binary32/sincos/sincosf.c"
#include "testvec.h"
#include <math.h>

extern int signgam;

void printSinCosF()
{
    for (int i = 0; i < TEST_FLOATS_LENGTH; i++)
    {
        float x = testFloats[i];
        float sin, cos;
        cr_sincosf(x, &sin, &cos);
        printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(sin), toUint32(cos));
    }

    for (int i = 0; i < ITERATION_LENGTH; i++)
    {
        float x = toFloat(next());
        float sin, cos;
        cr_sincosf(x, &sin, &cos);
        printf("%08" PRIx32 "\t%08" PRIx32 "\t%08" PRIx32 "\n", toUint32(x), toUint32(sin), toUint32(cos));
    }
}

int main(void)
{
    printSinCosF();

    return 0;
}
