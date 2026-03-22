import express from "express";
import { getRoomMessageHistory } from "../controllers/chatController.js";
import { auth } from "../middleware/authMiddleware.js";

const router = express.Router();

router.get("/:room", auth, getRoomMessageHistory);

export default router;