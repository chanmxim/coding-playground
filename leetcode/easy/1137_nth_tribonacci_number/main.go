package main

// ====== ITERATIVE APPROACH ======
func tribonacci(n int) int {
	if n <= 1 {
		return n
	}

	n1, n2, n3 := 0, 1, 1

	for i := 3; i <= n; i++ {
		n1, n2, n3 = n2, n3, n1+n2+n3
	}

	return n3
}

// ====== RECURSION WITH CACHING APPROACH ======
// func tribonacci(n int) int{
//     cache := map[int]int{
//         0: 0,
//         1: 1,
//         2: 1,
//     }
//
//     return tribonacciWorker(n, cache)
// }
//
// func tribonacciWorker(n int, cache map[int]int) int {
//
//     val, ok := cache[n]
//     if ok {return val}
//
//     cache[n] = tribonacciWorker(n - 1, cache) + tribonacciWorker(n-2, cache) + tribonacciWorker(n - 3, cache)
//
//     return cache[n]
// }
