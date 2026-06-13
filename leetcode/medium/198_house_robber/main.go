package main

// ====== ITERATIVE APPROACH ======
func rob(nums []int) int {

	if len(nums) == 1 {
		return nums[0]
	}

	if len(nums) == 2 {
		return max(nums[0], nums[1])
	}

	nums[2] = nums[2] + nums[0]
	max_money := max(nums[1], nums[2])

	for i := 3; i < len(nums); i++ {
		nums[i] = nums[i] + max(nums[i-2], nums[i-3])
		max_money = max(max_money, nums[i])
	}

	return max_money
}

// ====== RECURSION WITH CACHING APPROACH ======
// func rob(nums []int) int {
//     cache := make(map[int]int)
//
//     if len(nums) == 1 { return nums[0]}
//
//     return max(robWorker(nums, len(nums) - 1, cache), robWorker(nums, len(nums) - 2, cache))
// }
//
// func robWorker(nums []int, n int, cache map[int]int) int{
//     if n == 0 { return nums[n] }
//
//     if n == 1 { return max(nums[n], nums[n - 1]) }
//
//     if n == 2 { return max(nums[n] + nums[n - 2], nums[n - 1]) }
//
//     val, ok := cache[n]
//     if ok {
//         return val
//     }
//
//     cache[n] = nums[n] + max(robWorker(nums, n - 2, cache), robWorker(nums, n - 3, cache))
//
//     return cache[n]
// }
