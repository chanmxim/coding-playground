package solution

// you can also use imports, for example:
// import "os"

// you can write to stdout for debugging purposes, e.g.
// fmt.Println("this is a debug message")

func Solution(S string) int {
	// Remove leading zeros
	index_after_leading_zeros := 0
	empty := true
	for i := 0; i < len(S); i++ {
		if S[i] == '1' {
			index_after_leading_zeros = i
			empty = false
			break
		}
	}

	if empty == true {
		return 0
	}

	// Reminder for myself in case asked about the thought process
	// Example:
	// 00011100
	// 0001110
	// 000111
	// 000110
	// 00011
	// 00010
	// 0001
	// 0000

	n := 0
	for i := len(S) - 1; i >= index_after_leading_zeros; i-- {
		if S[i] == '0' {
			n += 1
		} else {
			n += 2
		}
	}

	return n - 1
}
