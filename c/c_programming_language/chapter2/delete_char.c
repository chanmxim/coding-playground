#include <stdio.h>

// Delete all c from s
void squeeze(char s[], int c);

int main() {
  char s[11] = "december";
  int c = 'e';

  squeeze(s, c);

  printf("%s\n", s);

  return 0;
}

void squeeze(char s[], int c) {
  int i, j;

  for (i = j = 0; s[i] != '\0'; i++) {
    if (s[i] != c)
      s[j++] = s[i];
  }

  s[j] = '\0';
}
