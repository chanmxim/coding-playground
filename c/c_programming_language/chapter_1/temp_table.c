#include <stdio.h>

/*
print Fahrenheit-Celcius
  table for fahr = 0, 20, ..., 300
*/

#define LOWER 0
#define UPPER 300
#define STEP 20

int main() {
  // float fahr, celcius;
  // float lower, upper, step;

  // Ex. 1.3
  // printf("Fahrenheit-Celcius Table\n");
  //
  // lower = 0;
  // upper = 300;
  // step = 20;
  //
  // fahr = lower;
  // while (fahr <= upper) {
  //   celcius = 5 * (fahr - 32) / 9;
  //   printf("%3.0f\t%6.1f\n", fahr, celcius);
  //   fahr = fahr + step;
  // }

  // Ex. 1.4
  // printf("\nFahrenheit-Celcius Table\n");
  //
  // lower = 0;
  // upper = 40;
  // step = 2;
  //
  // celcius = lower;
  // while (celcius <= upper) {
  //   fahr = (celcius * 9.0 / 5.0) + 32;
  //   printf("%3.0f\t%6.1f\n", celcius, fahr);
  //   celcius = celcius + step;
  // }

  // Ex. 1.5
  int fahr;

  for (fahr = UPPER; fahr >= LOWER; fahr = fahr - STEP)
    printf("%3d %6.1f\n", fahr, (5.0 / 9.0) * (fahr - 32));

  return 0;
}
