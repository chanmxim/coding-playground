package main

import (
	"fmt"
	"math"
)

// Context: An ATM needs to dispense a specific withdrawal amount to a customer using the fewest number of bills possible to save machine capacity, but it only has specific bill denominations available.
// Problem: You are given an integer array coins representing the different bill denominations the ATM holds, and an integer amount representing the total amount of money the customer wants to withdraw. Return the fewest number of bills that you need to make up that amount. If that amount of money cannot be made up by any combination of the bills, return -1.
//
// Assume you have an infinite number of each kind of bill.

// ====== ITERATIVE APPROACH(BOTTOM TOP) ======
func cash_dispense(coins []int, amount int) int {
	dp := make([]int, amount+1)
	dp[0] = 0

	for i := 1; i < amount+1; i++ {
		local_min := math.MaxInt32

		for _, coin := range coins {
			remains := i - coin

			if remains < 0 || dp[remains] == math.MaxInt32 {
				continue
			}

			coins_used := dp[remains] + 1
			local_min = min(local_min, coins_used)
		}

		dp[i] = local_min
	}

	if dp[amount] == math.MaxInt32 {
		return -1
	}
	return dp[amount]
}

// ====== RECURSION WITH CACHING APPROACH(DFS WITH BACKTRACKING) ======
// func cash_dispense(coins []int, amount int) int {
// 	cache := make(map[int]int)
//
// 	return worker(coins, amount, cache)
// }
//
// func worker(coins []int, amount int, cache map[int]int) int {
// 	if amount < 0 {
// 		return -1
// 	}
//
// 	if amount == 0 {
// 		return 0
// 	}
//
// 	val, ok := cache[amount]
// 	if ok {
// 		return val
// 	}
//
// 	min_coins := math.MaxInt32
// 	for _, coin := range coins {
// 		if result := worker(coins, amount-coin, cache); result != -1 {
// 			coins_used := result + 1
//
// 			if coins_used < min_coins {
// 				min_coins = coins_used
// 			}
// 		}
// 	}
//
// 	if min_coins == math.MaxInt32 {
// 		min_coins = -1
// 	}
//
// 	cache[amount] = min_coins
// 	return min_coins
// }

func main() {
	coins := []int{1, 5, 10, 20, 50, 100}
	amount := 276
	// coins := []int{2, 5}
	// amount := 3

	fmt.Println(cash_dispense(coins, amount))
}
