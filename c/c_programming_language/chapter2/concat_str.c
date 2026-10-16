#include "stdio.h"

// Concatenate t to end of s; s must be big enough
void my_strcat(char s[], char t[]);

int main() {
  char s[20] = "may";
  char t[] = "april";

  my_strcat(s, t);

  printf("%s\n", s);

  return 0;
}

void my_strcat(char s[], char t[]) {
  int i, j;

  i = j = 0;
  while (s[i] != '\0')
    i++;
  while ((s[i++] = t[j++]) != '\0')
    ;
}
