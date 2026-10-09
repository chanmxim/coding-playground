#include <stdio.h>

// Count digits, white space, others
// int main() {
//   int c, i, nwhite, nother;
//   int ndigit[10];
//
//   nwhite = nother = 0;
//   for (i = 0; i < 10; i++)
//     ndigit[i] = 0;
//
//   while ((c = getchar()) != EOF) {
//     if (c >= '0' && c <= '9')
//       ndigit[c - '0']++;
//     else if (c == ' ' || c == '\n' || c == '\t')
//       nwhite++;
//     else
//       nother++;
//   }
//
//   printf("digits = ");
//   for (i = 0; i < 10; i++)
//     printf(" %d", ndigit[i]);
//
//   printf(", white space = %d, other = %d\n", nwhite, nother);
//
//   return 0;
// }

// Ex. 1.13
#define MAX_LEN 10

int main() {
  int char_counts[MAX_LEN];
  int c, i, i1, n;

  for (i = 0; i < MAX_LEN; i++) {
    char_counts[i] = 0;
  }

  n = 0;
  while ((c = getchar()) != EOF) {
    if (c == ' ' || c == '\n' || c == '\t') {
      if (n != 0) {
        char_counts[n]++;
        n = 0;
      }
    } else {
      n++;
    }
  }

  for (i = 1; i < MAX_LEN; i++) {
    printf("[%d]: ", i);
    for (i1 = 0; i1 < char_counts[i]; i1++)
      printf("*");
    printf("\n");
  }

  return 0;
}
