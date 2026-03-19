package rotatelist


type ListNode struct {
 	Val int
	Next *ListNode
}


func rotateRight(head *ListNode, k int) *ListNode {
    
    if head == nil || head.Next == nil {return head}

    curr := head

    // Get list length
    length := 1
    for curr.Next != nil{
        curr = curr.Next
        length++
    }

    curr.Next = head // make infinite loop
    curr = head // reset head to initial position

    // Calculate the tail
    k = k % length
    n := length - k - 1

    // Move to the tail
    for n > 0{
        curr = curr.Next
        n--
    }

    head = curr.Next // assign new head
    curr.Next = nil // destrou infinite loop

    return head
}