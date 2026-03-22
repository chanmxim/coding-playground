import GroupMessage from "../models/GroupMessage.js";

export default function(io){
    io.on("connection", (socket) => {
        console.log("Client connected");

        socket.on("joinedRoom", ({username, room}) => {
            socket.join(room);
            
            console.log(`${username} joined ${room}`);

            socket.to(room).emit("message", {
                from_user: "Server",
                message: `${username} joined the room`
            })
        })

        socket.on("groupChatMessage", async ({from_user, room, message}) => {
            const newMsg = new GroupMessage({from_user, room, message});    
            await newMsg.save();
            io.to(room).emit("message", newMsg)
        })

        socket.on("leftRoom", ({username, room}) => {
            socket.leave(room)
            socket.to(room).emit("message", {
                from_user: "Server",
                message: `${username} left the room`
            })
        })

        socket.on("disconnect", () => {
            console.log("Client disconnected")
        })
    })
}