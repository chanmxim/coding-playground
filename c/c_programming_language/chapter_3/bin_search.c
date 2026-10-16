#include <stdio.h>

// Binary search in sorted v array
int binsearch(int x, int v[], int n);

int main() {
  int x = 5;
  int n = 10;
  int v[] = {1, 2, 3, 4, 5, 6, 8, 9, 0};

  printf("%d\n", binsearch(x, v, n));

  return 0;
}

int binsearch(int x, int v[], int n) {
  int low, high, mid;

  low = 0;
  high = n - 1;
  while (low <= high) {
    mid = (low + high) / 2;

    if (x < v[mid])
      high = mid + 1;
    else if (x > v[mid])
      low = mid + 1;
    else
      return mid;
  }

  return -1;
}
