package removeelement

func removeElement(nums []int, val int) int {
    writer := 0

    for i, _ := range nums{
        if (nums[i] != val){
            nums[writer] = nums[i]
            writer++
        }
    }

    return writer
}