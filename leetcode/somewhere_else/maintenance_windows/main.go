package main

import (
	"cmp"
	"fmt"
	"slices"
)

// Context: Scotiabank's backend servers require periodic maintenance. To minimize downtime, the infrastructure team wants to consolidate overlapping maintenance windows into single, continuous blocks of time.
// Problem: You are given an array of intervals where intervals[i] = [start_i, end_i], representing the start and end times of scheduled maintenance in hours from the start of the year. Merge all overlapping intervals, and return an array of the non-overlapping intervals that cover all the scheduled downtimes.

func merge_windows(intervals [][]int) [][]int {
	slices.SortFunc(intervals, func(a, b []int) int {
		return cmp.Compare(a[0], b[0])
	})

	merged_intervals := [][]int{intervals[0]}

	for i := 1; i < len(intervals); i++ {
		interval1 := merged_intervals[len(merged_intervals)-1]
		interval2 := intervals[i]

		if interval1[1] >= interval2[0] {
			merged_intervals[len(merged_intervals)-1][1] = max(interval1[1], interval2[1])

		} else {
			merged_intervals = append(merged_intervals, interval2)
		}
	}

	return merged_intervals
}

func main() {
	intervals := [][]int{{1, 6}, {2, 4}, {3, 7}, {15, 18}, {19, 20}}
	fmt.Println(merge_windows(intervals))
}
