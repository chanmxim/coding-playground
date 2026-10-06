#include <stdio.h>

// Copy input to output v1
// int main() {
//
//   int c;
//
//   c = getchar();
//   while (c != EOF) {
//     putchar(c);
//     c = getchar();
//   }
//
//   return 0;
// }

// Copy input to output v2
// int main() {
//
//   // Ex. 1.7
//   printf("%d\n", EOF);
//
//   int c;
//
//   while ((c = getchar()) != EOF) {
//     putchar(c);
//
//     // Ex. 1.6
//     printf("\nExpression result %d\n", c != EOF);
//   }
//
//   return 0;
// }

// Count characters in input v1
// int main() {
//
//   long nc;
//
//   nc = 0;
//   while (getchar() != EOF)
//     nc++;
//   printf("%ld\n", nc);
//
//   return 0;
// }

// Count characters in input v2
// int main() {
//
//   double nc;
//
//   for (nc = 0; getchar() != EOF; nc++)
//     ;
//   printf("%.0f\n", nc);
//
//   return 0;
// }

// Count lines in input
// int main() {
//   int c, nl;
//
//   nl = 0;
//   while ((c = getchar()) != EOF)
//     // Ex. 1.8
//     if (c == '\n' || c == ' ' || c == '\t')
//       nl++;
//   printf("%d\n", nl);
//
//   return 0;
// }

// Ex. 1.9
// int main() {
//   int c, prevc;
//
//   while ((c = getchar()) != EOF) {
//     if (c != ' ' || prevc != ' ') {
//       putchar(c);
//     }
//     prevc = c;
//   }
//
//   return 0;
// }

// Ex. 1.10
int main() {
  int c;

  while ((c = getchar()) != EOF) {
    if (c == '\t') {
      putchar('\\');
      putchar('t');

    } else if (c == '\b') {
      putchar('\\');
      putchar('b');

    } else if (c == '\\') {
      putchar('\\');
      putchar('\\');
    } else {
      putchar(c);
    }
  }

  return 0;
}
