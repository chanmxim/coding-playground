package main

// === Slice Approach + Skip ===
func strStr(haystack string, needle string) int {
    hl := len(haystack)
    nl := len(needle)

	if nl > hl{ return -1}
    
	i := 0

    for i <= hl - nl {
		if haystack[i] != needle[0]{
            i++
			continue
		}

		if (haystack[i:i + nl] == needle){
			return i
		}

		i++
	}

    return -1
}

// === Slice Approach ===
// func strStr(haystack string, needle string) int {
//     hl := len(haystack)
//     nl := len(needle)

//     for i := 0; i <= hl - nl; i++{
//         if haystack[i:i + nl] == needle{
//             return i
//         }
//     }

//     return -1
// }

// === Slice Approach ===
// func strStr(haystack string, needle string) int {
//     hl := len(haystack)
//     nl := len(needle)

//     for i := 0; i <= hl - nl; i++{
//         if haystack[i:i + nl] == needle{
//             return i
//         }
//     }

//     return -1
// }

// === Slightly Optimized Brute Force Approach (by adding extra variable for memorization and skipping)=== 
// TODO: consider edge case when len(needle) > len(haystack) left to be processed
// func strStr(haystack string, needle string) int {
//     if len(needle) > len(haystack){
//         return -1
//     }

//     index := 0

//     for index < len(haystack){
//         start := index
//         counter := 0
//         checkpoint := index
//         checkpointIsSet := false

//         for start < len(haystack) && haystack[start] == needle[counter]{
//             if start != index && checkpointIsSet != true && haystack[start] == needle[0]{
//                 checkpoint = start
//                 checkpointIsSet = true
//             }

//             counter++
//             start++

//             if counter == len(needle){
//                 return index
//             }
            
            
//         }

//         if checkpointIsSet == true{
//             index = checkpoint
//         } else{
//             index++
//         }

//     }

//     return -1
// }

// === Brute Force Approach ===
// func strStr(haystack string, needle string) int {
//     if len(needle) > len(haystack){
//         return -1
//     }

//     for index, _ := range haystack{
//         start := index
//         counter := 0

//         for start < len(haystack) && haystack[start] == needle[counter]{
//             counter++
//             start++

//             if counter == len(needle){
//                 return index
//             }

            
//         }

//     }

//     return -1
// }

func main(){
	println(strStr("aabaaabaaac", "aabaaac"))
}