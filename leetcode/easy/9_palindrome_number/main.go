package palindromenumber

// ====== Integer Approach ======
func isPalindrome(x int) bool {
    if x < 0 || (x % 10 == 0 && x != 0) {
        return false
    }
    
	// if x < 10{
	// 	return true
	// }

    reversedNum := 0

    for x > reversedNum{
        reversedNum = (reversedNum * 10) + (x % 10)
        x /= 10
    }

    return reversedNum == x || (reversedNum / 10) == x
}

// import "strconv"

// ====== String Approach ======
// func isPalindrome(x int) bool {
//     if x < 0{
//         return false
//     }

//     if x < 10{
//         return true
//     }

//     x_str := strconv.Itoa(x)

//     start := 0
//     end := len(x_str) - 1

//     for start <= end{
//         if (x_str[start] != x_str[end]){
//             return false
//         }

//         start++
//         end--
//     }

//     return true
// }