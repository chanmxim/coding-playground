import express from "express";
import dotenv from "dotenv";
import cors from "cors";

import userRouter from "./routers/userRouter.js"; 
import employeeRouter from "./routers/employeeRouter.js";
import { connectDb } from "./config/db.js";

dotenv.config();

const app = express();
const PORT = process.env.SERVER_PORT || 3000;

app.use(cors({
    origin: "http://localhost:5173",
}));
app.use(express.json());
app.use(express.urlencoded({ extended: true }))

app.use("/api/v1/user", userRouter);

app.use("/api/v1/emp", employeeRouter);

try{
    await connectDb();
    app.listen(PORT, () => {
        console.log("Server is running on port " + PORT);
    })
} catch (err){
    console.error("Failed to start server:", err);
}
