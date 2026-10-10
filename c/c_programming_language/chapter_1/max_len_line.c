#include <stdio.h>

#define MAXLINE 10

int get_line(char line[], int maxline);

void copy(char to[], char from[]);

// Print the longest input line
int main() {
  int len, max;
  char line[MAXLINE], longest[MAXLINE];

  max = 0;
  while ((len = get_line(line, MAXLINE)) > 0) {
    if (len > max) {
      max = len;
      copy(longest, line);
    }
  }

  if (max > 0) {
    printf("%s %d", longest, max);
  }

  return 0;
}

// Read a line into s, return length
int get_line(char s[], int lim) {
  int c, i;

  for (i = 0; (c = getchar()) != EOF && c != '\n'; i++) {
    // Ex. 1.16
    if (i < lim - 1)
      s[i] = c;
  }

  if (c == '\n') {
    // Ex. 1.16
    if (i < lim - 1)
      s[i] = c;
    i++;
  }

  s[i] = '\0';
  return i;
}

// Copy 'from' into 'to'
void copy(char to[], char from[]) {
  int i;

  i = 0;
  while ((to[i] = from[i]) != '\0') {
    i++;
  }
}
