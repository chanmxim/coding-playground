package solution

// you can also use imports, for example:
// import "fmt"
// import "os"

// you can write to stdout for debugging purposes, e.g.
// fmt.Println("this is a debug message")

func Solution(S string) string {
	// Implement your solution here
	for i := 0; i < len(S)-1; i++ {
		isAB := (S[i] == 'A' && S[i+1] == 'B') || (S[i] == 'B' && S[i+1] == 'A')
		isDC := (S[i] == 'D' && S[i+1] == 'C') || (S[i] == 'C' && S[i+1] == 'D')

		if isAB || isDC {
			newS := S[:i] + S[i+2:]
			return Solution(newS)
		}
	}

	return S
}
