#include <stdint.h>
#include <stdio.h>

int atoi(char s[]);

int main() {
  char s[] = "123421";

  printf("%d", atoi(s));

  return 0;
}

int atoi(char s[]) {
  int32_t n = 0;

  for (int i = 0; s[i] >= '0' && s[i] <= '9'; i++) {
    n = n * 10 + (s[i] - '0');
  }

  return n;
}
