package main

import "fmt"

// Problem: Given an array of integers representing a customer's daily net spending (positive numbers mean they spent money, negative numbers mean they received money), find the contiguous sub-array (containing at least one number) which has the largest sum and return its sum.

func fraud_detection(spending []int) int {
	highest_sum := spending[0]
	local_sum := spending[0]

	for i := 1; i < len(spending); i++ {
		local_sum = max(local_sum+spending[i], spending[i])

		if local_sum > highest_sum {
			highest_sum = local_sum
		}
	}

	return highest_sum
}

func main() {
	spending := []int{5, 4, -1, 7, 8}
	// spending := []int{-2, 1, -3, 4, -1, 2, 1, -5, 4}

	fmt.Println(fraud_detection(spending))
}
