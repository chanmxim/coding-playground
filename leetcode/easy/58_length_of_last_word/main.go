package main

// ====== BACKWARD (PERFORMANCE) APPROACH ======
func lengthOfLastWord(s string) int {
    counter := 0
    i := len(s) - 1

    for i >= 0 && s[i] == ' '{
        i--
    }

    for i >= 0 && s[i] != ' '{
        counter++
        i--
    }

    return counter
}

// ====== NAIVE APPROACH ======
// func lengthOfLastWord(s string) int {
//     counter := 0
//     reset := true
    
//     for _, char := range s{
//         if char != ' '{
//             if reset{
//                 counter = 0
//                 reset = false
//             }

//             counter++
//         } else{
//             reset = true
//         }
        
//     }

//     return counter
// }

func main(){
	println(lengthOfLastWord("Hello World"))
}