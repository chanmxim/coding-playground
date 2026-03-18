package removeduplicatesfromsortedlist

type ListNode struct {
 	Val int
	Next *ListNode
}

// ====== IF ALREADY SORTED APPROACH ======
func deleteDuplicates(head *ListNode) *ListNode {

    if head == nil || head.Next == nil {return head}

    prev := head
    curr := head.Next

    for curr != nil{

        if prev.Val == curr.Val {
            prev.Next = curr.Next
            curr.Next = nil
            curr = prev.Next
            continue
        }

        prev = curr
        curr = curr.Next
    }

    return head
}

// ====== HASHMAP APPROACH ======
// func deleteDuplicates(head *ListNode) *ListNode {
//     items := make(map[int]bool)

//     if head == nil || head.Next == nil {return head}

//     prev := head
//     curr := head.Next

//     items[prev.Val] = true 

//     for curr != nil{
//         _, ok := items[curr.Val]
//         if ok {
//             prev.Next = curr.Next
//             curr.Next = nil
//             curr = prev.Next
//             continue
//         }

//         items[curr.Val] = true
//         prev = curr
//         curr = curr.Next
//     }

//     return head
// }