import express from "express"
import { Server } from "socket.io"
import {createServer} from "http"
import dotenv from "dotenv";
import cors from "cors";

import authRoutes from "./routes/authRoutes.js"
import chatRoutes from "./routes/chatRoutes.js"
import chatSocket from "./sockets/chatSocket.js"
import { connectDb } from "./config/db.js";

dotenv.config();

const PORT = process.env.SERVER_PORT || 3000;

const app = express();
const httpServer = createServer(app);
const io = new Server(httpServer, {
    cors: {
        origin: "*",
        methods: ["GET", "POST"]
    }
});

// Middleware
app.use(express.json());
app.use(express.urlencoded({ extended: true }))
app.use(cors());

app.use("/api", authRoutes);
app.use("/api/chat", chatRoutes);

chatSocket(io);

try{
    await connectDb();
    httpServer.listen(PORT, () => {
        console.log("Server is running on port " + PORT);
    })
} catch (err){
    console.error("Failed to start server:", err);
}