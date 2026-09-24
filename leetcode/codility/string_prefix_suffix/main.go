package solution

// you can also use imports, for example:
// import "fmt"
// import "os"

// you can write to stdout for debugging purposes, e.g.
// fmt.Println("this is a debug message")

func Solution(S string) int {
	// Implement your solution here
	max_length := 0

	for i := 1; i < len(S); i++ {
		if S[:i] == S[len(S)-i:] {
			max_length = i
		}
	}

	return max_length
}
