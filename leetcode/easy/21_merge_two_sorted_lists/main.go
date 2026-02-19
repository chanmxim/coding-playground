package mergetwosortedlists

type ListNode struct {
    Val int
    Next *ListNode
}

func mergeTwoLists(list1 *ListNode, list2 *ListNode) *ListNode {
    
    prehead := ListNode{}
    current := &prehead

    for list1 != nil && list2 != nil{
        if (list1.Val < list2.Val){
            current.Next = list1
            list1 = list1.Next
        } else {
            current.Next = list2
            list2 = list2.Next
        }

        current = current.Next
    }

    if list1 != nil{
        current.Next = list1
    } else{
        current.Next = list2
    }

    return prehead.Next
}

// // ====== MORE VERBOSE ======
// func mergeTwoLists(list1 *ListNode, list2 *ListNode) *ListNode {
//     if (list1 == nil){
//         return list2
//     } else if list2 == nil{
//         return list1
//     }

//     var head, current *ListNode

//     if list1.Val < list2.Val{
//         head = list1
//         current = list1
//         list1 = list1.Next
//     } else {
//         head = list2
//         current = list2
//         list2 = list2.Next
//     }

//     for list1 != nil && list2 != nil{
//         if (list1.Val < list2.Val){
//             current.Next = list1
//             list1 = list1.Next
//         } else{
//             current.Next = list2
//             list2 = list2.Next
//         }

//         current = current.Next
//     }

//     if (list1 == nil){
//         current.Next = list2
//     } else if (list2 == nil){
//         current.Next = list1
//     }

//     return head
// }