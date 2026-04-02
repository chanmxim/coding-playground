package main

// ====== ITERATIVE APPROACH ======
func climbStairs(n int) int {
    if n <= 2{
        return n
    }   

    n1, n2 := 1, 2

    for i := 3; i <= n; i++{

        n1, n2 = n2, n1 + n2
    }


    return n2
}

// ====== RECURSION WITH CACHING APPROACH ======
// func climbStairs(n int) int {
//     cache := make(map[int]int)

//     return climbStairsHelper(n, cache)
// }

// func climbStairsHelper(n int, cache map[int]int) int{
//     if n <= 2 {return n}
    
//     val, ok := cache[n]
//     if ok{
//         return val
//     }

//     cache[n] = climbStairsHelper(n - 1, cache) + climbStairsHelper(n - 2, cache)
    
//     return cache[n]
// }