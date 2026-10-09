#include <stdio.h>

#define IN 1
#define OUT 0

// Count lines, words, and characters in input
// int main() {
//   int c, nl, nw, nc, state;
//
//   state = OUT;
//   nl = nw = nc = 0;
//   while ((c = getchar()) != EOF) {
//     nc++;
//
//     if (c == '\n')
//       nl++;
//     if (c == ' ' || c == '\n' || c == '\t')
//       state = OUT;
//     else if (state == OUT) {
//       state = IN;
//       nw++;
//     }
//   }
//
//   printf("%d %d %d\n", nl, nw, nc);
//
//   return 0;
// }

// Ex. 1.12
int main() {
  int c, state;

  state = OUT;
  while ((c = getchar()) != EOF) {
    if (c != ' ' && c != '\t' && c != '\n') {
      state = IN;
      putchar(c);

    } else if (state == IN) {
      state = OUT;
      putchar('\n');
    }
  }

  return 0;
}
