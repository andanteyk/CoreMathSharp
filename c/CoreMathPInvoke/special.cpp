#include "framework.h"

#include <math.h>
extern int signgam;

int cr_lgammaf_signgam() { return signgam; }
float cr_sqrtf(float x) { return sqrtf(x); }

int cr_lgamma_signgam() { return signgam; }
double cr_sqrt(double x) { return sqrt(x); }
