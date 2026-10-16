#include <stdio.h>
#include <string.h>

// Reverse string s in place
void reverse(char s[]);

int main() {
  char s[] = "december";

  reverse(s);

  printf("%s\n", s);

  return 0;
}

void reverse(char s[]) {
  int c, i, j;

  for (i = 0, j = strlen(s) - 1; i < j; i++, j--)
    c = s[i], s[i] = s[j], s[j] = c;
}
