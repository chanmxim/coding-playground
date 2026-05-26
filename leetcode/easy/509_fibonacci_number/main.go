package main

// ====== ITERATIVE APPROACH ======
func fib(n int) int {
	if n <= 1 {
		return n
	}

	num1, num2 := 0, 1

	for i := 2; i <= n; i++ {
		num1, num2 = num2, num1+num2
	}

	return num2
}

// ====== RECURSION WITH CACHING APPROACH ======
// func fib(n int) int {
// 	cache := make(map[int]int)
//
// 	return fibHelper(n, cache)
// }
//
// func fibHelper(n int, cache map[int]int) int {
// 	if n <= 1 {
// 		return n
// 	}
//
// 	val, ok := cache[n]
// 	if ok {
// 		return val
// 	}
//
// 	cache[n] = fibHelper(n-1, cache) + fibHelper(n-2, cache)
//
// 	return cache[n]
// }
