#include <stdio.h>
#include <stdlib.h>
#include <memory.h>
#include <inttypes.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include <float.h>
#pragma STDC FP_CONTRACT OFF

static inline uint64_t rotl(uint64_t x, int k)
{
	return x << k | x >> (-k & 0x3f);
}

static inline uint64_t mul128(uint64_t x, uint64_t y, uint64_t *lo)
{
	__uint128_t mul = (__uint128_t)x * y;
	*lo = mul;
	return mul >> 64;
}

static inline uint64_t toUint64(double x)
{
	uint64_t r;
	memcpy(&r, &x, sizeof(uint64_t));
	return r;
}

static inline double toDouble(uint64_t x)
{
	double r;
	memcpy(&r, &x, sizeof(double));
	return r;
}

static inline uint32_t toUint32(float x)
{
	uint32_t r;
	memcpy(&r, &x, sizeof(uint32_t));
	return r;
}

static inline float toFloat(uint32_t x)
{
	float r;
	memcpy(&r, &x, sizeof(float));
	return r;
}

static uint64_t State0 = 1, State1 = 1;
static inline uint64_t next()
{
	uint64_t s0 = State0, s1 = State1;
	uint64_t result = rotl((s0 + s1) * 9, 29) + s0;

	State0 = s0 ^ rotl(s1, 29);
	State1 = s0 ^ s1 << 9;

	return result;
}

static inline uint64_t nextULong(uint64_t max)
{
	uint64_t lo;
	uint64_t hi = mul128(next(), max, &lo);

	if (lo < max)
	{
		uint64_t mod = -max % max;
		while (lo < mod)
		{
			hi = mul128(next(), max, &lo);
		}
	}

	return hi;
}

static inline double nextDouble()
{
	return (next() >> 11) * (1.0 / ((uint64_t)1 << 53));
}

static inline double nextSignedDouble()
{
	return ((int64_t)next() >> 10) * (1.0 / ((uint64_t)1 << 53));
}

static inline double nextDoubleRange(double min, double max)
{
	double r = nextDouble();
	return (1.0 - r) * min + r * max;
}

static inline float nextFloat()
{
	return (next() >> 40) * (1.0f / (1 << 24));
}

static inline float nextSignedFloat()
{
	return ((int64_t)next() >> 39) * (1.0f / ((uint32_t)1 << 24));
}

static inline float nextFloatRange(float min, float max)
{
	float r = nextFloat();
	return (1.0f - r) * min + r * max;
}

#define TEST_DOUBLES_LENGTH (13)
const static double testDoubles[TEST_DOUBLES_LENGTH] = {
	2.718281828459045,
	5e-324,
	1.7976931348623157E+308,
	-1.7976931348623157E+308,
	0.0 / 0.0,
	-1.0 / 0.0,
	-0.0,
	3.141592653589793,
	1.0 / 0.0,
	3.141592653589793 * 2,
	-1.0,
	0.0,
	1.0};

#define TEST_FLOATS_LENGTH (13)
const static float testFloats[TEST_FLOATS_LENGTH] = {
	2.7182817f,
	1.4e-45f,
	3.40282346638528859e+38f,
	-3.40282346638528859e+38f,
	0.0f / 0.0f,
	-1.0f / 0.0f,
	-0.0f,
	3.1415927f,
	1.0f / 0.0f,
	6.283185307f,
	-1.0f,
	0.0f,
	1.0f};

#define ITERATION_LENGTH (1024)
