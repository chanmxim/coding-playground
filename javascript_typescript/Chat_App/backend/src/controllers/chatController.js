import GroupMessage from "../models/GroupMessage.js";

export const getRoomMessageHistory = async (req, res) => {
    try{
        const { room } = req.params;

        const messages = await GroupMessage.find({room})
            .sort({date_sent: 1})
        
        res.status(200).json({
            messages
        });
    } catch (err){
        res.status(500).json({
            error: "Error in fetching room chat history"
        })
    }
}