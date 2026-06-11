package main

// ====== ITERATIVE APPROACH ======
func minCostClimbingStairs(cost []int) int {

	for i := 2; i < len(cost); i++ {
		if cost[i-2] < cost[i-1] {
			cost[i] = cost[i-2] + cost[i]
		} else {
			cost[i] = cost[i-1] + cost[i]
		}
	}

	return min(cost[len(cost)-1], cost[len(cost)-2])

}

// ====== RECURSION WITH CACHING APPROACH ======
// func minCostClimbingStairs(cost []int) int {
//     cache := make(map[int]int)
//
//     return minCostWorker(cost, len(cost), cache)
// }
//
// func minCostWorker(cost []int, n int, cache map[int]int) int{
//
//     if n <= 1 { return 0 }
//
//     val, ok := cache[n]
//     if ok{
//         return val
//     }
//
//     cache[n] = min(cost[n - 1] + minCostWorker(cost, n - 1, cache), cost[n - 2] + minCostWorker(cost, n - 2, cache))
//
//     return cache[n]
// }
